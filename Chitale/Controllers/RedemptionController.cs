using BOTS_BL;
using BOTS_BL.Models.ChitaleModel;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Chitale.Controllers
{
    public class RedemptionController : Controller
    {
        RedemptionRepository RR = new RedemptionRepository();
        Exceptions newexception = new Exceptions();
        // GET: Redemption
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RedeemedList()
        {
            return View();
        }
        public ActionResult RedemptionValues()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetRedeemptionData(string type)
        {
            RedemptionValue objData = new RedemptionValue();
            objData = RR.GetRedeemptionData(type);
            return new JsonResult() { Data = objData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult GenerateOTP(string jsonData)
        {
            List<TgtVsAchParticipantWise> objParticipantData = new List<TgtVsAchParticipantWise>();
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                foreach (Dictionary<string, object> item in objData)
                {
                    string Type = Convert.ToString(item["Type"]);
                    string CashIncentive = Convert.ToString(item["CashIncentive"]);
                    string Infrastructure = Convert.ToString(item["Infrastructure"]);
                    string Deposit = Convert.ToString(item["Deposit"]);
                    string Promotion = Convert.ToString(item["Promotion"]);

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }

            return new JsonResult() { Data = objParticipantData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };

        }
    }
}