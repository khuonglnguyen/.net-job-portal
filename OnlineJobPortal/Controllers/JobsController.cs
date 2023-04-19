using OnlineJobPortal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
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
            var countries = _context.Countries.OrderBy(x => x.CountryName).ToList();
            ViewBag.countries = countries;
            return View(listJobs);
        }

        [HttpPost]
        public ActionResult Filter(string[] types = null, string country = null, string[] within = null)
        {
            IEnumerable<Job> jobs = null;
            jobs = _context.Jobs.ToList();
            if (types != null)
            {
                jobs = jobs.Where(x => types.Contains(x.JobType)).ToList();
            }

            if (country != null)
            {
                if (country != "")
                {
                    jobs = jobs.Where(x => x.Country == country).ToList();
                }
            }

            if (within != null)
            {
                DateTime date = DateTime.Now;
                if (within.Last() == "any")
                {

                }
                else if (within.Last() == "today")
                {

                }
                else if (within.Last() == "2days")
                {
                    date = date.AddDays(-2);
                }
                else if (within.Last() == "3days")
                {
                    date = date.AddDays(-3);
                }
                else if (within.Last() == "5days")
                {
                    date = date.AddDays(-5);
                }
                else if (within.Last() == "10days")
                {
                    date = date.AddDays(-10);
                }

                if (within.Last() == "today")
                {
                    jobs = jobs.Where(s => s.CreateDate.Value.Day == date.Day).ToList();
                }
                else
                {
                    var jobList = _context.Jobs.Where(s => EntityFunctions.TruncateTime(s.CreateDate) <= EntityFunctions.TruncateTime(date)).ToList();
                    if (jobList.Count > 0)
                    {
                        jobs = jobs.Where(x => jobList.Select(j => j.JobId).Contains(x.JobId)).ToList();
                    }
                    else
                    {
                        jobs = null;
                    }
                    return PartialView("JobPartial", jobs);
                }
            }

            return PartialView("JobPartial", jobs);
        }

        [HttpPost]
        public ActionResult JobCountry(string country)
        {
            if (country != "")
            {
                var listJobs = _context.Jobs.Where(x => x.Country == country).ToList();
                return PartialView("JobPartial", listJobs);
            }
            else
            {
                var listJobs = _context.Jobs.ToList();
                return PartialView("JobPartial", listJobs);
            }
        }

        [HttpPost]
        public ActionResult JobWithin(string[] within)
        {
            if (within != null)
            {
                DateTime date = DateTime.Now;
                if (within.Last() == "any")
                {

                }
                else if (within.Last() == "today")
                {

                }
                else if (within.Last() == "2days")
                {
                    date = date.AddDays(-2);
                }
                else if (within.Last() == "3days")
                {
                    date = date.AddDays(-3);
                }
                else if (within.Last() == "5days")
                {
                    date = date.AddDays(-5);
                }
                else if (within.Last() == "10days")
                {
                    date = date.AddDays(-10);
                }

                if (within.Last() == "today")
                {
                    var listJobs = _context.Jobs.Where(s => s.CreateDate.Value.Day == date.Day).ToList();
                    return PartialView("JobPartial", listJobs);
                }
                else
                {
                    var listJobs = _context.Jobs.Where(s => EntityFunctions.TruncateTime(s.CreateDate) <= EntityFunctions.TruncateTime(date)).ToList();
                    return PartialView("JobPartial", listJobs);
                }
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