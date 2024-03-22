using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using BOTS_BL;
using System.Data;
using System.Web.Script.Serialization;
using WebApp.App_Start;

namespace WebApp.Controllers
{
    public class EReceiptController : Controller
    {
        EReceiptRepository ERR = new EReceiptRepository();
        CommonFunctions common = new CommonFunctions();
        // GET: EReceipt
        public ActionResult Index(string data)
        {
            string invoiceNo = string.Empty;
            string groupId = string.Empty;
            if (!string.IsNullOrEmpty(data))
            {
                var parameterStr = common.DecryptString(data);
                var parameters = parameterStr.Split('&');
                foreach (var item in parameters)
                {
                    if (item.Contains("groupId"))
                    {
                        var groupIdParam = item.Split('=');
                        groupId = groupIdParam[1];
                    }
                    if (item.Contains("invoiceNo"))
                    {
                        var invoiceNoParam = item.Split('=');
                        invoiceNo = invoiceNoParam[1];
                    }
                }
            }
                    
            var objData = ERR.GetEReceiptJSON(invoiceNo, groupId);
            return View(objData);
        }
    }
}