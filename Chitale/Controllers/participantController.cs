using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL;
using BOTS_BL.Models.ChitaleModel;
using BOTS_BL.Repository;

namespace Chitale.Controllers
{
    public class participantController : Controller
    {
        ChitaleDashboardRepository CDR = new ChitaleDashboardRepository();
        Exceptions newexception = new Exceptions();
        // GET: participant
        public ActionResult Index()
        {
            return View();
        }

       
    }
}