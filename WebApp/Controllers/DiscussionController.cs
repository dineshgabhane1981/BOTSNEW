using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.ViewModel;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using System.Web.Script.Serialization;
using BOTS_BL;
using WebApp.App_Start;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using BOTS_BL.Models.CommonDB;

namespace WebApp.Controllers
{
    public class DiscussionController : Controller
    {
        DiscussionsRepository DR = new DiscussionsRepository();
        // GET: Discussion
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AllDiscussions(string groupId)
        {
            CommonFunctions common = new CommonFunctions();
            groupId = common.DecryptString(groupId);
            DiscussionViewModel objData = new DiscussionViewModel();            
            objData.lstDiscussions = DR.GetDiscussions(groupId);
            objData.lstCallTypes = DR.GetCallTypes();
            List<SelectListItem> callSubType = new List<SelectListItem>();
            SelectListItem item = new SelectListItem();
            item.Value = "0";
            item.Text = "Please Select";
            callSubType.Add(item);
            objData.lstCallSubTypes = callSubType;

            return View(objData);
        }

    }
}