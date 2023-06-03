using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Repository;
using BOTS_BL.Models;
using System.Text;
using System.Net;
using System.IO;
using BOTS_BL;

namespace Exhibition.Controllers
{
    public class InquiryController : Controller
    {
        ExhibitionRepository ER = new ExhibitionRepository();
        Exceptions newexception = new Exceptions();
        // GET: Inquiry
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SaveInquiryData(string MobileNo,string Name,string RetailName,string City,string BillingSystem)
        {
            bool status = false;
            var isExist = ER.CheckCustomerExist(MobileNo);
            if (isExist)
            {
                status = false;
            }
            else
            {
                tblExhibitionData objData = new tblExhibitionData();
                objData.ExibitionName = "UGJIS";
                objData.MobileNo = MobileNo.Trim();
                objData.ProspectName = Name.Trim();
                objData.ShopName = RetailName.Trim();
                objData.City = City.Trim();
                objData.BillingSystem = BillingSystem;
                objData.AddedDate = DateTime.Now;
                objData.WelcomeScript = GetWAScript(1, Name);

                objData.InactiveScript= GetWAScript(2, Name);
                var InactiveScriptDate = DateTime.Now.AddDays(1);
                objData.InactiveScriptDate = new DateTime(InactiveScriptDate.Year, InactiveScriptDate.Month, InactiveScriptDate.Day, 05, 30, 00); 
                
                objData.FestiveCampaignScript = GetWAScript(3, Name);
                objData.FestiveCampaignScriptDate = new DateTime(InactiveScriptDate.Year, InactiveScriptDate.Month, InactiveScriptDate.Day, 11, 30, 00);

                var DLCinformationDate = DateTime.Now.AddDays(2);
                objData.DLCinformationScript = GetWAScript(4, Name);
                objData.DLCinformationScriptDate= new DateTime(DLCinformationDate.Year, DLCinformationDate.Month, DLCinformationDate.Day, 05, 30, 00);

                var PointExpiryDate = DateTime.Now.AddDays(3);
                objData.PointExpiryScript = GetWAScript(5, Name);
                objData.PointExpiryScriptDate= new DateTime(PointExpiryDate.Year, PointExpiryDate.Month, PointExpiryDate.Day, 05, 30, 00);

                status = ER.AddExhibitionData(objData);
            }

            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public string GetWAScript(int type, string Name)
        {
            StringBuilder stb = new StringBuilder();
            string WAScript = string.Empty;
            if (type == 1)
            {
                WAScript = "✨Thank you for visiting *Blue Ocktopus* Stall in *UGJIS*";
                stb.AppendLine(WAScript);
                stb.AppendLine();
                stb.AppendLine("*Sample Loyalty Program Message*");
                stb.AppendLine();
                stb.AppendLine("🔶Dear *" + Name.Trim() + "*, Congratulations on receiving 200 Loyalty Points for your purchase today. Invoice no. is 13627. Your total Loyalty points are 350. We look forward to serving you again.");
                stb.AppendLine();
                stb.AppendLine("नमस्कार आपण *ब्लू ऑक्टोपस* च्या स्टॉलला भेट दिल्याबद्दल धन्यवाद.");
                stb.AppendLine();
                stb.AppendLine("🔶प्रिय *" + Name.Trim() + "*, अभिनंदन, आपणाला आजच्या खरेदीवर 200 लॉयल्टी पॉईंटस मिळाले आहेत. तुमचा बिल नं. 13627 आहे. आपले एकूण पॉईंट्स आहेत 350.");
                stb.AppendLine();
                stb.AppendLine("आम्हाला सेवेची पुन्हा संधी द्या,");
                stb.AppendLine();
                stb.AppendLine("✅आमच्या नव्या Offers आणि Latest updates साठी नंबर सेव करून ठेवा.");
                stb.AppendLine("✴️ *NOTE*:_This Massage is customizable as per requirement_");
                stb.AppendLine();
                stb.AppendLine("*Blue Ocktopus Team*");               
                stb.AppendLine("More information contact us *9766647094*");                
                stb.AppendLine("Or");                
                stb.AppendLine("click on link http://bitly.ws/G4WY");
                
            }
            if (type == 2)
            {
                WAScript = "*Sample Loyalty Program Message*";
                stb.AppendLine(WAScript);
                stb.AppendLine();
                stb.AppendLine("*Targeting your Inactive Customer*");
                stb.AppendLine();
                stb.AppendLine("🔶Dear *" + Name.Trim() + "*, It’s been a while since we've not seen you. You've 750 loyalty points worth Rs. 750. Come and redeem your favourite product, before your points expire.");
                stb.AppendLine();
                stb.AppendLine("नमस्कार *" + Name.Trim() + "*, आपणास खूप दिवस झाले आलेले नाहीत. आपल्या जवळ 750  लॉयलटी ज्याची किंमत रु. 750 आहे. कृपया भेट देऊन आपली  कॅशबॅक ची वैधता संपण्याआधी खरेदीसाठी वापर करा.");
                stb.AppendLine();
                stb.AppendLine("*Blue Ocktopus Team*");
                stb.AppendLine("More information contact us *9766647094*");
                stb.AppendLine("Or");
                stb.AppendLine("click on link http://bitly.ws/G4WY");
            }
            if (type == 3)
            {
                WAScript = "*Sample Loyalty Festive Campaign Message*";
                stb.AppendLine(WAScript);
                stb.AppendLine();
                stb.AppendLine("!! Happy Festival from ABC Jewellers!!");
                stb.AppendLine();
                stb.AppendLine("We are happy to give you a Festive gift of 500 bonus points. Come & Redeem them during festive season before 30-Nov for Antique, Temple, Peshwai, Maharashtrian, South Jewellery and Bridal Jewellery Sets.");
                stb.AppendLine();
                stb.AppendLine("कळवण्यास आनंद होत आहे कि आपणास या सणासाठी रु 500 चे पॉईंट्स देत आहोत. आपण याचा वापर 30-Nov-2023 च्या आगोदर Antique, Temple, Peshwai, Maharashtrian, South Jewellery and Bridal Jewellery खरेदीसाठी करू शकता.");
                stb.AppendLine();
                stb.AppendLine("*Blue Ocktopus Team*");
                stb.AppendLine("More information contact us *9766647094*");
                stb.AppendLine("Or");
                stb.AppendLine("click on link http://bitly.ws/G4WY");
            }
            if (type == 4)
            {
                WAScript = "*Sample Loyalty Program Message about Digital Loyalty Card*";
                stb.AppendLine(WAScript);
                stb.AppendLine();
                stb.AppendLine("(_Digital Loyalty link - this will help to business for generate referrals , profile update , store location, Points Gift and more_)");
                stb.AppendLine();
                stb.AppendLine("🔶Dear *" + Name.Trim() + "*,  Earn more loyalty points by updating your profile and referring friends and family.");
                stb.AppendLine();
                stb.AppendLine("नमस्कार *" + Name.Trim() + "*, आपली प्रोफाइल भरून आणि आपल्या मित्रपरिवाराला आमच्या इथे खरेदी साठी रेफर करा आणि जास्तीचे पॉईंट्स मिळावा.");
                stb.AppendLine();
                stb.AppendLine("*Blue Ocktopus Team*");
                stb.AppendLine("More information contact us *9766647094*");
                stb.AppendLine("Or");
                stb.AppendLine("click on link http://bitly.ws/G4WY");
            }
            if (type == 5)
            {
                WAScript = "*Sample Loyalty Program Message*";
                stb.AppendLine(WAScript);
                stb.AppendLine();
                stb.AppendLine("*Points Expity*");
                stb.AppendLine();
                stb.AppendLine("🔶Dear *" + Name.Trim() + "*, Your 750 Loyalty points are expiring on 15-June-2023. Don't let your hard-earned points go waste. Happy Shopping !!!");
                stb.AppendLine();
                stb.AppendLine("नमस्कार *" + Name.Trim() + "*, ब्लू ऑक्टोपस लॉयल्टी प्रोग्राम मध्ये आपले 750  लॉयलटी ज्याची किंमत रु. 750 आहे व त्याची अत्तिम तारीख 15-June-2023 आहे, आपण मिळवलेल्या पॉईंटची वैधता संपण्याआधी खरेदीसाठी वापर करा.");
                stb.AppendLine();
                stb.AppendLine("*Blue Ocktopus Team*");
                stb.AppendLine("More information contact us *9766647094*");
                stb.AppendLine("Or");
                stb.AppendLine("click on link http://bitly.ws/G4WY");
            }
            WAScript = stb.ToString();
            return WAScript;
        }
        public void SendScheduledMsgs()
        {
            var data = ER.GetExhibitionData();
            var inactiveData = data.Where(x => x.InactiveScriptSentDate == null && x.InactiveScriptDate < DateTime.Now).ToList();
            SendInactiveMsg(inactiveData);

            var festiveCampaignData = data.Where(x => x.FestiveCampaignSentDate == null && x.FestiveCampaignScriptDate < DateTime.Now).ToList();
            SendFestiveCampaignMsg(festiveCampaignData);

            var DLCInformationData = data.Where(x => x.DLCinformationSentDate == null && x.DLCinformationScriptDate < DateTime.Now).ToList();
            SendDLCInformationMsg(DLCInformationData);

            var PointExpiryData = data.Where(x => x.PointExpirySentDate == null && x.PointExpiryScriptDate < DateTime.Now).ToList();
            SendPointExpiryMsg(PointExpiryData);
        }
        public void SendInactiveMsg(List<tblExhibitionData> data)
        {
            foreach(var item in data)
            {
                var status = SendWAMessage(item.InactiveScript, item.MobileNo);
                if(status)
                {
                    item.InactiveScriptSentDate = DateTime.Now;
                    ER.UpdateDateTime(item);
                }
            }
        }
        public void SendFestiveCampaignMsg(List<tblExhibitionData> data)
        {
            foreach (var item in data)
            {
                var status = SendWAMessage(item.FestiveCampaignScript, item.MobileNo);
                if (status)
                {
                    item.FestiveCampaignSentDate = DateTime.Now;
                    ER.UpdateDateTime(item);
                }
            }
        }
        public void SendDLCInformationMsg(List<tblExhibitionData> data)
        {
            foreach (var item in data)
            {
                var status = SendWAMessage(item.DLCinformationScript, item.MobileNo);
                if (status)
                {
                    item.DLCinformationSentDate = DateTime.Now;
                    ER.UpdateDateTime(item);
                }
            }
        }
        public void SendPointExpiryMsg(List<tblExhibitionData> data)
        {
            foreach (var item in data)
            {
                var status = SendWAMessage(item.PointExpiryScript, item.MobileNo);
                if (status)
                {
                    item.PointExpirySentDate = DateTime.Now;
                    ER.UpdateDateTime(item);
                }
            }
        }
        public bool SendWAMessage(string msg, string MobileNo)
        {
            bool status = false;
            try
            {
                string responseString;
               
                StringBuilder sbposdata = new StringBuilder();
                sbposdata.AppendFormat("https://bo.enotify.app/api/sendText?");
                sbposdata.AppendFormat("token={0}", "5fc8ed623629423c01ce4221");
                sbposdata.AppendFormat("&phone={0}", "91" + MobileNo);
                
                sbposdata.AppendFormat("&message={0}", msg);

                string Url = sbposdata.ToString();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(Url);
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(sbposdata.ToString());
                httpWReq.Method = "POST";

                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;
                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                responseString = reader.ReadToEnd();
                reader.Close();
                response.Close();
                status = true;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SendWAMessage");
            }
            return status;
        }

        public ActionResult InquiryDataCount()
        {
            var Count = ER.InquiryCount();
            ViewBag.Count = Count;
           return View(Count);
        }

    }
}