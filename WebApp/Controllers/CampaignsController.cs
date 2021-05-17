using BOTS_BL.Models;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class CampaignsController : Controller
    {
        CampaignRepository CMPR = new CampaignRepository();
        // GET: Campaigns
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Campaign()
        {
            CampaignTiles objCampaignTiles = new CampaignTiles();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            objCampaignTiles = CMPR.GetCampaignTilesData(userDetails.GroupId, userDetails.connectionString);
            List<SelectListItem> MonthList = new List<SelectListItem>();
           
            for (int i = 0; i < 12; i++)
            {
                MonthList.Add(new SelectListItem
                {
                    Text = Convert.ToString(DateTime.Now.AddMonths(i).ToString("MMM")),
                    Value = Convert.ToString(DateTime.Now.AddMonths(i).Month)
                });               
            }
            List<SelectListItem> YearList = new List<SelectListItem>();
            int year = DateTime.Now.Year;
            for (int i = 0; i <= 9; i++)
            {
                YearList.Add(new SelectListItem
                {
                    Text = Convert.ToString(DateTime.Now.AddYears(i).Year.ToString()),
                    Value = Convert.ToString(year + i)
                });
            }

            objCampaignTiles.lstMonth = MonthList;
            objCampaignTiles.lstYear = YearList;

            return View(objCampaignTiles);
        }

        [HttpPost]
        public JsonResult GetCampaignCelebrationsData(string month, string year)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignCelebrations> objCampaignCelebrations = new List<CampaignCelebrations>();
            objCampaignCelebrations = CMPR.GetCampaignCelebrationsData(userDetails.GroupId, userDetails.connectionString, month, year);
            return new JsonResult() { Data = objCampaignCelebrations, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult GetCampaignCelebrationsSecondData(string month, string year, string type)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignCelebrationsData> objCampaignCelebrationsData = new List<CampaignCelebrationsData>();
            objCampaignCelebrationsData = CMPR.GetCampaignCelebrationsSecondData(userDetails.GroupId, userDetails.connectionString, month, year,type);
            return new JsonResult() { Data = objCampaignCelebrationsData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetCampaignPointsExpiryData(string month, string year)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignPointsExpiry> objCampaignPointsExpiry = new List<CampaignPointsExpiry>();
            objCampaignPointsExpiry = CMPR.GetCampaignPointsExpiryData(userDetails.GroupId, userDetails.connectionString, month, year);
            return new JsonResult() { Data = objCampaignPointsExpiry, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult GetCampaignPointsExpirySecondData(string month, string year)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignCelebrationsData> objCampaignCelebrationsData = new List<CampaignCelebrationsData>();
            objCampaignCelebrationsData = CMPR.GetCampaignPointsExpirySecondData(userDetails.GroupId, userDetails.connectionString, month, year);
            return new JsonResult() { Data = objCampaignCelebrationsData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetCampaignInactiveData(string month, string year)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignInactive> objCampaignInactive = new List<CampaignInactive>();
            objCampaignInactive = CMPR.GetCampaignInactiveData(userDetails.GroupId, userDetails.connectionString, month, year);
            return new JsonResult() { Data = objCampaignInactive, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult GetCampaignInactiveSecondData(string month, string year)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignInactiveData> objCampaignInactiveData = new List<CampaignInactiveData>();
            objCampaignInactiveData = CMPR.GetCampaignInactiveSecondData(userDetails.GroupId, userDetails.connectionString, month, year);
            return new JsonResult() { Data = objCampaignInactiveData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetCampaignFirstData(string month, string year)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<Campaign> objCampaignData = new List<Campaign>();
            objCampaignData = CMPR.GetCampaignFirstData(userDetails.GroupId, userDetails.connectionString, month, year);
            return new JsonResult() { Data = objCampaignData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetCampaignSMSBlastFirstData(string month, string year)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<CampaignSMSBlastFirst> objCampaignSMSBlastFirstData = new List<CampaignSMSBlastFirst>();
            objCampaignSMSBlastFirstData = CMPR.GetCampaignSMSBlastFirstData(userDetails.GroupId, userDetails.connectionString, month, year);
            return new JsonResult() { Data = objCampaignSMSBlastFirstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}