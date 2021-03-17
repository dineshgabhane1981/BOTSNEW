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
    public class ManagementDashboardController : Controller
    {
        ManagementDashboardRepository MDR = new ManagementDashboardRepository();
        Exceptions newexception = new Exceptions();
        // GET: ManagementDashboard
        public ActionResult Index()
        {
            ManagementViewModel objModel = new ManagementViewModel();
            objModel.ClusterList = MDR.GetClusterList();
            objModel.SubClusterList = MDR.GetSubClusterList();
            objModel.CityList = MDR.GetCityList();
            return View(objModel);
        }
        public JsonResult GetDashboardSummeryData(string jsonData)
        {
            Dashboardsummary SummeryData = new Dashboardsummary();
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                
                foreach (Dictionary<string, object> item in objData)
                {
                    string Flag = Convert.ToString(item["Flag"]);
                    string Cluster = Convert.ToString(item["Cluster"]);
                    string SubCluster = Convert.ToString(item["SubCluster"]);
                    string City = Convert.ToString(item["City"]);
                    string FromDate = Convert.ToString(item["FromDate"]);
                    string Todate = Convert.ToString(item["Todate"]);

                    bool IsBTD = false;
                    if (Flag == "1")
                        IsBTD = true;
                    SummeryData = MDR.GetSummeryDetails(IsBTD, Cluster, SubCluster, City, FromDate, Todate);
                    SummeryData.PurchaseOrderPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(SummeryData.PurchaseOrderPoints));
                    SummeryData.SalesOrderPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(SummeryData.SalesOrderPoints));
                    SummeryData.RedeemedPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(SummeryData.RedeemedPoints));
                    SummeryData.AddOnPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(SummeryData.AddOnPoints));
                    SummeryData.LostPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(SummeryData.LostPoints));
                    SummeryData.TotalPointsBalanceStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(SummeryData.TotalPointsBalance));
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return new JsonResult() { Data = SummeryData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult Top5Participants(string type)
        {
            List<object> lstData = new List<object>();
            try
            {

                List<CustomerDetail> dataCustomerDetail = new List<CustomerDetail>();
                dataCustomerDetail = MDR.GetTop5Participant(type);
                List<string> nameList = new List<string>();
                List<decimal> dataList = new List<decimal>();
                foreach (var item in dataCustomerDetail)
                {
                    nameList.Add(item.CustomerName);
                    dataList.Add(Convert.ToDecimal(item.Points)); 
                }
                lstData.Add(nameList);
                lstData.Add(dataList);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return new JsonResult() { Data = lstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult Bottom5Participants(string type)
        {
            List<object> lstData = new List<object>();
            try
            {

                List<CustomerDetail> dataCustomerDetail = new List<CustomerDetail>();
                dataCustomerDetail = MDR.Bottom5Participants(type);
                List<string> nameList = new List<string>();
                List<decimal> dataList = new List<decimal>();
                foreach (var item in dataCustomerDetail)
                {
                    nameList.Add(item.CustomerName);
                    dataList.Add(Convert.ToDecimal(item.Points));
                }
                lstData.Add(nameList);
                lstData.Add(dataList);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return new JsonResult() { Data = lstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult Top5LostParticipants(string type)
        {
            List<object> lstData = new List<object>();
            try
            {

                List<Top5LostParticipants> objTop5Participant = new List<Top5LostParticipants>();
                objTop5Participant = MDR.GetTop5LostParticipant(type);
                List<string> nameList = new List<string>();
                List<decimal> dataList = new List<decimal>();
                foreach (var item in objTop5Participant)
                {
                    nameList.Add(item.CustomerName);
                    dataList.Add(Convert.ToDecimal(item.Points));
                }
                lstData.Add(nameList);
                lstData.Add(dataList);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return new JsonResult() { Data = lstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult ParticipantList()
        {
            return View();
        }
        public ActionResult LeaderBoard()
        {
            return View();
        }
        public ActionResult OrdertoRavanaDays()
        {
            return View();
        }
        public ActionResult InvoicetoOrder()
        {
            return View();
        }


    }
}