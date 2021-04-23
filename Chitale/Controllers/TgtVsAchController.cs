using BOTS_BL;
using BOTS_BL.Models.ChitaleModel;
using BOTS_BL.Repository;
using Chitale.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Chitale.Controllers
{
    public class TgtVsAchController : Controller
    {
        ManagementDashboardRepository MDR = new ManagementDashboardRepository();
        TgtVsAchRepository TAR = new TgtVsAchRepository();
        ChitaleException newexception = new ChitaleException();
        // GET: TgtVsAch
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Participantwise()
        {
            ManagementViewModel objModel = new ManagementViewModel();
            objModel.ClusterList = MDR.GetClusterList();          
            string[] names = DateTimeFormatInfo.CurrentInfo.MonthNames;
            List<SelectListItem> MonthItems = new List<SelectListItem>();
            int Month = 1;
            foreach (var item in names)
            {
                MonthItems.Add(new SelectListItem
                {
                    Text = Convert.ToString(item),
                    Value = Convert.ToString(Month)
                });
                Month++;
            }
            MonthItems.RemoveAt(12);
            objModel.MonthsList = MonthItems;
            List<SelectListItem> YearItems = new List<SelectListItem>();
            for (int i = 0; i <= 10; i++)
            {
                int year = DateTime.Now.Year;
                YearItems.Add(new SelectListItem
                {
                    Text = Convert.ToString(year - i),
                    Value = Convert.ToString(year - i)
                });
            }
            objModel.YearList = YearItems;

            return View(objModel);
        }

        [HttpPost]
        public JsonResult GetParticipantData(string jsonData)
        {
            List<TgtVsAchParticipantWise> objParticipantData = new List<TgtVsAchParticipantWise>();
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                foreach (Dictionary<string, object> item in objData)
                {
                    string Cluster = Convert.ToString(item["Cluster"]);
                    string SubCluster = Convert.ToString(item["SubCluster"]);
                    string City = Convert.ToString(item["City"]);
                    string Month = Convert.ToString(item["Month"]);
                    string Year = Convert.ToString(item["Year"]);
                    string CustomerType = Convert.ToString(item["CustomerType"]);
                    objParticipantData = TAR.GetTgtVsAchParticipantWise(Cluster, SubCluster, City, Month, Year, CustomerType);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }

            return new JsonResult() { Data = objParticipantData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }


        public ActionResult Productwise()
        {

            return View();
        }
        
    }
}