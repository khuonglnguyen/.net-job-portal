using OnlineJobPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace OnlineJobPortal.Controllers
{
    public class JobsController : Controller
    {
        JobPortalDBEntities _context = new JobPortalDBEntities();
        // GET: Job
        public ActionResult Index()
        {
            var listJobs = _context.Jobs.ToList();
            return View(listJobs);
        }

        [HttpPost]
        public ActionResult Index(string[] types)
        {
            if (types != null)
            {
                var listJobs = _context.Jobs.Where(x => types.Contains(x.JobType)).ToList();
                return PartialView("JobPartial", listJobs);
            }
            else
            {
                var listJobs = _context.Jobs.ToList();
                return PartialView("JobPartial", listJobs);
            }
        }

        public ActionResult Detail(int id)
        {
            var job = _context.Jobs.Find(id);
            ViewBag.message = TempData["message"] != null ? TempData["message"].ToString() : null;

            var user = Session["User"] as User != null ? Session["User"] as User : null;
            var appliedCheck = user != null ? _context.AppliedJobs.ToList().LastOrDefault(x => x.JobId == id && x.UserId == user.UserId) : null;
            ViewBag.appliedCheck = appliedCheck;
            return View(job);
        }

        public ActionResult Apply(int id)
        {
            var user = Session["User"] as User;
            if (user == null)
            {
                return Redirect("/User/Login");
            }

            var applyJob = new AppliedJob();
            applyJob.JobId = id;
            applyJob.UserId = (Session["User"] as User).UserId;

            _context.AppliedJobs.Add(applyJob);
            _context.SaveChanges();

            TempData["message"] = "Job applied successfull!";
            return RedirectToAction("Detail", new { id = id });
        }
    }
}