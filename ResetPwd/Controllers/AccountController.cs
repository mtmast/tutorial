
using EmailService.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ResetPwd.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace ResetPwd.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMailService mailService;
        private static readonly Dictionary<string, DateTime> _tokenExpiry = new();
        private static readonly Dictionary<string, string> _emailTokens = new();
        public AccountController(IMailService MailService) { 
            mailService = MailService;
        }
        // GET: AccountController
        public ActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(30) // Set the cookie to expire in 30 minutes
            };

            if (string.IsNullOrEmpty(Request.Cookies["email"]))
            {
                Response.Cookies.Append("Email", "test@gmail.com", cookieOptions);
                Response.Cookies.Append("Password", "123", cookieOptions);
            }

            return View();
        }

        // POST: AccountController/Login
        [HttpPost]
        public async Task<ActionResult> Login(String Email, String password)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(password))
                {
                    ViewBag.Error = "Please Fill Data";
                    return View();
                }

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
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                    };
                    await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(ClaimsIdentity), AuthProperties);
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.Error = "Invalid Email or password";
                return View();
            }
            else
            {
                ViewBag.Error = "Please Fill Data";
                return View();
            }

        }

        // GET: AccountController/Forgot
        public ActionResult Forgot()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        // POST: AccountController/Forgot -- SendEmail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Forgot(string resetemail)
        {
            var token = Guid.NewGuid().ToString();
            DateTime expiry = DateTime.Now.AddMinutes(3);

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(resetemail))
                {
                    ViewBag.Error = "Please Fill Data -";
                    return View();
                }

                if (Request.Cookies["Email"] != resetemail)
                {
                    ViewBag.Error = "Sorry, Your Email does not register";
                    return View();
                }

                _emailTokens[resetemail] = token;
                _tokenExpiry[resetemail] = expiry;

                HTMLMailData mailData = new HTMLMailData
                {
                    EmailToId = resetemail,
                    EmailToName = "Aung Si Thu",
                };

                bool condition = mailService.SendHTMLMail(mailData, token, resetemail);

                if (condition)
                {
                    TempData["MessageType"] = "info";
                    TempData["Message"] = "Please Check Your Email";
                    TempData["Status"] = 1;
                    return View();
                }
                else
                {
                    TempData["MessageType"] = "fail";
                    TempData["Message"] = "Sorry, Service is currently not available";
                    TempData["Status"] = 1;
                    return View();
                }
                
            }
            else
            {
                ViewBag.Error = "Please Fill Data";
                return View();
            }
        }

        // GET: AccountController/Reset
        public ActionResult Reset(string token, string email)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            if(token == null || email == null)
            {
                return RedirectToAction("LinkExpired");
            }

            if (!_emailTokens.ContainsKey(email) || _emailTokens[email] != token || _tokenExpiry[email] < DateTime.Now)
            {
                return RedirectToAction("LinkExpired");
            }


            var model = new ResetPasswordViewModel
            {
                Token = token,
                ResetEmail = email,
                Expiry = _tokenExpiry[email],

            };

            return View(model);
        }

        // Reset Password
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reset(String NewPassword, String ConfirmPassword, ResetPasswordViewModel model)
        {
            string ResetEmail = model.ResetEmail ?? string.Empty;
            string Token = model.Token ?? string.Empty;

            if (!_emailTokens.ContainsKey(ResetEmail) || _emailTokens[ResetEmail] != Token || _tokenExpiry[ResetEmail] < DateTime.Now)
            {
                return RedirectToAction("LinkExpired");
            }


            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(ConfirmPassword))
                {
                    ViewBag.Error = "Please Fill Data";
                    return View(model);
                }

                if (NewPassword != ConfirmPassword)
                {
                    ViewBag.Error = "Your Password does not match";
                    return View(model);
                }

                Response.Cookies.Append("Password", NewPassword);
                _emailTokens.Remove(ResetEmail);
                _tokenExpiry.Remove(ResetEmail);

                TempData["MessageType"] = "success";
                TempData["Message"] = "Password Updated !";
                return RedirectToAction(nameof(Login));
            }
            else
            {
                ViewBag.Error = "Please Fill Data";
                return View(model);
            }

        }

        // GET : AccountController/LinkExpired
        public ActionResult LinkExpired()
        {
            return View();
        }

        // GET: AccountController/Logout
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login", "Account");
        }
    }
}
