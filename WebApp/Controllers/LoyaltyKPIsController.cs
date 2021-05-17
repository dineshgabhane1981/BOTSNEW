using BOTS_BL.Models;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class LoyaltyKPIsController : Controller
    {
        LoyaltyKPIsRepository LKR = new LoyaltyKPIsRepository();
        // GET: LoyaltyKPIs
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BizObj()
        {
            return View();
        }

        public ActionResult LoyaltyPerformance()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            LoyaltyKPIs objLoyaltyKPIs = new LoyaltyKPIs();
            objLoyaltyKPIs = LKR.GetobjLoyaltyKPIsData(userDetails.GroupId, userDetails.connectionString);
            var Sum = objLoyaltyKPIs.Redemption + objLoyaltyKPIs.Referrals + objLoyaltyKPIs.Campaigns + objLoyaltyKPIs.SMSBlastWA + objLoyaltyKPIs.NewMWPRegistration;

            objLoyaltyKPIs.RedemptionPer = Math.Round(((Convert.ToDecimal(objLoyaltyKPIs.Redemption) / Convert.ToDecimal(Sum)) * 100),2);
            objLoyaltyKPIs.ReferralsPer = Math.Round(((Convert.ToDecimal(objLoyaltyKPIs.Referrals) / Convert.ToDecimal(Sum)) * 100), 2);
            objLoyaltyKPIs.CampaignsPer = Math.Round(((Convert.ToDecimal(objLoyaltyKPIs.Campaigns) / Convert.ToDecimal(Sum)) * 100), 2);
            objLoyaltyKPIs.SMSBlastWAPer = Math.Round(((Convert.ToDecimal(objLoyaltyKPIs.SMSBlastWA) / Convert.ToDecimal(Sum)) * 100), 2);
            objLoyaltyKPIs.NewMWPRegistrationPer = Math.Round(((Convert.ToDecimal(objLoyaltyKPIs.NewMWPRegistration) / Convert.ToDecimal(Sum)) * 100), 2);


            return View(objLoyaltyKPIs);
        }

        public ActionResult LoyaltySegments()
        {
            return View();
        }
    }
}