using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BOTS_BL;
using BOTS_BL.Models.ChitaleModel;
using BOTS_BL.Repository;
using Chitale.ViewModel;
using ClosedXML.Excel;

namespace Chitale.Controllers
{
    public class ParticipantController : Controller
    {
        ChitaleDashboardRepository CDR = new ChitaleDashboardRepository();
        ParticipantRepository pr = new ParticipantRepository();
        Exceptions newexception = new Exceptions();
        ManagementDashboardRepository MDR = new ManagementDashboardRepository();
        NoActionRepository NAR = new NoActionRepository();
        // GET: participant
        public ActionResult Index()
        {
            var UserSession = (CustomerDetail)Session["ChitaleUser"];
            ParticipantList objlist = new ParticipantList();
            List<ParticipantList> lstparticipantLists = new List<ParticipantList>();
            lstparticipantLists = pr.GetParticipantList(UserSession.CustomerId, UserSession.Type);

            return View(lstparticipantLists);

        }
       
        public JsonResult GetParticipantList()
        {
            var UserSession = (CustomerDetail)Session["ChitaleUser"];
            ParticipantList objlist = new ParticipantList();
            List<ParticipantList> lstparticipantLists = new List<ParticipantList>();
            lstparticipantLists = pr.GetParticipantList(UserSession.CustomerId, UserSession.Type);
           
            return new JsonResult() { Data = lstparticipantLists, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult GetNestedParticipantList(string customerId,string CustomerType)
        {
            //var UserSession = (CustomerDetail)Session["ChitaleUser"];
            ParticipantList objlist = new ParticipantList();
            List<ParticipantList> lstparticipantLists = new List<ParticipantList>();

           lstparticipantLists = pr.GetParticipantList(customerId, CustomerType);
            
            return new JsonResult() { Data = lstparticipantLists, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult RedumptionData()
        {
            var UserSession = (CustomerDetail)Session["ChitaleUser"];
            RedemptionModel objData = new RedemptionModel();
            objData = pr.GetRedemptionData(UserSession.CustomerId);
            return View(objData);
        }
        public JsonResult GenerateOTP(string OutletId)
        {
            bool status = false;
            Random r = new Random();
            int randNum = r.Next(1000);
            string newOTP = randNum.ToString("D4");
            var UserSession = (CustomerDetail)Session["ChitaleUser"];
            OTPMaintenance objOTP = new OTPMaintenance();
            objOTP.MobileNo = UserSession.MobileNo;
            objOTP.Datetime = DateTime.Now;
            objOTP.CounterId = null;
            objOTP.OTP = newOTP;
            status = pr.GenerateOTP(objOTP);


            string _Url = "https://http2.myvfirst.com/smpp/sendsms?";
            string _MobileMessage = "Dear Participant, your OTP for Chitale's redemption request is " + newOTP + " - Chitale Bandhu";
            _MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
            string type1 = "TEXT";
            StringBuilder sbposdata1 = new StringBuilder();
            sbposdata1.AppendFormat("username={0}", "blueohttpotp");
            sbposdata1.AppendFormat("&password={0}", "bluoct87");
            sbposdata1.AppendFormat("&to={0}", UserSession.MobileNo);
            sbposdata1.AppendFormat("&from={0}", "CHiTLE");
            sbposdata1.AppendFormat("&text={0}", _MobileMessage);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            HttpWebRequest httpWReq1 = (HttpWebRequest)WebRequest.Create(_Url);
            UTF8Encoding encoding1 = new UTF8Encoding();
            byte[] data1 = encoding1.GetBytes(sbposdata1.ToString());
            httpWReq1.Method = "POST";
            httpWReq1.ContentType = "application/x-www-form-urlencoded";
            httpWReq1.ContentLength = data1.Length;
            using (Stream stream1 = httpWReq1.GetRequestStream())
            {
                stream1.Write(data1, 0, data1.Length);
            }
            HttpWebResponse response1 = (HttpWebResponse)httpWReq1.GetResponse();
            StreamReader reader1 = new StreamReader(response1.GetResponseStream());
            string responseString1 = reader1.ReadToEnd();
            reader1.Close();
            response1.Close();
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult RedemptionRequest(string Type, string Points, string OTP)
        {
            bool status = false;
            var UserSession = (CustomerDetail)Session["ChitaleUser"];

            var existingOTP = pr.GetOTP(UserSession.MobileNo);
            if (existingOTP == OTP)
            {
                tblRedemptionRequest objRedeem = new tblRedemptionRequest();
                objRedeem.CustomerId = UserSession.CustomerId;
                objRedeem.CustomerType = UserSession.Type;
                objRedeem.RedemptionType = Type;
                objRedeem.Points = Points;
                objRedeem.CreatedDate = DateTime.Now;
                status = pr.RedeemptionRequest(objRedeem);
            }

            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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
                var UserSession = (CustomerDetail)Session["ChitaleUser"];
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
                    objOrderData = pr.GetOrderVsRavanaDayData(Cluster, SubCluster, City, FromDate, Todate, UserSession.CustomerId);

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
            var UserSession = (CustomerDetail)Session["ChitaleUser"];
            NoActionModelTile objData = new NoActionModelTile();
            objData = pr.GetNoActionParticipantsTilesData(UserSession.CustomerId, UserSession.CustomerType);
            return new JsonResult() { Data = objData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult GetParticipantsData(string type)
        {
            var UserSession = (CustomerDetail)Session["ChitaleUser"];
            List<NoActionParticipantData> objData = new List<NoActionParticipantData>();
            objData = pr.GetNoActionParticipantsData(type, UserSession.CustomerId, UserSession.CustomerType);
            return new JsonResult() { Data = objData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult ExportOrdertoRavanaDaysParticipant(string Cluster, string SubCluster, string City, string FromDate, string Todate, string Type)
        {
            try
            {
                var UserSession = (CustomerDetail)Session["ChitaleUser"];
                List<OrderVsRavanaDay> objOrderData = new List<OrderVsRavanaDay>();
                objOrderData = pr.GetOrderVsRavanaDayData(Cluster, SubCluster, City, FromDate, Todate, UserSession.CustomerId);

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

        public ActionResult ExportNoActionParticipantParticipant(string Type)
        {
            try
            {
                var UserSession = (CustomerDetail)Session["ChitaleUser"];
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