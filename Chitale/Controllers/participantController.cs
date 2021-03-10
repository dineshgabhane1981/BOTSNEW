using System;
using System.Collections.Generic;
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
            var UserSession = (CustomerDetail)Session["ChitaleUser"];
            ParticipantList objlist = new ParticipantList();
            List<ParticipantList> lstparticipantLists = new List<ParticipantList>();
            lstparticipantLists = pr.GetParticipantList(UserSession.CustomerId, UserSession.CustomerType);

            return View(lstparticipantLists);

        }
       
        public JsonResult GetParticipantList()
        {
            var UserSession = (CustomerDetail)Session["ChitaleUser"];
            ParticipantList objlist = new ParticipantList();
            List<ParticipantList> lstparticipantLists = new List<ParticipantList>();
            lstparticipantLists = pr.GetParticipantList(UserSession.CustomerId, UserSession.CustomerType);
           
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


            //string _Url = "https://http2.myvfirst.com/smpp/sendsms?";
            //string _MobileMessage = "Dear Participant, your OTP for Chitale's redemption request is " + newOTP + " - Chitale Bandhu";
            //_MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
            //string type1 = "TEXT";
            //StringBuilder sbposdata1 = new StringBuilder();
            //sbposdata1.AppendFormat("username={0}", "blueohttpotp");
            //sbposdata1.AppendFormat("&password={0}", "bluoct87");
            //sbposdata1.AppendFormat("&to={0}", UserSession.MobileNo);
            //sbposdata1.AppendFormat("&from={0}", "CHiTLE");
            //sbposdata1.AppendFormat("&text={0}", _MobileMessage);
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            //HttpWebRequest httpWReq1 = (HttpWebRequest)WebRequest.Create(_Url);
            //UTF8Encoding encoding1 = new UTF8Encoding();
            //byte[] data1 = encoding1.GetBytes(sbposdata1.ToString());
            //httpWReq1.Method = "POST";
            //httpWReq1.ContentType = "application/x-www-form-urlencoded";
            //httpWReq1.ContentLength = data1.Length;
            //using (Stream stream1 = httpWReq1.GetRequestStream())
            //{
            //    stream1.Write(data1, 0, data1.Length);
            //}
            //HttpWebResponse response1 = (HttpWebResponse)httpWReq1.GetResponse();
            //StreamReader reader1 = new StreamReader(response1.GetResponseStream());
            //string responseString1 = reader1.ReadToEnd();
            //reader1.Close();
            //response1.Close();
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
                objRedeem.CustomerType = UserSession.CustomerType;
                objRedeem.RedemptionType = Type;
                objRedeem.Points = Points;
                objRedeem.CreatedDate = DateTime.Now;
                status = pr.RedeemptionRequest(objRedeem);
            }

            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}