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
        ChitaleException newexception = new ChitaleException();
        // GET: ManagementDashboard
        public ActionResult Index(string CustomerId, string CustomerType)
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

        [HttpPost]
        public JsonResult GetManagementDashboardLostOpp(string type)
        {
            List<int> dataList = new List<int>();
            List<double> percentageList = new List<double>();
            try
            {
                ManagementDashboardLostOpp objLostOpp = new ManagementDashboardLostOpp();
                objLostOpp = MDR.GetManagementDashboardLostOpp(type);
                var total = objLostOpp.LessThanTwo + objLostOpp.TwoToThree + objLostOpp.MoreThanThree;
                double LessThanTwo = (double)objLostOpp.LessThanTwo * 100 / total;
                double TwoToThree = (double)objLostOpp.TwoToThree * 100 / total;
                double MoreThanThree = (double)objLostOpp.MoreThanThree * 100 / total;

                LessThanTwo = Math.Round(LessThanTwo, 2);
                TwoToThree = Math.Round(TwoToThree, 2);
                MoreThanThree = Math.Round(MoreThanThree, 2);

                percentageList.Add(LessThanTwo);
                percentageList.Add(TwoToThree);
                percentageList.Add(MoreThanThree);

                var lstData = string.Join(" ", dataList);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return new JsonResult() { Data = percentageList, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetManagementTGTVsACHPerformance(string type)
        {
            List<object> lstData = new List<object>();
            try
            {
                List<ManagementTGTVsACHPerformance> objTGTVsACHPerformance = new List<ManagementTGTVsACHPerformance>();
                objTGTVsACHPerformance = MDR.GetManagementTGTVsACHPerformance(type);
                List<string> nameList = new List<string>();
                List<decimal> dataList = new List<decimal>();
                foreach (var item in objTGTVsACHPerformance)
                {
                    nameList.Add(item.CustomerName);
                    dataList.Add(Convert.ToDecimal(item.VolumeAchPercentage));
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
        public JsonResult GetManagementOrderToRavanaPerformance(string type)
        {
            List<object> lstData = new List<object>();
            try
            {
                List<ManagementOrderToRavanaPerformance> objdata = new List<ManagementOrderToRavanaPerformance>();
                objdata = MDR.GetManagementOrderToRavanaPerformance(type);
                List<string> nameList = new List<string>();
                List<decimal> dataList = new List<decimal>();
                foreach (var item in objdata)
                {
                    nameList.Add(item.CustomerName);
                    dataList.Add(Convert.ToDecimal(item.AvgDays));
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
            var lstCluster = MDR.GetClusterList();
            var lstCity = MDR.GetCityList();
            var lstSubcluster = MDR.GetSubClusterList();
            ViewBag.ClusterList = lstCluster;
            ViewBag.SubclusterList = lstSubcluster;
            ViewBag.CityList = lstCity;
            return View();
        }
        public JsonResult GetLeaderBoard(string jsonData)
        {
            List<LeaderBoardForMgt> listLeaderBoard = new List<LeaderBoardForMgt>();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in objData)
            {
                string radio = Convert.ToString(item["Radiobtnchk"]);

                int Cluster = Convert.ToInt32(item["Cluster"]);
                int SubCluster = Convert.ToInt32(item["SubCluster"]);
                int City = Convert.ToInt32(item["City"]);
                listLeaderBoard = MDR.GetLeaderBoardForMgts(radio, Cluster, SubCluster, City);
            }
            return new JsonResult() { Data = listLeaderBoard, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult GetParticipantListForMgt(string jsonData)
        {
            List<ParticipantListForManagement> listformgt = new List<ParticipantListForManagement>();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);

            foreach (Dictionary<string, object> item in objData)
            {
                string Flag = Convert.ToString(item["Flag"]);
                int Cluster = Convert.ToInt32(item["Cluster"]);
                int SubCluster = Convert.ToInt32(item["SubCluster"]);
                int City = Convert.ToInt32(item["City"]);
                listformgt = MDR.GetParticipantListForMgt(Cluster, SubCluster, City);

            }
            return new JsonResult() { Data = listformgt, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };

        }
        public JsonResult GetSubParticipantListForMgt(string Id, string ParticipantType)
        {
            List<ParticipantListForManagement> listformgt = new List<ParticipantListForManagement>();
            listformgt = MDR.GetSubParticipantListForMgt(Id, ParticipantType);

            return new JsonResult() { Data = listformgt, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };

        }
        public ActionResult LeaderBoard()
        {
            var lstCluster = MDR.GetClusterList();
            lstCluster.Add(new SelectListItem { Text = "All", Value = "0", Selected = true });
            var lstCity = MDR.GetCityList();
            var lstSubcluster = MDR.GetSubClusterList();
            ViewBag.ClusterList = lstCluster;
            ViewBag.SubclusterList = lstSubcluster;
            ViewBag.CityList = lstCity;
            return View();
        }
        public ActionResult OrdertoRavanaDays()
        {
            ManagementViewModel objModel = new ManagementViewModel();
            objModel.ClusterList = MDR.GetClusterList();
            objModel.SubClusterList = MDR.GetSubClusterList();
            objModel.CityList = MDR.GetCityList();
            return View(objModel);
        }
        public JsonResult GetOrdertoRavanaDaysData(string jsonData)
        {
            List<OrderVsRavanaDay> objOrderData = new List<OrderVsRavanaDay>();
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
                    string FromDate = Convert.ToString(item["FromDate"]);
                    string Todate = Convert.ToString(item["Todate"]);
                    string type = Convert.ToString(item["CustomerType"]);
                    objOrderData = MDR.GetOrderVsRavanaDayData(Cluster, SubCluster, City, type, FromDate, Todate);

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return new JsonResult() { Data = objOrderData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult InvoicetoOrder()
        {
            ManagementViewModel objModel = new ManagementViewModel();
            objModel.ClusterList = MDR.GetClusterList();
            objModel.SubClusterList = MDR.GetSubClusterList();
            objModel.CityList = MDR.GetCityList();
            return View(objModel);
        }

        public JsonResult GetInvoiceToOrderData(string jsonData)
        {
            List<InvoiceToOrder> objOrderData = new List<InvoiceToOrder>();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);

            foreach (Dictionary<string, object> item in objData)
            {
                string Cluster = Convert.ToString(item["Cluster"]);
                string SubCluster = Convert.ToString(item["SubCluster"]);
                string City = Convert.ToString(item["City"]);
                string FromDate = Convert.ToString(item["FromDate"]);
                string Todate = Convert.ToString(item["Todate"]);
                string type = Convert.ToString(item["CustomerType"]);
                objOrderData = MDR.GetInvoiceToOrderData(Cluster, SubCluster, City, type, FromDate, Todate);

            }
            return new JsonResult() { Data = objOrderData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }


    }
}