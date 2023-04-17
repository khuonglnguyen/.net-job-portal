using OnlineJobPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineJobPortal.Controllers
{
    public class UserController : Controller
    {
        JobPortalDBEntities _context = new JobPortalDBEntities();
        // GET: User
        public ActionResult Register()
        {
            var listCountry = _context.Countries.ToList();
            ViewBag.listCountry = listCountry;
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            var result = _context.Users.Add(user);
            _context.SaveChanges();
            if (result != null)
            {
                ViewBag.message = "Register Successfully!";
            }
            else
            {
                var listCountry = _context.Countries.ToList();
                ViewBag.listCountry = listCountry;
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            var check = _context.Users.SingleOrDefault(x => x.Username == user.Username && x.Password == user.Password);
            if (check != null)
            {
                Session["User"] = check;
                return Redirect("/");
            }
            ViewBag.message = "Username or Password is incorrect!";
            return View();
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            return Redirect("/User/Login");
        }
    }
}