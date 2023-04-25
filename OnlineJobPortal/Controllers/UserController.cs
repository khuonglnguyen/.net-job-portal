using OnlineJobPortal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineJobPortal.Controllers
{
    public class UserController : Controller
    {
        JobPortalDBEntities _context = new JobPortalDBEntities();
        // GET: User
        public ActionResult Profile()
        {
            var user = Session["User"] as User;
            return View(user);
        }
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
                var listCountry = _context.Countries.OrderBy(x => x.CountryName).ToList();
                ViewBag.listCountry = listCountry;
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user, int role)
        {
            var check = _context.Users.SingleOrDefault(x => x.Username == user.Username && x.Password == user.Password);
            if (check != null)
            {
                Session["User"] = check;
                if (check.RoleId == role && (role == 1 || role == 2))
                {
                    return Redirect("/admin");
                }
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

        public ActionResult EditResume()
        {
            var listCountry = _context.Countries.OrderBy(x => x.CountryName).ToList();
            ViewBag.listCountry = listCountry;
            return View();
        }

        [HttpPost]
        public ActionResult EditResume(User user, HttpPostedFileBase resume)
        {
            string fileName = "";
            if (resume != null)
            {
                //Get file name
                fileName = Path.GetFileName(resume.FileName);
                //Get path
                var path = Path.Combine(Server.MapPath("~/Resume"), fileName);
                //Check exitst
                if (!System.IO.File.Exists(path))
                {
                    //Add image into folder
                    resume.SaveAs(path);
                }
            }

            var userUpdate = _context.Users.Find(user.UserId);
            userUpdate.Username = user.Username;
            userUpdate.Name = user.Name;
            userUpdate.Email = user.Email;
            userUpdate.Mobile = user.Mobile;
            userUpdate.TenthGrade = user.TenthGrade;
            userUpdate.TwelfthGrade = user.TwelfthGrade;
            userUpdate.GraduationGrade = user.GraduationGrade;
            userUpdate.PostGraduationGrade = user.PostGraduationGrade;
            userUpdate.Phd = user.Phd;
            userUpdate.WorksOn = user.WorksOn;
            userUpdate.Experience = user.Experience;
            if (fileName != null)
            {
                userUpdate.Resume = fileName;
            }
            userUpdate.Address = user.Address;
            userUpdate.Country = user.Country;

            _context.SaveChanges();

            var listCountry = _context.Countries.OrderBy(x => x.CountryName).ToList();
            ViewBag.listCountry = listCountry;
            ViewBag.message = "Success";

            Session["User"] = userUpdate;
            return View();
        }
    }
}