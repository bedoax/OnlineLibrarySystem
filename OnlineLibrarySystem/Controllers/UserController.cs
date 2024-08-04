using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using OnlineLibrarySystem.Data;
using OnlineLibrarySystem.Models;
using System.Linq;
using System.Security.Claims;

namespace OnlineLibrarySystem.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var books = _context.Books.ToList();
            return View(books);
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Shop()
        {
            var books = _context.Books.ToList();

            var bookIds = _context.borrowedBooks.Select(b => b.BookId)
                            .Union(_context.OwnedBooks.Select(o => o.BookId))
                            .Distinct();
            var topThreeBooks = _context.Books.Where(x => bookIds.Contains(x.Id)).Take(3).ToList();
            ViewBag.TopThreeBooks = topThreeBooks;
            return View(books);
        }
        public IActionResult MyProfile()
        {
            return View();
        }
        public IActionResult EditMyProfile()
        {
            // Assuming you have some mechanism to fetch the user's data, e.g., from session or claims
            var user = _context.Users.FirstOrDefault(x => x.Id == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString()));
            if (user == null)
            {
                return NotFound();
            }

            var model = new ChangePasswordInMyProfileViewModel
            {
                Username = user.Username,
                Email = user.Email
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult EditMyProfile(ChangePasswordInMyProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _context.Users.FirstOrDefault(x => x.Username == model.Username && x.Email == model.Email);
            var CreatedAt = user.CreatedAt;
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or email.");
                return View(model);
            }

              if(model.OldPassword != user.PasswordHash)
              {
                ModelState.AddModelError(string.Empty, "Not Correct.");
                return View(model);
              }
                user.Email = model.Email;
                user.Username = model.Username;
                user.PasswordHash = model.NewPassword;
                user.UpdatedAt = DateTime.Now;
                user.CreatedAt = CreatedAt;
                _context.Users.Update(user);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Password changed successfully.";
                return RedirectToAction("Index");
        }
    }
}
