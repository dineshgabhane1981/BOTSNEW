using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApp.App_Start;
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
            CommonFunctions common = new CommonFunctions();
            if (!string.IsNullOrEmpty(groupId))
            {
                groupId = common.DecryptString(groupId);
            }
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            OnBoardingSalesViewModel objData = new OnBoardingSalesViewModel();
            try
            {
                List<SelectListItem> refferedname = new List<SelectListItem>();
                SelectListItem item = new SelectListItem();
                item.Value = "0";
                item.Text = "Please Select";
                refferedname.Add(item);
                objData.lstAllGroups = refferedname;

                objData.lstCity = CR.GetCity();
                objData.lstRetailCategory = CR.GetRetailCategory();
                objData.lstBillingPartner = CR.GetBillingPartner();
                objData.lstSourcedBy = CR.GetSourcedBy();
                objData.lstRMAssigned = CR.GetRMAssigned();
                objData.lstRefferedCategory = CR.GetAllRefferedCategory();
                objData.lstStates = CR.GetStates();
                
                if (!string.IsNullOrEmpty(groupId))
                {
                    objData.bots_TblGroupMaster = OBR.GetGroupMasterDetails(groupId);
                    objData.bots_TblDealDetails = OBR.GetDealMasterDetails(groupId);
                    objData.bots_TblPaymentDetails = OBR.GetPaymentDetails(groupId);
                    objData.objRetailList = OBR.GetRetailDetails(groupId);
                    objData.objInstallmentList = OBR.GetInstallmentDetails(groupId);
                    objData.lstOutlets = OBR.GetOutletDetails(groupId);
                    if (objData.lstOutlets.Count == 0)
                    {
                        var brandId = 1;
                        var outletId = 1;
                        foreach (var item1 in objData.objRetailList)
                        {                            
                            for (int i = 1; i <= item1.NoOfEnrolled; i++)
                            {
                                BOTS_TblOutletMaster outlet = new BOTS_TblOutletMaster();
                                outlet.Id = 0;
                                outlet.BrandId = Convert.ToString(brandId);
                                outlet.OutletId = Convert.ToString(outletId);
                                outlet.BrandName = item1.BrandName;
                                objData.lstOutlets.Add(outlet);
                                outletId++;
                            }
                            brandId++;
                        }                        
                    }
                    else
                    {
                        foreach (var item1 in objData.objRetailList)
                        {
                            foreach (var item2 in objData.lstOutlets)
                            {
                                item2.BrandName = item1.BrandName; 
                            }                             
                        }
                    }
                    objData.bots_TblGroupMaster.CategoryData = json_serializer.Serialize(objData.objRetailList);
                    objData.bots_TblGroupMaster.PaymentScheduleData = json_serializer.Serialize(objData.objInstallmentList);
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
            int GroupdId = 0;
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                List<BOTS_TblRetailMaster> objLstRetail = new List<BOTS_TblRetailMaster>();
                List<BOTS_TblInstallmentDetails> objLstInstallment = new List<BOTS_TblInstallmentDetails>();
                List<BOTS_TblOutletMaster> objLstOutlet = new List<BOTS_TblOutletMaster>();

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
                objData.lstRefferedCategory = CR.GetAllRefferedCategory();
                objData.lstStates = CR.GetStates();

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
              

                //if (string.IsNullOrEmpty(objData.bots_TblGroupMaster.GroupId))
                //    objData.bots_TblGroupMaster.CustomerStatus = "Draft";
                objData.bots_TblGroupMaster.CreatedBy = userDetails.UserId;
                objData.bots_TblGroupMaster.CreatedDate = DateTime.Now;
                var newCuscomer = true;
                if (Convert.ToInt32(objData.bots_TblGroupMaster.GroupId) > 0)
                {
                    newCuscomer = false;
                }
                if (objData.bots_TblGroupMaster.CustomerStatus == "CSUpdate")
                {
                    object[] objOutletData = (object[])json_serializer.DeserializeObject(objData.bots_TblGroupMaster.OutletData);
                    foreach (Dictionary<string, object> item in objOutletData)
                    {
                        BOTS_TblOutletMaster objItem = new BOTS_TblOutletMaster();
                        objItem.Id = Convert.ToInt32(item["Id"]);
                        objItem.GroupId = Convert.ToString(item["GroupId"]);
                        objItem.BrandId = Convert.ToString(item["BrandId"]);
                        objItem.OutletId = Convert.ToString(item["OutletId"]);
                        objItem.OutletName = Convert.ToString(item["OutletName"]);
                        objItem.AreaName = Convert.ToString(item["AreaName"]);
                        objItem.AuthorisedPerson = Convert.ToString(item["AuthorisedPerson"]);
                        objItem.RegisterMobileNo = Convert.ToString(item["RegisterMobileNo"]);
                        objItem.RegisterEmail = Convert.ToString(item["RegisterEmail"]);
                        objItem.Address = Convert.ToString(item["Address"]);
                        objItem.Latitude = Convert.ToString(item["Latitude"]);
                        objItem.Longitude = Convert.ToString(item["Longitude"]);
                        objItem.City = Convert.ToString(item["City"]);
                        objItem.PinCode = Convert.ToString(item["PinCode"]);
                        objItem.State = Convert.ToString(item["State"]);

                        objLstOutlet.Add(objItem);
                    }
                    GroupdId = OBR.AddOnboardingCustomer(objData.bots_TblGroupMaster, objLstRetail, objData.bots_TblDealDetails, objData.bots_TblPaymentDetails, objLstInstallment, objLstOutlet);
                }
                else
                {
                    GroupdId = OBR.AddOnboardingCustomer(objData.bots_TblGroupMaster, objLstRetail, objData.bots_TblDealDetails, objData.bots_TblPaymentDetails, objLstInstallment, objLstOutlet);
                }
                objData.objRetailList = OBR.GetRetailDetails(Convert.ToString(GroupdId));
                objData.lstOutlets = OBR.GetOutletDetails(Convert.ToString(GroupdId));
                if (objData.lstOutlets.Count > 0)
                {
                    var brandId = 1;
                    var outletId = 1;
                    foreach (var item2 in objData.objRetailList)
                    {
                        for (int i = 1; i <= item2.NoOfEnrolled; i++)
                        {
                            BOTS_TblOutletMaster outlet = new BOTS_TblOutletMaster();
                            outlet.Id = 0;
                            outlet.BrandId = Convert.ToString(brandId);
                            outlet.OutletId = Convert.ToString(outletId);
                            outlet.BrandName = item2.BrandName;
                            objData.lstOutlets.Add(outlet);
                            outletId++;
                        }
                        brandId++;
                    }

                }
                if (GroupdId > 0 && newCuscomer)
                {
                    SendEmail(GroupdId);
                }
                TempData["status"] = true;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "");
                TempData["error"] = "Error Occured";
                return View("Index");
            }

            return View("Index", objData);

        }


        [HttpPost]
        public JsonResult GetBillingPartnerProduct(string BPId)
        {
            var lstBillingPartnerProduct = OBR.GetBillingPartnerProduct(Convert.ToInt32(BPId));
            return new JsonResult() { Data = lstBillingPartnerProduct, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetRefferedName(string SourceType)
        {
            var lstRefferedName = OBR.GetRefferedName(SourceType);
            return new JsonResult() { Data = lstRefferedName, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public void SendEmail(int groupId)
        {
            try
            {
                var GroupDetails = OBR.GetGroupMasterDetails(Convert.ToString(groupId));
                var RetailList = OBR.GetRetailDetailsForEmail(Convert.ToString(groupId));
                var emailIds = OBR.GetAllInternalEmailIds();
                StringBuilder sb = new StringBuilder();
                sb.Append("<table border=\"1\" cellpadding=\"5\" cellspacing=\"5\">");
                sb.Append("<tr>");
                sb.Append("<td>Retail Name:</td><td>" + GroupDetails.GroupName + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>Customer Name:</td><td>" + GroupDetails.OwnerName + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>Mobile No:</td><td>" + GroupDetails.OwnerMobileNo + "</td>");
                sb.Append("</tr>");

                int count = 1;
                string Product = string.Empty;
                string BillingPartner = string.Empty;
                string Category = string.Empty;
                string NoOfOutlets = "";
                foreach (var item in RetailList)
                {
                    if (count == 1)
                    {
                        Category = item.CategoryName;
                        NoOfOutlets = Convert.ToString(item.NoOfOutlets);
                        BillingPartner = item.BillingPartner;
                        Product = item.BOProduct;
                    }
                    else
                    {
                        Category += "," + item.CategoryName;
                        NoOfOutlets += "," + item.NoOfOutlets;
                        BillingPartner += "," + item.BillingPartner;
                        Product += "," + item.BOProduct;
                    }
                    count++;
                }

                sb.Append("<tr>");
                sb.Append("<td>Product:</td><td>" + Product + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>Billing Partner:</td><td>" + BillingPartner + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>Category:</td><td>" + Category + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>No of Outlets:</td><td>" + NoOfOutlets + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>Email:</td><td>" + GroupDetails.OwnerEmailId + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>SMS Free Messages:</td><td>" + GroupDetails.NoOfFreeSMS + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>SMS Prepaid Messages:</td><td>" + GroupDetails.NoOfPaidSMS + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>Whatsapp:</td><td>" + GroupDetails.IsWhatsApp.ToString() + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>Whatsapp Free Messages:</td><td>" + GroupDetails.NoOfFreeWhatsAppMsg + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>Whatsapp Prepaid Messages:</td><td>" + GroupDetails.NoOfPaidWhatsAppMsg + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>DLC:</td><td>" + GroupDetails.IsMWP.ToString() + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>Comments:</td><td>" + GroupDetails.Comments + "</td>");
                sb.Append("</tr>");

                sb.Append("</table>");


                var userName = ConfigurationManager.AppSettings["Email"].ToString();
                var password = ConfigurationManager.AppSettings["EmailPassword"].ToString();
                SmtpClient smtp = new SmtpClient();
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new System.Net.NetworkCredential(userName, password);
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                var userDetails = (CustomerLoginDetail)Session["UserSession"];

                MailMessage email = new MailMessage();
                MailAddress from = new MailAddress(userName);
                email.From = from;
                foreach (var item in emailIds)
                {
                    email.To.Add(item);
                }

                email.Subject = "New Customer Onboarded - " + GroupDetails.GroupName;
                email.Body = sb.ToString();
                email.IsBodyHtml = true;
                email.Priority = MailPriority.High;
                smtp.Send(email);

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "Onboarding Email Error");
            }

        }

        public ActionResult CSEditing(string groupId)
        {
            CommonFunctions common = new CommonFunctions();
            if (!string.IsNullOrEmpty(groupId))
            {
                groupId = common.DecryptString(groupId);
            }
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            OnBoardingSalesViewModel objData = new OnBoardingSalesViewModel();
            try
            {
                objData.lstCity = CR.GetCity();
                objData.lstRetailCategory = CR.GetRetailCategory();
                objData.lstBillingPartner = CR.GetBillingPartner();
                objData.lstSourcedBy = CR.GetSourcedBy();
                objData.lstRMAssigned = CR.GetRMAssigned();
                objData.lstRefferedCategory = CR.GetAllRefferedCategory();
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
                    objData.bots_TblGroupMaster.PaymentScheduleData = json_serializer.Serialize(objData.objInstallmentList);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "");
            }
            return View(objData);
        }


    }
}