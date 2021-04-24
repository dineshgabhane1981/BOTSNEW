using BOTS_BL;
using BOTS_BL.Models.ChitaleModel;
using BOTS_BL.Repository;
using Chitale.ViewModel;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
        ParticipantRepository pr = new ParticipantRepository();
        // GET: Employee
        public ActionResult Index(string CustomerId, string CustomerType)
        {
            return View();
        }
        [HttpPost]
        public JsonResult Top5Participants(string type)
        {
            List<object> lstData = new List<object>();
            try
            {
                var UserSession = (CustomerDetail)Session["ChitaleEmployee"];
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
                var UserSession = (CustomerDetail)Session["ChitaleEmployee"];
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
                var UserSession = (CustomerDetail)Session["ChitaleEmployee"];
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
                var UserSession = (CustomerDetail)Session["ChitaleEmployee"];
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
            var UserSession = (CustomerDetail)Session["ChitaleEmployee"];
            List<ParticipantListForManagement> listformgt = new List<ParticipantListForManagement>();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);

            foreach (Dictionary<string, object> item in objData)
            {
                string Flag = Convert.ToString(item["Flag"]);
                string Cluster = Convert.ToString(item["Cluster"]);
                string SubCluster = Convert.ToString(item["SubCluster"]);
                string City = Convert.ToString(item["City"]);
                string CustomerId = UserSession.CustomerId;
                string CustomerType = UserSession.CustomerType;
                listformgt = ER.GetParticipantListForEmp(Cluster, SubCluster, City,CustomerId,CustomerType);

            }
            return new JsonResult() { Data = listformgt, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };

        }
        public JsonResult GetSubParticipantListForEmp(string Id, string ParticipantType)
        {
            var UserSession = (CustomerDetail)Session["ChitaleEmployee"];
            string CustomerType = UserSession.CustomerType;
            string CustomerId = UserSession.CustomerId;
            List<ParticipantListForManagement> listformgt = new List<ParticipantListForManagement>();
            listformgt = ER.GetSubParticipantListForEmp(Id, ParticipantType, CustomerType, CustomerId);

            return new JsonResult() { Data = listformgt, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };

        }
        public ActionResult LeaderBoard()
        {
            ManagementViewModel objModel = new ManagementViewModel();
            objModel.ClusterList = MDR.GetClusterList();           
            return View(objModel);
        }
        public ActionResult OrderToInvoice()
        {
            ManagementViewModel objModel = new ManagementViewModel();
            objModel.ClusterList = MDR.GetClusterList();           
            return View(objModel);
        }
        public JsonResult GetInvoiceToOrderData(string jsonData, string CustomerType)
        {
            var UserSession = (CustomerDetail)Session["ChitaleEmployee"];           
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

        public ActionResult OrdertoRavanaDays()
        {
            ManagementViewModel objModel = new ManagementViewModel();
            objModel.ClusterList = MDR.GetClusterList();
            return View(objModel);
        }

        public JsonResult GetOrdertoRavanaDaysData(string jsonData)
        {
            List<OrderVsRavanaDay> objOrderData = new List<OrderVsRavanaDay>();
            try
            {
                var UserSession = (CustomerDetail)Session["ChitaleEmployee"];
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
                    objOrderData = ER.GetOrderVsRavanaDayData(Cluster, SubCluster, City, FromDate, Todate, type, UserSession.CustomerId, UserSession.CustomerType);

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return new JsonResult() { Data = objOrderData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult NoActionParticipants()
        {
            return View();
        }
        public JsonResult GetParticipantsTilesData()
        {
            var UserSession = (CustomerDetail)Session["ChitaleEmployee"];
            NoActionModelTile objData = new NoActionModelTile();
            objData = pr.GetNoActionParticipantsTilesData(UserSession.CustomerId, UserSession.CustomerType);
            return new JsonResult() { Data = objData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult GetParticipantsData(string type)
        {
            var UserSession = (CustomerDetail)Session["ChitaleEmployee"];
            List<NoActionParticipantData> objData = new List<NoActionParticipantData>();
            objData = pr.GetNoActionParticipantsData(type, UserSession.CustomerId, UserSession.CustomerType);
            return new JsonResult() { Data = objData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult ExportOrdertoRavanaDaysEmployee(string Cluster, string SubCluster, string City, string FromDate, string Todate, string Type)
        {
            try
            {
                var UserSession = (CustomerDetail)Session["ChitaleEmployee"];
                List<OrderVsRavanaDay> objOrderData = new List<OrderVsRavanaDay>();
                objOrderData = ER.GetOrderVsRavanaDayData(Cluster, SubCluster, City, FromDate, Todate, Type, UserSession.CustomerId, UserSession.CustomerType);

                System.Data.DataTable tableToExport = new System.Data.DataTable();
                tableToExport.Columns.Add("Type");
                tableToExport.Columns.Add("ID");
                tableToExport.Columns.Add("Name");
                tableToExport.Columns.Add("Cluster");
                tableToExport.Columns.Add("Sub Cluster");
                tableToExport.Columns.Add("City");
                tableToExport.Columns.Add("Order Count");
                tableToExport.Columns.Add("Avg Diff in Days");
                tableToExport.Columns.Add("Days Diff 1");
                tableToExport.Columns.Add("Days Diff 2");
                tableToExport.Columns.Add("Days Diff 3");
                tableToExport.Columns.Add("Days Diff 4");

                foreach (var item in objOrderData)
                {
                    DataRow dr = tableToExport.NewRow();
                    dr["Type"] = item.CustomerType;
                    dr["ID"] = item.CustomerId;
                    dr["Name"] = item.CustomerName;
                    dr["Cluster"] = item.Cluster;
                    dr["Sub Cluster"] = item.SubCluster;
                    dr["City"] = item.City;
                    dr["Order Count"] = item.OrderCount;
                    dr["Avg Diff in Days"] = item.AvgDiffInDays;
                    dr["Days Diff 1"] = item.Diff1;
                    dr["Days Diff 2"] = item.Diff2;
                    dr["Days Diff 3"] = item.Diff3;
                    dr["Days Diff 4"] = item.Diff4;
                    tableToExport.Rows.Add(dr);
                }

                string ReportName = "OrdertoRavanaDays";
                string fileName = ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    tableToExport.TableName = ReportName;
                    wb.Worksheets.Add(tableToExport);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
                return null;
            }
        }

        public ActionResult ExportNoActionParticipantEmployee(string Type)
        {
            try
            {
                var UserSession = (CustomerDetail)Session["ChitaleEmployee"];
                List<NoActionParticipantData> objData = new List<NoActionParticipantData>();
                objData = pr.GetNoActionParticipantsData(Type, UserSession.CustomerId, UserSession.CustomerType);
                System.Data.DataTable tableToExport = new System.Data.DataTable();
                tableToExport.Columns.Add("Type");
                tableToExport.Columns.Add("ID");
                tableToExport.Columns.Add("Name");
                tableToExport.Columns.Add("Cluster");
                tableToExport.Columns.Add("Sub Cluster");
                tableToExport.Columns.Add("City");
                tableToExport.Columns.Add("Last Invoice Date");
                tableToExport.Columns.Add("Balance Points");
                tableToExport.Columns.Add("Days Since Last Inv");

                foreach (var item in objData)
                {
                    DataRow dr = tableToExport.NewRow();
                    dr["Type"] = item.Type;
                    dr["ID"] = item.Id;
                    dr["Name"] = item.Name;
                    dr["Cluster"] = item.Cluster;
                    dr["Sub Cluster"] = item.SubCluster;
                    dr["City"] = item.City;
                    dr["Last Invoice Date"] = item.LastInvoiceDate;
                    dr["Balance Points"] = item.BalancePoints;
                    dr["Days Since Last Inv"] = item.DaysSinceLastInvoice;

                    tableToExport.Rows.Add(dr);
                }

                string ReportName = "NoActionParticipant";
                string fileName = ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    tableToExport.TableName = ReportName;
                    wb.Worksheets.Add(tableToExport);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
                return null;
            }
            return null;
        }

        public ActionResult Products()
        {
            return View();
        }
    }
}