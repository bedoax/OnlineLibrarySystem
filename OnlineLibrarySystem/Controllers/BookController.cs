using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineLibrarySystem.Data;
using OnlineLibrarySystem.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineLibrarySystem.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AddBook()
        {
            return View();
        }
        [Authorize(Roles = "User")]
        public IActionResult BookDetails(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            var userId = User.Identity.Name; // Or however you get the current user ID
            var user = _context.Users.FirstOrDefault(u => u.Username == userId);
            if (user == null)
            {
                return Unauthorized();
            }

            var havePurchase = _context.OwnedBooks.Any(x => x.UserId == user.Id && x.BookId == book.Id);
            var borrowedBook = _context.borrowedBooks.FirstOrDefault(x => x.UserId == user.Id && x.BookId == book.Id);

            ViewBag.UserBalance = user.Balance;
            ViewBag.CanAfford = user.Balance >= book.Price;
            ViewBag.HavePurchase = havePurchase;
            ViewBag.HadBorrowed = borrowedBook != null;
            ViewBag.ReturnDate = borrowedBook?.ReturnDate;

            return View(book);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> Purchase(int bookId)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                return NotFound();
            }

            var userId = User.Identity.Name; // Or however you get the current user ID
            var user = _context.Users.FirstOrDefault(u => u.Username == userId);
            if (user == null)
            {
                return NotFound();
            }

            if (user.Balance < book.Price)
            {
                TempData["ErrorMessage"] = "You do not have enough balance to buy this book.";
                return RedirectToAction("BookDetails", new { id = bookId });
            }

            // Deduct the price from the user's balance
            user.Balance -= book.Price;

            // Save the book to the user's owned books
            var ownedBook = new OwnedBook
            {
                UserId = user.Id,
                BookId = book.Id,
                PurchaseDate = DateTime.Now
            };
            _context.OwnedBooks.Add(ownedBook);

            // Save changes to the database
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Book purchased successfully!";
            return RedirectToAction("BookDetails", new { id = bookId });
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> Borrow(int bookId)
        {
            var book = _context.Books.FirstOrDefault(x => x.Id == bookId);
            if (book == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userId = int.Parse(currentUserId);
            var borrowedBook = _context.borrowedBooks.FirstOrDefault(x => x.UserId == userId && x.BookId == bookId);

            if (borrowedBook == null)
            {
                var newBorrowedBook = new BorrowedBook
                {
                    DueDate = DateTime.Now.AddDays(7),
                    Status = "Borrowed",
                    UserId = userId,
                    BookId = bookId,
                    BorrowedDate = DateTime.Now,
                    ReturnDate = DateTime.Now.AddDays(7)
                };
                _context.borrowedBooks.Add(newBorrowedBook);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Book borrowed successfully!";
                return RedirectToAction("BookDetails", new { id = bookId });
            }

            TempData["ErrorMessage"] = "You have already borrowed this book.";
            return RedirectToAction("BookDetails", new { id = bookId });
        }
    }
}
