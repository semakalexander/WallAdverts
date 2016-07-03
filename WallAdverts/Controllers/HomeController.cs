using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WallAdverts.Filters;
using WallAdverts.Models;

namespace WallAdverts.Controllers
{
    [Culture]
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

            return PartialView("Home", db.Adverts);
        }

        public ActionResult ChangeLanguage(string lang)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath;
            List<string> cultures = new List<string> { "uk", "ru", "en" };
            if (!cultures.Contains(lang))
            {
                lang = "uk";
            }

            HttpCookie cultureCookie = HttpContext.Request.Cookies["lang"];
            if (cultureCookie != null)
                cultureCookie.Value = lang;
            else
            {
                cultureCookie = new HttpCookie("lang");
                cultureCookie.HttpOnly = false;
                cultureCookie.Value = lang;
                cultureCookie.Expires = DateTime.Now.AddYears(1);
            }

            Response.Cookies.Add(cultureCookie);
            return Redirect(returnUrl);
        }

        public ActionResult Home()
        {
            return View(db.Adverts);
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
                return RedirectToAction("Home", "Home", db.Adverts);
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
            return RedirectToAction("Home", "Home", db.Adverts);
        }

        [HttpPost]
        public ActionResult CreateAdvert(string nameAdvert, string descriptionAdvert)
        {
            if (nameAdvert.Trim() != "" && descriptionAdvert.Trim() != "")
            {
                Advert ad = new Advert();
                ad.AuthorId = Convert.ToInt32(HttpContext.Request.Cookies["id"].Value);
                ad.AuthorName = db.Users.FirstOrDefault(u => u.Id == ad.AuthorId).Login;
                ad.DateCreate = DateTime.Now;
                ad.Description = descriptionAdvert;
                ad.Name = nameAdvert;
                ad.ImageSrc = "";
                db.Adverts.Add(ad);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    string s = "";
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        s += string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                             eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            s += string.Format("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                        StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "LogDataBase.txt");
                        sw.Write(s);
                        sw.Close();
                    }
                    throw;
                }

            }
            return PartialView("Wall", db.Adverts);
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
                HttpContext.Response.Cookies["id"].Value = user.Id.ToString();
                Session["LayoutSrc"] = "~/Views/Shared/_LayoutAuth.cshtml";
                Session["Username"] = user.Login;
                return RedirectToAction("Home", "Home", db.Adverts);
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