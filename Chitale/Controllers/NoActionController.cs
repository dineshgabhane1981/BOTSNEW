using BOTS_BL.Models.ChitaleModel;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chitale.Controllers
{
    public class NoActionController : Controller
    {
        NoActionRepository NAR = new NoActionRepository();
        // GET: NoAction
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Participants()
        {
            return View();
        }
        public JsonResult GetParticipantsTilesData()
        {
            var UserSession = (CustomerDetail)Session["ChitaleManagement"];
            NoActionModelTile objData = new NoActionModelTile();
            objData = NAR.GetNoActionParticipantsTilesData(UserSession.CustomerId, UserSession.CustomerType);
            return new JsonResult() { Data = objData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult GetParticipantsData(string type)
        {
            var UserSession = (CustomerDetail)Session["ChitaleManagement"];
            List<NoActionParticipantData> objData = new List<NoActionParticipantData>();
            objData = NAR.GetNoActionParticipantsData(type, UserSession.CustomerId, UserSession.CustomerType);
            return new JsonResult() { Data = objData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult Products()
        {
            return View();
        }
    }
}