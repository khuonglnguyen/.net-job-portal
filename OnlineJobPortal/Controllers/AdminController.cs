using OnlineJobPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineJobPortal.Controllers
{
    public class AdminController : Controller
    {
        JobPortalDBEntities _context = new JobPortalDBEntities();
        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.TotalUsers = _context.Users.Count();
            ViewBag.TotalJobs = _context.Jobs.Count();
            ViewBag.AppliedJobs = _context.AppliedJobs.Count();
            ViewBag.ContactedUsers = _context.Contacts.Count();
            return View();
        }
    }
}