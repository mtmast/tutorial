using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace AuthCookie.Controllers
{
    public class AccountController : Controller
    {
        // GET: AccountController
        public ActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            Response.Cookies.Append("Email", "test@gmail.com");
            Response.Cookies.Append("Password", "123");
            return View();
        }

        // POST: AccountController/Login
        [HttpPost]
        public async Task<ActionResult> Login(String Email, String password)
        {
            string CookieEmail = Request.Cookies["Email"] ?? String.Empty;
            string CookiePassword = Request.Cookies["Password"] ?? String.Empty;
            if (Email == CookieEmail && password == CookiePassword)
            {
                List<Claim> Claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Email)
                };
                var ClaimsIdentity = new ClaimsIdentity(Claims, "CookieAuth");
                var AuthProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };
                await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(ClaimsIdentity), AuthProperties);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "Invalid Email or password";
            return View();
        }

        // GET: AccountController/Logout
        public async Task<ActionResult> Logout()
        {
            Response.Cookies.Delete("Email");
            Response.Cookies.Delete("Password");
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login", "Account");
        }
    }
}
