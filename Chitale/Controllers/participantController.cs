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
        ParticipantRepository pr = new ParticipantRepository();
        Exceptions newexception = new Exceptions();
        // GET: participant
        public ActionResult Index()
        {
                     
            return View();

        }

        public JsonResult GetParticipantList()
        {
            var UserSession = (CustomerDetail)Session["ChitaleUser"];
            ParticipantList objlist = new ParticipantList();
            List<ParticipantList> lstparticipantLists = new List<ParticipantList>();
            lstparticipantLists = pr.GetParticipantList(UserSession.CustomerId, UserSession.CustomerType);
            return new JsonResult() { Data = lstparticipantLists, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult RedumptionData()
        {
            return View();
        }
    }
}