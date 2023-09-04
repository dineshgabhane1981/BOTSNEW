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
using System.Net;
using System.Net.Mime;
using Rotativa;
using System.IO;

namespace WebApp.Controllers.OnBoarding
{
    public class CustomerOnBoardingController : Controller
    {
        CustomerRepository CR = new CustomerRepository();
        OnBoardingRepository OBR = new OnBoardingRepository();
        SalesLeadRepository SLR = new SalesLeadRepository();
        DashboardRepository DR = new DashboardRepository();
        Exceptions newexception = new Exceptions();
        // GET: CustomerOnBoarding
        public ActionResult Index(string groupId, string LeadId)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            if (userDetails != null)
            {
                if (userDetails.LoginType == "11")
                {
                    return RedirectToAction("DLTView", "CustomerOnBoarding", new { GroupId = groupId });
                }
            }
            CommonFunctions common = new CommonFunctions();
            if (!string.IsNullOrEmpty(groupId))
            {
                groupId = common.DecryptString(groupId);
            }

            if (!string.IsNullOrEmpty(groupId))
            {
                var GroupDetails = OBR.GetGroupMasterDetails(groupId);
                if (GroupDetails.CustomerStatus == "Submit For Approval" || GroupDetails.CustomerStatus == "Approved" || GroupDetails.CustomerStatus == "Send For Customer Approval" || GroupDetails.CustomerStatus == "Approved By Customer")
                {
                    var GroupIdOld = common.EncryptString(groupId);
                    return RedirectToAction("CheckerView", "CustomerOnBoarding", new { GroupId = GroupIdOld });
                }
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
                    objData.lstCommunicationSet = OBR.GetCommunicationSetsByGroupId(groupId);
                    objData.IsWA = false;
                    var dataWA = OBR.GetCommunicationWAConfigByGroupId(groupId);
                    if (dataWA != null)
                    {
                        if (dataWA.Count > 0)
                        {
                            objData.IsWA = true;
                        }
                    }

                    //Earn Data Fetch
                    objData.objEarnRuleConfig = OBR.GetEarnRuleConfig(groupId);
                    if (objData.objEarnRuleConfig == null)
                    {
                        BOTS_TblEarnRuleConfig objEarnRule = new BOTS_TblEarnRuleConfig();
                        objData.objEarnRuleConfig = objEarnRule;
                    }

                    //Burn Data Fetch
                    objData.objBurnRuleConfig = OBR.GetBurnRuleConfig(groupId);
                    if (objData.objBurnRuleConfig == null)
                    {
                        BOTS_TblBurnRuleConfig objBurnRule = new BOTS_TblBurnRuleConfig();
                        objData.objBurnRuleConfig = objBurnRule;
                    }

                    objData.lstSlabConfig = OBR.GetEarnRuleSlabConfig(groupId);

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
                    //Documents
                    objData.lstOtherDocs = OBR.GetOtherDocuments(groupId);
                }
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

                    objData.objDLCLinkConfig = OBR.GetDLCLinkData(GroupId);

                    if (objData.objDLCLinkConfig == null)
                    {
                        BOTS_TblDLCLinkConfig objDLCConfig = new BOTS_TblDLCLinkConfig();
                        objData.objDLCLinkConfig = objDLCConfig;
                    }
                    ViewBag.TempleteType = objData.TempleteType();

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

