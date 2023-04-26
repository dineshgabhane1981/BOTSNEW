using BOTS_BL;
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
        Exceptions newexception = new Exceptions();
        // GET: BOPromoCampaign
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GroupList(string groupId)
        {
            PromoCampaignViewModel objData = new PromoCampaignViewModel();
            try
            {
                objData.lstGroupList = PCR.GetGroupDetails();
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GroupList");
            }
            return View(objData);
        }

        
    }
}