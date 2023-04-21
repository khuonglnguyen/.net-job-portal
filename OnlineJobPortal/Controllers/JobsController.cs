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
            var user = Session["User"] as User != null ? Session["User"] as User : null;
            if (user != null)
            {
                var favouriteJobs = _context.FavouriteJobs.Where(x => x.UserId == user.UserId).ToList();
                ViewBag.favouriteJobs = favouriteJobs;
            }

            if (Session["Suggest"] != null)
            {
                listJobs = listJobs.Where(x => x.Country == user.Country).ToList();
            }

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
                DateTime date = DateTime.Now.Date;
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
                    var jobList = _context.Jobs.Where(s => EntityFunctions.TruncateTime(s.CreateDate) >= EntityFunctions.TruncateTime(date)).ToList();
                    if (jobList.Count > 0)
                    {
                        jobs = jobs.Where(x => jobList.Select(j => j.JobId).Contains(x.JobId)).ToList();
                    }
                    else
                    {
                        jobs = null;
                    }
                }
            }

            var user = Session["User"] as User != null ? Session["User"] as User : null;
            if (user != null)
            {
                var favouriteJobs = _context.FavouriteJobs.Where(x => x.UserId == user.UserId).ToList();
                TempData["favouriteJobs"] = favouriteJobs;
            }

            if (Session["Suggest"] != null)
            {
                jobs = jobs.Where(x => x.Country == user.Country).ToList();
            }

            return PartialView("JobPartial", jobs);
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

        public ActionResult AddFavouriteJob(int id)
        {
            var user = Session["User"] as User;
            var check = _context.FavouriteJobs.SingleOrDefault(x => x.UserId == user.UserId && x.JobId == id);
            if (check == null)
            {
                FavouriteJob favourite = new FavouriteJob();
                favourite.UserId = user.UserId;
                favourite.JobId = id;
                _context.FavouriteJobs.Add(favourite);
                _context.SaveChanges();
            }
            else
            {
                _context.FavouriteJobs.Remove(check);
                _context.SaveChanges();
            }
            return Redirect("/Jobs");
        }

        public ActionResult Suggest()
        {
            if (Session["Suggest"] != null)
            {
                Session["Suggest"] = null;
            }
            else
            {
                Session["Suggest"] = true;
            }
            return Redirect("/Jobs");
        }
    }
}