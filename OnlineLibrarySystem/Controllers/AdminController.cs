using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineLibrarySystem.Data;
using OnlineLibrarySystem.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineLibrarySystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Books()
        {
            var books = _context.Books.ToList();
            return View(books);
        }

        public IActionResult Users()
        {
            var users = _context.Users.ToList();
            return View(users);
        }
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {

            // Check if the book already exists
            var exists = await _context.Users.AnyAsync(x => x.Username == user.Username && x.Email == user.Email);
            if (exists)
            {
                ModelState.AddModelError("", "This book already exists.");
                return View(user);
            }

            // Set additional properties
            user.Balance = 0;
            user.RoleId = 2;
            user.CreatedAt = DateTime.Now;
            user.UpdatedAt = DateTime.Now;

            // Add the book to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Users");


        }
        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(int id,User user)
        {
            var oldUser = await _context.Users
             .AsNoTracking()
             .FirstOrDefaultAsync(x => x.Id == id);
            if (id != user.Id)
            {
                return NotFound();
            }

            user.CreatedAt = oldUser.CreatedAt;
            user.UpdatedAt = DateTime.Now;
            user.RoleId = oldUser.RoleId;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Users");
        }
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Users");
        }
        public IActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBook(Book book)
        {

                // Check if the book already exists
                var exists = await _context.Books.AnyAsync(x => x.Title == book.Title && x.Author == book.Author);
                if (exists)
                {
                    ModelState.AddModelError("", "This book already exists.");
                    return View(book);
                }

                    // Set additional properties
                    book.AddedAt = DateTime.Now;
                    book.UpdatedAt = DateTime.Now;

                    // Add the book to the database
                    _context.Books.Add(book);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Books));


        }

        public async Task<IActionResult> EditBook(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBook(int id, Book book)
        {
            var oldBook = await _context.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
            if (id != book.Id)
            {
                return NotFound();
            }
            if(oldBook == null)
            {
                return NotFound();
            }

                     book.AddedAt =oldBook.AddedAt;
                    book.UpdatedAt = DateTime.Now;
                    _context.Books.Update(book);
                    await _context.SaveChangesAsync();

                return RedirectToAction("Books");

        }
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return RedirectToAction("Books");

        }
    }
}
