using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using BOTS_BL;
using System.Data;
using System.Web.Script.Serialization;

namespace EReceipt.Controllers
{
    public class EReceiptController : Controller
    {
        CouponRepository CR = new CouponRepository();
        // GET: EReceipt
        public ActionResult Index()
        {
            return View();
        }
    }
}