using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using OnlineLibrarySystem.Data;
using OnlineLibrarySystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace OnlineLibrarySystem.Controllers
{
   
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;


        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]

        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(x => (x.Username == login.UsernameOrEmail || x.Email == login.UsernameOrEmail) && x.PasswordHash == login.Password);
                if (user != null)
                {
                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                        new Claim(ClaimTypes.Name,user.Username),
                        new Claim(ClaimTypes.Email,user.Email),
                        new Claim(ClaimTypes.Role,user.RoleId==1?"Admin":"User"),

                    };
                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                        IsPersistent = true,
                        AllowRefresh = true
                    });

                    return user.RoleId == 1 ? RedirectToAction("Books", "Admin") : RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Username/Email or password not correct");
                }
            }
            return View(login);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
