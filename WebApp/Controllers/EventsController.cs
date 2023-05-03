using BOTS_BL;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.ViewModel;

namespace WebApp.Controllers
{
    public class EventsController : Controller
    {
        Exceptions newexception = new Exceptions();
        EventsRepository EVR = new EventsRepository();
        // GET: Events List
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateEvent()
        {
            return View();
        }
        public ActionResult EventReport()
        {
            return View();
        }
        public ActionResult EventCustomers()
        {
            EventViewModel objData = new EventViewModel();
            objData.lstNeverOptFor = EVR.GetNeverOptForGroups(false);
            objData.lstActive = EVR.GetNeverOptForGroups(true);
            return View(objData);
        }

    }
}