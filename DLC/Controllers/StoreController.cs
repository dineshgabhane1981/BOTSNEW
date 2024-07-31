using BOTS_BL.Models;
using BOTS_BL.Repository;
using DLC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;

namespace DLC.Controllers
{
    public class StoreController : Controller
    {
        CustomerRepository objCustRepo = new CustomerRepository();
        DLCConfigRepository DCR = new DLCConfigRepository();
        // GET: Store
        public ActionResult Index()
        {
            DashboardViewModel objData = new DashboardViewModel();
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            objData.objDashboardConfig = sessionVariables.objDashboardConfig;
            //objData.dLCDashboardContent = DCR.GetDLCDashboardContent(sessionVariables.GroupId, sessionVariables.MobileNo);
            return View();
        }

        [HttpGet]
        public ActionResult GetOutlets()
        {
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            List<outletNmaelist> outletNmaelists = new List<outletNmaelist>();

            string connStr = objCustRepo.GetCustomerConnString(sessionVariables.GroupId);
            using (var context = new BOTSDBContext(connStr))
            {
                var Locationlist = context.tblOutletMasters
                    .Where(x => x.BrandId == sessionVariables.BrandId && !x.OutletName.ToLower().Contains("admin") && x.GroupId == sessionVariables.GroupId && x.IsActive == true)
                    .ToList();

                foreach (var item in Locationlist)
                {
                    outletNmaelist obj = new outletNmaelist();
                    obj.OutletName = item.OutletName;
                    obj.OutletId = item.OutletId;
                    outletNmaelists.Add(obj);
                }
            }

            return Json(outletNmaelists, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetMapUrl(string outletName)
        {
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            
            string connStr = objCustRepo.GetCustomerConnString(sessionVariables.GroupId);
            using (var context = new BOTSDBContext(connStr))
            {
                var outletDetails = context.tblOutletMasters.FirstOrDefault(x => x.BrandId == sessionVariables.BrandId && x.GroupId == sessionVariables.GroupId && x.IsActive == true && x.OutletName == outletName);
                if (outletDetails != null)
                {
                    var lat = outletDetails.Latitude;
                    var lng = outletDetails.Longitude;
                    var url = $"https://maps.google.com/maps?q={lat},{lng}&z=12&output=embed";
                    var result = new
                    {
                        url = url,
                        address = outletDetails.Address,
                        ownerName = outletDetails.OwnerName,
                        contactNo = outletDetails.Phone
                    };
                    return Json(result);
                }
                else
                {
                    return Json(null);
                }

            }
            
        }

    }
}