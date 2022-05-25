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
        Exceptions newexception = new Exceptions();
        // GET: TestingModule
        public ActionResult Index(string groupId)
        {
            TestingModuleViewModel TMV = new TestingModuleViewModel();
            var billingpartners = CR.GetBillingPartner();
            ViewBag.GroupId = groupId;
            ViewBag.BillingPartners = billingpartners;
            XmlDocument doc = new XmlDocument();
            doc.Load("C:\\Users\\91912\\source\\BOTS\\WebApp\\APIPackets.xml");
            XmlNode node = doc.DocumentElement.SelectSingleNode("/packets/Enrollnment");
            TMV.RequestPacket = node.InnerXml;

            XmlNode node1 = doc.DocumentElement.SelectSingleNode("/packets/url/EnrollnmentURL");
            TMV.RequestURL = node1.InnerXml;

            return View(TMV);
        }

        public ActionResult GetResponse(string RequestPacket, string RequestURL)
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
    }
}