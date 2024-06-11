using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.ViewModel;
using System.Web.Script.Serialization;
using System.IO;
using System.Configuration;
using BOTS_BL;
using System.Data;
using Newtonsoft.Json;
using BOTS_BL.Repository;
using WebApp.App_Start;
using System.Net;
using Newtonsoft.Json.Linq;
using QRCoder;
using System.Drawing;

namespace WebApp.Controllers.OnBoarding
{
    public class DLCConfigController : Controller
    {
        DLCConfigRepository DCR = new DLCConfigRepository();
        CustomerRepository CR = new CustomerRepository();
        CommonFunctions common = new CommonFunctions();
        Exceptions newexception = new Exceptions();
        // GET: DLCConfig
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DashboardConfig()
        {
            tblDLCDashboardConfig objDLCDashboard = new tblDLCDashboardConfig();
            DLCDashboardViewModel objData = new DLCDashboardViewModel();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            
            try
            {
                objDLCDashboard = DCR.GetDLCDashboardConfig(userDetails.GroupId);
                objData.objDLCDashboard = objDLCDashboard;
                objData.lstCodes= DCR.GetCountryCodes();

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "DashboardConfig");
            }
            return View(objData);
        }
        public ActionResult ProfileConfig()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<tblDLCProfileUpdateConfig> lstProfileData = new List<tblDLCProfileUpdateConfig>();
            DLCProfileUpdateViewModel objProfileVS = new DLCProfileUpdateViewModel();
            try
            {
                lstProfileData = DCR.GetProfileData(userDetails.GroupId);
                ViewBag.Mandatory = objProfileVS.MandatoryOrNot();
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ProfileConfig");
            }
            return View(lstProfileData);
        }
        public ActionResult SaveDashboard(string jsonData)
        {
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objDashboardData = (object[])json_serializer.DeserializeObject(jsonData);
            tblDLCDashboardConfig objDashboard = new tblDLCDashboardConfig();
            try
            {
                foreach (Dictionary<string, object> item in objDashboardData)
                {
                    string smallBase64String = Convert.ToString(item["SmallImageBase64"]);
                    if (!string.IsNullOrEmpty(smallBase64String))
                    {
                        byte[] newBytes = Convert.FromBase64String(smallBase64String);
                        MemoryStream ms = new MemoryStream(newBytes, 0, newBytes.Length);
                        ms.Write(newBytes, 0, newBytes.Length);
                        var fileName = Convert.ToString(userDetails.GroupId + "Small.jpg");
                        var path = Server.MapPath("~/DLCImages/" + fileName);
                        FileStream fileNew = new FileStream(path, FileMode.Create, FileAccess.Write);
                        ms.WriteTo(fileNew);
                        fileNew.Close();
                        ms.Close();
                    }
                    string mediumBase64String = Convert.ToString(item["MediumImageBase64"]);
                    if (!string.IsNullOrEmpty(smallBase64String))
                    {
                        byte[] newBytes = Convert.FromBase64String(mediumBase64String);
                        MemoryStream ms = new MemoryStream(newBytes, 0, newBytes.Length);
                        ms.Write(newBytes, 0, newBytes.Length);
                        var fileName = Convert.ToString(userDetails.GroupId + "Medium.jpg");
                        var path = Server.MapPath("~/DLCImages/" + fileName);
                        FileStream fileNew = new FileStream(path, FileMode.Create, FileAccess.Write);
                        ms.WriteTo(fileNew);
                        fileNew.Close();
                        ms.Close();
                    }
                    string bigBase64String = Convert.ToString(item["BigImageBase64"]);
                    if (!string.IsNullOrEmpty(smallBase64String))
                    {
                        byte[] newBytes = Convert.FromBase64String(bigBase64String);
                        MemoryStream ms = new MemoryStream(newBytes, 0, newBytes.Length);
                        ms.Write(newBytes, 0, newBytes.Length);
                        var fileName = Convert.ToString(userDetails.GroupId + "Big.jpg");
                        var path = Server.MapPath("~/DLCImages/" + fileName);
                        FileStream fileNew = new FileStream(path, FileMode.Create, FileAccess.Write);
                        ms.WriteTo(fileNew);
                        fileNew.Close();
                        ms.Close();
                    }
                    objDashboard.UseLogo = Convert.ToString(item["UseLogoSize"]);
                    var baseLogoPath = ConfigurationManager.AppSettings["DLCLogoBaseURL"].ToString();
                    if (objDashboard.UseLogo == "Small")
                        objDashboard.UseLogoURL = baseLogoPath + Convert.ToString(userDetails.GroupId + "Small.jpg");
                    if (objDashboard.UseLogo == "Medium")
                        objDashboard.UseLogoURL = baseLogoPath + Convert.ToString(userDetails.GroupId + "Medium.jpg");
                    if (objDashboard.UseLogo == "Big")
                        objDashboard.UseLogoURL = baseLogoPath + Convert.ToString(userDetails.GroupId + "Big.jpg");

                    objDashboard.UseCard = Convert.ToString(item["UseCard"]);
                    var BaseUrl = ConfigurationManager.AppSettings["BaseUrl"].ToString();

                    if (objDashboard.UseCard == "One")
                        objDashboard.UseCardURL = BaseUrl + "/Content/assets/Card1.jpg";
                    if (objDashboard.UseCard == "Two")
                        objDashboard.UseCardURL = BaseUrl + "/Content/assets/Card2.jpg";
                    if (objDashboard.UseCard == "Three")
                        objDashboard.UseCardURL = BaseUrl + "/Content/assets/Card3.png";

                    objDashboard.LoginWithOTP = Convert.ToString(item["LoginWithOTP"]);
                    objDashboard.RedirectToPage = Convert.ToString(item["RedirectToPage"]);

                    objDashboard.AddPersonalDetails = Convert.ToBoolean(item["AddPersonalDetails"]);
                    objDashboard.PersonalDetailsPoints = Convert.ToInt32(item["PersonalDetailsPoints"]);
                    objDashboard.AddReferFriend = Convert.ToBoolean(item["AddReferFriend"]);
                    objDashboard.ReferPoints = Convert.ToInt32(item["ReferPoints"]);
                    objDashboard.AddGiftPoints = Convert.ToBoolean(item["AddGiftPoints"]);
                    objDashboard.GiftPoints = Convert.ToInt32(item["GiftPoints"]);

                    objDashboard.IsExtraWidgetText1 = Convert.ToBoolean(item["IsExtraWidgetText1"]);
                    objDashboard.IsExtraWidgetText2 = Convert.ToBoolean(item["IsExtraWidgetText2"]);
                    objDashboard.IsExtraWidgetText3 = Convert.ToBoolean(item["IsExtraWidgetText3"]);

                    objDashboard.ExtraWidgetText1 = Convert.ToString(item["ExtraWidgetText1"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["ExtraWidgetPoints1"])))
                        objDashboard.ExtraWidgetPoints1 = Convert.ToInt32(item["ExtraWidgetPoints1"]);

                    objDashboard.ExtraWidgetText2 = Convert.ToString(item["ExtraWidgetText2"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["ExtraWidgetPoints2"])))
                        objDashboard.ExtraWidgetPoints2 = Convert.ToInt32(item["ExtraWidgetPoints2"]);

                    objDashboard.ExtraWidgetText3 = Convert.ToString(item["ExtraWidgetText3"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["ExtraWidgetPoints3"])))
                        objDashboard.ExtraWidgetPoints3 = Convert.ToInt32(item["ExtraWidgetPoints3"]);

                    objDashboard.ShowLogoToFooter = Convert.ToBoolean(item["ShowFooterImage"]);
                    objDashboard.CollectPersonalDataRandomly = Convert.ToBoolean(item["CollectInfoRendomly"]);
                    objDashboard.HeaderColor = Convert.ToString(item["HeaderColor"]);
                    objDashboard.FontColor = Convert.ToString(item["FontColor"]);
                    objDashboard.PrefferedLanguage = Convert.ToString(item["PrefferedLanguage"]);
                    objDashboard.CountryCode = Convert.ToString(item["CountryCode"]);
                    objDashboard.SlNo = Convert.ToInt32(item["SlNo"]);
                    objDashboard.AddedBy = userDetails.LoginId;
                    objDashboard.AddedDate = DateTime.Now;

                    status = DCR.SaveDLCDashboardConfig(userDetails.GroupId, objDashboard);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveDashboard");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult GetDashboardConfigDetails()
        {
            tblDLCDashboardConfig objDashboard = new tblDLCDashboardConfig();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            objDashboard = DCR.GetDLCDashboardConfig(userDetails.GroupId);
            try
            {
                var baseLogoPath = ConfigurationManager.AppSettings["DLCLogoBaseURL"].ToString();
                objDashboard.LogoFile1 = baseLogoPath + Convert.ToString(userDetails.GroupId + "Small.jpg");
                objDashboard.LogoFile2 = baseLogoPath + Convert.ToString(userDetails.GroupId + "Medium.jpg");
                objDashboard.LogoFile3 = baseLogoPath + Convert.ToString(userDetails.GroupId + "Big.jpg");
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDashboardConfigDetails");
            }
            return new JsonResult() { Data = objDashboard, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult SaveProfileUpdate(string jsonData)
        {
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objDashboardData = (object[])json_serializer.DeserializeObject(jsonData);
            //DataTable table = JsonConvert.DeserializeObject<DataTable>(jsonData);
            List<tblDLCProfileUpdateConfig> lstProfileData = new List<tblDLCProfileUpdateConfig>();
            try
            {
                foreach (Dictionary<string, object> item in objDashboardData)
                {
                    tblDLCProfileUpdateConfig objItem = new tblDLCProfileUpdateConfig();
                    objItem.Slno= Convert.ToInt64(item["Slno"]);
                    objItem.IsDisplay = Convert.ToBoolean(item["IsDisplay"]);
                    objItem.IsMandatory = Convert.ToBoolean(item["IsMandatory"]);
                    objItem.FieldName = Convert.ToString(item["Name"]);
                    lstProfileData.Add(objItem);

                    //status = DCR.ProfileDataInsert(userDetails.GroupId, Name, NameMandStat, Gender, GenderMandStat, BirthDate, BirthMandStat, Marrital, MargMandStat, Area, AreaMandStat, City, CityMandStat, Pincode, PinMandStat, Email, MailMandStat);
                }
                status = DCR.UpdateProfileData(userDetails.GroupId, lstProfileData);

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveProfileUpdate");
            }

            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };

        }
    
        public ActionResult PublishDLCDashboardConfig()
        {
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                status = DCR.PublishDLCDashboardConfig(userDetails);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "PublishDLCDashboardConfig");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult PublishProfileUpdate()
        {
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                status = DCR.PublishDLCProfileUpdate(userDetails.GroupId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "PublishProfileUpdate");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult GenerateDLCLink()
        {
            DLCLinksViewModel objData = new DLCLinksViewModel();
            List<tblDLCCampaignMaster> lstLinks = new List<tblDLCCampaignMaster>();
            var BaseUrl= ConfigurationManager.AppSettings["baseDLCUrl"];            
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
           
            try
            {
                objData.lstLinks = DCR.GetDlcLinks(userDetails.connectionString);
                objData.lstBrands = DCR.GetBrandsByGroupId(userDetails.GroupId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GenerateDLCLink");
            }
            return View(objData);
        }

        public ActionResult CreateDLCLink(string SourceName, string StartDate,string EndDate,string BrandId)
        {
            bool status = false;
            string result_00003;
            var BaseUrl = ConfigurationManager.AppSettings["baseDLCUrl"];
            var userDetails = (CustomerLoginDetail)Session["UserSession"];

            string encryptStr = "BrandId=" + BrandId;
            encryptStr += "&Source=" + SourceName;
            string entoken = common.EncryptString(encryptStr);
            var DLCLink = BaseUrl + "?data=" + entoken;            

            string _Url = "https://api-ssl.bitly.com/v4/shorten";
            var httpWebRequest_00003 = (HttpWebRequest)WebRequest.Create(_Url);
            httpWebRequest_00003.ContentType = "application/json";
            httpWebRequest_00003.Headers.Add("Authorization", "f22c9274b5565860b85d1e4af701d4d6a4c795fa");

            httpWebRequest_00003.Method = "POST";

            using (var streamWriter_00003 = new StreamWriter(httpWebRequest_00003.GetRequestStream()))
            {

                string json_00003 =
                                "{\"long_url\":\"" + DLCLink + "\"," +
                                "\"domain\":\"bit.ly\"," +
                                "\"group_guid\":\"\"}";
                streamWriter_00003.Write(json_00003);
            }

            var httpResponse_00003 = (HttpWebResponse)httpWebRequest_00003.GetResponse();
            using (var streamReader_00003 = new StreamReader(httpResponse_00003.GetResponseStream()))
            {
                result_00003 = streamReader_00003.ReadToEnd();
            }

            JObject jsonObj = JObject.Parse(result_00003);
            var Balance = JObject.Parse(result_00003)["id"];
            string itemshortUrl = (string)Balance;

            status = DCR.SaveDLCLink(userDetails.connectionString, SourceName, itemshortUrl, StartDate, EndDate);
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult DownloadDLCQRCode(string DLCName)
        {
            string imageUrl = string.Empty;
            string failedMessage = string.Empty;
            var qrCodeImage = GenerateAndDownloadQR(DLCName);
            if (qrCodeImage == null)
            {
                failedMessage = "Failed to generate QR code image.";
            }
            if (qrCodeImage != null)
            {
                var filePath = SaveQrCodeImage(qrCodeImage, DLCName);
                if (string.IsNullOrEmpty(filePath))
                {
                    failedMessage = "Failed to save QR code image.";
                }
                imageUrl = Url.Content("~/Downloads/GeneratedQRcodeImage/" + Path.GetFileName(filePath));
            }
            if (!string.IsNullOrEmpty(failedMessage))
                return Json(new { success = false, message = failedMessage }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { success = true, imageUrl }, JsonRequestBehavior.AllowGet);
        }
        public Bitmap GenerateAndDownloadQR(string DLCName)
        {
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                var DlcDetails= DCR.GetDlcLinkByName(userDetails.connectionString, DLCName);
                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                {
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(DlcDetails.DLCLink, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);
                    return qrCodeImage;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GenerateQrCodeImage");
                return null;
            }
        }
        private string SaveQrCodeImage(Bitmap qrCodeImage, string DLCName)
        {
            try
            {
                var folder = Server.MapPath("~/Downloads/GeneratedQRcodeImage");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string filePath = Path.Combine(folder, $"QRCode_{DLCName}.png");
                qrCodeImage.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                return filePath;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveQrCodeImage");
                return null;
            }
        }

    }
}