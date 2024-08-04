using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineLibrarySystem.Data;
using OnlineLibrarySystem.Models;

namespace OnlineLibrarySystem.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static string otpCode;
        private static User tempUser;
        private readonly ISendOtpMessage _emailSender;
        public RegisterController(ApplicationDbContext context, ISendOtpMessage emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(Registration registration)
        {
            if (ModelState.IsValid)
            {
                var checkUserExist = _context.Users.FirstOrDefault(x => x.Email == registration.Email || x.Username == registration.UserName);
                if (checkUserExist == null)
                {
                    var account = new User
                    {
                        Email = registration.Email,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        Username = registration.UserName,
                        PasswordHash = registration.Password,
                        RoleId = 2,
                        Balance = 0
                    };
                    otpCode = GenerateOTP();
                    tempUser = account;

                    // Send OTP to the user's email
                    _emailSender.SendEmailAsync(tempUser.Email, "OTP Code", $"Your OTP code is: {otpCode}");

                    return RedirectToAction("OTP");
                }
                if (checkUserExist.Username == registration.UserName)
                {
                    ModelState.AddModelError("", "Username has already been registered.");
                }
                if (checkUserExist.Email == registration.Email)
                {
                    ModelState.AddModelError("", "Email has already been registered.");
                }
            }
            return View(registration);
        }


        public IActionResult OTP()
        {
            return View();
        }

        [HttpPost]
        public IActionResult OTP(OTP key)
        {
            if (key.otp == otpCode)
            {
                try
                {
                    _context.Users.Add(tempUser);
                    _context.SaveChanges();
                    ModelState.Clear();
                    ViewBag.Message = $"{tempUser.Username} registered successfully. Please login.";

                    // Clear temporary values
                    tempUser = null;
                    otpCode = null;

                    return RedirectToAction("Login", "Login");
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Please enter a unique email or password");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "OTP is incorrect. Please try again.");
            }
            return View(key);
        }
        private string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}
