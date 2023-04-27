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
        OnBoardingRepository OBR = new OnBoardingRepository();
        CustomerRepository CR = new CustomerRepository();
        ReportsRepository RR = new ReportsRepository();
        TestingModuleRepository TMR = new TestingModuleRepository();
        Exceptions newexception = new Exceptions();
        // GET: TestingModule
        public ActionResult Index(string groupId)
        {
            TestingModuleViewModel TMV = new TestingModuleViewModel();
            try
            {
                TMV = GetXMLData();
                ViewBag.GroupId = groupId;
                CommonFunctions comm = new CommonFunctions();
                groupId = comm.DecryptString(groupId);
                TMV.billingpartners = TMR.GetBillingPartners(groupId);

                List<SelectListItem> lstData = new List<SelectListItem>();
                TMV.BPProduct = lstData;

                var liveGroupId = TMR.GetLiveGroupId(groupId);
                var connectionString = CR.GetCustomerConnString(liveGroupId);
                TMV.lstOutlets = RR.GetOutletList(liveGroupId, connectionString);
                Session["groupId"] = liveGroupId;

                //PointRules
                TMV.objEarnRuleConfig = OBR.GetEarnRuleConfig(groupId);
                TMV.objBurnRuleConfig = OBR.GetBurnRuleConfig(groupId);
                TMV.lstSlabConfig = OBR.GetEarnRuleSlabConfig(groupId);
                TMV.lstProductUpload = OBR.GetProductUpload(groupId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "Index");
            }
            return View(TMV);
        }

        public TestingModuleViewModel GetXMLData()
        {
            TestingModuleViewModel objTMV = new TestingModuleViewModel();
            try
            {
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
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetXMLData");
            }
            return objTMV;
        }

        public string GetURLData(string BPProduct)
        {
            string ReturnURL = string.Empty;
            TestingModuleViewModel objurldata = new TestingModuleViewModel();
            try
            {
                XmlDocument doc = new XmlDocument();
                //doc.Load("E:\\Projects\\NEWBOTS\\WebApp\\APIPackets.xml");
                var path = ConfigurationManager.AppSettings["PacketXMLPath"].ToString();
                doc.Load(path);

                if (BPProduct == "RetailWare")
                {
                    XmlNode url = doc.DocumentElement.SelectSingleNode("/packets/url/RetailWareURL");
                    ReturnURL = url.InnerXml;
                }
                if (BPProduct == "11")
                {
                    XmlNode url = doc.DocumentElement.SelectSingleNode("/packets/url/LNBURL");
                    ReturnURL = url.InnerXml;
                }

                if (BPProduct == "10")
                {
                    XmlNode url = doc.DocumentElement.SelectSingleNode("/packets/url/AccurateURL");
                    ReturnURL = url.InnerXml;
                }

                if (BPProduct == "1")
                {
                    XmlNode url = doc.DocumentElement.SelectSingleNode("/packets/url/TallyURL");
                    ReturnURL = url.InnerXml;
                }

                if (BPProduct == "7")
                {
                    XmlNode url = doc.DocumentElement.SelectSingleNode("/packets/url/AcmeInfinityURL");
                    ReturnURL = url.InnerXml;
                }

                if (BPProduct == "9")
                {
                    XmlNode url = doc.DocumentElement.SelectSingleNode("/packets/url/AcmeInsightURL");
                    ReturnURL = url.InnerXml;
                }

                if (BPProduct == "8")
                {
                    XmlNode url = doc.DocumentElement.SelectSingleNode("/packets/url/AcmePadmaURL");
                    ReturnURL = url.InnerXml;
                }

                if (BPProduct == "RetailPro")
                {
                    XmlNode url = doc.DocumentElement.SelectSingleNode("/packets/url/RetailProURL");
                    ReturnURL = url.InnerXml;
                }

                if (BPProduct == "2")
                {
                    XmlNode url = doc.DocumentElement.SelectSingleNode("/packets/url/RWProURL");
                    ReturnURL = url.InnerXml;
                }

                if (BPProduct == "4")
                {
                    XmlNode url = doc.DocumentElement.SelectSingleNode("/packets/url/RWJewelSoftURL");
                    ReturnURL = url.InnerXml;
                }

                if (BPProduct == "5")
                {
                    XmlNode url = doc.DocumentElement.SelectSingleNode("/packets/url/RPJewelPlusURL");
                    ReturnURL = url.InnerXml;
                }

                if (BPProduct == "13")
                {
                    XmlNode url = doc.DocumentElement.SelectSingleNode("/packets/url/RetailwareBasicURL");
                    ReturnURL = url.InnerXml;
                }

                if (BPProduct == "18")
                {
                    XmlNode url = doc.DocumentElement.SelectSingleNode("/packets/url/OctaXSURL");
                    ReturnURL = url.InnerXml;
                }

                if (BPProduct == "19")
                {
                    XmlNode url = doc.DocumentElement.SelectSingleNode("/packets/url/RWBasicURL");
                    ReturnURL = url.InnerXml;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetURLData");
            }
            return ReturnURL;

        }

        public ActionResult GetResponse(string RequestPacket, string APIType, string RequestURL)
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
            SaveAPIData(Convert.ToString(Session["groupId"]), APIType, RequestPacket, RequestURL, responseStr);
            return new JsonResult() { Data = responseStr, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult ResetSection(string Id)
        {
            TestingModuleViewModel TMV = new TestingModuleViewModel();
            try
            {
                if (Convert.ToInt32(Id) == 1)
                {
                    XmlDocument doc = new XmlDocument();
                    //doc.Load("E:\\Projects\\NEWBOTS\\WebApp\\APIPackets.xml");
                    var path = ConfigurationManager.AppSettings["PacketXMLPath"].ToString();
                    doc.Load(path);

                    //Enrollnment
                    XmlNode node = doc.DocumentElement.SelectSingleNode("/packets/Enrollnment");
                    TMV.RequestPacketEnrollnment = node.InnerXml;

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

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ResetSection");
            }
            return Json(TMV, JsonRequestBehavior.AllowGet);
        }

        public void SaveAPIData(string GroupId, string APIType, string RequestPacket, string RequestURL, string ResponsePacket)
        {

            var userDetails = (CustomerLoginDetail)Session["UserSession"];

            GroupTestingLog objgroupTesting = new GroupTestingLog();
            try
            {
                objgroupTesting.GroupId = GroupId;
                objgroupTesting.APIType = APIType;
                objgroupTesting.RequestPacket = RequestPacket;
                objgroupTesting.RequestURL = RequestURL;
                objgroupTesting.ResponsePacket = ResponsePacket;

                objgroupTesting.AddedBy = userDetails.LoginId;
                objgroupTesting.AddedDate = DateTime.Now;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveAPIData");
            }
            var result = TMR.SaveAPIData(objgroupTesting);

        }
    }
}