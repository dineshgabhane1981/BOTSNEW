using BOTS_BL.Models;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.ViewModel;

namespace WebApp.Controllers
{
    public class BOPromoCampaignController : Controller
    {
        PromoCampaignRepository PCR = new PromoCampaignRepository();

        // GET: BOPromoCampaign
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GroupList(string groupId)
        {
            PromoCampaignViewModel objData = new PromoCampaignViewModel();
            objData.lstGroupList = PCR.GetGroupDetails();

            return View(objData);
        }

        
    }
}