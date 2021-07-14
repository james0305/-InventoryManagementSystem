using InventoryManagementSystem.Models.EF;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Controllers
{
    public class LoginController : Controller
    {
        private readonly InventoryManagementSystemContext _dbContext;

        public LoginController(InventoryManagementSystemContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult userLogin(User user)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("equipQryUser", "Equips");
            }

            if (ModelState.IsValid)
            {
                if(String.IsNullOrEmpty(user.Username) && String.IsNullOrEmpty(user.Password))
                {
                    return View(user);
                }

                var userItem = _dbContext.Users.Where(u => u.Username == user.Username && u.Password == user.Password).FirstOrDefault();
                if (userItem == null)
                {
                    ViewBag.Error = "登入失敗!!!";
                    return View(user);
                }

                var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, userItem.Username),
                        new Claim("FullName", userItem.FullName),
                        new Claim(ClaimTypes.Role, "user")
                    };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddHours(1)
                };
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            }

            return RedirectToAction("equipQryUser", "Equips");
        }

        public async Task<IActionResult> logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("userLogin");
        }
    }
}
