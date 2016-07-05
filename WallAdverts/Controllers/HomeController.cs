using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
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
            Session["LayoutSrc"] = "~/Views/Shared/_Layout.cshtml";
            int cookieId;
            if (HttpContext.Request.Cookies["id"] != null)
            {
                if (int.TryParse(HttpContext.Request.Cookies["id"].Value, out cookieId))
                {
                    var user = db.Users.FirstOrDefault(u => u.Id == cookieId);
                    if (user != null)
                    {
                        Session["LayoutSrc"] = "~/Views/Shared/_LayoutAuth.cshtml";
                        Session["Username"] = user.Login;
                        return PartialView("HomeAuth", db.Adverts.OrderByDescending(m => m.DateCreate).ToPagedList(1, 10));
                    }
                }
            }
            return PartialView("Home", db.Adverts.OrderByDescending(m => m.DateCreate).ToPagedList(1, 10));

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

        public ActionResult Home(int page = 1)
        {
            if (Session["LayoutSrc"].ToString() != "~/Views/Shared/_LayoutAuth.cshtml")
                return View(db.Adverts.OrderByDescending(m => m.DateCreate).ToPagedList(page, 10));
            else
                return View("HomeAuth", db.Adverts.OrderByDescending(m => m.DateCreate).ToPagedList(page, 10));
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
                ad.Description = descriptionAdvert.Replace("_/_/NEWLINE_/_/", "\n");
                ad.Name = nameAdvert;
                int idMax = 0;
                if (db.Adverts.ToList().Count > 0)
                    idMax = db.Adverts.Max(item => item.Id) + 1;


                if (Request.Files.Count > 0)
                {
                    var name = "/Images/Adverts/" + ad.AuthorName + idMax + Path.GetExtension(Request.Files[0].FileName);
                    Request.Files[0].SaveAs(Server.MapPath("~" + name));
                    ad.ImageSrc = name;
                }
                else
                    ad.ImageSrc = "/Images/Adverts/Default.jpg";

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
            //  return Json(db.Adverts,"text/html", JsonRequestBehavior.AllowGet);
            return PartialView("Wall", db.Adverts.OrderByDescending(m => m.DateCreate).ToPagedList(1, 10));
        }


        public ActionResult FilterByDate(string dateFrom, string dateTo)
        {
            DateTime dateF = new DateTime();
            if (dateFrom == "null")
                dateF = db.Adverts.Min(advert => advert.DateCreate);
            else
                DateTime.TryParseExact(dateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateF);

            DateTime dateT = new DateTime();
            if (dateTo == "null")
                dateT = db.Adverts.Max(advert => advert.DateCreate);
            else
                DateTime.TryParseExact(dateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateT);

            var list = db.Adverts.Where(advert => advert.DateCreate > dateF && advert.DateCreate < dateT)
                .OrderByDescending(advert => advert.DateCreate);
            return PartialView("Wall",list               
                .ToPagedList(1, 10));
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
                string path = AppDomain.CurrentDomain.BaseDirectory + "Images\\Users\\" + user.Login + Path.GetExtension(fileUpload.FileName);
                fileUpload.SaveAs(path);
                if (user.Number == null)
                    user.Number = "";
                user.ImageSrc = "\\Images\\Users\\" + user.Login + Path.GetExtension(fileUpload.FileName);
                user.DateRegister = DateTime.Now;
                user.Role = "User";
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

        public ActionResult ShowProfile(int id)
        {
            User user = db.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return HttpNotFound();
            var adverts = db.Adverts.Where(a => a.AuthorId == id).ToList();
            MyProfile profile = new MyProfile() { User = user, Adverts = adverts };
            return View(profile);
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