using OnlineJobPortal.Models;
using System;
using System.Collections.Generic;
using System.IO;
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

        public ActionResult NewJobs()
        {
            var countries = _context.Countries.OrderBy(x => x.CountryName).ToList();
            return View(countries);
        }

        [HttpPost]
        public ActionResult NewJobs(Job job, HttpPostedFileBase companyImage)
        {
            //Get file name
            var fileName = Path.GetFileName(companyImage.FileName);
            //Get path
            var path = Path.Combine(Server.MapPath("~/assets/img/icon"), fileName);
            //Check exitst
            if (!System.IO.File.Exists(path))
            {
                //Add image into folder
                companyImage.SaveAs(path);
            }

            job.CreateDate = DateTime.Now;
            job.CreatedBy = (Session["User"] as User).UserId;
            job.CompanyImage = fileName;
            _context.Jobs.Add(job);
            _context.SaveChanges();

            return Redirect("/Admin/Jobs");
        }

        public ActionResult Jobs()
        {
            var list = _context.Jobs.ToList();
            return View(list);
        }


        public ActionResult JobsEdit(int id)
        {
            var job = _context.Jobs.Find(id);
            ViewBag.countries = _context.Countries.OrderBy(x => x.CountryName).ToList();
            return View(job);
        }

        [HttpPost]
        public ActionResult JobsEdit(Job job, HttpPostedFileBase companyImage)
        {
            var jobUpdate = _context.Jobs.Find(job.JobId);
            if (companyImage != null)
            {
                //Get file name
                var fileName = Path.GetFileName(companyImage.FileName);
                //Get path
                var path = Path.Combine(Server.MapPath("~/assets/img/icon"), fileName);
                //Check exitst
                if (!System.IO.File.Exists(path))
                {
                    //Add image into folder
                    companyImage.SaveAs(path);
                }

                jobUpdate.CompanyImage = fileName;
            }

            jobUpdate.Title = job.Title;
            jobUpdate.NoOfPost = job.NoOfPost;
            jobUpdate.Description = job.Description;
            jobUpdate.Qualification = job.Qualification;
            jobUpdate.Experience = job.Experience;
            jobUpdate.Specialization = job.Specialization;
            jobUpdate.LastDateToApply = job.LastDateToApply;
            jobUpdate.Salary = job.Salary;
            jobUpdate.JobType = job.JobType;
            jobUpdate.CompanyName = job.CompanyName;
            jobUpdate.Website = job.Website;
            jobUpdate.Email = job.Email;
            jobUpdate.Address = job.Address;
            jobUpdate.Country = job.Country;
            jobUpdate.State = job.State;

            _context.SaveChanges();

            return Redirect("/Admin/Jobs");
        }

        public ActionResult DeleteJobs(int id)
        {
            var job = _context.Jobs.Find(id);
            _context.Jobs.Remove(job);
            _context.SaveChanges();
            return Redirect("/Admin/Jobs");
        }
    }
}