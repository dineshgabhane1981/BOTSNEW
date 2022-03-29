using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApp.App_Start;
using WebApp.ViewModel;
using BOTS_BL.Models.SalesLead;
using BOTS_BL.Models.OnBoarding;
using System.Data.OleDb;
using System.Data;
using System.Linq;

namespace WebApp.Controllers.OnBoarding
{
    public class CustomerOnBoardingController : Controller
    {
        CustomerRepository CR = new CustomerRepository();
        OnBoardingRepository OBR = new OnBoardingRepository();
        SalesLeadRepository SLR = new SalesLeadRepository();
        Exceptions newexception = new Exceptions();
        // GET: CustomerOnBoarding
        public ActionResult Index(string groupId, string LeadId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            if (userDetails.LoginType == "11")
            {
                return RedirectToAction("DLTView", "CustomerOnBoarding", new { GroupId = groupId });
            }
            CommonFunctions common = new CommonFunctions();
            if (!string.IsNullOrEmpty(groupId))
            {
                groupId = common.DecryptString(groupId);
            }

            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            OnBoardingSalesViewModel objData = new OnBoardingSalesViewModel();
            try
            {
                BOTS_TblPointsEarnRuleConfig objpointsearncofig = new BOTS_TblPointsEarnRuleConfig();
                BOTS_TblEarnPointsSlabConfig objpointsslabconfig = new BOTS_TblEarnPointsSlabConfig();
                BOTS_TblPointsBurnRuleConfig objpointsburncofig = new BOTS_TblPointsBurnRuleConfig();
                List<SelectListItem> refferedname = new List<SelectListItem>();

                SelectListItem item = new SelectListItem();
                item.Value = "0";
                item.Text = "Please Select";
                refferedname.Add(item);
                objData.lstAllGroups = refferedname;
                objData.lstBrands = refferedname;

                objData.lstCity = CR.GetCity();
                objData.lstRetailCategory = CR.GetRetailCategory();
                objData.lstBillingPartner = CR.GetBillingPartner();
                objData.lstSourcedBy = CR.GetSourcedBy();
                objData.lstRMAssigned = CR.GetRMAssigned();
                objData.lstRefferedCategory = CR.GetAllRefferedCategory();
                objData.lstStates = CR.GetStates();
                if (!string.IsNullOrEmpty(LeadId))
                {
                    var leadDetails = SLR.GetsalesLeadByLeadId(Convert.ToInt32(LeadId));
                    objData.bots_TblGroupMaster = MeargeLeadData(leadDetails);
                    objData.LeadId = LeadId;
                }

                if (!string.IsNullOrEmpty(groupId))
                {
                    objData.bots_TblGroupMaster = OBR.GetGroupMasterDetails(groupId);
                    objData.bots_TblDealDetails = OBR.GetDealMasterDetails(groupId);
                    objData.bots_TblPaymentDetails = OBR.GetPaymentDetails(groupId);
                    objData.objRetailList = OBR.GetRetailDetails(groupId);
                    objData.objInstallmentList = OBR.GetInstallmentDetails(groupId);
                    objData.lstOutlets = OBR.GetOutletDetails(groupId);

                    foreach (var brand in objData.objRetailList)
                    {
                        objData.lstBrands.Add(new SelectListItem
                        {
                            Text = brand.BrandName,
                            Value = Convert.ToString(brand.BrandId)
                        });
                    }


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
                                if (item1.BrandId == item2.BrandId)
                                    item2.BrandName = item1.BrandName;
                            }
                        }
                    }
                    objData.bots_TblGroupMaster.CategoryData = json_serializer.Serialize(objData.objRetailList);
                    objData.bots_TblGroupMaster.PaymentScheduleData = json_serializer.Serialize(objData.objInstallmentList);
                }
                objData.lstearnpoint = FillEarnPointLevel();
                objData.objpointsearnruleconfig = objpointsearncofig;
                objData.objearnpointslab = objpointsslabconfig;
                objData.objpointsburnruleconfig = objpointsburncofig;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "");
            }
            return View(objData);
        }

        public ActionResult DLTView(string GroupId)
        {
            OnBoardingSalesViewModel objData = new OnBoardingSalesViewModel();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            CommonFunctions common = new CommonFunctions();
            if (!string.IsNullOrEmpty(GroupId))
            {
                GroupId = common.DecryptString(GroupId);
                if (!string.IsNullOrEmpty(GroupId))
                {
                    List<SelectListItem> refferedname = new List<SelectListItem>();
                    SelectListItem item = new SelectListItem();
                    item.Value = "0";
                    item.Text = "Please Select";
                    refferedname.Add(item);
                    objData.lstAllGroups = refferedname;
                    objData.lstBrands = refferedname;

                    objData.lstCity = CR.GetCity();
                    objData.lstRetailCategory = CR.GetRetailCategory();
                    objData.lstBillingPartner = CR.GetBillingPartner();
                    objData.lstSourcedBy = CR.GetSourcedBy();
                    objData.lstRMAssigned = CR.GetRMAssigned();
                    objData.lstRefferedCategory = CR.GetAllRefferedCategory();
                    objData.lstStates = CR.GetStates();

                    objData.bots_TblGroupMaster = OBR.GetGroupMasterDetails(GroupId);
                    objData.bots_TblDealDetails = OBR.GetDealMasterDetails(GroupId);
                    objData.bots_TblPaymentDetails = OBR.GetPaymentDetails(GroupId);
                    objData.objRetailList = OBR.GetRetailDetails(GroupId);
                    objData.objInstallmentList = OBR.GetInstallmentDetails(GroupId);
                    objData.lstOutlets = OBR.GetOutletDetails(GroupId);

                    foreach (var brand in objData.objRetailList)
                    {
                        objData.lstBrands.Add(new SelectListItem
                        {
                            Text = brand.BrandName,
                            Value = Convert.ToString(brand.BrandId)
                        });
                    }
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
                                if (item1.BrandId == item2.BrandId)
                                    item2.BrandName = item1.BrandName;
                            }
                        }
                    }
                    objData.bots_TblGroupMaster.CategoryData = json_serializer.Serialize(objData.objRetailList);
                    objData.bots_TblGroupMaster.PaymentScheduleData = json_serializer.Serialize(objData.objInstallmentList);

                    objData.lstSMSConfig = OBR.GetCommunicationSMSConfigForDLT(GroupId);
                    var BrandCount = objData.lstSMSConfig.Where(x => x.BrandId != "All").Count();
                    if (BrandCount > 0)
                    {
                        objData.IsBrand = true;
                    }
                    else
                    {
                        objData.IsBrand = false;
                    }

                }
            }
            return View(objData);
        }

        public BOTS_TblGroupMaster MeargeLeadData(SALES_tblLeads leadDetails)
        {
            BOTS_TblGroupMaster objGroupDetails = new BOTS_TblGroupMaster();
            objGroupDetails.GroupName = leadDetails.BusinessName;
            objGroupDetails.OwnerName = leadDetails.AuthorizedPerson;
            objGroupDetails.OwnerMobileNo = leadDetails.MobileNo;
            objGroupDetails.City = leadDetails.City;
            objGroupDetails.OwnerEmailId = leadDetails.EmailId;

            return objGroupDetails;
        }

        [HttpPost]
        public ActionResult AddCustomer(OnBoardingSalesViewModel objData)
        {
            bool status = false;
            int GroupdId = 0;
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                List<BOTS_TblRetailMaster> objLstRetail = new List<BOTS_TblRetailMaster>();
                List<BOTS_TblInstallmentDetails> objLstInstallment = new List<BOTS_TblInstallmentDetails>();
                List<BOTS_TblOutletMaster> objLstOutlet = new List<BOTS_TblOutletMaster>();
                SALES_tblLeads leadDetails = new SALES_tblLeads();
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
                    objItem.BrandId = Convert.ToString(item["BrandId"]);
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

                objData.bots_TblGroupMaster.CreatedBy = userDetails.LoginId;
                objData.bots_TblGroupMaster.CreatedDate = DateTime.Now;

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
                    if (objData.LeadId != null)
                    {
                        status = SLR.UpdateStatus(Convert.ToInt32(objData.LeadId), Convert.ToString(GroupdId));
                    }
                }
                else
                {
                    GroupdId = OBR.AddOnboardingCustomer(objData.bots_TblGroupMaster, objLstRetail, objData.bots_TblDealDetails, objData.bots_TblPaymentDetails, objLstInstallment, objLstOutlet);
                    if (objData.LeadId != null)
                    {
                        status = SLR.UpdateStatus(Convert.ToInt32(objData.LeadId), Convert.ToString(GroupdId));
                    }
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
                if (GroupdId > 0 && objData.bots_TblGroupMaster.CustomerStatus == "CS")
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
            CommonFunctions common = new CommonFunctions();
            var groupId = common.EncryptString(Convert.ToString(GroupdId));
            //return View("Index", objData);
            //return RedirectToAction("Index", "CustomerOnBoarding", groupId);
            return RedirectToAction("Index", "CustomerOnBoarding", new { @groupId = groupId });
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
                        NoOfOutlets = Convert.ToString(item.NoOfEnrolled);
                        BillingPartner = item.BillingPartner;
                        Product = item.BOProduct;
                    }
                    else
                    {
                        Category += "," + item.CategoryName;
                        NoOfOutlets += "," + Convert.ToString(item.NoOfEnrolled);
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
                sb.Append("<td>No of Enrolled Outlets:</td><td>" + NoOfOutlets + "</td>");
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
                sb.Append("<td>Existing Loyalty:</td><td>" + GroupDetails.IsExistingLoyalty.ToString() + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>DLC:</td><td>" + GroupDetails.IsMWP.ToString() + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>Assigned CS:</td><td>" + Convert.ToString(GroupDetails.AssignedCSName) + "</td>");
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

        public JsonResult GetSMSandWAData(string groupId, string brandId)
        {
            CommunicationConfigViewModel objData = new CommunicationConfigViewModel();
            objData.SMSConfig = OBR.GetCommunicationSMSConfig(groupId, brandId);
            objData.WAConfig = OBR.GetCommunicationWAConfig(groupId, brandId);
            return new JsonResult() { Data = objData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult SaveCommunicationConfig(string jsonData)
        {
            bool status = false;
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                BOTS_TblSMSConfig objSMSConfig = new BOTS_TblSMSConfig();
                BOTS_TblWAConfig objWAConfig = new BOTS_TblWAConfig();
                List<SMSTemplate> lstSMSTemplate = new List<SMSTemplate>();
                List<SMSTemplate> lstWATemplate = new List<SMSTemplate>();
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objCommConfigData = (object[])json_serializer.DeserializeObject(jsonData);
                foreach (Dictionary<string, object> item in objCommConfigData)
                {
                    var isSMS = Convert.ToBoolean(item["IsSMS"]);
                    if (isSMS)
                    {
                        objSMSConfig.IsSMS = true;
                        objSMSConfig.SMSProvider = Convert.ToString(item["SMSProvider"]);
                        objSMSConfig.GroupId = Convert.ToString(item["GroupId"]);
                        objSMSConfig.BrandId = Convert.ToString(item["BrandId"]);
                        objSMSConfig.SMSSenderID = Convert.ToString(item["SMSSenderId"]);
                        objSMSConfig.SMSUsername = Convert.ToString(item["SMSUserName"]);
                        objSMSConfig.SMSPassword = Convert.ToString(item["SMSPassword"]);
                        objSMSConfig.SMSlink = Convert.ToString(item["SMSLink"]);
                        //objSMSConfig.DLTStatus = "Submitted";

                        SMSTemplate objSMSTemplate1 = new SMSTemplate();
                        objSMSTemplate1.MessageId = 100;
                        if (!string.IsNullOrEmpty(Convert.ToString(item["SMSEnrollmentId"])))
                            objSMSTemplate1.Id = Convert.ToInt32(item["SMSEnrollmentId"]);
                        objSMSTemplate1.TemplateScript = Convert.ToString(item["SMSEnrollment"]);
                        lstSMSTemplate.Add(objSMSTemplate1);

                        SMSTemplate objSMSTemplate2 = new SMSTemplate();
                        objSMSTemplate2.MessageId = 101;
                        if (!string.IsNullOrEmpty(Convert.ToString(item["SMSEarnId"])))
                            objSMSTemplate2.Id = Convert.ToInt32(item["SMSEarnId"]);
                        objSMSTemplate2.TemplateScript = Convert.ToString(item["SMSEarn"]);
                        lstSMSTemplate.Add(objSMSTemplate2);

                        SMSTemplate objSMSTemplate3 = new SMSTemplate();
                        objSMSTemplate3.MessageId = 102;
                        if (!string.IsNullOrEmpty(Convert.ToString(item["SMSBurnId"])))
                            objSMSTemplate3.Id = Convert.ToInt32(item["SMSBurnId"]);
                        objSMSTemplate3.TemplateScript = Convert.ToString(item["SMSBurn"]);
                        lstSMSTemplate.Add(objSMSTemplate3);

                        SMSTemplate objSMSTemplate4 = new SMSTemplate();
                        objSMSTemplate4.MessageId = 103;
                        if (!string.IsNullOrEmpty(Convert.ToString(item["SMSCancelEarnId"])))
                            objSMSTemplate4.Id = Convert.ToInt32(item["SMSCancelEarnId"]);
                        objSMSTemplate4.TemplateScript = Convert.ToString(item["SMSCancelEarn"]);
                        lstSMSTemplate.Add(objSMSTemplate4);

                        SMSTemplate objSMSTemplate5 = new SMSTemplate();
                        objSMSTemplate5.MessageId = 104;
                        if (!string.IsNullOrEmpty(Convert.ToString(item["SMSCancelBurnId"])))
                            objSMSTemplate5.Id = Convert.ToInt32(item["SMSCancelBurnId"]);
                        objSMSTemplate5.TemplateScript = Convert.ToString(item["SMSCancelBurn"]);
                        lstSMSTemplate.Add(objSMSTemplate5);

                        SMSTemplate objSMSTemplate6 = new SMSTemplate();
                        objSMSTemplate6.MessageId = 105;
                        if (!string.IsNullOrEmpty(Convert.ToString(item["SMSOTPId"])))
                            objSMSTemplate6.Id = Convert.ToInt32(item["SMSOTPId"]);
                        objSMSTemplate6.TemplateScript = Convert.ToString(item["SMSOTP"]);
                        lstSMSTemplate.Add(objSMSTemplate6);

                        SMSTemplate objSMSTemplate7 = new SMSTemplate();
                        objSMSTemplate7.MessageId = 106;
                        if (!string.IsNullOrEmpty(Convert.ToString(item["SMSBalanceInquiryId"])))
                            objSMSTemplate7.Id = Convert.ToInt32(item["SMSBalanceInquiryId"]);
                        objSMSTemplate7.TemplateScript = Convert.ToString(item["SMSBalanceInquiry"]);
                        lstSMSTemplate.Add(objSMSTemplate7);

                        SMSTemplate objSMSTemplate8 = new SMSTemplate();
                        objSMSTemplate8.MessageId = 107;
                        if (!string.IsNullOrEmpty(Convert.ToString(item["SMSAnyCancelId"])))
                            objSMSTemplate8.Id = Convert.ToInt32(item["SMSAnyCancelId"]);
                        objSMSTemplate8.TemplateScript = Convert.ToString(item["SMSAnyCancel"]);
                        lstSMSTemplate.Add(objSMSTemplate8);

                        SMSTemplate objSMSTemplate9 = new SMSTemplate();
                        objSMSTemplate9.MessageId = 108;
                        if (!string.IsNullOrEmpty(Convert.ToString(item["SMSEnrollmentAndEarnId"])))
                            objSMSTemplate9.Id = Convert.ToInt32(item["SMSEnrollmentAndEarnId"]);
                        objSMSTemplate9.TemplateScript = Convert.ToString(item["SMSEnrollmentAndEarn"]);
                        lstSMSTemplate.Add(objSMSTemplate9);
                    }
                    else
                    {
                        objSMSConfig.IsSMS = false;
                    }
                    var isWA = Convert.ToBoolean(item["IsWA"]);
                    if (isWA)
                    {
                        objWAConfig.IsWA = true;
                        objWAConfig.WAProvider = Convert.ToString(item["WAProvider"]);
                        objWAConfig.GroupId = Convert.ToString(item["GroupId"]);
                        objWAConfig.BrandId = Convert.ToString(item["BrandId"]);
                        objWAConfig.WANumber = Convert.ToString(item["WANumber"]);
                        objWAConfig.WAUsername = Convert.ToString(item["WAUserName"]);
                        objWAConfig.WAPassword = Convert.ToString(item["WAPassword"]);
                        objWAConfig.WAlink = Convert.ToString(item["WALink"]);
                        objWAConfig.TokenId = Convert.ToString(item["WATokenId"]);

                        SMSTemplate objWATemplate1 = new SMSTemplate();
                        objWATemplate1.MessageId = 100;
                        if (!string.IsNullOrEmpty(Convert.ToString(item["WAEnrollmentId"])))
                            objWATemplate1.Id = Convert.ToInt32(item["WAEnrollmentId"]);
                        objWATemplate1.TemplateScript = Convert.ToString(item["WAEnrollment"]);
                        lstWATemplate.Add(objWATemplate1);

                        SMSTemplate objWATemplate2 = new SMSTemplate();
                        objWATemplate2.MessageId = 101;
                        if (!string.IsNullOrEmpty(Convert.ToString(item["WAEarnId"])))
                            objWATemplate2.Id = Convert.ToInt32(item["WAEarnId"]);
                        objWATemplate2.TemplateScript = Convert.ToString(item["WAEarn"]);
                        lstWATemplate.Add(objWATemplate2);

                        SMSTemplate objWATemplate3 = new SMSTemplate();
                        objWATemplate3.MessageId = 102;
                        if (!string.IsNullOrEmpty(Convert.ToString(item["WABurnId"])))
                            objWATemplate3.Id = Convert.ToInt32(item["WABurnId"]);
                        objWATemplate3.TemplateScript = Convert.ToString(item["WABurn"]);
                        lstWATemplate.Add(objWATemplate3);

                        SMSTemplate objWATemplate4 = new SMSTemplate();
                        objWATemplate4.MessageId = 103;
                        if (!string.IsNullOrEmpty(Convert.ToString(item["WACancelEarnId"])))
                            objWATemplate4.Id = Convert.ToInt32(item["WACancelEarnId"]);
                        objWATemplate4.TemplateScript = Convert.ToString(item["WACancelEarn"]);
                        lstWATemplate.Add(objWATemplate4);

                        SMSTemplate objWATemplate5 = new SMSTemplate();
                        objWATemplate5.MessageId = 104;
                        if (!string.IsNullOrEmpty(Convert.ToString(item["WACancelBurnId"])))
                            objWATemplate5.Id = Convert.ToInt32(item["WACancelBurnId"]);
                        objWATemplate5.TemplateScript = Convert.ToString(item["WACancelBurn"]);
                        lstWATemplate.Add(objWATemplate5);

                        SMSTemplate objWATemplate6 = new SMSTemplate();
                        objWATemplate6.MessageId = 105;
                        if (!string.IsNullOrEmpty(Convert.ToString(item["WAOTPId"])))
                            objWATemplate6.Id = Convert.ToInt32(item["WAOTPId"]);
                        objWATemplate6.TemplateScript = Convert.ToString(item["WAOTP"]);
                        lstWATemplate.Add(objWATemplate6);

                        SMSTemplate objWATemplate7 = new SMSTemplate();
                        objWATemplate7.MessageId = 106;
                        if (!string.IsNullOrEmpty(Convert.ToString(item["WABalanceInquiryId"])))
                            objWATemplate7.Id = Convert.ToInt32(item["WABalanceInquiryId"]);
                        objWATemplate7.TemplateScript = Convert.ToString(item["WABalanceInquiry"]);
                        lstWATemplate.Add(objWATemplate7);

                        SMSTemplate objWATemplate8 = new SMSTemplate();
                        objWATemplate8.MessageId = 107;
                        if (!string.IsNullOrEmpty(Convert.ToString(item["WAAnyCancelId"])))
                            objWATemplate8.Id = Convert.ToInt32(item["WAAnyCancelId"]);
                        objWATemplate8.TemplateScript = Convert.ToString(item["WAAnyCancel"]);
                        lstWATemplate.Add(objWATemplate8);

                        SMSTemplate objWATemplate9 = new SMSTemplate();
                        objWATemplate9.MessageId = 108;
                        if (!string.IsNullOrEmpty(Convert.ToString(item["WAEnrollmentAndEarnId"])))
                            objWATemplate9.Id = Convert.ToInt32(item["WAEnrollmentAndEarnId"]);
                        objWATemplate9.TemplateScript = Convert.ToString(item["WAEnrollmentAndEarn"]);
                        lstWATemplate.Add(objWATemplate9);
                    }
                    else
                    {
                        objWAConfig.IsWA = false;
                    }
                }
                status = OBR.SaveCommunicationConfig(objSMSConfig, objWAConfig, lstSMSTemplate, lstWATemplate, userDetails.LoginId);

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveCommunicationConfigController");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult SendCommunicationToDLT(string GroupId)
        {
            bool status = false;
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];

                status = OBR.SendCommunicationToDLT(GroupId, userDetails.LoginId);

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveCommunicationConfigController");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult SaveCommunicationDLTConfig(string jsonData)
        {
            bool status = false;
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                BOTS_TblSMSConfig objSMSConfig = new BOTS_TblSMSConfig();
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objCommConfigData = (object[])json_serializer.DeserializeObject(jsonData);
                foreach (Dictionary<string, object> item in objCommConfigData)
                {
                    var id = Convert.ToInt32(item["Id"]);
                    objSMSConfig = OBR.GetCommunicationSMSConfigById(id);
                    if (objSMSConfig != null)
                    {
                        objSMSConfig.TemplateId = Convert.ToString(item["TemplateId"]);
                        objSMSConfig.TemplateName = Convert.ToString(item["TemplateName"]);
                        objSMSConfig.TemplateType = Convert.ToString(item["TemplateType"]);
                        objSMSConfig.SMSScript = Convert.ToString(item["Script"]);
                        objSMSConfig.SMSScriptDLT = Convert.ToString(item["ScriptDLT"]);
                        objSMSConfig.UpdatedBy = userDetails.LoginId;
                        objSMSConfig.UpdatedDate = DateTime.Now;
                    }
                }
                status = OBR.SaveIndividualSMSConfig(objSMSConfig);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveCommunicationDLTConfig");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult UpdateDLTStatusOfCommunicationConfig(string ConfigId, string DLTNewStatus,string RejectReason)
        {
            bool status = false;
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                status = OBR.UpdateStatusSMSConfig(Convert.ToInt32(ConfigId), DLTNewStatus, userDetails.LoginId, RejectReason);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateDLTStatusOfCommunicationConfig");
            }

            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult SaveDLCLinkConfig(string jsonData)
        {
            bool status = false;
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objDLCConfigData = (object[])json_serializer.DeserializeObject(jsonData);
                BOTS_TblDLCLinkConfig objDLCLink = new BOTS_TblDLCLinkConfig();
                foreach (Dictionary<string, object> item in objDLCConfigData)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(item["Id"])))
                    {
                        objDLCLink.Id = Convert.ToInt32(item["Id"]);
                    }
                    objDLCLink.GroupId = Convert.ToString(item["GroupId"]);
                    objDLCLink.ProfileUpdatePoints = Convert.ToInt32(item["ProfileUpdatePoints"]);
                    objDLCLink.ReferralPoints = Convert.ToInt32(item["ReferralPoints"]);
                    objDLCLink.ReferredPoints = Convert.ToInt32(item["ReferredPoints"]);
                    objDLCLink.MaxNoOfReferrals = Convert.ToInt32(item["MaxNoOfReferrals"]);
                    objDLCLink.ValidityOfReferralPoints = Convert.ToInt32(item["ValidityOfReferralPoints"]);
                    objDLCLink.ReferralReminder = Convert.ToInt32(item["ReferralReminder"]);
                    objDLCLink.ToTheReferralSMSScript = Convert.ToString(item["SMSToTheReferral"]);
                    objDLCLink.ReminderForPointsUsageSMSScript = Convert.ToString(item["SMSReminderForPointsUsage"]);
                    objDLCLink.ReferredSuccessOnReferralTxnSMSScript = Convert.ToString(item["SMSReferredSuccessOnReferralTxn"]);
                    objDLCLink.ToTheReferralWAScript = Convert.ToString(item["WAToTheReferral"]);
                    objDLCLink.ReminderForPointsUsageWAScript = Convert.ToString(item["WAReminderForPointsUsage"]);
                    objDLCLink.ReferredSuccessOnReferralTxnWAScript = Convert.ToString(item["WAReferredSuccessOnReferralTxn"]);
                    if (objDLCLink.Id > 0)
                    {
                        objDLCLink.UpdatedBy = userDetails.LoginId;
                        objDLCLink.UpdatedDate = DateTime.Now;
                    }
                    else
                    {
                        objDLCLink.AddedBy = userDetails.LoginId;
                        objDLCLink.AddedDate = DateTime.Now;
                    }
                }
                status = OBR.SaveDLCLinkConfig(objDLCLink);

            }
            catch (Exception ex)
            {

            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult GetDLCLinkData(string groupId)
        {
            BOTS_TblDLCLinkConfig objData = new BOTS_TblDLCLinkConfig();
            objData = OBR.GetDLCLinkData(groupId);

            return new JsonResult() { Data = objData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public List<EarnPointLevel> FillEarnPointLevel()
        {
            List<EarnPointLevel> lstearn = new List<EarnPointLevel>();

            lstearn.Add(new EarnPointLevel { EarnPointLevelId = "invoiceamt", EarnPointLevelName = "Invoice Amount" });
            lstearn.Add(new EarnPointLevel { EarnPointLevelId = "department", EarnPointLevelName = "Department" });
            lstearn.Add(new EarnPointLevel { EarnPointLevelId = "product", EarnPointLevelName = "Product" });
            lstearn.Add(new EarnPointLevel { EarnPointLevelId = "Category", EarnPointLevelName = "Category" });
            lstearn.Add(new EarnPointLevel { EarnPointLevelId = "subcategory", EarnPointLevelName = "Sub Category" });
            lstearn.Add(new EarnPointLevel { EarnPointLevelId = "brand", EarnPointLevelName = "Brand" });
            return lstearn;
        }

        public ActionResult AddEarnRule(string EarnRule, string BlockOnearnrule, string Redemptionrule)
        {

            OnBoardingSalesViewModel objdata = new OnBoardingSalesViewModel();
            bool status = false;

            try
            {

                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                BOTS_TblPointsEarnRuleConfig objpointearn = new BOTS_TblPointsEarnRuleConfig();
                BOTS_TblPointsBurnRuleConfig objpointburn = new BOTS_TblPointsBurnRuleConfig();
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objEarnrule = (object[])json_serializer.DeserializeObject(EarnRule);
                object[] objBurnrule = (object[])json_serializer.DeserializeObject(BlockOnearnrule);
                object[] objredemption = (object[])json_serializer.DeserializeObject(Redemptionrule);
                object[] slab = new object[20];
                string slabtype = "";
                decimal makingslabdirectortelescopic = 0;
                decimal fullamtslabdirectortelescopic = 0;
                string makingSlabdirectortele = "";
                string fullamtSlabdirectortele = "";
                string CommonSlabdirectortele = "";
                decimal commonlabdirectortelescopic = 0;
                DataSet ds = new DataSet();
                foreach (Dictionary<string, object> item in objEarnrule)
                {
                    objpointearn.CategoryId = Convert.ToString(item["CategoryId"]);
                    objpointearn.GroupId = Convert.ToString(item["Groupid"]);
                    objpointearn.BrandId = Convert.ToString(item["brandid"]);
                    objpointearn.AddedBy = userDetails.LoginId;
                    objpointearn.AddedDate = DateTime.Today;
                    objpointearn.OnePointValueInRs = Convert.ToDecimal(item["pointvalue"]);
                    objpointearn.EarnPointLevel = Convert.ToString(item["earnlevel"]);
                    if (objpointearn.CategoryId == "8")
                    {
                        objpointearn.EarnPointLevelType = Convert.ToString(item["jwlLevel"]);
                    }
                    else
                    {
                        objpointearn.EarnPointLevelType = Convert.ToString(item["commonfixedorslab"]);
                    }
                    if (objpointearn.EarnPointLevelType == "commonFixed")
                    {
                        objpointearn.commonfixedpercentageorwithperce = Convert.ToString(item["common%withorFixed"]);
                        if ((Convert.ToString(item["common%withorFixed"])) == "fixedpercentage")
                            objpointearn.FixedEarnPointPercentage = Convert.ToDecimal(item["commonFixed%"]);
                        if ((Convert.ToString(item["common%withorFixed"])) == "percentwith")
                            objpointearn.FixedEarnPointPecentageWith = Convert.ToDecimal(item["common%with"]);

                        if (!string.IsNullOrEmpty(Convert.ToString(item["commonsingleorcumulative"])))
                        {
                            objpointearn.IncrementedValue = Convert.ToString(item["commonsingleorcumulative"]);
                            if ((Convert.ToString(item["commoncumulative%withorFixed"])) == "fixedpercentage")
                                objpointearn.IncrementedFixedPercentage = Convert.ToDecimal(item["commonsingleFixed%"]);
                            if ((Convert.ToString(item["commoncumulative%withorFixed"])) == "percentwith")
                                objpointearn.IncrementedpercentageWith = Convert.ToDecimal(item["commoncumulative%with"]);
                        }
                    }
                    if (objpointearn.EarnPointLevelType == "commonSlab")
                    {
                        slabtype = Convert.ToString(item["commonslabtype"]);
                        slab = (object[])item["Commonslab"];
                        CommonSlabdirectortele = Convert.ToString(item["commonslabdirectortelescopic"]);

                        if ((Convert.ToString(item["commonslabdirectortelescopic"])) == "commonslabdirect")
                        {
                            commonlabdirectortelescopic = Convert.ToDecimal(item["commonslabdirectvalue"]);
                        }
                        else if ((Convert.ToString(item["commonslabdirectortelescopic"])) == "commonslabtelescoping")
                        {
                            commonlabdirectortelescopic = Convert.ToDecimal(item["commonslabtelescopicvalue"]);
                        }
                    }
                    if (objpointearn.EarnPointLevelType == "Making")
                    {
                        objpointearn.EarnOnMaking = Convert.ToString(item["makingfixedorslab"]);
                        if (objpointearn.EarnOnMaking == "makingFixed")
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(item["making%withorFixed"])))
                            {
                                if ((Convert.ToString(item["making%withorFixed"])) == "fixedpercentage")
                                    objpointearn.FixedEarnPointPercentage = Convert.ToDecimal(item["makingFixed%"]);
                                if ((Convert.ToString(item["making%withorFixed"])) == "percentwith")
                                    objpointearn.FixedEarnPointPecentageWith = Convert.ToDecimal(item["making%with"]);
                            }
                        }
                        else if (objpointearn.EarnOnMaking == "makingSlab")
                        {
                            slabtype = Convert.ToString(item["slabtype"]);
                            slab = (object[])item["slab"];
                            makingSlabdirectortele = Convert.ToString(item["makingslabdirectortelescopic"]);

                            if ((Convert.ToString(item["makingslabdirectortelescopic"])) == "makingslabDirect")
                            {
                                makingslabdirectortelescopic = Convert.ToDecimal(item["makingslabdirectvalue"]);
                            }
                            else if ((Convert.ToString(item["makingslabdirectortelescopic"])) == "makingslabtescopic")
                            {
                                makingslabdirectortelescopic = Convert.ToDecimal(item["makingslabtelescopicvalue"]);

                            }

                        }
                    }
                    if (objpointearn.EarnPointLevelType == "FullAmount")
                    {
                        objpointearn.EarnFullAmtGstOrWithoutGst = Convert.ToString(item["fullamtwithgstornongst"]);
                        objpointearn.EarnFullAmtFixedOrSlab = Convert.ToString(item["fullamtfixedorslab"]);

                        if (objpointearn.EarnFullAmtFixedOrSlab == "FullamtFixed")
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(item["fullamt%withorfixed%"])))
                            {
                                if ((Convert.ToString(item["fullamt%withorfixed%"])) == "fixedpercentage")
                                    objpointearn.EarnFullAmtFixedPercentage = Convert.ToDecimal(item["fullamtFixed%"]);
                                if ((Convert.ToString(item["fullamt%withorfixed%"])) == "percentwith")
                                    objpointearn.EarnFullAmtPercentageWith = Convert.ToDecimal(item["fullamt%with"]);
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(item["fullamtincrement"])))
                            {
                                objpointearn.EarnFullAmtIncremented = Convert.ToString(item["fullamtincrement"]);
                                if ((Convert.ToString(item["fullamt%withorfixed%"])) == "fixedpercentage")
                                    objpointearn.EarnFullAmtSingleOrCumulativeFixedPercentage = Convert.ToDecimal(item["fullamtincrementedfixed%"]);
                                if ((Convert.ToString(item["fullamt%withorfixed%"])) == "percentwith")
                                    objpointearn.EarnFullAmtSingleOrCumulativeWithPercentage = Convert.ToDecimal(item["fullamtincremented%with"]);
                            }

                        }
                        else if (objpointearn.EarnFullAmtFixedOrSlab == "FullamtSlab")
                        {
                            slabtype = Convert.ToString(item["fullamtslabtype"]);
                            slab = (object[])item["fullamtslab"];
                            fullamtSlabdirectortele = Convert.ToString(item["fullamtslabdirectortele"]);

                            if ((Convert.ToString(item["fullamtslabdirectortele"])) == "fullamtslabDirect")
                            {
                                fullamtslabdirectortelescopic = Convert.ToDecimal(item["fullamtslabdirect"]);
                            }
                            else if ((Convert.ToString(item["fullamtslabdirectortele"])) == "fullamtslabtescopic")
                            {
                                fullamtslabdirectortelescopic = Convert.ToDecimal(item["fullamtslabtele"]);
                            }

                        }



                    }
                }
                foreach (Dictionary<string, object> item in objBurnrule)
                {
                    objpointearn.BlockOnEarnType = Convert.ToString(item["Blockonearnrule"]);
                    if (objpointearn.BlockOnEarnType == "InvoiceAmount")
                    {
                        objpointearn.BlockOnInvoiceAmtMin = Convert.ToDecimal(item["Minvalofinvamt"]);
                        objpointearn.BlockOnInvoiceAmtMax = Convert.ToDecimal(item["Maxvalofinvamt"]);
                    }
                }
                foreach (Dictionary<string, object> item in objredemption)
                {
                    objpointburn.GroupId = objpointearn.GroupId;
                    objpointburn.BrandId = objpointearn.BrandId;
                    objpointburn.CategoryId = objpointburn.CategoryId;
                    if (Convert.ToString(item["redemptionlevel"]) != "")
                    {
                        objpointburn.BurnType = Convert.ToString(item["redemptionlevel"]);

                        objpointburn.PointBurnfirsttimeorsubsequent = Convert.ToString(item["burntimetype"]);
                        if (objpointburn.PointBurnfirsttimeorsubsequent == "firsttime")
                        {
                            objpointburn.FirstTime = Convert.ToDecimal(item["burnfirsttime"]);

                        }
                        else if (objpointburn.PointBurnfirsttimeorsubsequent == "Subsequent")
                        {
                            objpointburn.SubsequentTime = Convert.ToDecimal(item["burnsubsequenttime"]);
                        }
                        objpointburn.EarnWhileBurn = Convert.ToString(item["Earnwhileburn"]);
                        objpointburn.PointValidity = Convert.ToString(item["pointvalidity"]);
                        if (Convert.ToDecimal(item["pointvalidityvalue"]) != 0)
                        {
                            objpointburn.PointValidityValue = Convert.ToDecimal(item["pointvalidityvalue"]);
                        }
                    }
                }
                if (Request.Files.Count > 0)
                {

                    HttpPostedFileBase fileburnonearn = Request.Files["Fileuploadburnonearn"];

                    HttpPostedFileBase fileearn = Request.Files["FileuploadEarn"];
                    if (fileearn != null)
                    {
                        string fileName = "ProductUpload_" + objpointearn.GroupId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                        var path = ConfigurationManager.AppSettings["CustomerDocuments"].ToString();
                        var fullFilePath = path + "\\" + fileName;
                        fileearn.SaveAs(path + "\\" + fileName);

                        string conString = string.Empty;

                        conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fullFilePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";

                        using (OleDbConnection connExcel = new OleDbConnection(conString))
                        {
                            using (OleDbCommand cmdExcel = new OleDbCommand())
                            {
                                using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                                {
                                    cmdExcel.Connection = connExcel;
                                    //Get the name of First Sheet.
                                    connExcel.Open();
                                    DataTable dtExcelSchema;
                                    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                    connExcel.Close();

                                    //Read Data from First Sheet.
                                    connExcel.Open();
                                    cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                    odaExcel.SelectCommand = cmdExcel;
                                    odaExcel.Fill(ds);
                                    connExcel.Close();
                                }
                            }
                        }
                        ds.Tables[0].Columns.Add("GroupId");
                        ds.Tables[0].Columns.Add("BrandId");
                        ds.Tables[0].Columns.Add("CategoryId");
                        ds.Tables[0].Columns.Add("UploadType");
                        ds.Tables[0].Columns.Add("AddedBy");
                        ds.Tables[0].Columns.Add("AddedDate");
                        ds.Tables[0].Columns.Add("UpdatedBy");
                        ds.Tables[0].Columns.Add("UpdatedDate");
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            dr["GroupId"] = objpointearn.GroupId;
                            dr["BrandId"] = objpointearn.BrandId;
                            dr["CategoryId"] = objpointearn.CategoryId;
                            dr["UploadType"] = "Earn";
                            dr["AddedBy"] = userDetails.LoginId;
                            dr["AddedDate"] = DateTime.Now;
                            dr["UpdatedBy"] = "";
                            dr["UpdatedDate"] = DateTime.Now;
                        }

                    }
                    if (fileburnonearn != null)
                    {
                        DataTable dt = new DataTable();
                        string fileName = "ProductUploadForBUrn_" + objpointearn.GroupId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                        var path = ConfigurationManager.AppSettings["CustomerDocuments"].ToString();
                        var fullFilePath = path + "\\" + fileName;
                        fileearn.SaveAs(path + "\\" + fileName);

                        string conString = string.Empty;

                        conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fullFilePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";

                        using (OleDbConnection connExcel = new OleDbConnection(conString))
                        {
                            using (OleDbCommand cmdExcel = new OleDbCommand())
                            {
                                using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                                {
                                    cmdExcel.Connection = connExcel;
                                    //Get the name of First Sheet.
                                    connExcel.Open();
                                    DataTable dtExcelSchema;
                                    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                    connExcel.Close();

                                    //Read Data from First Sheet.
                                    connExcel.Open();
                                    cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                    odaExcel.SelectCommand = cmdExcel;
                                    odaExcel.Fill(dt);
                                    connExcel.Close();
                                }
                            }
                        }
                        dt.Columns.Add("GroupId");
                        dt.Columns.Add("BrandId");
                        dt.Columns.Add("CategoryId");
                        dt.Columns.Add("UploadType");
                        dt.Columns.Add("AddedBy");
                        dt.Columns.Add("AddedDate");
                        dt.Columns.Add("UpdatedBy");
                        dt.Columns.Add("UpdatedDate");
                        foreach (DataRow dr in dt.Rows)
                        {
                            dr["GroupId"] = objpointearn.GroupId;
                            dr["BrandId"] = objpointearn.BrandId;
                            dr["CategoryId"] = objpointearn.CategoryId;
                            dr["UploadType"] = "Burn";
                            dr["AddedBy"] = userDetails.LoginId;
                            dr["AddedDate"] = DateTime.Now;
                            dr["UpdatedBy"] = "";
                            dr["UpdatedDate"] = DateTime.Now;
                        }
                        ds.Tables.Add(dt);
                    }

                }
                if (objpointearn.EarnPointLevelType == "Making")
                {
                    status = OBR.AddEarnAndBurnRule(objpointearn, slab, slabtype, makingslabdirectortelescopic, makingSlabdirectortele, userDetails.LoginId, ds, objpointburn);
                }
                if (objpointearn.EarnPointLevelType == "FullAmount")
                {
                    status = OBR.AddEarnAndBurnRule(objpointearn, slab, slabtype, fullamtslabdirectortelescopic, fullamtSlabdirectortele, userDetails.LoginId, ds, objpointburn);
                }
                if (objpointearn.EarnPointLevelType == "commonFixed" || objpointearn.EarnPointLevelType == "commonSlab")
                {
                    status = OBR.AddEarnAndBurnRule(objpointearn, slab, slabtype, commonlabdirectortelescopic, CommonSlabdirectortele, userDetails.LoginId, ds, objpointburn);
                }
            }
            catch (Exception ex)
            {

            }

            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult SaveVelocityCheckConfig(string jsonData)
        {
            bool status = false;
            try
            {
                string groupId = string.Empty;
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objVelocityChecksConfigData = (object[])json_serializer.DeserializeObject(jsonData);
                List<BOTS_TblVelocityChecksConfig> objData = new List<BOTS_TblVelocityChecksConfig>();
                foreach (Dictionary<string, object> item in objVelocityChecksConfigData)
                {
                    var TxnCountConfig = (object[])item["TxnCountConfig"];
                    var IACConfig = (object[])item["IACConfig"];
                    var IAConfig = (object[])item["IAConfig"];
                    foreach (Dictionary<string, object> item1 in TxnCountConfig)
                    {
                        BOTS_TblVelocityChecksConfig objItem = new BOTS_TblVelocityChecksConfig();
                        objItem.GroupId = Convert.ToString(item1["GroupId"]);
                        groupId = Convert.ToString(item1["GroupId"]);
                        objItem.VelocityType = 1;
                        objItem.CountFrom = Convert.ToInt32(item1["From"]);
                        objItem.CountTo = Convert.ToInt32(item1["To"]);
                        objItem.LastDays = Convert.ToInt32(item1["LastDays"]);
                        objItem.Action = Convert.ToString(item1["Action"]);
                        if (objItem.Id > 0)
                        {
                            objItem.UpdatedBy = userDetails.LoginId;
                            objItem.UpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            objItem.AddedBy = userDetails.LoginId;
                            objItem.AddedDate = DateTime.Now;
                        }
                        objData.Add(objItem);
                    }
                    foreach (Dictionary<string, object> item1 in IACConfig)
                    {
                        BOTS_TblVelocityChecksConfig objItem = new BOTS_TblVelocityChecksConfig();
                        objItem.GroupId = Convert.ToString(item1["GroupId"]);
                        groupId = Convert.ToString(item1["GroupId"]);
                        objItem.VelocityType = 2;
                        objItem.CountFrom = Convert.ToInt32(item1["From"]);
                        objItem.CountTo = Convert.ToInt32(item1["To"]);
                        objItem.LastDays = Convert.ToInt32(item1["LastDays"]);
                        objItem.Action = Convert.ToString(item1["Action"]);
                        if (objItem.Id > 0)
                        {
                            objItem.UpdatedBy = userDetails.LoginId;
                            objItem.UpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            objItem.AddedBy = userDetails.LoginId;
                            objItem.AddedDate = DateTime.Now;
                        }
                        objData.Add(objItem);
                    }
                    foreach (Dictionary<string, object> item1 in IAConfig)
                    {
                        BOTS_TblVelocityChecksConfig objItem = new BOTS_TblVelocityChecksConfig();
                        objItem.GroupId = Convert.ToString(item1["GroupId"]);
                        groupId = Convert.ToString(item1["GroupId"]);
                        objItem.VelocityType = 3;
                        objItem.CountFrom = Convert.ToInt32(item1["From"]);
                        objItem.CountTo = Convert.ToInt32(item1["To"]);
                        objItem.LastDays = Convert.ToInt32(item1["LastDays"]);
                        objItem.Action = Convert.ToString(item1["Action"]);
                        if (objItem.Id > 0)
                        {
                            objItem.UpdatedBy = userDetails.LoginId;
                            objItem.UpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            objItem.AddedBy = userDetails.LoginId;
                            objItem.AddedDate = DateTime.Now;
                        }
                        objData.Add(objItem);
                    }
                }
                status = OBR.SaveVelocityCheckConfig(objData, groupId);
            }
            catch (Exception ex)
            {

            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult GetVelocityCheckData(string groupId)
        {
            List<BOTS_TblVelocityChecksConfig> objData = new List<BOTS_TblVelocityChecksConfig>();
            objData = OBR.GetVelocityChecksData(groupId);

            return new JsonResult() { Data = objData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public ActionResult UploadCustomers(string groupId)
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    DataSet ds = new DataSet();
                    HttpPostedFileBase files = Request.Files[0];
                    string fileName = "CustomerUpload_" + groupId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                    var path = ConfigurationManager.AppSettings["CustomerDocuments"].ToString();
                    var fullFilePath = path + "\\" + fileName;
                    files.SaveAs(path + "\\" + fileName);

                    string conString = string.Empty;

                    conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fullFilePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;
                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                connExcel.Close();

                                //Read Data from First Sheet.
                                connExcel.Open();
                                cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(ds);
                                connExcel.Close();
                            }
                        }
                    }
                    ds.Tables[0].Columns.Add("GroupId");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr["GroupId"] = groupId;
                    }

                    var status = OBR.BulkInsert(ds.Tables[0]);

                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "UploadCustomers");
                }

            }
            return Json("File Not Uploaded Successfully!");

        }

        public ActionResult SaveCampaignOtherConfig(string jsonData)
        {
            bool status = false;
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                BOTS_TblCampaignOtherConfig objData = new BOTS_TblCampaignOtherConfig();
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objBirthdayAndAnniversaryConfigData = (object[])json_serializer.DeserializeObject(jsonData);

                foreach (Dictionary<string, object> item in objBirthdayAndAnniversaryConfigData)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(item["Id"])))
                    {
                        objData.Id = Convert.ToInt32(item["Id"]);
                    }
                    objData.GroupId = Convert.ToString(item["GroupId"]);
                    objData.CampaignType = Convert.ToString(item["CampaignType"]);
                    objData.SMSType = Convert.ToString(item["SMSType"]);
                    objData.BonusPoints = Convert.ToInt32(item["BonusPoints"]);
                    objData.Frequency = Convert.ToString(item["Frequency"]);

                    objData.IntroDays1 = Convert.ToInt32(item["IntroDays1"]);
                    objData.IntroScript1 = Convert.ToString(item["IntroScript1"]);
                    objData.IntroDays2 = Convert.ToInt32(item["IntroDays2"]);
                    objData.IntroScript2 = Convert.ToString(item["IntroScript2"]);
                    objData.ReminderDays1 = Convert.ToInt32(item["ReminderDays1"]);
                    objData.ReminderWhen1 = Convert.ToString(item["ReminderWhen1"]);
                    objData.ReminderScript1 = Convert.ToString(item["ReminderScript1"]);
                    objData.ReminderDays2 = Convert.ToInt32(item["ReminderDays2"]);
                    objData.ReminderWhen2 = Convert.ToString(item["ReminderWhen2"]);
                    objData.ReminderScript2 = Convert.ToString(item["ReminderScript2"]);
                    objData.OnDayType = Convert.ToString(item["OnDayType"]);
                    objData.OnDayScript = Convert.ToString(item["OnDayScript"]);

                    if (objData.Id > 0)
                    {
                        objData.UpdatedBy = userDetails.LoginId;
                        objData.UpdatedDate = DateTime.Now;
                    }
                    else
                    {
                        objData.AddedBy = userDetails.LoginId;
                        objData.AddedDate = DateTime.Now;
                    }
                    status = OBR.SaveCampaignOtherConfig(objData);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveBirthdayAndAnniversaryConfig");
            }

            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult GetCampaignOtherConfig(string GroupId, string Type)
        {
            BOTS_TblCampaignOtherConfig objData = new BOTS_TblCampaignOtherConfig();
            objData = OBR.GetCampaignOtherConfig(GroupId, Type);
            return new JsonResult() { Data = objData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult SaveInactiveConfig(string jsonData)
        {
            bool status = false;
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                List<BOTS_TblCampaignInactive> lstNewData = new List<BOTS_TblCampaignInactive>();

                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objInactiveConfigData = (object[])json_serializer.DeserializeObject(jsonData);
                string groupID = string.Empty;
                string type = string.Empty;
                foreach (Dictionary<string, object> item in objInactiveConfigData)
                {
                    BOTS_TblCampaignInactive objData = new BOTS_TblCampaignInactive();
                    if (!string.IsNullOrEmpty(Convert.ToString(item["Id"])))
                    {
                        objData.Id = Convert.ToInt32(item["Id"]);
                    }
                    objData.GroupId = Convert.ToString(item["GroupId"]);
                    objData.InactiveType = Convert.ToString(item["InactiveType"]);
                    objData.SMSorWA = Convert.ToString(item["SMSorWA"]);
                    objData.Days = Convert.ToInt32(item["Days"]);
                    objData.LessThanDays = Convert.ToInt32(item["LessThanDays"]);
                    objData.LessThanDaysScript = Convert.ToString(item["LessThanDaysScript"]);
                    objData.GreaterThanDays = Convert.ToInt32(item["GreaterThanDays"]);
                    objData.GreaterThanDaysScript = Convert.ToString(item["GreaterThanDaysScript"]);

                    if (objData.Id > 0)
                    {
                        objData.UpdatedBy = userDetails.LoginId;
                        objData.UpdatedDate = DateTime.Now;
                    }
                    else
                    {
                        objData.AddedBy = userDetails.LoginId;
                        objData.AddedDate = DateTime.Now;
                    }
                    groupID = objData.GroupId;
                    type = objData.InactiveType;
                    lstNewData.Add(objData);
                }
                status = OBR.SaveInactiveConfig(lstNewData, groupID, type);

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveBirthdayAndAnniversaryConfig");
            }

            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult GetInactiveConfig(string GroupId, string Type)
        {
            List<BOTS_TblCampaignInactive> lstExistingData = new List<BOTS_TblCampaignInactive>();
            lstExistingData = OBR.GetInactiveConfigData(GroupId, Type);
            return new JsonResult() { Data = lstExistingData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult GetEarnBurnDataAndUploaddata(string Groupid)
        {
            OnBoardingSalesViewModel objonboardingviewmodel = new OnBoardingSalesViewModel();
            objonboardingviewmodel.objpointsburnruleconfig = OBR.GetAllBurnRuleData(Groupid);
            objonboardingviewmodel.objpointsearnruleconfig = OBR.GetAllEarnRuleData(Groupid);
            objonboardingviewmodel.lstearnpointslabconfig = OBR.GetPointsSlabData(Groupid);
            return new JsonResult() { Data = objonboardingviewmodel, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };

        }

    }
}