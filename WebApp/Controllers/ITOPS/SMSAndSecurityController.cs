using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.App_Start;

namespace WebApp.Controllers.ITOPS
{
    public class SMSAndSecurityController : Controller
    {
        ITOpsRepository ITOPS = new ITOpsRepository();
        ReportsRepository RR = new ReportsRepository();
        CustomerRepository objCustRepo = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        // GET: SMSAndSecurity
        public ActionResult Index(string groupId)
        {
            try
            {
                CommonFunctions common = new CommonFunctions();
                groupId = common.DecryptString(groupId);
                string connStr = objCustRepo.GetCustomerConnString(groupId);
                var lstOutlet = RR.GetOutletList(groupId, connStr);
                var lstBrand = RR.GetBrandList(groupId, connStr);
                var GroupDetails = objCustRepo.GetGroupDetails(Convert.ToInt32(groupId));
                ViewBag.OutletList = lstOutlet;
                ViewBag.BranchList = lstBrand;
                ViewBag.GroupId = groupId;
                ViewBag.GroupName = GroupDetails.RetailName;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, groupId);
            }
            return View();
            
        }

        public ActionResult SecurityKey()
        {

            var groupId = (string)Session["GroupId"];
            string connStr = objCustRepo.GetCustomerConnString(groupId);
            var lstOutlet = RR.GetOutletList(groupId, connStr);
            var lstBrand = RR.GetBrandList(groupId, connStr);
            var GroupDetails = objCustRepo.GetGroupDetails(Convert.ToInt32(groupId));
            ViewBag.OutletList = lstOutlet;
            ViewBag.BranchList = lstBrand;
            ViewBag.GroupId = groupId;
            ViewBag.GroupName = GroupDetails.RetailName;

            return View();
        }

        public ActionResult GetChangeNameData(string MobileNo, string CardNo)
        {
            MemberData objCustomerDetail = new MemberData();
            var groupId = (string)Session["GroupId"];
            if (!string.IsNullOrEmpty(MobileNo))
            {
                objCustomerDetail = ITOPS.GetChangeNameByMobileNo(groupId, MobileNo);
            }
            if (!string.IsNullOrEmpty(CardNo))
            {
                objCustomerDetail = ITOPS.GetChangeNameByCardNo(groupId, CardNo);
            }

            return Json(objCustomerDetail, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetOutletByBrandId(string BrandId)
        {
            var GroupId = (string)Session["GroupId"];
            string connStr = objCustRepo.GetCustomerConnString(GroupId);
            SPResponse result = new SPResponse();
            var lstoutletlist = RR.GetOutletListByBrandId(BrandId, connStr);
            ViewBag.OutletListByBrand = lstoutletlist;
            return Json(lstoutletlist, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetLoginIdByOutlets(int outletId)
        {
            var GroupId = (string)Session["GroupId"];
            SPResponse result = new SPResponse();
            ResetSecurityKey objreset = new ResetSecurityKey();
            try
            {

                objreset.lstloginid = ITOPS.GetLoginIdByOutlet(GroupId, outletId);
            }
            catch (Exception ex)
            {

                newexception.AddException(ex, GroupId);
            }

            return Json(objreset, JsonRequestBehavior.AllowGet);
        }
        public bool UpdateSecurityKey(string GroupId, string CounterId)
        {
            bool result = false;
            try
            {
                result = ITOPS.UpdateSecurityKey(GroupId, CounterId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return result;
        }
    }
}