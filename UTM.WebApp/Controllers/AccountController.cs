using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UTM.BusLogic;
using UTM.Database;
using UTM.Domain;
using UTM.WebApp.ViewModels;

namespace UTM.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly WatchStoreContext db = new WatchStoreContext();

        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.SingleOrDefault(u => u.Username == model.Username);
                if (user != null)
                {
                    string hashedPassword = PasswordHelper.HashPassword(model.Password, user.Salt);
                    if (user.PasswordHash == hashedPassword)
                    {
                        var authTicket = new FormsAuthenticationTicket(
                            1, // version
                            user.Username,
                            DateTime.Now,
                            DateTime.Now.AddMinutes(30), // expiry
                            model.RememberMe,
                            user.Role, // user roles
                            "/");

                        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                        var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                        HttpContext.Response.Cookies.Add(authCookie);

                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Invalid username or password");
            }
            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Any(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("", "Username already exists");
                }
                else
                {
                    string salt = PasswordHelper.GenerateSalt();
                    string hashedPassword = PasswordHelper.HashPassword(model.Password, salt);

                    User newUser = new User
                    {
                        Username = model.Username,
                        PasswordHash = hashedPassword,
                        Salt = salt,
                        Role = "User"
                    };

                    db.Users.Add(newUser);
                    db.SaveChanges();

                    var authTicket = new FormsAuthenticationTicket(
                        1, // version
                        newUser.Username,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(30), // expiry
                        false, // persistent
                        newUser.Role, // user roles
                        "/");

                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    HttpContext.Response.Cookies.Add(authCookie);

                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            authCookie.Expires = DateTime.Now.AddYears(-1);
            HttpContext.Response.Cookies.Add(authCookie);

            return RedirectToAction("Login", "Account");
        }
    }
}
