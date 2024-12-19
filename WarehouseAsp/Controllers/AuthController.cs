using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WarehouseAsp.Models;


namespace WarehouseAsp.Controllers
{
    public class AuthController : Controller
    {
        public ActionResult Login(string returnUrl = "/")
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(User credentials, string returnUrl = "/")
        {
            using (AuthEntities context = new AuthEntities())
            {
                {
                    // ottengo i byte della password
                    byte[] dataBytes = Encoding.UTF8.GetBytes(credentials.Password);
                    using (SHA512 sha512 = SHA512.Create())
                    {
                        // calcolo l'hash della password
                        byte[] hashBytes = sha512.ComputeHash(dataBytes);
                        // converto l'hash in stringa
                        string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                        var result = await context.Credentials.FirstOrDefaultAsync(q => q.Username == credentials.Username && q.Password == hash);


                        if (result == null)
                        {
                            TempData["LoginError"] = 1;
                            return RedirectToAction("Login");
                        }
                        else
                        {
                            TempData.Remove("LoginError");
                            FormsAuthentication.SetAuthCookie(result.Username, false);
                            return Redirect(returnUrl);
                        }
                    }
                }
            }
        }

            [Authorize]
            public ActionResult Logout()
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }
        }
}


