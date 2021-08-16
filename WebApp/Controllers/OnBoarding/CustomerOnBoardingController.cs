using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApp.ViewModel;

namespace WebApp.Controllers.OnBoarding
{
    public class CustomerOnBoardingController : Controller
    {
        CustomerRepository CR = new CustomerRepository();
        OnBoardingRepository OBR = new OnBoardingRepository();
        Exceptions newexception = new Exceptions();
        // GET: CustomerOnBoarding
        public ActionResult Index(string groupId)
        {
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            OnBoardingSalesViewModel objData = new OnBoardingSalesViewModel();
            try
            {
                objData.lstCity = CR.GetCity();
                objData.lstRetailCategory = CR.GetRetailCategory();
                objData.lstBillingPartner = CR.GetBillingPartner();
                objData.lstSourcedBy = CR.GetSourcedBy();
                objData.lstRMAssigned = CR.GetRMAssigned();
                List<SelectListItem> refferedname = new List<SelectListItem>();
                SelectListItem item = new SelectListItem();
                item.Value = "0";
                item.Text = "Please Select";
                refferedname.Add(item);
                objData.lstAllGroups = refferedname;
                if (!string.IsNullOrEmpty(groupId))
                {
                    objData.bots_TblGroupMaster = OBR.GetGroupMasterDetails(groupId);
                    objData.bots_TblDealDetails = OBR.GetDealMasterDetails(groupId);
                    objData.bots_TblPaymentDetails = OBR.GetPaymentDetails(groupId);
                    objData.objRetailList = OBR.GetRetailDetails(groupId);
                    objData.objInstallmentList = OBR.GetInstallmentDetails(groupId);
                    objData.bots_TblGroupMaster.CategoryData = json_serializer.Serialize(objData.objRetailList);
                    objData.bots_TblGroupMaster.PaymentScheduleData= json_serializer.Serialize(objData.objInstallmentList);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "");
            }
            return View(objData);
        }
        [HttpPost]
        public ActionResult AddCustomer(OnBoardingSalesViewModel objData)
        {
            bool status = false;
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                List<BOTS_TblRetailMaster> objLstRetail = new List<BOTS_TblRetailMaster>();
                List<BOTS_TblInstallmentDetails> objLstInstallment = new List<BOTS_TblInstallmentDetails>();
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objCategoryData = (object[])json_serializer.DeserializeObject(objData.bots_TblGroupMaster.CategoryData);
                object[] objPaymentScheduleData = (object[])json_serializer.DeserializeObject(objData.bots_TblGroupMaster.PaymentScheduleData);

                foreach (Dictionary<string, object> item in objCategoryData)
                {
                    BOTS_TblRetailMaster objItem = new BOTS_TblRetailMaster();
                    objItem.GroupId = Convert.ToString(item["GroupId"]);
                    objItem.CategoryId = Convert.ToString(item["CategoryId"]);
                    objItem.CategoryName = Convert.ToString(item["CategoryName"]);
                    objItem.BrandName = Convert.ToString(item["BrandName"]);
                    objItem.NoOfOutlets = Convert.ToInt64(item["NoOfOutlets"]);
                    objItem.NoOfEnrolled = Convert.ToInt64(item["NoOfEnrolled"]);
                    objItem.BOProduct = Convert.ToString(item["BOProduct"]);
                    objItem.BillingPartner = Convert.ToString(item["BillingPartner"]);
                    objItem.BillingProduct = Convert.ToString(item["BillingProduct"]);

                    objLstRetail.Add(objItem);
                }
                foreach (Dictionary<string, object> item in objPaymentScheduleData)
                {
                    BOTS_TblInstallmentDetails objItem = new BOTS_TblInstallmentDetails();

                    objItem.GroupId = Convert.ToString(item["GroupId"]);
                    objItem.Installment = Convert.ToInt32(item["Installment"]);
                    objItem.PaymentDate = Convert.ToDateTime(item["PaymentDate"]);
                    objItem.PaymentAmount = Convert.ToDecimal(item["PaymentAmount"]);

                    objLstInstallment.Add(objItem);
                }
                if (string.IsNullOrEmpty(objData.bots_TblGroupMaster.GroupId))
                    objData.bots_TblGroupMaster.CustomerStatus = "Draft";
                objData.bots_TblGroupMaster.CreatedBy = userDetails.UserId;
                objData.bots_TblGroupMaster.CreatedDate = DateTime.Now;
                status = OBR.AddOnboardingCustomer(objData.bots_TblGroupMaster, objLstRetail, objData.bots_TblDealDetails, objData.bots_TblPaymentDetails, objLstInstallment);
                TempData["status"] = status;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "");
                TempData["error"] = "Error Occured";
                return View("Index");
            }
            List<SelectListItem> refferedname = new List<SelectListItem>();
            SelectListItem item1 = new SelectListItem();
            item1.Value = "0";
            item1.Text = "Please Select";
            refferedname.Add(item1);
            objData.lstAllGroups = refferedname;
            objData.lstCity = CR.GetCity();
            objData.lstRetailCategory = CR.GetRetailCategory();
            objData.lstBillingPartner = CR.GetBillingPartner();
            objData.lstSourcedBy = CR.GetSourcedBy();
            objData.lstRMAssigned = CR.GetRMAssigned();
            return View("Index", objData);

        }

    }
}