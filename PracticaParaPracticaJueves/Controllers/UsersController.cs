using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PracticaParaPracticaJueves.Models;

namespace PracticaParaPracticaJueves.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Resto de métodos del controlador...

        // Login methods
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([Bind("Username,Password")] User usuario, string returnUrl)
        {
            if (_context == null || _context.Users == null)
            {
                // Manejar la referencia nula aquí
                return NotFound();
            }

            usuario.Password = CalculateMD5Hash(usuario.Password);
            var user = await _context.Users.FirstOrDefaultAsync(s => s.Username == usuario.Username && s.Password == usuario.Password);
            if (user != null && user.Id >= 0 && user.Username == usuario.Username)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Rol.Name),
                    new Claim("Id", user.Id.ToString())
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = true });

                if (!string.IsNullOrWhiteSpace(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Credenciales incorrectas";
                ViewBag.ReturnUrl = returnUrl;
                return View(usuario);
            }
        }

        // Helper method to calculate MD5 hash
        private string CalculateMD5Hash(string text)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(text);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
