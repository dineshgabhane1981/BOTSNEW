using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Feedback.Controllers
{
    public class AabharBonusController : Controller
    {
        Exceptions newexception = new Exceptions();
        FeedBackRepository FBR = new FeedBackRepository();// GET: Feedback
        // GET: AabharBonus
        public ActionResult Index(string groupid)
        {
            return View();
        }
    }
}