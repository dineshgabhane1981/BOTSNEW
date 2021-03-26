using BOTS_BL;
using BOTS_BL.Models.ChitaleModel;
using BOTS_BL.Repository;
using Chitale.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Chitale.Controllers
{
    public class EmployeeController : Controller
    {
        ManagementDashboardRepository MDR = new ManagementDashboardRepository();
        ChitaleException newexception = new ChitaleException();
        EmployeeRepository ER = new EmployeeRepository();
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Top5Participants(string type)
        {
            List<object> lstData = new List<object>();
            try
            {
                var UserSession = (CustomerDetail)Session["ChitaleUser"];
                List<CustomerDetail> dataCustomerDetail = new List<CustomerDetail>();
                dataCustomerDetail = ER.GetTop5Participant(type, UserSession.CustomerId, UserSession.CustomerType);
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
                var UserSession = (CustomerDetail)Session["ChitaleUser"];
                List<CustomerDetail> dataCustomerDetail = new List<CustomerDetail>();
                dataCustomerDetail = ER.Bottom5Participants(type, UserSession.CustomerId, UserSession.CustomerType);
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
        public JsonResult GetTop5LostOppsParticipant(string type)
        {
            List<object> lstData = new List<object>();
            try
            {
                var UserSession = (CustomerDetail)Session["ChitaleUser"];
                List<Top5LostParticipants> dataLostOpps = new List<Top5LostParticipants>();
                dataLostOpps = ER.GetTop5LostOppsParticipant(type, UserSession.CustomerId, UserSession.CustomerType);
                List<string> nameList = new List<string>();
                List<decimal> dataList = new List<decimal>();
                foreach (var item in dataLostOpps)
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
        public JsonResult GetTop5TgtVsAchPerformanceEmp(string type)
        {
            List<object> lstData = new List<object>();
            try
            {
                var UserSession = (CustomerDetail)Session["ChitaleUser"];
                List<Top5TgtVsAchPerformanceEmp> dataTop5TgtVsAch = new List<Top5TgtVsAchPerformanceEmp>();
                dataTop5TgtVsAch = ER.GetTop5TgtVsAchPerformanceEmp(type, UserSession.CustomerId, UserSession.CustomerType);
                List<string> nameList = new List<string>();
                List<decimal> dataList = new List<decimal>();
                foreach (var item in dataTop5TgtVsAch)
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
        public JsonResult GetParticipantListForEmp(string jsonData)
        {
            var UserSession = (CustomerDetail)Session["ChitaleUser"];
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
                string CustomerId = UserSession.CustomerId;
                string CustomerType = UserSession.CustomerType;
                listformgt = ER.GetParticipantListForEmp(Cluster, SubCluster, City,CustomerId,CustomerType);

            }
            return new JsonResult() { Data = listformgt, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };

        }
        //public JsonResult GetSubParticipantListForEmp(string Id, string ParticipantType)
        //{
        //    List<ParticipantListForManagement> listformgt = new List<ParticipantListForManagement>();
        //    listformgt = ER.GetSubParticipantListForEmp(Id, ParticipantType);

        //    return new JsonResult() { Data = listformgt, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };

        //}
        public ActionResult LeaderBoard()
        {

            ManagementViewModel objModel = new ManagementViewModel();
            objModel.ClusterList = MDR.GetClusterList();
            objModel.SubClusterList = MDR.GetSubClusterList();
            objModel.CityList = MDR.GetCityList();
            return View(objModel);
        }
        public ActionResult OrderToInvoice()
        {
            ManagementViewModel objModel = new ManagementViewModel();
            objModel.ClusterList = MDR.GetClusterList();
            objModel.SubClusterList = MDR.GetSubClusterList();
            objModel.CityList = MDR.GetCityList();
            return View(objModel);
        }
        public JsonResult GetInvoiceToOrderData(string jsonData, string CustomerType)
        {
            var UserSession = (CustomerDetail)Session["ChitaleUser"];           
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
                objOrderData = ER.GetInvoiceToOrderData(Cluster, SubCluster, City, type, FromDate, Todate, UserSession.CustomerId, UserSession.CustomerType);

            }
            return new JsonResult() { Data = objOrderData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}