                    //objData.lstSMSConfig = OBR.GetCommunicationSMSConfigForDLT(GroupId);
                    //var BrandCount = objData.lstSMSConfig.Where(x => x.BrandId != "All").Count();
                    //if (BrandCount > 0)
                    //{
                    //    objData.IsBrand = true;
                    //}
                    //else
                    //{
                    //    objData.IsBrand = false;
                    //}

                }
            }
            return View(objData);
        }

        public ActionResult GetDLTCommunicationSetData(string GroupId, string SetId)
        {
            OnBoardingSalesViewModel objData = new OnBoardingSalesViewModel();
            objData.lstSMSConfig = OBR.GetCommunicationSMSConfigForDLT(GroupId, Convert.ToInt32(SetId));
            return PartialView("_DLTCommunication", objData);

        }

        public ActionResult GetConvertedScript(string CSScript)
        {
            var convertedScript = OBR.GetConvertedScript(CSScript);
            return new JsonResult() { Data = convertedScript, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };

        }

        public BOTS_TblGroupMaster MeargeLeadData(SALES_tblLeads leadDetails)
        {
            BOTS_TblGroupMaster objGroupDetails = new BOTS_TblGroupMaster();
            objGroupDetails.GroupName = leadDetails.BusinessName;
            objGroupDetails.OwnerName = leadDetails.AuthorizedPerson;
            objGroupDetails.OwnerMobileNo = leadDetails.MobileNo;
            objGroupDetails.City = leadDetails.City;
            objGroupDetails.OwnerEmailId = leadDetails.EmailId;
            objGroupDetails.IsFromLead = "Yes";

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
                if (objData.bots_TblGroupMaster.SINo > 0)
                {
                    objData.bots_TblGroupMaster.UpdatedBy = userDetails.LoginId;
                    objData.bots_TblGroupMaster.CreatedDate = DateTime.Now;
                }
                else
                {
                    objData.bots_TblGroupMaster.CreatedBy = userDetails.LoginId;
                    objData.bots_TblGroupMaster.CreatedDate = DateTime.Now;
                }
                objData.bots_TblGroupMaster.IsLive = false;
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
                    //SendSalesEmailToCustomer(Convert.ToInt32(GroupdId));
                }
                //SendSalesEmailToCustomer(Convert.ToInt32(GroupdId));
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
                string BillingProduct = string.Empty;
                foreach (var item in RetailList)
                {
                    if (count == 1)
                    {
                        Category = item.CategoryName;
                        NoOfOutlets = Convert.ToString(item.NoOfEnrolled);
                        BillingPartner = item.BillingPartner;
                        Product = item.BOProduct;
                        BillingProduct = item.BillingProduct;
                    }
                    else
                    {
                        Category += "," + item.CategoryName;
                        NoOfOutlets += "," + Convert.ToString(item.NoOfEnrolled);
                        BillingPartner += "," + item.BillingPartner;
                        Product += "," + item.BOProduct;
                        BillingProduct += "," + item.BillingProduct;
                    }
                    count++;
                }

                sb.Append("<tr>");
                sb.Append("<td>BO Product:</td><td>" + Product + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>Billing Partner:</td><td>" + BillingPartner + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>Billing Product:</td><td>" + BillingProduct + "</td>");
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
                sb.Append("<td>Sales Person:</td><td>" + Convert.ToString(GroupDetails.SourceByName) + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>Comments:</td><td>" + GroupDetails.Comments + "</td>");
                sb.Append("</tr>");

                sb.Append("</table>");


                //var userName = ConfigurationManager.AppSettings["FrmEmailOnboarding"].ToString();
                //var password = ConfigurationManager.AppSettings["FrmEmailOnboardingPwd"].ToString();
                //SmtpClient smtp = new SmtpClient();
                //smtp.UseDefaultCredentials = true;
                //smtp.Credentials = new System.Net.NetworkCredential(userName, password);
                //smtp.Host = "smtp.zoho.com";
                //smtp.Port = 587;
                //smtp.EnableSsl = true;
                //var userDetails = (CustomerLoginDetail)Session["UserSession"];

                //MailMessage email = new MailMessage();
                //MailAddress from = new MailAddress(userName);
                //email.From = from;
                //foreach (var item in emailIds)
                //{
                //    email.To.Add(item);
                //}

                //email.Subject = "New Customer Onboarded - " + GroupDetails.GroupName;
                //email.SubjectEncoding = System.Text.Encoding.Default;

                //email.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                //email.Body = sb.ToString();
                //email.IsBodyHtml = true;
                //email.Priority = MailPriority.High;
                //smtp.Send(email);

                var from = ConfigurationManager.AppSettings["FrmEmailOnboarding"].ToString();
                var PWD = ConfigurationManager.AppSettings["FrmEmailOnboardingPwd"].ToString();
                MailMessage mail = new MailMessage();
                MailAddress fromMail = new MailAddress(from);
                mail.From = fromMail;
                foreach (var item in emailIds)
                {
                    mail.To.Add(item);
                }
                //mail.From = from;
                mail.Subject = "New Customer Onboarded - " + GroupDetails.GroupName;
                mail.SubjectEncoding = System.Text.Encoding.Default;
                mail.Body = sb.ToString();
                mail.IsBodyHtml = true;
                mail.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.zoho.com";
                smtp.EnableSsl = true;
                NetworkCredential networkCredential = new NetworkCredential(from, PWD);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = networkCredential;
                smtp.Port = 587;
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "Onboarding Email Error");
            }
        }

        public void SendSalesEmailToCustomer(int groupId)
        {
            OnBoardingSalesViewModel objData = new OnBoardingSalesViewModel();
            try
            {
                CommonFunctions common = new CommonFunctions();
                var NewgroupId = Convert.ToString(groupId);
                objData.bots_TblGroupMaster = OBR.GetGroupMasterDetails(NewgroupId);
                objData.bots_TblDealDetails = OBR.GetDealMasterDetails(NewgroupId);
                objData.bots_TblPaymentDetails = OBR.GetPaymentDetails(NewgroupId);
                objData.objRetailList = OBR.GetRetailDetails(NewgroupId);
                var a = new ViewAsPdf();
                a.ViewName = "CustomerTerms";
                a.Model = objData;

                var pdfBytes = a.BuildFile(this.ControllerContext);

                // Optionally save the PDF to server in a proper IIS location.
                var fileName = objData.bots_TblGroupMaster.GroupName + ".pdf";
                var path = Server.MapPath("~/Temp/" + fileName);
                System.IO.File.WriteAllBytes(path, pdfBytes);

                StringBuilder sb = new StringBuilder();
                sb.Append("Dear <b>" + objData.bots_TblGroupMaster.OwnerName + "</b>,");
                sb.Append("<br/>");
                sb.Append("<br/>");
                sb.Append("Retail Name : <b>" + objData.bots_TblGroupMaster.GroupName + "</b>");
                sb.Append("<br/>");
                sb.Append("<br/>");
                sb.Append("Legal Name : <b>" + objData.bots_TblGroupMaster.GroupName + "</b>");
                sb.Append("<br/>");
                sb.Append("<br/>");
                sb.Append("City : <b>" + objData.bots_TblGroupMaster.CityName + "</b>");
                sb.Append("<br/>");
                sb.Append("<br/>");
                sb.Append("Outlet Count : <b>" + objData.objRetailList[0].NoOfEnrolled + "</b> Outlets");
                sb.Append("<br/>");
                sb.Append("<br/>");
                sb.Append("We appreciate your decision to join hands with <b>Blue Ocktopus</b>, which has helped 200+ retail friends to take advantage of our sophisticated Loyalty & Data driven tool. We are confident that we can replicate the same for you, as well!!");
                sb.Append("<br/>");
                sb.Append("<br/>");
                sb.Append("Attached for your reference are the Programme commercials, timelines and key things for your ready perusal.");
                sb.Append("<br/>");
                sb.Append("<br/>");
                sb.Append("We value you as our esteemed partner & look forward for a fruitful business association.");
                sb.Append("<br/>");
                sb.Append("<br/>");
                sb.Append("For any assistance required, please feel free to connect with us.");
                sb.Append("<br/>");
                sb.Append("<br/>");
                sb.Append("Thanks & Regards,");
                sb.Append("<br/>");
                sb.Append("<br/>");
                sb.Append("BLUE OCKTOPUS.");
                sb.Append("<br/>");
                sb.Append("<br/>");
                sb.Append("<br/>");
                sb.Append("<br/>");

                sb.Append("** This is an Auto generated email. Do not reply to this email id.");

                var from = ConfigurationManager.AppSettings["FrmEmailOnboarding"].ToString();
                var PWD = ConfigurationManager.AppSettings["FrmEmailOnboardingPwd"].ToString();
                MailMessage mail = new MailMessage();
                MailAddress fromMail = new MailAddress(from);
                mail.From = fromMail;

                mail.To.Add(objData.bots_TblGroupMaster.OwnerEmailId);
                string assignedCSEmail = OBR.GetEmailAssignedCS(objData.bots_TblGroupMaster.AssignedCS);
                string SourceEmail = OBR.GetEmailSourceBy(objData.bots_TblGroupMaster.SourcedBy);
                mail.CC.Add(assignedCSEmail);
                mail.CC.Add(SourceEmail);
                var CCEmails = ConfigurationManager.AppSettings["EmailsForOnboarding"].ToString();
                var CCEmailAll = CCEmails.Split(',');
                foreach(var item in CCEmailAll)
                {
                    mail.CC.Add(item);
                }
                
                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(path);
                mail.Attachments.Add(attachment);

                //mail.From = from;
                mail.Subject = "Welcome to Blue Ocktopus | Programme Terms & Key Details";// + GroupDetails.GroupName;
                mail.SubjectEncoding = System.Text.Encoding.Default;
                mail.Body = sb.ToString();
                mail.IsBodyHtml = true;
                mail.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.zoho.com";
                smtp.EnableSsl = true;
                NetworkCredential networkCredential = new NetworkCredential(from, PWD);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = networkCredential;
                smtp.Port = 587;
                smtp.Send(mail);
            }
            catch(Exception ex)
            {
                newexception.AddException(ex, Convert.ToString(groupId));
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

        public JsonResult GetSMSandWAData(string groupId, string setId)
        {
            CommunicationConfigViewModel objData = new CommunicationConfigViewModel();
            objData.SMSConfig = OBR.GetCommunicationSMSConfig(groupId, Convert.ToInt32(setId));
            objData.WAConfig = OBR.GetCommunicationWAConfig(groupId, Convert.ToInt32(setId));
            objData.objSetDetails = OBR.GetSetDetails(Convert.ToInt32(setId));
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
                BOTS_TblCommunicationSet objSetDetails = new BOTS_TblCommunicationSet();
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objCommConfigData = (object[])json_serializer.DeserializeObject(jsonData);
                foreach (Dictionary<string, object> item in objCommConfigData)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(item["SetId"])))
                        objSetDetails.SetId = Convert.ToInt32(item["SetId"]);
                    objSetDetails.SetName = Convert.ToString(item["SetName"]);
                    objSetDetails.GroupId = Convert.ToString(item["GroupId"]);
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
                        if (!string.IsNullOrEmpty(Convert.ToString(item["SMSSetId"])))
                            objSMSConfig.SetId = Convert.ToInt32(item["SMSSetId"]);

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

                        //SMSTemplate objSMSTemplate4 = new SMSTemplate();
                        //objSMSTemplate4.MessageId = 103;
                        //if (!string.IsNullOrEmpty(Convert.ToString(item["SMSCancelEarnId"])))
                        //    objSMSTemplate4.Id = Convert.ToInt32(item["SMSCancelEarnId"]);
                        //objSMSTemplate4.TemplateScript = Convert.ToString(item["SMSCancelEarn"]);
                        //lstSMSTemplate.Add(objSMSTemplate4);

                        //SMSTemplate objSMSTemplate5 = new SMSTemplate();
                        //objSMSTemplate5.MessageId = 104;
                        //if (!string.IsNullOrEmpty(Convert.ToString(item["SMSCancelBurnId"])))
                        //    objSMSTemplate5.Id = Convert.ToInt32(item["SMSCancelBurnId"]);
                        //objSMSTemplate5.TemplateScript = Convert.ToString(item["SMSCancelBurn"]);
                        //lstSMSTemplate.Add(objSMSTemplate5);

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
                        //objWAConfig.WANumber = Convert.ToString(item["WANumber"]);
                        objWAConfig.WAUsername = Convert.ToString(item["WAUserName"]);
                        objWAConfig.WAPassword = Convert.ToString(item["WAPassword"]);
                        objWAConfig.WAlink = Convert.ToString(item["WALink"]);
                        objWAConfig.TokenId = Convert.ToString(item["WATokenId"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(item["WASetId"])))
                            objWAConfig.SetId = Convert.ToInt32(item["WASetId"]);

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

                        //SMSTemplate objWATemplate4 = new SMSTemplate();
                        //objWATemplate4.MessageId = 103;
                        //if (!string.IsNullOrEmpty(Convert.ToString(item["WACancelEarnId"])))
                        //    objWATemplate4.Id = Convert.ToInt32(item["WACancelEarnId"]);
                        //objWATemplate4.TemplateScript = Convert.ToString(item["WACancelEarn"]);
                        //lstWATemplate.Add(objWATemplate4);

                        //SMSTemplate objWATemplate5 = new SMSTemplate();
                        //objWATemplate5.MessageId = 104;
                        //if (!string.IsNullOrEmpty(Convert.ToString(item["WACancelBurnId"])))
                        //    objWATemplate5.Id = Convert.ToInt32(item["WACancelBurnId"]);
                        //objWATemplate5.TemplateScript = Convert.ToString(item["WACancelBurn"]);
                        //lstWATemplate.Add(objWATemplate5);

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
                status = OBR.SaveCommunicationConfig(objSMSConfig, objWAConfig, objSetDetails, lstSMSTemplate, lstWATemplate, userDetails.LoginId);

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

        public ActionResult UpdateDLTStatusOfCommunicationConfig(string ConfigId, string DLTNewStatus, string RejectReason, string jsonData)
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
                status = OBR.UpdateStatusSMSConfig(Convert.ToInt32(ConfigId), DLTNewStatus, userDetails.LoginId, RejectReason, objSMSConfig);
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
                    //objDLCLink.MaxNoOfReferrals = Convert.ToInt32(item["MaxNoOfReferrals"]);
                    objDLCLink.ValidityOfReferralPoints = Convert.ToInt32(item["ValidityOfReferralPoints"]);
                    objDLCLink.ReferralReminder = Convert.ToInt32(item["ReferralReminder"]);
                    objDLCLink.ToTheReferralSMSScript = Convert.ToString(item["SMSToTheReferral"]);
                    objDLCLink.ReminderForPointsUsageSMSScript = Convert.ToString(item["SMSReminderForPointsUsage1"]);
                    objDLCLink.ReminderForPointsUsageSMSScript8 = Convert.ToString(item["SMSReminderForPointsUsage2"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["SMSReminderForPointsUsageDays1"])))
                        objDLCLink.ReminderForPointsUsageSMSScript1Days = Convert.ToInt32(item["SMSReminderForPointsUsageDays1"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(item["SMSReminderForPointsUsageDays2"])))
                        objDLCLink.ReminderForPointsUsageSMSScript2Days = Convert.ToInt32(item["SMSReminderForPointsUsageDays2"]);

                    objDLCLink.ReferredSuccessOnReferralTxnSMSScript = Convert.ToString(item["SMSReferredSuccessOnReferralTxn"]);
                    objDLCLink.DLCOTPScriptSMS = Convert.ToString(item["DLCOTPScriptSMS"]);
                    objDLCLink.GiftPointsOTPScriptSMS = Convert.ToString(item["GiftPointsOTPScriptSMS"]);
                    objDLCLink.GiftPointsDebitOTPScriptSMS = Convert.ToString(item["GiftPointsDebitOTPScriptSMS"]);
                    objDLCLink.GiftPointsCreditOTPScriptSMS = Convert.ToString(item["GiftPointsCreditOTPScriptSMS"]);
                    objDLCLink.ToTheReferralWAScript = Convert.ToString(item["ToTheReferralWAScript"]);
                    objDLCLink.ReminderForPointsUsageWAScript = Convert.ToString(item["ReminderForPointsUsageWAScript1"]);
                    objDLCLink.ReminderForPointsUsageWAScript8 = Convert.ToString(item["ReminderForPointsUsageWAScript2"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(item["ReminderForPointsUsageWAScriptDays1"])))
                        objDLCLink.ReminderForPointsUsageWAScript1Days = Convert.ToInt32(item["ReminderForPointsUsageWAScriptDays1"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(item["ReminderForPointsUsageWAScriptDays2"])))
                        objDLCLink.ReminderForPointsUsageWAScript2Days = Convert.ToInt32(item["ReminderForPointsUsageWAScriptDays2"]);


                    objDLCLink.ReferredSuccessOnReferralTxnWAScript = Convert.ToString(item["ReferredSuccessOnReferralTxnWAScript"]);
                    objDLCLink.DLCOTPScriptWA = Convert.ToString(item["DLCOTPScriptWA"]);
                    objDLCLink.GiftPointsOTPScriptWA = Convert.ToString(item["GiftPointsOTPScriptWA"]);
                    objDLCLink.GiftPointsDebitOTPScriptWA = Convert.ToString(item["GiftPointsDebitOTPScriptWA"]);
                    objDLCLink.GiftPointsCreditOTPScriptWA = Convert.ToString(item["GiftPointsCreditOTPScriptWA"]);
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

        public ActionResult SendDLCToDLT(string GroupId)
        {
            bool status = false;
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];

                status = OBR.SendDLCToDLT(GroupId, userDetails.LoginId);

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveDLCLinkConfigController");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult GetDLCLinkData(string groupId)
        {
            BOTS_TblDLCLinkConfig objDLCLinkConfig = new BOTS_TblDLCLinkConfig();
            objDLCLinkConfig = OBR.GetDLCLinkData(groupId);

            return new JsonResult() { Data = objDLCLinkConfig, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
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

        [HttpPost]
        public ActionResult UploadLogo(string groupId, string brandId)
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    var groupName = OBR.GetOnboardingGroupName(groupId);
                    var brandName = OBR.GetOnboardingBrandName(groupId, brandId);
                    string fileName = groupName + brandName + "Logo.png";
                    DataSet ds = new DataSet();
                    HttpPostedFileBase files = Request.Files[0];

                    var path = ConfigurationManager.AppSettings["LogoPhysicalURL"].ToString();

                    files.SaveAs(path + "\\" + fileName);

                    var URL = "https://blueocktopus.in/Logo/" + fileName;
                    var status = OBR.UploadBrandLogo(groupId, brandId, URL);

                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "UploadCustomers");
                }

            }
            return Json("File Not Uploaded Successfully!");

        }

        [HttpPost]
        public ActionResult UploadOtherDocs(string groupId, string docName)
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    var userDetails = (CustomerLoginDetail)Session["UserSession"];
                    var groupName = OBR.GetOnboardingGroupName(groupId);

                    HttpPostedFileBase files = Request.Files[0];
                    var fileExt = System.IO.Path.GetExtension(files.FileName).Substring(1);

                    string fileName = docName + "_" + groupName + "." + fileExt;
                    DataSet ds = new DataSet();


                    var path = ConfigurationManager.AppSettings["CustomerDocuments"].ToString();
                    files.SaveAs(path + "\\" + fileName);

                    var status = OBR.UploadOtherDocs(groupId, docName, fileName, userDetails.LoginId);

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
                    if (objData.CampaignType == "Birthday" || objData.CampaignType == "Anniversary")
                    {
                        objData.PointsValidBefore = Convert.ToInt32(item["PointsValidBefore"]);
                        objData.PointsValidAfter = Convert.ToInt32(item["PointsValidAfter"]);
                    }
                    objData.Frequency = Convert.ToString(item["Frequency"]);

                    objData.IntroDays1 = Convert.ToInt32(item["IntroDays1"]);
                    objData.IntroScript1 = Convert.ToString(item["IntroScript1"]);
                    objData.ReminderDays1 = Convert.ToInt32(item["ReminderDays1"]);
                    objData.ReminderWhen1 = Convert.ToString(item["ReminderWhen1"]);
                    objData.ReminderScript1 = Convert.ToString(item["ReminderScript1"]);
                    objData.ReminderDays2 = Convert.ToInt32(item["ReminderDays2"]);
                    objData.ReminderWhen2 = Convert.ToString(item["ReminderWhen2"]);
                    objData.ReminderScript2 = Convert.ToString(item["ReminderScript2"]);
                    objData.OnDayTypePT = Convert.ToString(item["OnDayTypePT"]);
                    objData.OnDayScriptPT = Convert.ToString(item["OnDayScriptPT"]);
                    objData.OnDayTypeNPT = Convert.ToString(item["OnDayTypeNPT"]);
                    objData.OnDayScriptNPT = Convert.ToString(item["OnDayScriptNPT"]);
                    if (objData.CampaignType == "Reminder Bulk Uploaded Users" || objData.CampaignType == "Balance Updates" || objData.CampaignType == "DLC Update Reminder" || objData.CampaignType == "DLC Referral Reminder")
                    {
                        objData.IntroDays2 = Convert.ToInt32(item["IntroDays2"]);
                        objData.IntroScript2 = Convert.ToString(item["IntroScript2"]);
                    }

                    if (objData.SMSType != "SMS")
                    {
                        if (objData.CampaignType == "Birthday" || objData.CampaignType == "Anniversary")
                        {
                            objData.SMSScript1 = Convert.ToString(item["SMSScript1"]);
                            objData.SMSScript3 = Convert.ToString(item["SMSScript3"]);
                            objData.SMSScript4 = Convert.ToString(item["SMSScript4"]);
                            objData.SMSScript5 = Convert.ToString(item["SMSScript5"]);
                            objData.SMSScript6 = Convert.ToString(item["SMSScript6"]);
                        }
                        if (objData.CampaignType == "Reminder Bulk Uploaded Users" || objData.CampaignType == "Balance Updates" || objData.CampaignType == "DLC Update Reminder" || objData.CampaignType == "DLC Referral Reminder")
                        {
                            objData.SMSScript1 = Convert.ToString(item["SMSScript1"]);
                            objData.SMSScript2 = Convert.ToString(item["SMSScript2"]);
                        }
                    }

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

                    if (objData.SMSorWA != "SMS")
                    {
                        if (objData.InactiveType == "Inactive" || objData.InactiveType == "Only Once Inactive" || objData.InactiveType == "Non Redemption Inactive" || objData.InactiveType == "Point Expiry")
                        {
                            objData.SMSScript1 = Convert.ToString(item["SMSScript1"]);
                            objData.SMSScript2 = Convert.ToString(item["SMSScript2"]);
                        }
                    }

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

        public ActionResult SaveCommunicationUniqueValuesConfig(string jsonData)
        {
            bool status = false;
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                BOTS_TblSMSConfig objSMSConfig = new BOTS_TblSMSConfig();

                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objInactiveConfigData = (object[])json_serializer.DeserializeObject(jsonData);
                string groupID = string.Empty;
                string type = string.Empty;
                foreach (Dictionary<string, object> item in objInactiveConfigData)
                {
                    objSMSConfig.GroupId = Convert.ToString(item["GroupID"]);
                    objSMSConfig.PEID = Convert.ToString(item["PEID"]);
                    objSMSConfig.SMSProvider = Convert.ToString(item["SMSProvider"]);
                    objSMSConfig.SMSSenderID = Convert.ToString(item["SMSSenderId"]);
                    objSMSConfig.SMSUsername = Convert.ToString(item["SMSUserName"]);
                    objSMSConfig.SMSPassword = Convert.ToString(item["SMSPassword"]);
                    objSMSConfig.SMSlink = Convert.ToString(item["SMSLink"]);
                }
                status = OBR.UpdateUniqueSMSValues(objSMSConfig, userDetails.LoginId);

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveBirthdayAndAnniversaryConfig");
            }

            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult GetCommunicationSetList(string GroupId)
        {
            var SetList = OBR.GetCommunicationSetList(GroupId);
            return new JsonResult() { Data = SetList, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult GetOutletListWithAssignment(string GroupId, string SetId)
        {
            var SetList = OBR.GetOutletListWithAssignment(GroupId, SetId);
            return new JsonResult() { Data = SetList, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult AssignCommunicationSetsToOutlets(string GroupId, string SetId, string OutletIds)
        {
            bool status = false;
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                List<BOTS_TblCommunicationSetAssignment> objData = new List<BOTS_TblCommunicationSetAssignment>();
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objOutletData = (object[])json_serializer.DeserializeObject(OutletIds);
                foreach (var item in objOutletData)
                {
                    BOTS_TblCommunicationSetAssignment objItem = new BOTS_TblCommunicationSetAssignment();
                    objItem.SetId = Convert.ToInt32(SetId);
                    objItem.GroupId = Convert.ToString(GroupId);
                    objItem.OutletId = Convert.ToString(item);
                    objItem.CreatedBy = userDetails.LoginId;
                    objItem.CreatedDate = DateTime.Now;
                    objData.Add(objItem);
                }
                status = OBR.AssignCommunicationSetsToOutlets(GroupId, Convert.ToInt32(SetId), objData);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AssignCommunicationSetsToOutlets");
            }

            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult SendPerpetualCampaignToDLT(string GroupId, string CampaignId, string CampaignType)
        {
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            status = OBR.SendPerpetualCampaignToDLT(GroupId, Convert.ToInt32(CampaignId), CampaignType, userDetails.LoginId);
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult GetVariableWordsList()
        {
            var objData = OBR.GetVariableWordsList();
            return PartialView("_VariableWordsList", objData);
        }

        public JsonResult AddVariableWords(string NewWord)
        {
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            status = OBR.AddVariableWord(NewWord);
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult GetCampaignOtherConfigForDLT(string GroupId, string Type)
        {
            OnBoardingSalesViewModel objDataView = new OnBoardingSalesViewModel();
            ViewBag.TempleteType = objDataView.TempleteType();
            var objData = OBR.GetCampaignOtherConfigForDLT(GroupId, Type);
            return PartialView("_DLTBirthdayAndAnniversary", objData);
        }

        public ActionResult UpdateBADLTStatus(string id, string statusid, string status, string reason)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            result = OBR.UpdateBADLTStatus(Convert.ToInt32(id), Convert.ToInt32(statusid), status, userDetails.LoginId, reason);
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult SaveBADLTConfig(string id, string statusid, string status, string jsonData)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];

            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objBAConfigData = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in objBAConfigData)
            {
                var IntroDays = Convert.ToString(item["IntroDays"]);
                var IntroDaysDLT = Convert.ToString(item["IntroDaysDLT"]);
                var TemplateId = Convert.ToString(item["TemplateId"]);
                var TemplateName = Convert.ToString(item["TemplateName"]);
                var TemplateType = Convert.ToString(item["TemplateType"]);
                result = OBR.SaveBADLTConfig(Convert.ToInt32(id), Convert.ToInt32(statusid), status, IntroDays, IntroDaysDLT, TemplateId, TemplateName, TemplateType, userDetails.LoginId);
            }

            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult GetCampaignAllInactiveDLT(string GroupId, string Type)
        {
            OnBoardingSalesViewModel objDataView = new OnBoardingSalesViewModel();
            ViewBag.TempleteType = objDataView.TempleteType();
            var objData = OBR.GetCampaignAllInactiveForDLT(GroupId, Type);
            return PartialView("_DLTInactiveAll", objData);
        }

        public ActionResult UpdateInactiveDLTStatus(string id, string statusid, string status, string reason)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            result = OBR.UpdateInactiveDLTStatus(Convert.ToInt32(id), Convert.ToInt32(statusid), status, userDetails.LoginId, reason);
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult SaveInactiveDLTConfig(string jsonData)
        {
            bool status = false;
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                BOTS_TblCampaignInactive objInactiveConfig = new BOTS_TblCampaignInactive();
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objInactiveConfigData = (object[])json_serializer.DeserializeObject(jsonData);
                foreach (Dictionary<string, object> item in objInactiveConfigData)
                {
                    var id = Convert.ToInt32(item["Id"]);
                    var num = Convert.ToInt32(item["statusId"]);
                    objInactiveConfig = OBR.GetInactiveConfigById(id);
                    if (objInactiveConfig != null)
                    {
                        if (num == 1)
                        {
                            if (objInactiveConfig.SMSorWA != "SMS")
                            {
                                objInactiveConfig.SMSScript1 = Convert.ToString(item["Script"]);
                            }
                            else
                            {
                                objInactiveConfig.LessThanDaysScript = Convert.ToString(item["Script"]);
                            }

                            objInactiveConfig.LessThanDaysScriptDLT = Convert.ToString(item["ScriptDLT"]);
                            objInactiveConfig.TemplateId1 = Convert.ToString(item["TemplateId"]);
                            objInactiveConfig.TemplateName1 = Convert.ToString(item["TemplateName"]);
                            objInactiveConfig.TemplateType1 = Convert.ToString(item["TemplateType"]);
                            if (Convert.ToString(item["Status"]) == "Approved")
                            {
                                objInactiveConfig.DLTStatus1 = "Approved";
                            }
                        }
                        if (num == 2)
                        {
                            if (objInactiveConfig.SMSorWA != "SMS")
                            {
                                objInactiveConfig.SMSScript2 = Convert.ToString(item["Script"]);
                            }
                            else
                            {
                                objInactiveConfig.GreaterThanDaysScript = Convert.ToString(item["Script"]);
                            }
                            objInactiveConfig.GreaterThanDaysScriptDLT = Convert.ToString(item["ScriptDLT"]);
                            objInactiveConfig.TemplateId2 = Convert.ToString(item["TemplateId"]);
                            objInactiveConfig.TemplateName2 = Convert.ToString(item["TemplateName"]);
                            objInactiveConfig.TemplateType2 = Convert.ToString(item["TemplateType"]);
                            if (Convert.ToString(item["Status"]) == "Approved")
                            {
                                objInactiveConfig.DLTStatus2 = "Approved";
                            }
                        }
                        objInactiveConfig.UpdatedBy = userDetails.LoginId;
                        objInactiveConfig.UpdatedDate = DateTime.Now;
                    }
                }
                status = OBR.SaveInactiveDLTConfig(objInactiveConfig);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveInactiveDLTConfig");
            }
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult GetCampaignRemainingDLT(string GroupId, string Type)
        {
            OnBoardingSalesViewModel objDataView = new OnBoardingSalesViewModel();
            ViewBag.TempleteType = objDataView.TempleteType();
            var objData = OBR.GetCampaignRemainingForDLT(GroupId, Type);
            return PartialView("_DLTRemainingAll", objData);
        }

        public ActionResult UpdateRemainingDLTStatus(string id, string statusid, string status, string reason)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            result = OBR.UpdateRemainingDLTStatus(Convert.ToInt32(id), Convert.ToInt32(statusid), status, userDetails.LoginId, reason);
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult UpdateDLCLinkDLTStatus(string id, string statusid, string status, string reason)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            result = OBR.UpdateDLCLinkDLTStatus(Convert.ToInt32(id), Convert.ToInt32(statusid), status, userDetails.LoginId, reason);
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult SaveDLCLinkDLTConfig(string id, string statusid, string status, string jsonData)
        {
            bool result = false;
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                BOTS_TblDLCLinkConfig objDLCLinkConfig = new BOTS_TblDLCLinkConfig();
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                foreach (Dictionary<string, object> item in objData)
                {
                    var Id = Convert.ToInt32(id);
                    var num = Convert.ToInt32(statusid);
                    objDLCLinkConfig = OBR.GetDLCLinkDLTConfigById(Id);
                    if (objDLCLinkConfig != null)
                    {
                        if (num == 1)
                        {
                            objDLCLinkConfig.ToTheReferralSMSScript = Convert.ToString(item["Script"]);
                            objDLCLinkConfig.ToTheReferralSMSScriptDLT = Convert.ToString(item["ScriptDLT"]);
                            objDLCLinkConfig.TemplateId1 = Convert.ToString(item["TemplateId"]);
                            objDLCLinkConfig.TemplateName1 = Convert.ToString(item["TemplateName"]);
                            objDLCLinkConfig.TemplateType1 = Convert.ToString(item["TemplateType"]);
                            if (Convert.ToString(status) == "Approved")
                            {
                                objDLCLinkConfig.DLTStatus1 = "Approved";
                            }
                        }
                        if (num == 2)
                        {
                            objDLCLinkConfig.ReminderForPointsUsageSMSScript = Convert.ToString(item["Script"]);
                            objDLCLinkConfig.ReminderForPointsUsageSMSScriptDLT = Convert.ToString(item["ScriptDLT"]);
                            objDLCLinkConfig.TemplateId2 = Convert.ToString(item["TemplateId"]);
                            objDLCLinkConfig.TemplateName2 = Convert.ToString(item["TemplateName"]);
                            objDLCLinkConfig.TemplateType2 = Convert.ToString(item["TemplateType"]);
                            if (Convert.ToString(status) == "Approved")
                            {
                                objDLCLinkConfig.DLTStatus2 = "Approved";
                            }
                        }
                        if (num == 3)
                        {
                            objDLCLinkConfig.ReferredSuccessOnReferralTxnSMSScript = Convert.ToString(item["Script"]);
                            objDLCLinkConfig.ReferredSuccessOnReferralTxnSMSScriptDLT = Convert.ToString(item["ScriptDLT"]);
                            objDLCLinkConfig.TemplateId3 = Convert.ToString(item["TemplateId"]);
                            objDLCLinkConfig.TemplateName3 = Convert.ToString(item["TemplateName"]);
                            objDLCLinkConfig.TemplateType3 = Convert.ToString(item["TemplateType"]);
                            if (Convert.ToString(status) == "Approved")
                            {
                                objDLCLinkConfig.DLTStatus3 = "Approved";
                            }
                        }
                        if (num == 4)
                        {
                            objDLCLinkConfig.DLCOTPScriptSMS = Convert.ToString(item["Script"]);
                            objDLCLinkConfig.DLCOTPScriptSMSDLT = Convert.ToString(item["ScriptDLT"]);
                            objDLCLinkConfig.TemplateId4 = Convert.ToString(item["TemplateId"]);
                            objDLCLinkConfig.TemplateName4 = Convert.ToString(item["TemplateName"]);
                            objDLCLinkConfig.TemplateType4 = Convert.ToString(item["TemplateType"]);
                            if (Convert.ToString(status) == "Approved")
                            {
                                objDLCLinkConfig.DLTStatus4 = "Approved";
                            }
                        }
                        if (num == 5)
                        {
                            objDLCLinkConfig.GiftPointsOTPScriptSMS = Convert.ToString(item["Script"]);
                            objDLCLinkConfig.GiftPointsOTPScriptSMSDLT = Convert.ToString(item["ScriptDLT"]);
                            objDLCLinkConfig.TemplateId5 = Convert.ToString(item["TemplateId"]);
                            objDLCLinkConfig.TemplateName5 = Convert.ToString(item["TemplateName"]);
                            objDLCLinkConfig.TemplateType5 = Convert.ToString(item["TemplateType"]);
                            if (Convert.ToString(status) == "Approved")
                            {
                                objDLCLinkConfig.DLTStatus5 = "Approved";
                            }
                        }

                        if (num == 6)
                        {
                            objDLCLinkConfig.GiftPointsDebitOTPScriptSMS = Convert.ToString(item["Script"]);
                            objDLCLinkConfig.GiftPointsDebitOTPScriptSMSDLT = Convert.ToString(item["ScriptDLT"]);
                            objDLCLinkConfig.TemplateId6 = Convert.ToString(item["TemplateId"]);
                            objDLCLinkConfig.TemplateName6 = Convert.ToString(item["TemplateName"]);
                            objDLCLinkConfig.TemplateType6 = Convert.ToString(item["TemplateType"]);
                            if (Convert.ToString(status) == "Approved")
                            {
                                objDLCLinkConfig.DLTStatus6 = "Approved";
                            }
                        }
                        if (num == 7)
                        {
                            objDLCLinkConfig.GiftPointsDebitOTPScriptSMS = Convert.ToString(item["Script"]);
                            objDLCLinkConfig.GiftPointsCreditOTPScriptSMSDLT = Convert.ToString(item["ScriptDLT"]);
                            objDLCLinkConfig.TemplateId7 = Convert.ToString(item["TemplateId"]);
                            objDLCLinkConfig.TemplateName7 = Convert.ToString(item["TemplateName"]);
                            objDLCLinkConfig.TemplateType7 = Convert.ToString(item["TemplateType"]);
                            if (Convert.ToString(status) == "Approved")
                            {
                                objDLCLinkConfig.DLTStatus7 = "Approved";
                            }
                        }
                        if (num == 8)
                        {
                            objDLCLinkConfig.ReminderForPointsUsageSMSScript8 = Convert.ToString(item["Script"]);
                            objDLCLinkConfig.ReminderForPointsUsageSMSScriptDLT8 = Convert.ToString(item["ScriptDLT"]);
                            objDLCLinkConfig.TemplateId8 = Convert.ToString(item["TemplateId"]);
                            objDLCLinkConfig.TemplateName8 = Convert.ToString(item["TemplateName"]);
                            objDLCLinkConfig.TemplateType8 = Convert.ToString(item["TemplateType"]);
                            if (Convert.ToString(status) == "Approved")
                            {
                                objDLCLinkConfig.DLTStatus8 = "Approved";
                            }
                        }

                        objDLCLinkConfig.UpdatedBy = userDetails.LoginId;
                        objDLCLinkConfig.UpdatedDate = DateTime.Now;
                    }
                }
                result = OBR.SaveDLCLinkDLTConfig(objDLCLinkConfig);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveDLCLinkDLTConfig(");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult SaveEarnRuleConfig(string jsonData)
        {
            bool result = false;
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                BOTS_TblEarnRuleConfig objEarnRule = new BOTS_TblEarnRuleConfig();
                List<BOTS_TblSlabConfig> lstSlab = new List<BOTS_TblSlabConfig>();
                List<BOTS_TblProductUpload> lstProdUpload = new List<BOTS_TblProductUpload>();
                List<BOTS_TblProductUpload> lstBlockEarnUpload = new List<BOTS_TblProductUpload>();
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);

                foreach (Dictionary<string, object> item in objData)
                {
                    objEarnRule.RuleId = Convert.ToInt32(item["RuleId"]);
                    objEarnRule.GroupId = Convert.ToString(item["GroupId"]);
                    objEarnRule.MinInvoiceAmt = Convert.ToInt32(item["MinInvoiceAmt"]);
                    objEarnRule.PointsValueInRS = Convert.ToDecimal(item["PointsValueInRS"]);
                    objEarnRule.PointsValidityInMonths = Convert.ToInt32(item["PointsValidityInMonths"]);
                    objEarnRule.RevolvingExpiry = Convert.ToBoolean(item["RevolvingExpiry"]);
                    objEarnRule.IsDiscountPoints = Convert.ToBoolean(item["IsDiscountPoints"]);
                    objEarnRule.IsBase = Convert.ToBoolean(item["IsBase"]);
                    objEarnRule.IsSlab = Convert.ToBoolean(item["IsSlab"]);
                    objEarnRule.IsProductWise = Convert.ToBoolean(item["IsProductWise"]);
                    if (objEarnRule.IsBase.Value)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(item["BasePercentage"])))
                            objEarnRule.BasePercentage = Convert.ToDecimal(item["BasePercentage"]);

                        if (!string.IsNullOrEmpty(Convert.ToString(item["FixedPointPerRS"])))
                            objEarnRule.FixedPointPerRS = Convert.ToDecimal(item["FixedPointPerRS"]);

                        if (!string.IsNullOrEmpty(Convert.ToString(item["FixedPointPerTXN"])))
                            objEarnRule.FixedPointPerTXN = Convert.ToDecimal(item["FixedPointPerTXN"]);
                    }
                    if (objEarnRule.IsProductWise.Value)
                    {

                        objEarnRule.ProductWiseType = Convert.ToString(item["ProductWiseType"]);

                        if (!string.IsNullOrEmpty(Convert.ToString(item["ProductWiseFile"])))
                        {
                            var path = ConfigurationManager.AppSettings["CustomerDocuments"].ToString();
                            string fileName = "ProductUploadForEarn_" + objEarnRule.GroupId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                            path = path + "\\" + fileName;
                            DataTable dt = new DataTable();
                            System.IO.File.WriteAllBytes(path, Convert.FromBase64String(Convert.ToString(item["ProductWiseFile"])));
                            string conString = string.Empty;

                            conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";

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
                            foreach (DataRow dr in dt.Rows)
                            {
                                BOTS_TblProductUpload objItem = new BOTS_TblProductUpload();
                                objItem.GroupId = objEarnRule.GroupId;
                                objItem.ProductCode = Convert.ToString(dr["ProductCode"]);
                                objItem.ProductName = Convert.ToString(dr["ProductName"]);
                                objItem.Percentage = Convert.ToDecimal(dr["Percentage"]);
                                objItem.Type = "Product Earn";
                                lstProdUpload.Add(objItem);
                            }
                        }

                    }
                    if (objEarnRule.IsSlab.Value)
                    {
                        var SlabData = (object[])item["SlabData"];
                        foreach (Dictionary<string, object> item1 in SlabData)
                        {
                            BOTS_TblSlabConfig objSlab = new BOTS_TblSlabConfig();
                            objSlab.GroupId = Convert.ToString(item["GroupId"]);
                            objEarnRule.SlabType = Convert.ToString(item["SlabType"]);
                            objSlab.SlabFrom = Convert.ToInt32(item1["SlabFrom"]);
                            objSlab.SlabTo = Convert.ToInt32(item1["SlabTo"]);
                            objSlab.SlabPercentage = Convert.ToDecimal(item1["SlabPercentage"]);

                            lstSlab.Add(objSlab);
                        }
                    }
                    objEarnRule.IsBlockForEarn = Convert.ToBoolean(item["IsBlockForEarn"]);
                    if (objEarnRule.IsBlockForEarn.Value)
                    {
                        objEarnRule.BlockProductWiseType = Convert.ToString(item["BlockProductWiseType"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(item["ProductBlockEarn"])))
                        {
                            var path = ConfigurationManager.AppSettings["CustomerDocuments"].ToString();
                            string fileName = "ProductUploadForEarnBlock_" + objEarnRule.GroupId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                            path = path + "\\" + fileName;
                            DataTable dt = new DataTable();
                            System.IO.File.WriteAllBytes(path, Convert.FromBase64String(Convert.ToString(item["ProductBlockEarn"])));
                            string conString = string.Empty;

                            conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";

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
                            foreach (DataRow dr in dt.Rows)
                            {
                                BOTS_TblProductUpload objItem = new BOTS_TblProductUpload();
                                objItem.GroupId = objEarnRule.GroupId;
                                objItem.ProductCode = Convert.ToString(dr["ProductCode"]);
                                objItem.ProductName = Convert.ToString(dr["ProductName"]);
                                objItem.Percentage = Convert.ToDecimal(dr["Percentage"]);
                                objItem.Type = "Block Earn";
                                lstBlockEarnUpload.Add(objItem);
                            }
                        }
                    }
                }
                if (objEarnRule.RuleId > 0)
                {
                    objEarnRule.UpdatedBy = userDetails.LoginId;
                    objEarnRule.UpdatedDate = DateTime.Now;
                }
                else
                {
                    objEarnRule.AddedBy = userDetails.LoginId;
                    objEarnRule.AddedDate = DateTime.Now;
                }
                result = OBR.SaveEarnRule(objEarnRule, lstSlab, lstProdUpload, lstBlockEarnUpload);
            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "SaveEarnRuleConfig");
            }
            
            

            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }


        public ActionResult CheckerView(string GroupId)
        {

            OnBoardingSalesViewModel objData = new OnBoardingSalesViewModel();
            try
            {
                //return RedirectToAction("CustomerTerms", new { groupId = GroupId });
                CommonFunctions common = new CommonFunctions();
                GroupId = common.DecryptString(GroupId);

                //Customer Details
                objData.bots_TblGroupMaster = OBR.GetGroupMasterDetails(GroupId);
                objData.bots_TblDealDetails = OBR.GetDealMasterDetails(GroupId);
                objData.bots_TblPaymentDetails = OBR.GetPaymentDetails(GroupId);
                objData.objRetailList = OBR.GetRetailDetails(GroupId);

                objData.objInstallmentList = OBR.GetInstallmentDetails(GroupId);

                //Communication Details
                objData.lstCommunicationSet = OBR.GetCommunicationSetsByGroupId(GroupId);
                objData.lstSMSConfig = OBR.GetCommunicationSMSConfigByGroupId(GroupId);
                objData.lstWAConfig = OBR.GetCommunicationWAConfigByGroupId(GroupId);
                objData.lstCommunicationSetAssignment = OBR.GetOutletsByAssignmentSetId(GroupId);

                //Digital Loyalty Card
                objData.objDLCLinkConfig = OBR.GetDLCLinkDLTConfigByGroupId(GroupId);

                //Outlet Details
                objData.lstOutlets = OBR.GetOutletDetailsByGroupId(GroupId);
                //objData.objRetailList = OBR.GetOutletsBrandId(GroupId);

                //Perpetual Campaigns
                objData.lstCampaignOtherConfig = OBR.GetCampaignOtherConfigByGroupId(GroupId);
                objData.lstCampaignInactive = OBR.GetCampaignInactiveByGroupId(GroupId);


                foreach (var item in objData.lstCampaignOtherConfig)
                {
                    int count = 0;
                    if (item.CampaignType == "Birthday" || item.CampaignType == "Anniversary")
                    {
                        if (!string.IsNullOrEmpty(item.IntroScript1))
                        {
                            count++;
                        }
                        if (!string.IsNullOrEmpty(item.ReminderScript1))
                        {
                            count++;
                        }
                        if (!string.IsNullOrEmpty(item.ReminderScript2))
                        {
                            count++;
                        }
                        if (!string.IsNullOrEmpty(item.OnDayScriptPT))
                        {
                            count++;
                        }
                        if (!string.IsNullOrEmpty(item.OnDayScriptNPT))
                        {
                            count++;
                        }
                        if (item.CampaignType == "Birthday")
                            objData.BirthdayScriptCount = count;
                        if (item.CampaignType == "Anniversary")
                            objData.AnniversaryScriptCount = count;
                    }
                    if (item.CampaignType == "DLC Update Reminder" || item.CampaignType == "Balance Updates" || item.CampaignType == "Reminder Bulk Uploaded Users" || item.CampaignType == "DLC Referral Reminder")
                    {
                        if (!string.IsNullOrEmpty(item.IntroScript1))
                        {
                            count++;
                        }
                        if (!string.IsNullOrEmpty(item.IntroScript2))
                        {
                            count++;
                        }
                        if (item.CampaignType == "DLC Update Reminder")
                            objData.DLCUpdateReminderScriptCount = count;
                        if (item.CampaignType == "Balance Updates")
                            objData.BalanceUpdatesScriptCount = count;
                        if (item.CampaignType == "Reminder Bulk Uploaded Users")
                            objData.DLCReferralReminderScriptCount = count;
                        if (item.CampaignType == "DLC Referral Reminder")
                            objData.DLCReferralReminderScriptCount = count;
                    }
                }

                var ICount = 0;
                var OOCount = 0;
                var NRCount = 0;
                var PECount = 0;
                foreach (var item in objData.lstCampaignInactive)
                {
                    if (item.InactiveType == "Inactive")
                    {
                        if (!string.IsNullOrEmpty(item.LessThanDaysScript))
                        {
                            ICount++;
                        }
                        if (!string.IsNullOrEmpty(item.GreaterThanDaysScript))
                        {
                            ICount++;
                        }
                    }
                    if (item.InactiveType == "Only Once Inactive")
                    {
                        if (!string.IsNullOrEmpty(item.LessThanDaysScript))
                        {
                            OOCount++;
                        }
                        if (!string.IsNullOrEmpty(item.GreaterThanDaysScript))
                        {
                            OOCount++;
                        }
                    }
                    if (item.InactiveType == "Non Redemption Inactive")
                    {
                        if (!string.IsNullOrEmpty(item.LessThanDaysScript))
                        {
                            NRCount++;
                        }
                        if (!string.IsNullOrEmpty(item.GreaterThanDaysScript))
                        {
                            NRCount++;
                        }
                    }
                    if (item.InactiveType == "Point Expiry")
                    {
                        if (!string.IsNullOrEmpty(item.LessThanDaysScript))
                        {
                            PECount++;
                        }
                        if (!string.IsNullOrEmpty(item.GreaterThanDaysScript))
                        {
                            PECount++;
                        }
                    }
                }
                objData.InactiveScriptCount = ICount;
                objData.OnlyOnceInactiveScriptCount = OOCount;
                objData.NonRedemptionInactiveScriptCount = NRCount;
                objData.PointExpiryScriptCount = PECount;

                // Velocity Checks
                objData.BOTS_TblVelocityChecksConfig = OBR.GetVelocityChecksData(GroupId);

                //PointRules
                objData.objEarnRuleConfig = OBR.GetEarnRuleConfig(GroupId);
                objData.objBurnRuleConfig = OBR.GetBurnRuleConfig(GroupId);
                objData.lstSlabConfig = OBR.GetEarnRuleSlabConfig(GroupId);
                objData.lstProductUpload = OBR.GetProductUpload(GroupId);

                //Bulk Upload
                objData.BulkUploadCount = OBR.GetBulkUpload(GroupId);
                List<tblStandardRulesSetting> objStandardRulesList = new List<tblStandardRulesSetting>();
                foreach (var item in objData.objRetailList)
                {
                    tblStandardRulesSetting objItem = new tblStandardRulesSetting();
                    objItem = OBR.GetStandardConfigurationRules(Convert.ToInt32(item.CategoryId));
                    if (objItem != null)
                        objStandardRulesList.Add(objItem);
                }
                objData.objStandardRulesList = objStandardRulesList;

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "CheckerView");
            }
            return View(objData);

        }

        public ActionResult SaveBurnRuleConfig(string jsonData)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            List<BOTS_TblProductUpload> lstBlockBurnUpload = new List<BOTS_TblProductUpload>();
            BOTS_TblBurnRuleConfig objBurnRule = new BOTS_TblBurnRuleConfig();

            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in objData)
            {
                objBurnRule.RuleId = Convert.ToInt32(item["RuleId"]);
                objBurnRule.GroupId = Convert.ToString(item["GroupId"]);
                objBurnRule.MinInvoiceAmt = Convert.ToInt32(item["MinInvoiceAmt"]);
                objBurnRule.PercentageToRedeemPts = Convert.ToInt32(item["PercentageToRedeemPts"]);
                objBurnRule.PercentageToRedeemExtPts = Convert.ToInt32(item["PercentageToRedeemExtPts"]);
                objBurnRule.MinThreshholdPtsFisttime = Convert.ToInt32(item["MinThreshholdPtsFisttime"]);
                objBurnRule.MinThreshholdPtsSubsequent = Convert.ToInt32(item["MinThreshholdPtsSubsequent"]);
                objBurnRule.PartialEarn = Convert.ToString(item["PartialEarn"]);
                objBurnRule.IsProductCodeBlocking = Convert.ToBoolean(item["IsProductCodeBlocking"]);
                if (objBurnRule.IsProductCodeBlocking)
                {
                    objBurnRule.ProductCodeBlockingType = Convert.ToString(item["ProductBurnType"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(item["ProductBlockBurnFile"])))
                    {
                        var path = ConfigurationManager.AppSettings["CustomerDocuments"].ToString();
                        string fileName = "ProductBlockForBurn_" + objBurnRule.GroupId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                        path = path + "\\" + fileName;
                        DataTable dt = new DataTable();
                        System.IO.File.WriteAllBytes(path, Convert.FromBase64String(Convert.ToString(item["ProductBlockBurnFile"])));
                        string conString = string.Empty;

                        conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";

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
                        foreach (DataRow dr in dt.Rows)
                        {
                            BOTS_TblProductUpload objItem = new BOTS_TblProductUpload();
                            objItem.GroupId = objBurnRule.GroupId;
                            objItem.ProductCode = Convert.ToString(dr["ProductCode"]);
                            objItem.ProductName = Convert.ToString(dr["ProductName"]);
                            objItem.Percentage = Convert.ToDecimal(dr["Percentage"]);
                            objItem.Type = "Product Block Burn";
                            lstBlockBurnUpload.Add(objItem);
                        }
                    }
                }
            }
            if (objBurnRule.RuleId > 0)
            {
                objBurnRule.UpdatedBy = userDetails.LoginId;
                objBurnRule.UpdatedDate = DateTime.Now;
            }
            else
            {
                objBurnRule.AddedBy = userDetails.LoginId;
                objBurnRule.AddedDate = DateTime.Now;
            }

            result = OBR.SaveBurnRule(objBurnRule, lstBlockBurnUpload);
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult SendForApproval(string GroupId)
        {
            bool result = true;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            result = OBR.SendForApproval(GroupId, userDetails.LoginId);
            if (result)
            {
                var GroupName = OBR.GetOnboardingGroupName(GroupId);
                var CSHead = OBR.GetCSHeadEmailId();
                var message = "Configuration submitted for approval for Group - " + GroupName;
                var subject = "Configuration submitted for approval " + GroupName;
                var isEmail = SendEmailOnBoarding(CSHead, subject, message);
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult UpdateConfigurationStatus(string groupId, string status, string reason, string ownermobileno)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var loginId = ownermobileno;
            if (userDetails != null)
            {
                loginId = userDetails.LoginId;
            }
            result = OBR.UpdateConfigurationStatus(groupId, status, loginId, reason);
            if (status == "Rejected")
            {
                var GroupName = OBR.GetOnboardingGroupName(groupId);
                var CSEmail = userDetails.EmailId;
                var message = "Configuration Rejected by CS Head for Group - " + GroupName;
                var subject = "Configuration Rejected - " + GroupName;
                var isEmail = SendEmailOnBoarding(CSEmail, subject, message);
            }
            if (status == "Approved")
            {
                var GroupName = OBR.GetOnboardingGroupName(groupId);
                var CSEmail = userDetails.EmailId;
                var message = "Configuration Approved by CS Head for Group - " + GroupName;
                var subject = "Configuration Approved - " + GroupName;
                var isEmail = SendEmailOnBoarding(CSEmail, subject, message);
            }
            if (status == "Rejected By Customer")
            {
                var CSName = OBR.GetAssignedCSNameForOnboarding(groupId);
                var CSHead = OBR.GetCSHeadEmailId();
                var GroupName = OBR.GetOnboardingGroupName(groupId);
                var CSEmail = CSHead + "," + CSName;
                var message = "Configuration Rejected By Customer for Group - " + GroupName + ", Reason is - " + reason;
                var subject = "Configuration Rejected By Customer - " + GroupName;
                var isEmail = SendEmailOnBoarding(CSEmail, subject, message);
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult SendConfigurationToCustomer(string groupId)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            var groupDetails = OBR.GetGroupMasterDetails(groupId);
            result = OBR.UpdateConfigurationStatus(groupId, "Send For Customer Approval", userDetails.LoginId, "");
            if (result)
                result = SendEmailForCustomerApproval(groupDetails);
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public bool SendEmailForCustomerApproval(BOTS_TblGroupMaster groupDetails)
        {
            CommonFunctions comm = new CommonFunctions();
            bool result = false;
            var BaseUrl = ConfigurationManager.AppSettings["BaseUrl"].ToString();
            string from = System.Configuration.ConfigurationManager.AppSettings["FrmEmailOnboarding"];
            string PWD = System.Configuration.ConfigurationManager.AppSettings["FrmEmailOnboardingPwd"];

            var Url = BaseUrl + "CustomerOnBoarding/CheckerView?GroupId=" + comm.EncryptString(groupDetails.GroupId);

            string To = groupDetails.OwnerEmailId;

            using (MailMessage mail = new MailMessage(from, To))
            {
                StringBuilder str = new StringBuilder();
                str.AppendLine("Dear " + groupDetails.OwnerName);
                str.AppendLine();
                str.AppendLine("We thank you for your decision to join hands with Blue Ocktopus and are confident that you will be really happy to see how this helps your current business grow.");
                str.AppendLine();
                str.AppendLine("As per your discussion with" + groupDetails.AssignedCSName + "," + "we have configured the Loyalty Programme Rules");
                str.AppendLine();
                str.AppendLine("We request you to kindly click on this link " + Url + " to approve the programme rules for us to start with the set-up process.");
                str.AppendLine();
                str.AppendLine("Timelines for the programme set-up post your approval will be as :");
                str.AppendLine();
                str.AppendLine("We value your association with us and look forward for your continued support");
                str.AppendLine();
                str.AppendLine(" Warm Regards,");
                str.AppendLine("  BlueOcktopus Team");

                mail.Subject = "Loyalty Program Configuration";
                mail.SubjectEncoding = System.Text.Encoding.Default;


                //string htmlBody = "<html><body><h1>Picture</h1><br><img src=\"cid:filename\"></body></html>";
                //AlternateView avHtml = AlternateView.CreateAlternateViewFromString
                //   (htmlBody, null, MediaTypeNames.Text.Html);

                //string filePath = Server.MapPath("~/Content/assets/BotsLoginBackground.jpeg");
                //LinkedResource inline = new LinkedResource(filePath, MediaTypeNames.Image.Jpeg);
                //inline.ContentId = Guid.NewGuid().ToString();
                //avHtml.LinkedResources.Add(inline);

                ////MailMessage mail = new MailMessage();
                //mail.AlternateViews.Add(avHtml);

                //Attachment att = new Attachment(filePath);
                //att.ContentDisposition.Inline = true;

                //mail.Subject = "Client:  Has Sent You A Screenshot";
                //mail.Body = String.Format(
                //           "<h3>Client: Has Sent You A Screenshot</h3>" +
                //           @"<img src=""cid:{0}"" />", att.ContentId);

                //mail.IsBodyHtml = true;
                //mail.Attachments.Add(att);



                mail.Body = str.ToString();
                mail.IsBodyHtml = false;
                mail.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.zoho.com";
                smtp.EnableSsl = true;
                NetworkCredential networkCredential = new NetworkCredential(from, PWD);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = networkCredential;
                smtp.Port = 587;
                smtp.Send(mail);

                result = true;
            }
            return result;
        }

        public ActionResult SendOTPForApproval(string groupId, string custMobileNo)
        {
            bool result = false;
            result = new HomeController().SendOTP(custMobileNo);

            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult CustomerApprovalConfiguration(string groupId, string custMobileNo, string otp)
        {
            NewCustDetails result = new NewCustDetails();
            try
            {
                var status = DR.VerifyOTP(custMobileNo, Convert.ToInt32(otp));
                if (status)
                {
                    var updateStatus = OBR.UpdateConfigurationStatus(groupId, "Approved By Customer", custMobileNo, "");
                    if (updateStatus)
                    {
                        //Create Database
                        result = OBR.CreateCustomerDatabase(groupId);

                        //Send Email
                        if (result.result)
                        {
                            var CSName = OBR.GetAssignedCSNameForOnboarding(groupId);
                            var CSHead = OBR.GetCSHeadEmailId();
                            var GroupName = OBR.GetOnboardingGroupName(groupId);
                            var CSEmail = CSHead + "," + CSName;
                            var message = "Configuration Approved By Customer for Group - " + GroupName;
                            var subject = "Configuration Approved By Customer - " + GroupName;
                            var isEmail = SendEmailOnBoarding(CSEmail, subject, message);
                            var opssubject = "Integration Details - " + GroupName;

                            //Send email to OPS
                            var isOPSEmail = SendEmailToOPS("operations@blueocktopus.in", opssubject, result);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "CustomerApprovalConfiguration");
            }
            return new JsonResult() { Data = result.result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public bool SendEmailOnBoarding(string to, string subject, string message)
        {
            bool status = false;
            try
            {
                var from = ConfigurationManager.AppSettings["FrmEmailOnboarding"].ToString();
                var PWD = ConfigurationManager.AppSettings["FrmEmailOnboardingPwd"].ToString();
                using (MailMessage mail = new MailMessage(from, to))
                {
                    StringBuilder str = new StringBuilder();
                    str.AppendLine("Dear Sir/Madam,");
                    str.AppendLine();
                    str.AppendLine(message);
                    str.AppendLine();
                    str.AppendLine("Regards,");
                    str.AppendLine(" - BlueOcktopus Team");

                    mail.Subject = subject;
                    mail.SubjectEncoding = System.Text.Encoding.Default;
                    mail.Body = str.ToString();
                    mail.IsBodyHtml = false;
                    mail.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.zoho.com";
                    smtp.EnableSsl = true;
                    NetworkCredential networkCredential = new NetworkCredential(from, PWD);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = networkCredential;
                    smtp.Port = 587;
                    smtp.Send(mail);

                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "CustomerApprovalConfiguration");
            }
            return status;
        }

        public bool SendEmailToOPS(string to, string subject,NewCustDetails objData)
        {
            bool status = false;
            try
            {
                var from = ConfigurationManager.AppSettings["FrmEmailOnboarding"].ToString();
                var PWD = ConfigurationManager.AppSettings["FrmEmailOnboardingPwd"].ToString();
                using (MailMessage mail = new MailMessage(from, to))
                {
                    StringBuilder str = new StringBuilder();
                    str.AppendLine("Dear Sir/Madam,");
                    str.AppendLine();
                    str.AppendLine("Please find below Integration details");
                    str.AppendLine();
                    str.AppendLine("Billing Partner Url - "+objData.BillingPartnerUrl);
                    str.AppendLine();
                    str.AppendLine("DLC Link - " + objData.DLCLink);
                    str.AppendLine();
                    foreach(var item in objData.lstCounterIdDetails)
                    {
                        str.AppendLine("Outlet Name - " + item.OutletName);
                        str.AppendLine();
                        str.AppendLine("CounterId - " + item.CounterId);
                        str.AppendLine();
                        str.AppendLine("Securitykey - " + item.Securitykey);
                        str.AppendLine();
                        str.AppendLine();
                    }
                    str.AppendLine();
                    str.AppendLine("Regards,");
                    str.AppendLine(" - BlueOcktopus Team");

                    mail.Subject = subject;
                    mail.SubjectEncoding = System.Text.Encoding.Default;
                    mail.Body = str.ToString();
                    mail.IsBodyHtml = false;
                    mail.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.zoho.com";
                    smtp.EnableSsl = true;
                    NetworkCredential networkCredential = new NetworkCredential(from, PWD);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = networkCredential;
                    smtp.Port = 587;
                    smtp.Send(mail);

                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "CustomerApprovalConfiguration");
            }
            return status;
        }
        public ActionResult SaveOutletDataConfig(string jsonData)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];

            List<BOTS_TblOutletMaster> lstOutlets = new List<BOTS_TblOutletMaster>();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);

            foreach (Dictionary<string, object> item in objData)
            {
                BOTS_TblOutletMaster objOutletData = new BOTS_TblOutletMaster();
                objOutletData.Id = Convert.ToInt32(item["Id"]);
                objOutletData.GroupId = Convert.ToString(item["GroupId"]);
                objOutletData.BrandId = Convert.ToString(item["BrandId"]);
                objOutletData.OutletId = Convert.ToString(item["OutletId"]);
                objOutletData.OutletName = Convert.ToString(item["OutletName"]);
                objOutletData.AreaName = Convert.ToString(item["AreaName"]);
                objOutletData.AuthorisedPerson = Convert.ToString(item["AuthorisedPerson"]);
                objOutletData.RegisterMobileNo = Convert.ToString(item["RegisterMobileNo"]);
                objOutletData.RegisterEmail = Convert.ToString(item["RegisterEmail"]);
                objOutletData.Address = Convert.ToString(item["Address"]);
                objOutletData.ProgramLanguage = Convert.ToString(item["ProgramLanguage"]);
                objOutletData.State = Convert.ToString(item["State"]);
                objOutletData.City = Convert.ToString(item["City"]);
                objOutletData.PinCode = Convert.ToString(item["PinCode"]);


                objOutletData.UpdatedBy = userDetails.LoginId;
                objOutletData.UpdatedDate = DateTime.Now;

                lstOutlets.Add(objOutletData);

            }
            result = OBR.SaveOutletData(lstOutlets);

            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };

        }
        public ActionResult SavePreferredLanguage(string groupId, string PreferredLanguage)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            bool result = OBR.SavePreferredLanguage(groupId, PreferredLanguage, userDetails.LoginId);

            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult SavePogramLanguage(string groupId, string ProgramLanguage)
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            bool result = OBR.SaveProgramLanguage(groupId, ProgramLanguage, userDetails.LoginId);

            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult CustomerTerms(string groupId)
        {
            OnBoardingSalesViewModel objData = new OnBoardingSalesViewModel();
            CommonFunctions common = new CommonFunctions();
            groupId = common.DecryptString(groupId);
            objData.bots_TblGroupMaster = OBR.GetGroupMasterDetails(groupId);
            objData.bots_TblDealDetails = OBR.GetDealMasterDetails(groupId);
            objData.bots_TblPaymentDetails = OBR.GetPaymentDetails(groupId);
            objData.objRetailList = OBR.GetRetailDetails(groupId);

            return View(objData);
        }
    }
}

