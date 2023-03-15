using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Repository;
using DLC.App_Start;
using DLC.ViewModel;

namespace DLC.Controllers
{
    public class StartController : Controller
    {
        DLCConfigRepository DCR = new DLCConfigRepository();
        // GET: Start
        public ActionResult Index(string data)
        {
            string brandId = string.Empty;
            string groupId = string.Empty;
            string source = string.Empty;
            CommonFunctions common = new CommonFunctions();
            if (!string.IsNullOrEmpty(data))
            {
                var parameterStr = common.DecryptString(data);
                var parameters = parameterStr.Split('&');
                foreach (var item in parameters)
                {
                    if (item.Contains("BrandId"))
                    {
                        var brandData = item.Split('=');
                        brandId = brandData[1];
                        groupId = brandId.Substring(0, 4);
                    }
                    if (item.Contains("Source"))
                    {
                        var sourceData = item.Split('=');
                        source = sourceData[1];                    
                    }
                }
            }
            DLCDashboardFrontData objData = new DLCDashboardFrontData();
            if (!string.IsNullOrEmpty(groupId))
            {
                objData.objDashboardConfig = DCR.GetPublishDLCDashboardConfig(groupId);
                objData.lstDLCFrontEndPageData = DCR.GetDLCFrontEndPageData(groupId);
                Session["HeaderColor"] = objData.objDashboardConfig.HeaderColor;
                Session["LogoUrl"] = objData.objDashboardConfig.UseLogoURL;
                Session["LogoSize"] = objData.objDashboardConfig.UseLogo;                
            }
            ViewBag.Codes = DCR.GetCountryCodes();
            return View(objData);
        }
    }
}