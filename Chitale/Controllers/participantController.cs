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
    public class ParticipantController : Controller
    {
        ChitaleDashboardRepository CDR = new ChitaleDashboardRepository();
        Exceptions newexception = new Exceptions();
        // GET: participant
        public ActionResult Index()
        {
            return View();
            GetParticipantList();
        }

        public ParticipantList GetParticipantList()
        {
            var UserSession = (CustomerDetail)Session["ChitaleUser"];
            ParticipantList objlist = new ParticipantList();
            objlist = CDR.GetParticipantList(UserSession.CustomerId, UserSession.CustomerType);
            return objlist;
        }
        public ActionResult RedumptionData()
        {
            return View();
        }
    }
}