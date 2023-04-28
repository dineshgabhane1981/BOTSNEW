using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApp.App_Start;

namespace WebApp.Controllers.ITOPS
{
    public class MemberController : Controller
    {
        CustomerRepository objCustRepo = new CustomerRepository();
        ITOpsRepository ITOPS = new ITOpsRepository();
        Exceptions newexception = new Exceptions();
        // GET: Member
        public ActionResult Index(string groupId)
        {
            {
                try
                {
                    if (!string.IsNullOrEmpty(groupId))
                    {                       
                        CommonFunctions common = new CommonFunctions();
                        groupId = common.DecryptString(groupId);
                        Session["GroupId"] = groupId;
                        var userDetails = (CustomerLoginDetail)Session["UserSession"];
                        userDetails.GroupId = groupId;
                        userDetails.connectionString = objCustRepo.GetCustomerConnString(groupId);
                        userDetails.CustomerName = objCustRepo.GetCustomerName(groupId);
                        Session["UserSession"] = userDetails;
                        Session["buttons"] = "ITOPS";
                        ViewBag.GroupId = groupId;
                    }

                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "Index");
                }
                return View();
            }
        }
        public ActionResult DeleteUser()
        {
            var groupId = (string)Session["GroupId"];
            ViewBag.GroupId = groupId;
            return View();
        }
        public ActionResult GetChangeNameData(string MobileNo, string CardNo)
        {
            MemberData objCustomerDetail = new MemberData();
            var groupId = (string)Session["GroupId"];
            try
            {
                if (!string.IsNullOrEmpty(MobileNo))
                {
                    objCustomerDetail = ITOPS.GetChangeNameByMobileNo(groupId, MobileNo);
                }
                if (!string.IsNullOrEmpty(CardNo))
                {
                    objCustomerDetail = ITOPS.GetChangeNameByCardNo(groupId, CardNo);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetChangeNameData");
            }
            return Json(objCustomerDetail, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteUserDetails(string jsonData)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var result = false;
            string GroupId, MobileNo;
            GroupId = string.Empty;
            MobileNo = string.Empty;
            try
            {
               
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                tblAudit objAudit = new tblAudit();
                string CustomerId = "";               

                foreach (Dictionary<string, object> item in objData)
                {
                    CustomerId = Convert.ToString(item["CustomerId"]);
                    GroupId = Convert.ToString(item["GroupID"]);
                    MobileNo = Convert.ToString(item["MobileNo"]);
                    objAudit.GroupId = GroupId;
                    objAudit.RequestedFor = "Delete User";
                    objAudit.RequestedEntity = "CustomerId - " + CustomerId;
                    //objAudit.RequestedBy = Convert.ToString(item["RequestedBy"]);
                    //objAudit.RequestedOnForum = Convert.ToString(item["RequestedForum"]);
                    objAudit.RequestedOn = DateTime.Now;
                    objAudit.AddedBy = userDetails.LoginId;
                    //objAudit.AddedDate = DateTime.Now;
                }

                result = ITOPS.DeleteUser(GroupId,MobileNo, CustomerId, objAudit);
                
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "DeleteUser");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
            
        }       
    }
}