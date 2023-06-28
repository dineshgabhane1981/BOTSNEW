using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using DLC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DLC.Controllers
{
    public class TransactionHistoryController : Controller
    {
        DLCConfigRepository DCR = new DLCConfigRepository();
        Exceptions newexception = new Exceptions();
        ReportsRepository RR = new ReportsRepository();
        // GET: TransactionHistory
        public ActionResult Index()
        {
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            var lstData = RR.GetDLCMeamberTransactionHistory(sessionVariables.GroupId, sessionVariables.MobileNo);
            return View(lstData);
        }
    }
}