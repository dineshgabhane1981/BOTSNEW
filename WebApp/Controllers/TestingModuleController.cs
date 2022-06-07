using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.App_Start;
using System.Web.Script.Serialization;
using WebApp.ViewModel;
using BOTS_BL.Models.OnBoarding;
using System.Data.OleDb;
using System.Data;
using System.Net;
using System.Xml;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.IO;

namespace WebApp.Controllers
{
    public class TestingModuleController : Controller
    {
        CustomerRepository CR = new CustomerRepository();
        ReportsRepository RR = new ReportsRepository();
        TestingModuleRepository TMR = new TestingModuleRepository();
        Exceptions newexception = new Exceptions();
        // GET: TestingModule
        public ActionResult Index(string groupId)
        {
            TestingModuleViewModel TMV = new TestingModuleViewModel();
            TMV = GetXMLData();
            CommonFunctions comm = new CommonFunctions();
            groupId = comm.DecryptString(groupId);
            TMV.billingpartners = TMR.GetBillingPartners(groupId);

            var liveGroupId = TMR.GetLiveGroupId(groupId);
            var connectionString = CR.GetCustomerConnString(liveGroupId);
            TMV.lstOutlets = RR.GetOutletList(liveGroupId, connectionString);

            return View(TMV);
        }

        public TestingModuleViewModel GetXMLData()
        {
            TestingModuleViewModel objTMV = new TestingModuleViewModel();
            XmlDocument doc = new XmlDocument();
            //doc.Load("E:\\Projects\\NEWBOTS\\WebApp\\APIPackets.xml");
            var path = ConfigurationManager.AppSettings["PacketXMLPath"].ToString();
            doc.Load(path);

            //Enrollnment
            XmlNode node = doc.DocumentElement.SelectSingleNode("/packets/Enrollnment");
            objTMV.RequestPacketEnrollnment = node.InnerXml;

            //Enrollnment With Earn
            XmlNode node2 = doc.DocumentElement.SelectSingleNode("/packets/EnrolnmentWithEarn");
            objTMV.RequestPacketEnrollnmentWithEarn = node2.InnerXml;

            //Earn
            XmlNode node3 = doc.DocumentElement.SelectSingleNode("/packets/Earn");
            objTMV.RequestPacketEarn = node3.InnerXml;

            //Burn Validation
            XmlNode node4 = doc.DocumentElement.SelectSingleNode("/packets/BurnValidation");
            objTMV.RequestPacketBurnValidation = node4.InnerXml;

            //Burn
            XmlNode node5 = doc.DocumentElement.SelectSingleNode("/packets/Burn");
            objTMV.RequestPacketBurn = node5.InnerXml;

            //Cancel
            XmlNode node6 = doc.DocumentElement.SelectSingleNode("/packets/Cancel");
            objTMV.RequestPacketCancel = node6.InnerXml;

            //SendOtp
            XmlNode node7 = doc.DocumentElement.SelectSingleNode("/packets/SendOTP");
            objTMV.RequestPacketSendOTP = node7.InnerXml;


            XmlNode node1 = doc.DocumentElement.SelectSingleNode("/packets/url/EnrollnmentURL");
            objTMV.RequestURL = node1.InnerXml;

            return objTMV;
        }

