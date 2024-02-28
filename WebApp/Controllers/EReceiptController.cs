using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using BOTS_BL;
using System.Data;
using System.Web.Script.Serialization;

namespace WebApp.Controllers
{
    public class EReceiptController : Controller
    {
        EReceiptRepository ERR = new EReceiptRepository();
        // GET: EReceipt
        public ActionResult Index()
        {
            string invoiceNo = "TMB-Dec23-022909";
            var objData = ERR.GetEReceiptJSON(invoiceNo, "1002");
            return View();
        }
    }
}