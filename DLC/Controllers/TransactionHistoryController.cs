using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DLC.Controllers
{
    public class TransactionHistoryController : Controller
    {
        // GET: TransactionHistory
        public ActionResult Index()
        {
            return View();
        }
    }
}