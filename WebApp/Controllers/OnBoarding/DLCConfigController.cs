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

namespace WebApp.Controllers.OnBoarding
{
    public class DLCConfigController : Controller
    {
        DLCConfigRepository DCR = new DLCConfigRepository();
        // GET: DLCConfig
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DashboardConfig()
        {
            tblDLCDashboardConfig objDLCDashboard = new tblDLCDashboardConfig();
            DLCDashboardViewModel objData = new DLCDashboardViewModel();
            objData.objDLCDashboard = objDLCDashboard;
            return View(objData);
        }
        public ActionResult ProfileConfig()
        {
            DLCProfileUpdate objDLCProfUpdt = new DLCProfileUpdate();
            DLCProfileUpdateViewModel objProfData = new DLCProfileUpdateViewModel();
            objProfData.objDLCProfUpdt = objDLCProfUpdt;
            return View(objProfData);
        }
        public ActionResult SaveDashboard(string jsonData)
        {
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objDashboardData = (object[])json_serializer.DeserializeObject(jsonData);
            tblDLCDashboardConfig objDashboard = new tblDLCDashboardConfig();
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

                objDashboard.LoginWithOTP = Convert.ToString(item["LoginWithOTP"]);
                objDashboard.RedirectToPage = Convert.ToString(item["RedirectToPage"]);

                objDashboard.AddPersonalDetails = Convert.ToBoolean(item["AddPersonalDetails"]);
                objDashboard.PersonalDetailsPoints = Convert.ToInt32(item["PersonalDetailsPoints"]);
                objDashboard.AddReferFriend = Convert.ToBoolean(item["AddReferFriend"]);
                objDashboard.ReferPoints = Convert.ToInt32(item["ReferPoints"]);
                objDashboard.AddGiftPoints = Convert.ToBoolean(item["AddGiftPoints"]);
                objDashboard.GiftPoints = Convert.ToInt32(item["GiftPoints"]);

                objDashboard.ExtraWidgetText = Convert.ToString(item["ExtraWidgetText"]);
                objDashboard.ExtraWidgetPoints = Convert.ToInt32(item["ExtraWidgetPoints"]);

                objDashboard.ShowLogoToFooter = Convert.ToBoolean(item["ShowFooterImage"]);
                objDashboard.CollectPersonalDataRandomly = Convert.ToBoolean(item["CollectInfoRendomly"]);
                objDashboard.AddedBy = userDetails.LoginId;
                objDashboard.AddedDate = DateTime.Now;

                status = DCR.SaveDLCDashboardConfig(userDetails.GroupId, objDashboard);
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}