        public ActionResult GetResponse(string RequestPacket, string RequestURL, string APIType)
        {
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://blueocktopus.in/NewRetailwareWebService/Service.asmx/RW");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(RequestURL);
            byte[] bytes;
            RequestPacket = "InputData=" + RequestPacket;
            //bytes = System.Text.Encoding.ASCII.GetBytes("InputData=<BOLOYALTY> <TxnType>07</TxnType> <TranSource>3</TranSource> <Datetime>23052022213707</Datetime> <CounterId>1051100101</CounterId> <BillingId>1</BillingId> <MobileNo></MobileNo> <CustomerId>2277</CustomerId> <CardNo></CardNo> <FirstName>Renaldo</FirstName> <LastName></LastName> <Gender>M</Gender> <EmailId></EmailId> <DOB></DOB> <City></City> <MaritalStatus></MaritalStatus> <SpouseName></SpouseName> <SpouseDOB></SpouseDOB> <AnniversaryDate></AnniversaryDate> <NoOfChild></NoOfChild> <Child1DOB></Child1DOB> <Child2DOB></Child2DOB> <AreaName></AreaName> <CustomerCategory></CustomerCategory> <MAC>7D5926A0E947B7E1802D406B0251A395</MAC><NewMobileNo>9003552567</NewMobileNo> </BOLOYALTY>");
            bytes = System.Text.Encoding.ASCII.GetBytes(RequestPacket);
            request.ContentType = "application/x-www-form-urlencoded";// "application/x-www-form-urlencoded";//"text/xml; encoding='utf-8'";
            request.ContentLength = bytes.Length;
            request.Method = "POST";
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();

            string responseStr = "";
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                responseStr = new StreamReader(responseStream).ReadToEnd();
            }
            return new JsonResult() { Data = responseStr, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult ResetSection(string Id)
        {
            TestingModuleViewModel TMV = new TestingModuleViewModel();
            if (Convert.ToInt32(Id) == 1)
            {
                XmlDocument doc = new XmlDocument();
                //doc.Load("E:\\Projects\\NEWBOTS\\WebApp\\APIPackets.xml");
                var path = ConfigurationManager.AppSettings["PacketXMLPath"].ToString();
                doc.Load(path);

                //Enrollnment
                XmlNode node = doc.DocumentElement.SelectSingleNode("/packets/Enrollnment");
                TMV.RequestPacketEnrollnment = node.InnerXml;
                XmlNode node1 = doc.DocumentElement.SelectSingleNode("/packets/url/EnrollnmentURL");
                TMV.RequestURL = node1.InnerXml;
            }
            if (Convert.ToInt32(Id) == 2)
            {
                XmlDocument doc = new XmlDocument();
                //doc.Load("E:\\Projects\\NEWBOTS\\WebApp\\APIPackets.xml");
                var path = ConfigurationManager.AppSettings["PacketXMLPath"].ToString();
                doc.Load(path);


                //Enrollnment With Earn
                XmlNode node2 = doc.DocumentElement.SelectSingleNode("/packets/EnrolnmentWithEarn");
                TMV.RequestPacketEnrollnmentWithEarn = node2.InnerXml;
                XmlNode node1 = doc.DocumentElement.SelectSingleNode("/packets/url/EnrollnmentURL");
                TMV.RequestURL = node1.InnerXml;
            }
            if (Convert.ToInt32(Id) == 3)
            {
                XmlDocument doc = new XmlDocument();
                //doc.Load("E:\\Projects\\NEWBOTS\\WebApp\\APIPackets.xml");
                var path = ConfigurationManager.AppSettings["PacketXMLPath"].ToString();
                doc.Load(path);

                //Earn
                XmlNode node3 = doc.DocumentElement.SelectSingleNode("/packets/Earn");
                TMV.RequestPacketEarn = node3.InnerXml;
                XmlNode node1 = doc.DocumentElement.SelectSingleNode("/packets/url/EnrollnmentURL");
                TMV.RequestURL = node1.InnerXml;
            }
            if (Convert.ToInt32(Id) == 4)
            {
                XmlDocument doc = new XmlDocument();
                //doc.Load("E:\\Projects\\NEWBOTS\\WebApp\\APIPackets.xml");
                var path = ConfigurationManager.AppSettings["PacketXMLPath"].ToString();
                doc.Load(path);

                //Burn Validation
                XmlNode node4 = doc.DocumentElement.SelectSingleNode("/packets/BurnValidation");
                TMV.RequestPacketBurnValidation = node4.InnerXml;
                XmlNode node1 = doc.DocumentElement.SelectSingleNode("/packets/url/EnrollnmentURL");
                TMV.RequestURL = node1.InnerXml;
            }
            if (Convert.ToInt32(Id) == 5)
            {
                XmlDocument doc = new XmlDocument();
                //doc.Load("E:\\Projects\\NEWBOTS\\WebApp\\APIPackets.xml");
                var path = ConfigurationManager.AppSettings["PacketXMLPath"].ToString();
                doc.Load(path);

                //Burn
                XmlNode node5 = doc.DocumentElement.SelectSingleNode("/packets/Burn");
                TMV.RequestPacketBurn = node5.InnerXml;
                XmlNode node1 = doc.DocumentElement.SelectSingleNode("/packets/url/EnrollnmentURL");
                TMV.RequestURL = node1.InnerXml;
            }
            if (Convert.ToInt32(Id) == 6)
            {
                XmlDocument doc = new XmlDocument();
                //doc.Load("E:\\Projects\\NEWBOTS\\WebApp\\APIPackets.xml");
                var path = ConfigurationManager.AppSettings["PacketXMLPath"].ToString();
                doc.Load(path);

                //Cancel
                XmlNode node6 = doc.DocumentElement.SelectSingleNode("/packets/Cancel");
                TMV.RequestPacketCancel = node6.InnerXml;
                XmlNode node1 = doc.DocumentElement.SelectSingleNode("/packets/url/EnrollnmentURL");
                TMV.RequestURL = node1.InnerXml;
            }
            if (Convert.ToInt32(Id) == 7)
            {
                XmlDocument doc = new XmlDocument();
                //doc.Load("E:\\Projects\\NEWBOTS\\WebApp\\APIPackets.xml");
                var path = ConfigurationManager.AppSettings["PacketXMLPath"].ToString();
                doc.Load(path);

                //Send Otp
                XmlNode node7 = doc.DocumentElement.SelectSingleNode("/packets/SendOTP");
                TMV.RequestPacketSendOTP = node7.InnerXml;
                XmlNode node1 = doc.DocumentElement.SelectSingleNode("/packets/url/EnrollnmentURL");
                TMV.RequestURL = node1.InnerXml;
            }

            return Json(TMV, JsonRequestBehavior.AllowGet);
        }

       
    }
}