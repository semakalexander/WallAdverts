using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WallAdverts.Models;

namespace WallAdverts.Controllers
{
    public class HomeController : Controller
    {
        WallAdvertsContext db = new WallAdvertsContext();

        public ActionResult Index()
        {

            if (HttpContext.Request.Cookies["id"] != null)
            {
                int cookieId;
                if (int.TryParse(HttpContext.Request.Cookies["id"].Value, out cookieId))
                {
                    var user = db.Users.FirstOrDefault(u => u.Id == cookieId);
                    if (user != null)
                    {
                        Session["LayoutSrc"] = "~/Views/Shared/_LayoutAuth.cshtml";

                        Session["Username"] = user.Login;
                    }
                    else
                        Session["LayoutSrc"] = "~/Views/Shared/_Layout.cshtml";
                }
                else
                {
                    Session["LayoutSrc"] = "~/Views/Shared/_Layout.cshtml";
                }
            }
            else
                Session["LayoutSrc"] = "~/Views/Shared/_Layout.cshtml";

            return View("Home", db.Users);
        }


        public ActionResult Home()
        {
            return View(db.Users);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Email, string Password)
        {
            var userGet = db.Users.FirstOrDefault(u => u.Email == Email && u.Password == Password);
            if (userGet != null)
            {
                HttpContext.Response.Cookies["id"].Value = userGet.Id.ToString();
                Session["LayoutSrc"] = "~/Views/Shared/_LayoutAuth.cshtml";
                Session["Username"] = userGet.Login;
                return RedirectToAction("Home", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Користувача з такими даними не знайдено";
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session["Username"] = "";
            Session["LayoutSrc"] = "~/Views/Shared/_Layout.cshtml";
            HttpContext.Response.Cookies["id"].Value = "";
            return RedirectToAction("Home", "Home");
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(User user, HttpPostedFileBase fileUpload)
        {

            if (ModelState.IsValid && fileUpload != null)
            {

                string path = AppDomain.CurrentDomain.BaseDirectory + "Images/Users/" + user.Login + Path.GetExtension(fileUpload.FileName);
                fileUpload.SaveAs(path);

                user.ImageSrc = path;
                user.DateRegister = DateTime.Now;
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Home", "Home");
            }
            else
                return View();
        }

        [HttpGet]
        public JsonResult CheckLogin(string login)
        {
            var result = db.Users.FirstOrDefault(u => u.Login == login) == null;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CheckEmail(string email)
        {
            var result = db.Users.FirstOrDefault(u => u.Email == email) == null;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}