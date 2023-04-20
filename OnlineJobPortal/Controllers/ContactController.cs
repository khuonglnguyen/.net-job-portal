using OnlineJobPortal.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineJobPortal.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string message, string name, string email, string subject)
        {
            EmailSender.Send(subject, email, "khuongip564gb@gmail.com", "cjwbneedakkwoxnb", message);
            return Json(new { status = true });
        }
    }
}