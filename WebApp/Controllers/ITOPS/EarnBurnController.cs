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
    public class EarnBurnController : Controller
    {
        ITOpsRepository ITOPS = new ITOpsRepository();
        ReportsRepository RR = new ReportsRepository();
        CustomerRepository objCustRepo = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        // GET: EarnBurn
        string groupId;
        //var userDetails = (CustomerLoginDetail)Session["UserSession"];
        //var GroupId = userDetails.GroupId;
        //var roleId = userDetails.LoginType;
        //var level = userDetails.LevelIndicator;
        

        

        public ActionResult Index()
        {
            try
            {
                CommonFunctions common = new CommonFunctions();
                //groupId = common.DecryptString(groupId);
                groupId = Session["GroupId"].ToString();
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

        [HttpPost]
       public ActionResult GetData(string GroupId, string MobileNo, string CardNo)
        {
            MemberData objCustomerDetail = new MemberData();
            if (!string.IsNullOrEmpty(MobileNo))
            {
                objCustomerDetail = ITOPS.GetChangeNameByMobileNo(GroupId, MobileNo);
            }
            if (!string.IsNullOrEmpty(CardNo))
            {
                objCustomerDetail = ITOPS.GetChangeNameByCardNo(GroupId, CardNo);
            }

            return Json(objCustomerDetail, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Burn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddEarnData(string jsonData)
        {
            SPResponse result = new SPResponse();
            string GroupId = "";
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                tblAudit objAudit = new tblAudit();
                bool IsSMS = false;

                string MobileNo = "";
                string TransactionDate = "";
                string InvoiceNumber = "";
                string InvoiceAmount = "";
                string OutletId = "";
                decimal points = 0;

                foreach (Dictionary<string, object> item in objData)
                {
                    GroupId = Convert.ToString(item["GroupID"]);
                    MobileNo = Convert.ToString(item["MobileNo"]);
                    OutletId = Convert.ToString(item["OutletId"]);
                    TransactionDate = Convert.ToString(item["TransactionDate"]);
                    InvoiceNumber = Convert.ToString(item["InvoiceNumber"]);
                    InvoiceAmount = Convert.ToString(item["InvoiceAmount"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["Points"])))
                    {
                        points = Convert.ToDecimal(item["Points"]);
                    }
                    objAudit.GroupId = GroupId;
                    objAudit.RequestedFor = "Add / Earn";
                    objAudit.RequestedEntity = "Mobile No - " + MobileNo;
                    objAudit.RequestedBy = Convert.ToString(item["RequestedBy"]);
                    objAudit.RequestedOnForum = Convert.ToString(item["RequestedForum"]);
                    objAudit.RequestedOn = Convert.ToDateTime(item["RequestedOn"]);
                    IsSMS = Convert.ToBoolean(item["IsSMS"]);
                }

                result = ITOPS.AddEarnData(GroupId, MobileNo, OutletId, Convert.ToDateTime(TransactionDate), DateTime.Now, InvoiceNumber, InvoiceAmount, Convert.ToString(IsSMS), points, objAudit);
                if (result.ResponseCode == "00")
                {
                    var subject = "Earning updated for mobile no  - " + MobileNo;
                    var body = "Earning updated for mobile no - " + MobileNo;
                    body += "<br/><br/> Regards <br/> Blue Ocktopus Team";

                    //SendEmail(GroupId, subject, body);
                }

                if (IsSMS)
                {
                    //Logic to send SMS to Customer whose Name is changed
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}