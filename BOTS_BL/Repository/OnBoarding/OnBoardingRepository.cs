﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models;
using BOTS_BL.Models.CommonDB;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.IO;
using BOTS_BL;
using System.Data.Entity.Validation;
using System.Net;
using System.Web.Script.Serialization;
using System.Configuration;
using BOTS_BL.Models.OnBoarding;
using System.Data;

namespace BOTS_BL.Repository
{
    public class OnBoardingRepository
    {
        Exceptions newexception = new Exceptions();
        CustomerOnBoardingRepository COBR = new CustomerOnBoardingRepository();
        public int AddOnboardingCustomer(BOTS_TblGroupMaster objGroup, List<BOTS_TblRetailMaster> objLstRetail,
            BOTS_TblDealDetails objDeal, BOTS_TblPaymentDetails objPayment, List<BOTS_TblInstallmentDetails> objLstInstallment, List<BOTS_TblOutletMaster> objLstOutlet)
        {
            bool status = false;
            var GroupId = 0;
            using (var context = new CommonDBContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (string.IsNullOrEmpty(objGroup.GroupId))
                        {
                            var LastGroupId = context.BOTS_TblGroupMaster.OrderByDescending(x => x.GroupId).Take(1).Select(y => y.GroupId).FirstOrDefault();

                            if (!string.IsNullOrEmpty(LastGroupId))
                            {
                                GroupId = Convert.ToInt32(LastGroupId) + 1;
                            }
                            else
                            {
                                GroupId = 2001;
                            }
                            objGroup.GroupId = Convert.ToString(GroupId);
                        }
                        else
                        {
                            GroupId = Convert.ToInt32(objGroup.GroupId);
                        }

                        var path = ConfigurationManager.AppSettings["CustomerDocuments"].ToString();
                        var ClientFolder = path + "\\" + GroupId;
                        if (!Directory.Exists(ClientFolder))
                        {
                            System.IO.Directory.CreateDirectory(ClientFolder);
                        }

                        if (objGroup.PANDocumentFile != null)
                        {
                            using (Stream inputStream = objGroup.PANDocumentFile.InputStream)
                            {
                                byte[] data;
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    inputStream.CopyTo(ms);
                                    data = ms.ToArray();
                                }
                                System.IO.File.WriteAllBytes(Path.Combine(ClientFolder, objGroup.PANDocumentFile.FileName), data);
                            }
                            objGroup.PANDocument = objGroup.PANDocumentFile.FileName;
                        }
                        if (objGroup.GSTDocumentFile != null)
                        {
                            using (Stream inputStream = objGroup.GSTDocumentFile.InputStream)
                            {
                                byte[] data;
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    inputStream.CopyTo(ms);
                                    data = ms.ToArray();
                                }
                                System.IO.File.WriteAllBytes(Path.Combine(ClientFolder, objGroup.GSTDocumentFile.FileName), data);
                            }
                            objGroup.GSTDocument = objGroup.GSTDocumentFile.FileName;
                        }


                        if (objGroup.CustomerStatus == "CSUpdate")
                        {
                            var groupDetails = context.BOTS_TblGroupMaster.Where(x => x.GroupId == objGroup.GroupId).FirstOrDefault();
                            groupDetails.CustomerStatus = "CSUpdate";
                            groupDetails.UpdatedDate = DateTime.Now;
                            groupDetails.UpdatedBy = objGroup.UpdatedBy;
                            context.BOTS_TblGroupMaster.AddOrUpdate(groupDetails);
                            context.SaveChanges();

                            foreach (var item in objLstOutlet)
                            {
                                context.BOTS_TblOutletMaster.AddOrUpdate(item);
                                context.SaveChanges();
                            }
                        }
                        else
                        {
                            if (objGroup.Referredby != "1" && objGroup.Referredby != "3" && objGroup.Referredby != "4" && objGroup.Referredby != "6")
                            {
                                if (!string.IsNullOrEmpty(objGroup.ReferredNameNew))
                                {
                                    objGroup.ReferredName = objGroup.ReferredNameNew;
                                }
                            }
                            context.BOTS_TblGroupMaster.AddOrUpdate(objGroup);
                            context.SaveChanges();

                            var lstRetail = context.BOTS_TblRetailMaster.Where(x => x.GroupId == objGroup.GroupId).ToList();
                            foreach (var item in lstRetail)
                            {
                                if (string.IsNullOrEmpty(item.BillingPartner))
                                {
                                    item.BillingPartner = "0";
                                }
                                if (string.IsNullOrEmpty(item.BillingProduct))
                                {
                                    item.BillingProduct = "0";
                                }
                                context.BOTS_TblRetailMaster.Remove(item);
                                context.SaveChanges();
                            }

                            foreach (var item in objLstRetail)
                            {
                                var id = Convert.ToInt32(item.CategoryId);
                                var categoryName = context.tblCategories.Where(x => x.CategoryId == id).Select(y => y.CategoryName).FirstOrDefault();
                                item.CategoryName = categoryName;
                                item.GroupId = Convert.ToString(objGroup.GroupId);
                                if (item.BOProduct == "2")
                                {
                                    var BillingPId = context.tblBillingPartners.Where(x => x.BillingPartnerName == "OctaXS").Select(y => y.BillingPartnerId).FirstOrDefault();
                                    item.BillingPartner = Convert.ToString(BillingPId);
                                }
                                context.BOTS_TblRetailMaster.AddOrUpdate(item);
                                context.SaveChanges();
                            }

                            objDeal.GroupId = Convert.ToString(objGroup.GroupId);
                            context.BOTS_TblDealDetails.AddOrUpdate(objDeal);
                            context.SaveChanges();

                            objPayment.GroupId = Convert.ToString(objGroup.GroupId);
                            context.BOTS_TblPaymentDetails.AddOrUpdate(objPayment);
                            context.SaveChanges();

                            var lstInstallments = context.BOTS_TblInstallmentDetails.Where(x => x.GroupId == objGroup.GroupId).ToList();
                            foreach (var item in lstInstallments)
                            {
                                context.BOTS_TblInstallmentDetails.Remove(item);
                                context.SaveChanges();
                            }
                            foreach (var item in objLstInstallment)
                            {
                                item.GroupId = Convert.ToString(objGroup.GroupId);
                                context.BOTS_TblInstallmentDetails.AddOrUpdate(item);
                                context.SaveChanges();
                            }
                        }
                        transaction.Commit();
                        if (objGroup.UpdatedBy == null || objGroup.UpdatedBy == "")
                            AddTracking(Convert.ToString(GroupId), "Customer Add/Update Done", objGroup.CreatedBy);
                        else
                            AddTracking(Convert.ToString(GroupId), "Customer Add/Update Done", objGroup.UpdatedBy);

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        newexception.AddException(ex, "AddOnboardingCustomer");
                        GroupId = 0;
                        //throw ex;
                    }
                }
            }
            return GroupId;
        }

        public List<OnBoardingListing> GetOnBoardingListings(CustomerLoginDetail userDetails)
        {
            List<OnBoardingListing> onBoardingListings = new List<OnBoardingListing>();
            List<BOTS_TblGroupMaster> lstGroups = new List<BOTS_TblGroupMaster>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    //SuperAdmin
                    if (userDetails.LoginType == "1")
                    {
                        lstGroups = context.BOTS_TblGroupMaster.Where(x => x.IsLive.Value == false).ToList();
                    }
                    //Sales Head
                    if (userDetails.LoginType == "5")
                    {
                        lstGroups = context.BOTS_TblGroupMaster.Where(x => x.IsLive.Value == false).ToList();
                    }
                    //CS Head
                    if (userDetails.LoginType == "6")
                    {
                        lstGroups = context.BOTS_TblGroupMaster.Where(x => x.CustomerStatus != "Draft" && x.IsLive.Value == false).ToList();
                    }
                    //CS Success
                    if (userDetails.LoginType == "7")
                    {
                        lstGroups = context.BOTS_TblGroupMaster.Where(x => x.AssignedCS == userDetails.LoginId && x.CustomerStatus != "Draft" && x.IsLive.Value == false).ToList();
                    }
                    if (userDetails.LoginType != "1" && userDetails.LoginType != "5" && userDetails.LoginType != "6" && userDetails.LoginType != "7")
                    {
                        lstGroups = context.BOTS_TblGroupMaster.Where(x => x.CustomerStatus != "Draft" && x.IsLive.Value == false).ToList();
                    }

                    foreach (var item in lstGroups)
                    {
                        OnBoardingListing objItem = new OnBoardingListing();
                        objItem.GroupId = Convert.ToInt32(item.GroupId);
                        objItem.GroupName = item.GroupName;
                        objItem.OwnerMobileNo = item.OwnerMobileNo;
                        var city = COBR.GetCityById(Convert.ToInt32(item.City));
                        objItem.City = city.CityName;
                        objItem.PaymentStatus = context.BOTS_TblDealDetails.Where(x => x.GroupId == item.GroupId).Select(y => y.PaymentStatus).FirstOrDefault();

                        var BPId = context.BOTS_TblRetailMaster.Where(x => x.GroupId == item.GroupId).Select(y => y.BillingPartner).FirstOrDefault();
                        if (!string.IsNullOrEmpty(BPId))
                        {
                            var bId = Convert.ToInt32(BPId);
                            objItem.BillingPartnerName = context.tblBillingPartners.Where(x => x.BillingPartnerId == bId).Select(y => y.BillingPartnerName).FirstOrDefault();
                        }
                        objItem.CustomerStatus = item.CustomerStatus;
                        if (item.IntroductionCall.HasValue)
                            objItem.IsIntroCall = item.IntroductionCall.Value;
                        else
                            objItem.IsIntroCall = false;

                        if (!string.IsNullOrEmpty(item.AssignedCS))
                        {
                            objItem.CSAssigned = context.CustomerLoginDetails.Where(x => x.LoginId == item.AssignedCS).Select(y => y.UserName).FirstOrDefault();
                        }

                        onBoardingListings.Add(objItem);
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOnBoardingListings");
            }
            return onBoardingListings;
        }

        public BOTS_TblGroupMaster GetGroupMasterDetails(string GroupId)
        {
            BOTS_TblGroupMaster objData = new BOTS_TblGroupMaster();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objData = context.BOTS_TblGroupMaster.Where(x => x.GroupId == GroupId).FirstOrDefault();
                    var cityId = Convert.ToInt32(objData.City);
                    objData.CityName = context.tblCities.Where(x => x.CityId == cityId).Select(y => y.CityName).FirstOrDefault();
                    var SBy = Convert.ToInt32(objData.SourcedBy);
                    objData.SourceByName = context.tblSourcedBies.Where(x => x.SourcedbyId == SBy).Select(y => y.SourcedbyName).FirstOrDefault();
                    //var AssignedCS = Convert.ToInt32(objData.AssignedCS);
                    objData.AssignedCSName = context.tblRMAssigneds.Where(x => x.LoginId == objData.AssignedCS).Select(y => y.RMAssignedName).FirstOrDefault();
                    var SourceType = Convert.ToInt32(objData.Referredby);
                    objData.SourceTypeName = context.tblSourceTypes.Where(x => x.SourceTypeId == SourceType).Select(y => y.SourceTypeName).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetGroupMasterDetails");
            }
            return objData;
        }

        public BOTS_TblDealDetails GetDealMasterDetails(string GroupId)
        {
            BOTS_TblDealDetails objData = new BOTS_TblDealDetails();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objData = context.BOTS_TblDealDetails.Where(x => x.GroupId == GroupId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDealMasterDetails");
            }

            return objData;
        }

        public BOTS_TblPaymentDetails GetPaymentDetails(string GroupId)
        {
            BOTS_TblPaymentDetails objData = new BOTS_TblPaymentDetails();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objData = context.BOTS_TblPaymentDetails.Where(x => x.GroupId == GroupId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetPaymentDetails");
            }
            return objData;
        }

        public List<BOTS_TblRetailMaster> GetRetailDetails(string GroupId)
        {
            List<BOTS_TblRetailMaster> objData = new List<BOTS_TblRetailMaster>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objData = context.BOTS_TblRetailMaster.Where(x => x.GroupId == GroupId).ToList();
                    foreach (var item in objData)
                    {
                        if (!string.IsNullOrEmpty(item.BillingPartner))
                        {
                            var BillingPartner = Convert.ToInt32(item.BillingPartner);
                            item.BillingPartnerName = context.tblBillingPartners.Where(x => x.BillingPartnerId == BillingPartner).Select(y => y.BillingPartnerName).FirstOrDefault();
                        }
                        if (!string.IsNullOrEmpty(item.BillingProduct))
                        {
                            var BillingProduct = Convert.ToInt32(item.BillingProduct);
                            item.BillingProductName = context.BOTS_TblBillingPartnerProduct.Where(x => x.BillingPartnerProductId == BillingProduct).Select(y => y.BillingPartnerProductName).FirstOrDefault();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetRetailDetails");
            }

            return objData;
        }

        public List<BOTS_TblOutletMaster> GetOutletDetails(string GroupId)
        {
            List<BOTS_TblOutletMaster> objData = new List<BOTS_TblOutletMaster>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objData = context.BOTS_TblOutletMaster.Where(x => x.GroupId == GroupId).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOutletDetails");
            }
            return objData;
        }

        public List<BOTS_TblInstallmentDetails> GetInstallmentDetails(string GroupId)
        {
            List<BOTS_TblInstallmentDetails> objData = new List<BOTS_TblInstallmentDetails>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objData = context.BOTS_TblInstallmentDetails.Where(x => x.GroupId == GroupId).ToList();
                    foreach (var item in objData)
                    {
                        item.PaymentDateStr = item.PaymentDate.ToString("yyyy-MM-dd");
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetInstallmentDetails");
            }

            return objData;
        }

        public List<SelectListItem> GetBillingPartnerProduct(int Id)
        {
            List<SelectListItem> lstBillingPartnerProduct = new List<SelectListItem>();
            SelectListItem item1 = new SelectListItem();
            item1.Value = "0";
            item1.Text = "Please Select";
            lstBillingPartnerProduct.Add(item1);
            try
            {
                using (var context = new CommonDBContext())
                {
                    var BillingPartnerProduct = context.BOTS_TblBillingPartnerProduct.Where(x => x.BillingPartnerId == Id).ToList();
                    foreach (var item in BillingPartnerProduct)
                    {
                        lstBillingPartnerProduct.Add(new SelectListItem
                        {
                            Text = item.BillingPartnerProductName,
                            Value = Convert.ToString(item.BillingPartnerProductId)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetBillingPartnerProduct");
            }
            return lstBillingPartnerProduct;
        }

        public List<SelectListItem> GetRefferedName(string SourceType)
        {
            List<SelectListItem> lstRefferedName = new List<SelectListItem>();
            SelectListItem item1 = new SelectListItem();
            item1.Value = "0";
            item1.Text = "Please Select";
            lstRefferedName.Add(item1);
            try
            {
                using (var context = new CommonDBContext())
                {
                    if (SourceType == "1")
                    {
                        var CustomerList = context.tblGroupDetails.ToList();
                        foreach (var item in CustomerList)
                        {
                            lstRefferedName.Add(new SelectListItem
                            {
                                Text = item.GroupName,
                                Value = item.GroupName
                            });
                        }
                    }
                    if (SourceType == "3")
                    {
                        var BillingPartners = context.tblBillingPartners.ToList();
                        foreach (var item in BillingPartners)
                        {
                            lstRefferedName.Add(new SelectListItem
                            {
                                Text = item.BillingPartnerName,
                                Value = item.BillingPartnerName
                            });
                        }
                    }
                    if (SourceType == "4")
                    {
                        var ChannelPartners = context.tblChannelPartners.ToList();
                        foreach (var item in ChannelPartners)
                        {
                            lstRefferedName.Add(new SelectListItem
                            {
                                Text = item.CPartnerName,
                                Value = Convert.ToString(item.CPId)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetRefferedName");
            }
            return lstRefferedName;
        }

        public List<RetailDetails> GetRetailDetailsForEmail(string GroupId)
        {
            List<RetailDetails> objData = new List<RetailDetails>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    var data = context.BOTS_TblRetailMaster.Where(x => x.GroupId == GroupId).ToList();

                    foreach (var item in data)
                    {
                        RetailDetails item1 = new RetailDetails();
                        item1.BrandName = item.BrandName;
                        item1.CategoryName = item.CategoryName;
                        item1.NoOfOutlets = item.NoOfOutlets;
                        item1.NoOfEnrolled = item.NoOfEnrolled;
                        var id = Convert.ToInt32(item.BillingPartner);
                        var BPID = Convert.ToInt32(item.BillingProduct);
                        item1.BillingPartner = context.tblBillingPartners.Where(x => x.BillingPartnerId == id).Select(y => y.BillingPartnerName).FirstOrDefault();
                        item1.BOProduct = item.BOProduct == "1" ? "Octa Plus" : "Octa XS";
                        item1.BillingProduct = context.BOTS_TblBillingPartnerProduct.Where(x => x.BillingPartnerProductId == BPID).Select(y => y.BillingPartnerProductName).FirstOrDefault();
                        objData.Add(item1);
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetRetailDetailsForEmail");
            }
            return objData;
        }
        public string GetEmailAssignedCS(string assignedCS)
        {
            string email = string.Empty;
            using (var context = new CommonDBContext())
            {
                email = context.CustomerLoginDetails.Where(x => x.LoginId == assignedCS).Select(y => y.EmailId).FirstOrDefault();
            }
            return email;
        }
        public string GetEmailSourceBy(string Id)
        {
            string email = string.Empty;
            using (var context = new CommonDBContext())
            {
                var SourcedbyId = Convert.ToInt32(Id);
                var loginId = context.tblSourcedBies.Where(x => x.SourcedbyId == SourcedbyId).Select(y => y.LoginId).FirstOrDefault();
                email = context.CustomerLoginDetails.Where(x => x.LoginId == loginId).Select(y => y.EmailId).FirstOrDefault();
            }
            return email;
        }
        public List<string> GetAllInternalEmailIds()
        {
            List<string> lstEmails = new List<string>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    var loginTypeList = new List<string> { "5", "6", "9", "10" };
                    lstEmails = context.CustomerLoginDetails.Where(x => x.EmailId != null && x.LoginType != null && loginTypeList.Contains(x.LoginType) && x.UserStatus == true).Select(y => y.EmailId).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetAllInternalEmailIds");
            }
            return lstEmails;
        }

        public OnBoardingCustomerDetails GetOnBoardingCustomerDetails(string groupId)
        {
            OnBoardingCustomerDetails objData = new OnBoardingCustomerDetails();
            try
            {
                using (var context = new CommonDBContext())
                {
                    var GroupDetails = context.BOTS_TblGroupMaster.Where(x => x.GroupId == groupId).FirstOrDefault();
                    objData.GroupId = GroupDetails.GroupId;
                    objData.GroupName = GroupDetails.GroupName;
                    objData.OwnerMobileNo = GroupDetails.OwnerMobileNo;
                    objData.OwnerEmailId = GroupDetails.OwnerEmailId;
                    objData.City = GroupDetails.City;
                    objData.AlternateMobileNo = GroupDetails.AlternateMobileNo;
                    objData.AlternateEmailId = GroupDetails.AlternateEmailId;
                    objData.NoOfRetailCategory = GroupDetails.NoOfRetailCategory;
                    objData.IsMWP = GroupDetails.IsMWP;
                    objData.IsWhatsApp = GroupDetails.IsWhatsApp;
                    objData.NoOfFreeWhatsAppMsg = GroupDetails.NoOfFreeWhatsAppMsg;
                    objData.NoOfFreeSMS = GroupDetails.NoOfFreeSMS;
                    objData.NoOfPaidWhatsAppMsg = GroupDetails.NoOfPaidWhatsAppMsg;
                    objData.NoOfPaidSMS = GroupDetails.NoOfPaidSMS;
                    objData.IsMobileApp = GroupDetails.IsMobileApp;
                    objData.SourcedBy = GroupDetails.SourcedBy;
                    objData.AssignedCS = GroupDetails.AssignedCS;
                    objData.Comments = GroupDetails.Comments;
                    objData.Referredby = GroupDetails.Referredby;
                    objData.ReferredName = GroupDetails.ReferredName;
                    objData.OtherFees = GroupDetails.OtherFees;
                    objData.OtherFeesDescription = GroupDetails.OtherFeesDescription;
                    objData.IsKeyAccount = GroupDetails.IsKeyAccount;
                    objData.CreatedDate = GroupDetails.CreatedDate;
                    objData.CreatedBy = GroupDetails.CreatedBy;
                    objData.GSTDocument = GroupDetails.GSTDocument;
                    objData.PANDocument = GroupDetails.PANDocument;

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOnBoardingCustomerDetails");
            }
            return objData;
        }

        public bool SaveCommunicationConfig(BOTS_TblSMSConfig objSMSData, BOTS_TblWAConfig objWAData, BOTS_TblCommunicationSet objSet, List<SMSTemplate> lstSMS, List<SMSTemplate> lstWA, string loginId)
        {
            bool status = false;

            using (var context = new CommonDBContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var existingSet = context.BOTS_TblCommunicationSet.Where(x => x.SetId == objSet.SetId).FirstOrDefault();
                        if (existingSet != null)
                        {
                            objSet.CreatedBy = existingSet.CreatedBy;
                            objSet.CreatedDate = existingSet.CreatedDate;
                            objSet.UpdatedBy = loginId;
                            objSet.UpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            objSet.CreatedBy = loginId;
                            objSet.CreatedDate = DateTime.Now;
                        }
                        context.BOTS_TblCommunicationSet.AddOrUpdate(objSet);
                        context.SaveChanges();

                        if (objSMSData.IsSMS)
                        {
                            foreach (var itemSMS in lstSMS)
                            {
                                BOTS_TblSMSConfig objItemNew = new BOTS_TblSMSConfig();
                                objItemNew.IsSMS = objSMSData.IsSMS;
                                objItemNew.SMSProvider = objSMSData.SMSProvider;
                                objItemNew.GroupId = objSMSData.GroupId;
                                objItemNew.BrandId = objSMSData.BrandId;
                                objItemNew.SMSSenderID = objSMSData.SMSSenderID;
                                objItemNew.SMSUsername = objSMSData.SMSUsername;
                                objItemNew.SMSPassword = objSMSData.SMSPassword;
                                objItemNew.SMSlink = objSMSData.SMSlink;
                                objItemNew.SetId = objSet.SetId;

                                objItemNew.AddedBy = loginId;
                                objItemNew.AddedDate = DateTime.Now;
                                objItemNew.Id = itemSMS.Id;

                                if (objItemNew.Id > 0)
                                {
                                    var oldData = context.BOTS_TblSMSConfig.Where(x => x.Id == objItemNew.Id).FirstOrDefault();
                                    if (oldData != null)
                                    {
                                        objItemNew.AddedBy = oldData.AddedBy;
                                        objItemNew.AddedDate = oldData.AddedDate;
                                        objItemNew.TemplateId = oldData.TemplateId;
                                        objItemNew.TemplateName = oldData.TemplateName;
                                        objItemNew.TemplateType = oldData.TemplateType;
                                        objItemNew.DLTStatus = oldData.DLTStatus;
                                        objItemNew.SMSScriptDLT = oldData.SMSScriptDLT;
                                        objItemNew.PEID = oldData.PEID;
                                        objItemNew.RejectReason = oldData.RejectReason;
                                    }
                                    objItemNew.UpdatedBy = loginId;
                                    objItemNew.UpdatedDate = DateTime.Now;
                                }
                                objItemNew.MessageId = itemSMS.MessageId;
                                objItemNew.SMSScript = itemSMS.TemplateScript;

                                context.BOTS_TblSMSConfig.AddOrUpdate(objItemNew);
                                context.SaveChanges();

                            }
                            status = true;
                        }
                        if (objWAData.IsWA)
                        {
                            foreach (var itemWA in lstWA)
                            {
                                BOTS_TblWAConfig objItemWA = new BOTS_TblWAConfig();
                                objItemWA.IsWA = objWAData.IsWA;

                                objItemWA.WAProvider = objWAData.WAProvider;
                                objItemWA.GroupId = objWAData.GroupId;
                                objItemWA.BrandId = objWAData.BrandId;
                                objItemWA.WANumber = objWAData.WANumber;
                                objItemWA.WAUsername = objWAData.WAUsername;
                                objItemWA.WAPassword = objWAData.WAPassword;
                                objItemWA.WAlink = objWAData.WAlink;
                                objItemWA.TokenId = objWAData.TokenId;
                                objItemWA.SetId = objSet.SetId;

                                objItemWA.AddedBy = loginId;
                                objItemWA.AddedDate = DateTime.Now;
                                objItemWA.Id = itemWA.Id;

                                if (objItemWA.Id > 0)
                                {
                                    var oldData = context.BOTS_TblWAConfig.Where(x => x.Id == objItemWA.Id).FirstOrDefault();
                                    if (oldData != null)
                                    {
                                        objItemWA.AddedBy = oldData.AddedBy;
                                        objItemWA.AddedDate = oldData.AddedDate;
                                    }
                                    objItemWA.UpdatedBy = loginId;
                                    objItemWA.UpdatedDate = DateTime.Now;
                                }
                                objItemWA.MessageId = itemWA.MessageId;
                                objItemWA.WAScript = itemWA.TemplateScript;
                                context.BOTS_TblWAConfig.AddOrUpdate(objItemWA);
                                context.SaveChanges();
                            }
                            status = true;
                        }
                        transaction.Commit();
                        var GroupId = "";
                        if (objSMSData != null)
                            GroupId = objSMSData.GroupId;
                        if (objWAData != null)
                            GroupId = objWAData.GroupId;

                        AddTracking(Convert.ToString(GroupId), "Communication Data Added/Updated", loginId);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        newexception.AddException(ex, "SaveCommunicationConfig");
                    }
                }
            }
            return status;
        }

        public bool SendCommunicationToDLT(string groupId, string loginId)
        {
            bool status = false;
            using (var context = new CommonDBContext())
            {
                try
                {
                    var lstComm = context.BOTS_TblSMSConfig.Where(x => x.GroupId == groupId).ToList();
                    foreach (var itemNew in lstComm)
                    {
                        itemNew.DLTStatus = "Submitted";
                        itemNew.UpdatedBy = loginId;
                        itemNew.UpdatedDate = DateTime.Now;
                        context.BOTS_TblSMSConfig.AddOrUpdate(itemNew);
                        context.SaveChanges();
                    }
                    status = true;
                    AddTracking(Convert.ToString(groupId), "Communication Scripts (SMS) Sent for DLT", loginId);
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "SendCommunicationToDLT");
                }
            }
            return status;
        }

        public List<BOTS_TblSMSConfig> GetCommunicationSMSConfig(string GroupId, int SetId)
        {
            List<BOTS_TblSMSConfig> objData = new List<BOTS_TblSMSConfig>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    if (SetId > 0)
                    {
                        objData = context.BOTS_TblSMSConfig.Where(x => x.GroupId == GroupId && x.SetId == SetId).ToList();
                    }
                    else
                    {
                        objData = context.BOTS_TblSMSConfig.Where(x => x.GroupId == GroupId).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCommunicationSMSConfig");
            }
            return objData;
        }

        public List<BOTS_TblSMSConfig> GetCommunicationSMSConfigForDLT(string GroupId, int SetId)
        {
            List<BOTS_TblSMSConfig> objData = new List<BOTS_TblSMSConfig>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objData = context.BOTS_TblSMSConfig.Where(x => x.GroupId == GroupId && x.SetId == SetId && x.DLTStatus != "" && x.DLTStatus != null).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCommunicationSMSConfigForDLT");
            }
            return objData;
        }
        public List<BOTS_TblVariableWords> GetVariableWordsList()
        {
            List<BOTS_TblVariableWords> objData = new List<BOTS_TblVariableWords>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objData = context.BOTS_TblVariableWords.ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetVariableWordsList");
            }
            return objData;
        }

        public BOTS_TblSMSConfig GetCommunicationSMSConfigById(int Id)
        {
            BOTS_TblSMSConfig objSMSConfig = new BOTS_TblSMSConfig();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objSMSConfig = context.BOTS_TblSMSConfig.Where(x => x.Id == Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCommunicationSMSConfigById");
            }
            return objSMSConfig;
        }
        public bool SaveIndividualSMSConfig(BOTS_TblSMSConfig objItem)
        {
            bool status = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    context.BOTS_TblSMSConfig.AddOrUpdate(objItem);
                    context.SaveChanges();
                    status = true;
                    if (objItem.UpdatedBy == null || objItem.UpdatedBy == "")
                        AddTracking(Convert.ToString(objItem.GroupId), "SMS Script saved", objItem.AddedBy);
                    else
                        AddTracking(Convert.ToString(objItem.GroupId), "SMS Script saved", objItem.UpdatedBy);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveIndividualSMSConfig");
            }
            return status;
        }

        public bool UpdateStatusSMSConfig(int ItemId, string DLTStatus, string LoginId, string rejectReason, BOTS_TblSMSConfig objSMSConfig)
        {
            bool status = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    var objItem = context.BOTS_TblSMSConfig.Where(x => x.Id == ItemId).FirstOrDefault();
                    if (objItem != null)
                    {
                        objItem.UpdatedBy = LoginId;
                        objItem.UpdatedDate = DateTime.Now;
                        objItem.DLTStatus = DLTStatus;
                        if (DLTStatus == "Rejected")
                        {
                            if (!string.IsNullOrEmpty(objItem.RejectReason))
                                objItem.RejectReason = objItem.RejectReason + " // " + rejectReason;
                            else
                                objItem.RejectReason = rejectReason;
                        }
                        if (DLTStatus == "Approved")
                        {
                            objItem.TemplateId = objSMSConfig.TemplateId;
                            objItem.TemplateName = objSMSConfig.TemplateName;
                            objItem.TemplateType = objSMSConfig.TemplateType;
                            objItem.SMSScriptDLT = objSMSConfig.SMSScriptDLT;
                            objItem.SMSScript = objSMSConfig.SMSScript;
                        }
                        context.BOTS_TblSMSConfig.AddOrUpdate(objItem);
                        context.SaveChanges();
                        status = true;

                        AddTracking(Convert.ToString(objItem.GroupId), "DLT Status Changed - " + DLTStatus, LoginId);
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateStatusSMSConfig");
            }
            return status;
        }

        public bool UpdateUniqueSMSValues(BOTS_TblSMSConfig objData, string LoginId)
        {
            bool status = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    var lstOldData = context.BOTS_TblSMSConfig.Where(x => x.GroupId == objData.GroupId).ToList();
                    foreach (var item in lstOldData)
                    {
                        item.PEID = objData.PEID;
                        item.SMSProvider = objData.SMSProvider;
                        item.SMSSenderID = objData.SMSSenderID;
                        item.SMSUsername = objData.SMSUsername;
                        item.SMSPassword = objData.SMSPassword;
                        item.SMSlink = objData.SMSlink;
                        item.UpdatedBy = LoginId;
                        item.UpdatedDate = DateTime.Now;

                        context.BOTS_TblSMSConfig.AddOrUpdate(item);
                        context.SaveChanges();
                    }
                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateUniqueSMSValues");
            }
            return status;
        }

        public List<BOTS_TblWAConfig> GetCommunicationWAConfig(string GroupId, int SetId)
        {
            List<BOTS_TblWAConfig> objData = new List<BOTS_TblWAConfig>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    if (SetId > 0)
                    {
                        objData = context.BOTS_TblWAConfig.Where(x => x.GroupId == GroupId && x.SetId == SetId).ToList();
                    }
                    else
                    {
                        objData = context.BOTS_TblWAConfig.Where(x => x.GroupId == GroupId).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCommunicationWAConfig");
            }
            return objData;
        }

        public BOTS_TblCommunicationSet GetSetDetails(int SetId)
        {
            BOTS_TblCommunicationSet objSetData = new BOTS_TblCommunicationSet();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objSetData = context.BOTS_TblCommunicationSet.Where(x => x.SetId == SetId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetSetDetails");
            }
            return objSetData;
        }

        public bool SaveDLCLinkConfig(BOTS_TblDLCLinkConfig objData)
        {
            bool status = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    if (objData.Id > 0)
                    {
                        var oldData = context.BOTS_TblDLCLinkConfig.Where(x => x.Id == objData.Id).FirstOrDefault();
                        if (oldData != null)
                        {
                            objData.AddedBy = oldData.AddedBy;
                            objData.AddedDate = oldData.AddedDate;
                            objData.DLTStatus1 = oldData.DLTStatus1;
                            objData.DLTStatus2 = oldData.DLTStatus2;
                            objData.DLTStatus3 = oldData.DLTStatus3;
                            objData.DLTStatus4 = oldData.DLTStatus4;
                            objData.DLTStatus5 = oldData.DLTStatus5;
                            objData.DLTStatus6 = oldData.DLTStatus6;
                            objData.DLTStatus7 = oldData.DLTStatus7;
                            objData.TemplateId1 = oldData.TemplateId1;
                            objData.TemplateId2 = oldData.TemplateId2;
                            objData.TemplateId3 = oldData.TemplateId3;
                            objData.TemplateId4 = oldData.TemplateId4;
                            objData.TemplateId5 = oldData.TemplateId5;
                            objData.TemplateId6 = oldData.TemplateId6;
                            objData.TemplateId7 = oldData.TemplateId7;
                            objData.TemplateName1 = oldData.TemplateName1;
                            objData.TemplateName2 = oldData.TemplateName2;
                            objData.TemplateName3 = oldData.TemplateName3;
                            objData.TemplateName4 = oldData.TemplateName4;
                            objData.TemplateName5 = oldData.TemplateName5;
                            objData.TemplateName5 = oldData.TemplateName6;
                            objData.TemplateName6 = oldData.TemplateName7;
                            objData.TemplateType1 = oldData.TemplateType1;
                            objData.TemplateType2 = oldData.TemplateType2;
                            objData.TemplateType3 = oldData.TemplateType3;
                            objData.TemplateType4 = oldData.TemplateType4;
                            objData.TemplateType5 = oldData.TemplateType5;
                            objData.TemplateType6 = oldData.TemplateType6;
                            objData.TemplateType7 = oldData.TemplateType7;
                            objData.RejectReason1 = oldData.RejectReason1;
                            objData.RejectReason2 = oldData.RejectReason2;
                            objData.RejectReason3 = oldData.RejectReason3;
                            objData.RejectReason4 = oldData.RejectReason4;
                            objData.RejectReason5 = oldData.RejectReason5;
                            objData.RejectReason6 = oldData.RejectReason6;
                            objData.RejectReason7 = oldData.RejectReason7;
                        }
                    }
                    context.BOTS_TblDLCLinkConfig.AddOrUpdate(objData);
                    context.SaveChanges();
                    status = true;
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveDLCLinkConfig");
            }
            return status;
        }

        public BOTS_TblDLCLinkConfig GetDLCLinkData(string groupId)
        {
            BOTS_TblDLCLinkConfig objDLCLinkConfig = new BOTS_TblDLCLinkConfig();
            try
            {
                using (var context = new CommonDBContext())
                {

                    objDLCLinkConfig = context.BOTS_TblDLCLinkConfig.Where(x => x.GroupId == groupId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDLCLinkData");
            }
            return objDLCLinkConfig;
        }

        public bool SaveVelocityCheckConfig(List<BOTS_TblVelocityChecksConfig> lstVelocityCheck, string groupId)
        {
            bool status = false;
            using (var context = new CommonDBContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var oldData = context.BOTS_TblVelocityChecksConfig.Where(x => x.GroupId == groupId).ToList();
                        foreach (var item in oldData)
                        {
                            context.BOTS_TblVelocityChecksConfig.Remove(item);
                            context.SaveChanges();
                        }
                        foreach (var item in lstVelocityCheck)
                        {
                            context.BOTS_TblVelocityChecksConfig.AddOrUpdate(item);
                            context.SaveChanges();
                            status = true;
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        newexception.AddException(ex, "SaveVelocityCheckConfig");
                    }
                }
            }

            return status;
        }

        public List<BOTS_TblVelocityChecksConfig> GetVelocityChecksData(string groupId)
        {
            List<BOTS_TblVelocityChecksConfig> objData = new List<BOTS_TblVelocityChecksConfig>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objData = context.BOTS_TblVelocityChecksConfig.Where(x => x.GroupId == groupId).OrderBy(y => y.VelocityType).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetVelocityChecksData");
            }

            return objData;
        }

        public bool BulkInsert(DataTable dt)
        {
            bool status = false;
            try
            {
                var conStr = ConfigurationManager.ConnectionStrings["CommonDBContext"].ToString();

                SqlConnection con = new SqlConnection(conStr);
                SqlBulkCopy objbulk = new SqlBulkCopy(con);
                objbulk.DestinationTableName = "BOTS_TblBulkUpload";

                objbulk.ColumnMappings.Add("GroupId", "GroupId");
                //objbulk.ColumnMappings.Add("CustId", "CustId");
                objbulk.ColumnMappings.Add("CustName", "CustName");
                objbulk.ColumnMappings.Add("MobileNo", "MobileNo");
                //objbulk.ColumnMappings.Add("OutletId", "OutletId");
                objbulk.ColumnMappings.Add("Gender", "Gender");
                //objbulk.ColumnMappings.Add("Status", "Status");
                objbulk.ColumnMappings.Add("DOB", "DOB");
                objbulk.ColumnMappings.Add("AOB", "AOB");
                objbulk.ColumnMappings.Add("EmailId", "EmailId");
                objbulk.ColumnMappings.Add("City", "City");
                objbulk.ColumnMappings.Add("Area", "Area");
                //objbulk.ColumnMappings.Add("CustomerCategory", "CustomerCategory");
                objbulk.ColumnMappings.Add("CardNo", "CardNo");
                objbulk.ColumnMappings.Add("Points", "Points");
                objbulk.ColumnMappings.Add("OutletName", "OutletName");

                con.Open();
                objbulk.WriteToServer(dt);
                con.Close();
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "BulkInsert");
            }

            return status;
        }

        public bool SaveCampaignOtherConfig(BOTS_TblCampaignOtherConfig objData)
        {
            bool status = false;
            using (var context = new CommonDBContext())
            {
                try
                {
                    if (objData.Id > 0)
                    {
                        var oldRecord = context.BOTS_TblCampaignOtherConfig.Where(x => x.Id == objData.Id).FirstOrDefault();

                        objData.AddedBy = oldRecord.AddedBy;
                        objData.AddedDate = oldRecord.AddedDate;

                        objData.IntroScript1DLT = oldRecord.IntroScript1DLT;
                        objData.TemplateId1 = oldRecord.TemplateId1;
                        objData.TemplateName1 = oldRecord.TemplateName1;
                        objData.TemplateType1 = oldRecord.TemplateType1;
                        objData.DLTStatus1 = oldRecord.DLTStatus1;
                        objData.RejectReason1 = oldRecord.RejectReason1;

                        objData.IntroScript2DLT = oldRecord.IntroScript2DLT;
                        objData.TemplateId2 = oldRecord.TemplateId2;
                        objData.TemplateName2 = oldRecord.TemplateName2;
                        objData.TemplateType2 = oldRecord.TemplateType2;
                        objData.DLTStatus2 = oldRecord.DLTStatus2;
                        objData.RejectReason2 = oldRecord.RejectReason2;

                        objData.ReminderScript1DLT = oldRecord.ReminderScript1DLT;
                        objData.TemplateId3 = oldRecord.TemplateId3;
                        objData.TemplateName3 = oldRecord.TemplateName3;
                        objData.TemplateType3 = oldRecord.TemplateType3;
                        objData.DLTStatus3 = oldRecord.DLTStatus3;
                        objData.RejectReason3 = oldRecord.RejectReason3;

                        objData.ReminderScript2DLT = oldRecord.ReminderScript2DLT;
                        objData.TemplateId4 = oldRecord.TemplateId4;
                        objData.TemplateName4 = oldRecord.TemplateName4;
                        objData.TemplateType4 = oldRecord.TemplateType4;
                        objData.DLTStatus4 = oldRecord.DLTStatus4;
                        objData.RejectReason4 = oldRecord.RejectReason4;

                        objData.OnDayScriptPTDLT = oldRecord.OnDayScriptPTDLT;
                        objData.TemplateId5 = oldRecord.TemplateId5;
                        objData.TemplateName5 = oldRecord.TemplateName5;
                        objData.TemplateType5 = oldRecord.TemplateType5;
                        objData.DLTStatus5 = oldRecord.DLTStatus5;
                        objData.RejectReason5 = oldRecord.RejectReason5;

                        objData.OnDayScriptNPTDLT = oldRecord.OnDayScriptNPTDLT;
                        objData.TemplateId6 = oldRecord.TemplateId6;
                        objData.TemplateName6 = oldRecord.TemplateName6;
                        objData.TemplateType6 = oldRecord.TemplateType6;
                        objData.DLTStatus6 = oldRecord.DLTStatus6;
                        objData.RejectReason6 = oldRecord.RejectReason6;


                    }
                    context.BOTS_TblCampaignOtherConfig.AddOrUpdate(objData);
                    context.SaveChanges();
                    status = true;
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "SaveCampaignOtherConfig");
                }
            }
            return status;
        }

        public BOTS_TblCampaignOtherConfig GetCampaignOtherConfig(string groupId, string type)
        {
            BOTS_TblCampaignOtherConfig objData = new BOTS_TblCampaignOtherConfig();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objData = context.BOTS_TblCampaignOtherConfig.Where(x => x.GroupId == groupId && x.CampaignType == type).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCampaignOtherConfig");
            }
            return objData;
        }

        public bool SaveInactiveConfig(List<BOTS_TblCampaignInactive> lstData, string GroupID, string type)
        {
            bool status = false;

            using (var context = new CommonDBContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var existingRecords = context.BOTS_TblCampaignInactive.Where(x => x.GroupId == GroupID && x.InactiveType == type).ToList();
                        foreach (var item in existingRecords)
                        {
                            var oldItem = lstData.Where(x => x.Id == item.Id).FirstOrDefault();
                            if (oldItem == null)
                            {
                                context.BOTS_TblCampaignInactive.Remove(item);
                                context.SaveChanges();
                            }
                        }
                        foreach (var objItem in lstData)
                        {
                            if (objItem.Id > 0)
                            {
                                var oldRecord = context.BOTS_TblCampaignInactive.Where(x => x.Id == objItem.Id).FirstOrDefault();
                                objItem.AddedBy = oldRecord.AddedBy;
                                objItem.AddedDate = oldRecord.AddedDate;

                                objItem.LessThanDaysScriptDLT = oldRecord.LessThanDaysScriptDLT;
                                objItem.TemplateId1 = oldRecord.TemplateId1;
                                objItem.TemplateName1 = oldRecord.TemplateName1;
                                objItem.TemplateType1 = oldRecord.TemplateType1;
                                objItem.DLTStatus1 = oldRecord.DLTStatus1;
                                objItem.RejectReason1 = oldRecord.RejectReason1;

                                objItem.GreaterThanDaysScriptDLT = oldRecord.GreaterThanDaysScriptDLT;
                                objItem.TemplateId2 = oldRecord.TemplateId2;
                                objItem.TemplateName2 = oldRecord.TemplateName2;
                                objItem.TemplateType2 = oldRecord.TemplateType2;
                                objItem.DLTStatus2 = oldRecord.DLTStatus2;
                                objItem.RejectReason2 = oldRecord.RejectReason2;
                            }
                            context.BOTS_TblCampaignInactive.AddOrUpdate(objItem);
                            context.SaveChanges();
                        }
                        status = true;
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        newexception.AddException(ex, "SaveInactiveConfig");
                    }
                }
            }
            return status;
        }

        public List<BOTS_TblCampaignInactive> GetInactiveConfigData(string groupId, string type)
        {
            List<BOTS_TblCampaignInactive> lstData = new List<BOTS_TblCampaignInactive>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    lstData = context.BOTS_TblCampaignInactive.Where(x => x.GroupId == groupId && x.InactiveType == type).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetInactiveConfigData");
            }
            return lstData;
        }

        public List<BOTS_TblCommunicationSet> GetCommunicationSetsByGroupId(string GroupId)
        {
            List<BOTS_TblCommunicationSet> lstSets = new List<BOTS_TblCommunicationSet>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    lstSets = context.BOTS_TblCommunicationSet.Where(x => x.GroupId == GroupId).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCommunicationSetsByGroupId");
            }
            return lstSets;
        }

        public List<SelectListItem> GetCommunicationSetList(string GroupId)
        {
            List<SelectListItem> lstCommunicationSet = new List<SelectListItem>();
            SelectListItem item = new SelectListItem();
            item.Value = "0";
            item.Text = "Please Select";
            lstCommunicationSet.Add(item);
            try
            {
                using (var context = new CommonDBContext())
                {
                    var CommunicationSets = context.BOTS_TblCommunicationSet.Where(x => x.GroupId == GroupId).ToList();
                    foreach (var item1 in CommunicationSets)
                    {
                        lstCommunicationSet.Add(new SelectListItem
                        {
                            Text = item1.SetName,
                            Value = Convert.ToString(item1.SetId)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCommunicationSetList");
            }
            return lstCommunicationSet;
        }

        public List<SelectListItem> GetOutletListWithAssignment(string GroupId, string SetId)
        {
            List<SelectListItem> lstOutlets = new List<SelectListItem>();
            List<BOTS_TblOutletMaster> objData = new List<BOTS_TblOutletMaster>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objData = context.BOTS_TblOutletMaster.Where(x => x.GroupId == GroupId).ToList();
                    var SetIdInt = Convert.ToInt32(SetId);
                    var outletForSet = context.BOTS_TblCommunicationSetAssignment.Where(x => x.GroupId == GroupId && x.SetId == SetIdInt).ToList();

                    var outletForSetNew = context.BOTS_TblCommunicationSetAssignment.Where(x => x.GroupId == GroupId && x.SetId != SetIdInt).ToList();

                    foreach (var outlet in objData)
                    {
                        var brandName = context.BOTS_TblRetailMaster.Where(x => x.BrandId == outlet.BrandId && x.GroupId == GroupId).Select(y => y.BrandName).FirstOrDefault();
                        SelectListItem newItem = new SelectListItem();
                        newItem.Value = Convert.ToString(outlet.Id);
                        newItem.Text = brandName + " - " + outlet.OutletName;
                        var exist = outletForSet.Where(x => x.OutletId == outlet.Id.ToString()).FirstOrDefault();
                        if (exist != null)
                        {
                            newItem.Selected = true;
                            //newItem.Disabled = true;
                        }
                        var existNew = outletForSetNew.Where(x => x.OutletId == outlet.Id.ToString()).FirstOrDefault();
                        if (existNew != null)
                        {
                            newItem.Disabled = true;
                        }

                        lstOutlets.Add(newItem);
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOutletListWithAssignment");
            }
            return lstOutlets;
        }

        public bool AssignCommunicationSetsToOutlets(string GroupId, int SetId, List<BOTS_TblCommunicationSetAssignment> lstData)
        {
            bool status = false;
            using (var context = new CommonDBContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var oldData = context.BOTS_TblCommunicationSetAssignment.Where(x => x.GroupId == GroupId && x.SetId == SetId).ToList();
                        foreach (var item in oldData)
                        {
                            context.BOTS_TblCommunicationSetAssignment.Remove(item);
                            context.SaveChanges();
                        }

                        foreach (var newItem in lstData)
                        {
                            context.BOTS_TblCommunicationSetAssignment.AddOrUpdate(newItem);
                            context.SaveChanges();
                        }
                        transaction.Commit();
                        status = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        newexception.AddException(ex, "AssignCommunicationSetsToOutlets");
                    }
                }
            }
            return status;
        }

        public string GetConvertedScript(string CSScript)
        {
            string convertStr = string.Empty;
            var arrStr = CSScript.Split(' ');
            try
            {
                using (var context = new CommonDBContext())
                {
                    foreach (var item in arrStr)
                    {
                        if (item.Contains("#"))
                        {
                            convertStr = convertStr + "{#var#}";
                            if (item.Contains(","))
                            {
                                convertStr = convertStr + ", ";
                            }
                            else if (item.Contains("."))
                            {
                                convertStr = convertStr + ". ";
                            }
                            else
                            {
                                convertStr = convertStr + " ";
                            }
                        }
                        else
                        {
                            var isExist = context.BOTS_TblVariableWords.Where(x => x.VariableWords == item.ToLower()).FirstOrDefault();
                            if (isExist != null)
                            {
                                convertStr = convertStr + "{#var#} ";
                            }
                            else
                            {
                                convertStr = convertStr + item + " ";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetConvertedScript");
            }
            return convertStr.Trim();
        }

        public bool SendPerpetualCampaignToDLT(string GroupId, int CampaignId, string CampaignType, string LoginId)
        {
            bool status = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    if (CampaignType == "Birthday" || CampaignType == "Anniversary" || CampaignType == "DLC Update Reminder" || CampaignType == "Balance Updates" || CampaignType == "Reminder Bulk Uploaded Users" || CampaignType == "DLC Referral Reminder")
                    {
                        var objCampaign = context.BOTS_TblCampaignOtherConfig.Where(x => x.GroupId == GroupId && x.Id == CampaignId && x.CampaignType == CampaignType).FirstOrDefault();
                        if (objCampaign != null)
                        {
                            objCampaign.DLTStatus1 = "Submitted";
                            objCampaign.DLTStatus2 = "Submitted";
                            objCampaign.DLTStatus3 = "Submitted";
                            objCampaign.DLTStatus4 = "Submitted";
                            objCampaign.DLTStatus5 = "Submitted";
                            objCampaign.DLTStatus6 = "Submitted";

                            objCampaign.UpdatedBy = LoginId;
                            objCampaign.UpdatedDate = DateTime.Now;
                            context.BOTS_TblCampaignOtherConfig.AddOrUpdate(objCampaign);
                            context.SaveChanges();
                            status = true;
                        }
                    }
                    if (CampaignType == "Inactive" || CampaignType == "Only Once Inactive" || CampaignType == "Non Redemption Inactive" || CampaignType == "Point Expiry")
                    {
                        var objCampaignLST = context.BOTS_TblCampaignInactive.Where(x => x.GroupId == GroupId && x.InactiveType == CampaignType).ToList();
                        foreach (var item in objCampaignLST)
                        {
                            item.DLTStatus1 = "Submitted";
                            item.DLTStatus2 = "Submitted";
                            item.UpdatedBy = LoginId;
                            item.UpdatedDate = DateTime.Now;
                            context.BOTS_TblCampaignInactive.AddOrUpdate(item);
                            context.SaveChanges();
                            status = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SendPerpetualCampaignToDLT");
            }
            return status;
        }

        public bool AddVariableWord(string word)
        {
            bool status = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    var isexist = context.BOTS_TblVariableWords.Where(x => x.VariableWords == word).FirstOrDefault();
                    if (isexist == null)
                    {
                        BOTS_TblVariableWords objData = new BOTS_TblVariableWords();
                        objData.VariableWords = word;
                        context.BOTS_TblVariableWords.AddOrUpdate(objData);
                        context.SaveChanges();
                        status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AddVariableWord");
            }
            return status;
        }

        public BOTS_TblCampaignOtherConfig GetCampaignOtherConfigForDLT(string groupId, string type)
        {
            BOTS_TblCampaignOtherConfig objData = new BOTS_TblCampaignOtherConfig();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objData = context.BOTS_TblCampaignOtherConfig.Where(x => x.GroupId == groupId && x.CampaignType == type).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCampaignOtherConfigForDLT");
            }
            return objData;
        }
        public List<BOTS_TblCampaignInactive> GetCampaignAllInactiveForDLT(string groupId, string type)
        {
            List<BOTS_TblCampaignInactive> objData = new List<BOTS_TblCampaignInactive>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objData = context.BOTS_TblCampaignInactive.Where(x => x.GroupId == groupId && x.InactiveType == type).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCampaignAllInactiveForDLT");
            }
            return objData;
        }
        public bool UpdateBADLTStatus(int id, int statusid, string status, string loginid, string reason)
        {
            bool result = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    var objData = context.BOTS_TblCampaignOtherConfig.Where(x => x.Id == id).FirstOrDefault();
                    if (objData != null)
                    {
                        if (statusid == 1)
                        {
                            objData.DLTStatus1 = status;
                            if (status == "Rejected")
                            {
                                if (string.IsNullOrEmpty(objData.RejectReason1))
                                    objData.RejectReason1 = reason;
                                else
                                    objData.RejectReason1 = objData.RejectReason1 + " // " + reason;
                            }
                        }
                        if (statusid == 2)
                        {
                            objData.DLTStatus2 = status;
                            if (status == "Rejected")
                            {
                                if (string.IsNullOrEmpty(objData.RejectReason2))
                                    objData.RejectReason2 = reason;
                                else
                                    objData.RejectReason2 = objData.RejectReason2 + " // " + reason;
                            }
                        }
                        if (statusid == 3)
                        {
                            objData.DLTStatus3 = status;
                            if (status == "Rejected")
                            {
                                if (string.IsNullOrEmpty(objData.RejectReason3))
                                    objData.RejectReason3 = reason;
                                else
                                    objData.RejectReason3 = objData.RejectReason3 + " // " + reason;
                            }
                        }
                        if (statusid == 4)
                        {
                            objData.DLTStatus4 = status;
                            if (status == "Rejected")
                            {
                                if (string.IsNullOrEmpty(objData.RejectReason4))
                                    objData.RejectReason4 = reason;
                                else
                                    objData.RejectReason4 = objData.RejectReason4 + " // " + reason;
                            }
                        }
                        if (statusid == 5)
                        {
                            objData.DLTStatus5 = status;
                            if (status == "Rejected")
                            {
                                if (string.IsNullOrEmpty(objData.RejectReason5))
                                    objData.RejectReason5 = reason;
                                else
                                    objData.RejectReason5 = objData.RejectReason5 + " // " + reason;
                            }
                        }
                        if (statusid == 6)
                        {
                            objData.DLTStatus6 = status;
                            if (status == "Rejected")
                            {
                                if (string.IsNullOrEmpty(objData.RejectReason6))
                                    objData.RejectReason6 = reason;
                                else
                                    objData.RejectReason6 = objData.RejectReason6 + " // " + reason;
                            }
                        }

                        objData.UpdatedBy = loginid;
                        objData.UpdatedDate = DateTime.Now;
                        context.BOTS_TblCampaignOtherConfig.AddOrUpdate(objData);
                        context.SaveChanges();
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateBADLTStatus");
            }
            return result;
        }

        public bool SaveBADLTConfig(int id, int statusid, string status, string IntroDays, string IntroDaysDLT, string TemplateId, string TemplateName, string TemplateType, string loginid)
        {
            bool result = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    var existingObj = context.BOTS_TblCampaignOtherConfig.Where(x => x.Id == id).FirstOrDefault();
                    if (existingObj != null)
                    {
                        if (!string.IsNullOrEmpty(status))
                        {
                            if (statusid == 1)
                            {
                                existingObj.DLTStatus1 = status;
                            }
                            if (statusid == 2)
                            {
                                existingObj.DLTStatus2 = status;
                            }
                            if (statusid == 3)
                            {
                                existingObj.DLTStatus3 = status;
                            }
                            if (statusid == 4)
                            {
                                existingObj.DLTStatus4 = status;
                            }
                            if (statusid == 5)
                            {
                                existingObj.DLTStatus5 = status;
                            }
                            if (statusid == 6)
                            {
                                existingObj.DLTStatus6 = status;
                            }
                        }
                        if (statusid == 1)
                        {
                            if (existingObj.SMSType == "SMS")
                                existingObj.IntroScript1 = IntroDays;
                            else
                                existingObj.SMSScript1 = IntroDays;

                            existingObj.IntroScript1DLT = IntroDaysDLT;
                            existingObj.TemplateId1 = TemplateId;
                            existingObj.TemplateName1 = TemplateName;
                            existingObj.TemplateType1 = TemplateType;
                        }
                        if (statusid == 2)
                        {
                            if (existingObj.SMSType == "SMS")
                                existingObj.IntroScript2 = IntroDays;
                            else
                                existingObj.SMSScript2 = IntroDays;

                            //existingObj.IntroScript2 = IntroDays;
                            existingObj.IntroScript2DLT = IntroDaysDLT;
                            existingObj.TemplateId2 = TemplateId;
                            existingObj.TemplateName2 = TemplateName;
                            existingObj.TemplateType2 = TemplateType;
                        }
                        if (statusid == 3)
                        {
                            if (existingObj.SMSType == "SMS")
                                existingObj.ReminderScript1 = IntroDays;
                            else
                                existingObj.SMSScript3 = IntroDays;

                            //existingObj.ReminderScript1 = IntroDays;
                            existingObj.ReminderScript1DLT = IntroDaysDLT;
                            existingObj.TemplateId3 = TemplateId;
                            existingObj.TemplateName3 = TemplateName;
                            existingObj.TemplateType3 = TemplateType;
                        }
                        if (statusid == 4)
                        {
                            if (existingObj.SMSType == "SMS")
                                existingObj.ReminderScript2 = IntroDays;
                            else
                                existingObj.SMSScript4 = IntroDays;

                            //existingObj.ReminderScript2 = IntroDays;
                            existingObj.ReminderScript2DLT = IntroDaysDLT;
                            existingObj.TemplateId4 = TemplateId;
                            existingObj.TemplateName4 = TemplateName;
                            existingObj.TemplateType4 = TemplateType;
                        }
                        if (statusid == 5)
                        {
                            if (existingObj.SMSType == "SMS")
                                existingObj.OnDayScriptPT = IntroDays;
                            else
                                existingObj.SMSScript5 = IntroDays;

                            existingObj.OnDayScriptPT = IntroDays;
                            existingObj.OnDayScriptPTDLT = IntroDaysDLT;
                            existingObj.TemplateId5 = TemplateId;
                            existingObj.TemplateName5 = TemplateName;
                            existingObj.TemplateType5 = TemplateType;
                        }
                        if (statusid == 6)
                        {
                            if (existingObj.SMSType == "SMS")
                                existingObj.OnDayScriptNPT = IntroDays;
                            else
                                existingObj.SMSScript6 = IntroDays;

                            //existingObj.OnDayScriptNPT = IntroDays;
                            existingObj.OnDayScriptNPTDLT = IntroDaysDLT;
                            existingObj.TemplateId6 = TemplateId;
                            existingObj.TemplateName6 = TemplateName;
                            existingObj.TemplateType6 = TemplateType;
                        }

                        existingObj.UpdatedBy = loginid;
                        existingObj.UpdatedDate = DateTime.Now;
                        context.BOTS_TblCampaignOtherConfig.AddOrUpdate(existingObj);
                        context.SaveChanges();
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveBADLTConfig");
            }

            return result;
        }

        public bool UpdateInactiveDLTStatus(int id, int statusid, string status, string loginid, string reason)
        {
            bool result = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    var objData = context.BOTS_TblCampaignInactive.Where(x => x.Id == id).FirstOrDefault();
                    if (objData != null)
                    {
                        if (statusid == 1)
                        {
                            objData.DLTStatus1 = status;
                            if (status == "Rejected")
                            {
                                if (string.IsNullOrEmpty(objData.RejectReason1))
                                    objData.RejectReason1 = reason;
                                else
                                    objData.RejectReason1 = objData.RejectReason1 + " // " + reason;
                            }
                        }
                        if (statusid == 2)
                        {
                            objData.DLTStatus2 = status;
                            if (status == "Rejected")
                            {
                                if (string.IsNullOrEmpty(objData.RejectReason2))
                                    objData.RejectReason2 = reason;
                                else
                                    objData.RejectReason2 = objData.RejectReason2 + " // " + reason;
                            }
                        }

                        objData.UpdatedBy = loginid;
                        objData.UpdatedDate = DateTime.Now;
                        context.BOTS_TblCampaignInactive.AddOrUpdate(objData);
                        context.SaveChanges();
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateInactiveDLTStatus");
            }
            return result;
        }

        public BOTS_TblCampaignInactive GetInactiveConfigById(int Id)
        {
            BOTS_TblCampaignInactive objInactiveConfig = new BOTS_TblCampaignInactive();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objInactiveConfig = context.BOTS_TblCampaignInactive.Where(x => x.Id == Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetInactiveConfigById");
            }
            return objInactiveConfig;
        }

        public bool SaveInactiveDLTConfig(BOTS_TblCampaignInactive objData)
        {
            bool result = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    context.BOTS_TblCampaignInactive.AddOrUpdate(objData);
                    context.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveInactiveDLTConfig");
            }
            return result;
        }

        public BOTS_TblCampaignOtherConfig GetCampaignRemainingForDLT(string groupId, string type)
        {
            BOTS_TblCampaignOtherConfig objData = new BOTS_TblCampaignOtherConfig();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objData = context.BOTS_TblCampaignOtherConfig.Where(x => x.GroupId == groupId && x.CampaignType == type).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCampaignRemainingForDLT");
            }
            return objData;
        }

        public bool UpdateRemainingDLTStatus(int id, int statusid, string status, string loginid, string reason)
        {
            bool result = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    var objData = context.BOTS_TblCampaignOtherConfig.Where(x => x.Id == id).FirstOrDefault();
                    if (objData != null)
                    {
                        if (statusid == 1)
                        {
                            objData.DLTStatus1 = status;
                            if (status == "Rejected")
                            {
                                if (string.IsNullOrEmpty(objData.RejectReason1))
                                    objData.RejectReason1 = reason;
                                else
                                    objData.RejectReason1 = objData.RejectReason1 + " // " + reason;
                            }
                        }
                        if (statusid == 2)
                        {
                            objData.DLTStatus2 = status;
                            if (status == "Rejected")
                            {
                                if (string.IsNullOrEmpty(objData.RejectReason2))
                                    objData.RejectReason2 = reason;
                                else
                                    objData.RejectReason2 = objData.RejectReason2 + " // " + reason;
                            }
                        }

                        objData.UpdatedBy = loginid;
                        objData.UpdatedDate = DateTime.Now;
                        context.BOTS_TblCampaignOtherConfig.AddOrUpdate(objData);
                        context.SaveChanges();
                        result = true;
                    }
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateRemainingDLTStatus");
            }
            return result;
        }

        public bool SendDLCToDLT(string groupId, string loginId)
        {
            bool status = false;
            using (var context = new CommonDBContext())
            {
                try
                {
                    var objDLCLinkConfig = context.BOTS_TblDLCLinkConfig.Where(x => x.GroupId == groupId).FirstOrDefault();
                    if (objDLCLinkConfig != null)
                    {
                        objDLCLinkConfig.DLTStatus1 = "Submitted";
                        objDLCLinkConfig.DLTStatus2 = "Submitted";
                        objDLCLinkConfig.DLTStatus3 = "Submitted";
                        objDLCLinkConfig.DLTStatus4 = "Submitted";
                        objDLCLinkConfig.DLTStatus5 = "Submitted";
                        objDLCLinkConfig.DLTStatus6 = "Submitted";
                        objDLCLinkConfig.DLTStatus7 = "Submitted";


                        objDLCLinkConfig.UpdatedBy = loginId;
                        objDLCLinkConfig.UpdatedDate = DateTime.Now;
                        context.BOTS_TblDLCLinkConfig.AddOrUpdate(objDLCLinkConfig);
                        context.SaveChanges();
                    }
                    status = true;
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "SendDLCToDLT");
                }
            }
            return status;
        }

        public bool UpdateDLCLinkDLTStatus(int id, int statusid, string status, string loginid, string reason)
        {
            bool result = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    var objDLCLinkConfig = context.BOTS_TblDLCLinkConfig.Where(x => x.Id == id).FirstOrDefault();
                    if (objDLCLinkConfig != null)
                    {
                        if (statusid == 1)
                        {
                            objDLCLinkConfig.DLTStatus1 = status;
                            if (status == "Rejected")
                            {
                                if (string.IsNullOrEmpty(objDLCLinkConfig.RejectReason1))
                                    objDLCLinkConfig.RejectReason1 = reason;
                                else
                                    objDLCLinkConfig.RejectReason1 = objDLCLinkConfig.RejectReason1 + " // " + reason;
                            }
                        }
                        if (statusid == 2)
                        {
                            objDLCLinkConfig.DLTStatus2 = status;
                            if (status == "Rejected")
                            {
                                if (string.IsNullOrEmpty(objDLCLinkConfig.RejectReason2))
                                    objDLCLinkConfig.RejectReason2 = reason;
                                else
                                    objDLCLinkConfig.RejectReason2 = objDLCLinkConfig.RejectReason2 + " // " + reason;
                            }
                        }
                        if (statusid == 3)
                        {
                            objDLCLinkConfig.DLTStatus3 = status;
                            if (status == "Rejected")
                            {
                                if (string.IsNullOrEmpty(objDLCLinkConfig.RejectReason3))
                                    objDLCLinkConfig.RejectReason3 = reason;
                                else
                                    objDLCLinkConfig.RejectReason3 = objDLCLinkConfig.RejectReason3 + " // " + reason;
                            }
                        }
                        if (statusid == 4)
                        {
                            objDLCLinkConfig.DLTStatus4 = status;
                            if (status == "Rejected")
                            {
                                if (string.IsNullOrEmpty(objDLCLinkConfig.RejectReason4))
                                    objDLCLinkConfig.RejectReason4 = reason;
                                else
                                    objDLCLinkConfig.RejectReason4 = objDLCLinkConfig.RejectReason4 + " // " + reason;
                            }
                        }
                        if (statusid == 5)
                        {
                            objDLCLinkConfig.DLTStatus5 = status;
                            if (status == "Rejected")
                            {
                                if (string.IsNullOrEmpty(objDLCLinkConfig.RejectReason5))
                                    objDLCLinkConfig.RejectReason5 = reason;
                                else
                                    objDLCLinkConfig.RejectReason5 = objDLCLinkConfig.RejectReason5 + " // " + reason;
                            }
                        }
                        if (statusid == 6)
                        {
                            objDLCLinkConfig.DLTStatus6 = status;
                            if (status == "Rejected")
                            {
                                if (string.IsNullOrEmpty(objDLCLinkConfig.RejectReason6))
                                    objDLCLinkConfig.RejectReason6 = reason;
                                else
                                    objDLCLinkConfig.RejectReason6 = objDLCLinkConfig.RejectReason6 + " // " + reason;
                            }
                        }
                        if (statusid == 7)
                        {
                            objDLCLinkConfig.DLTStatus7 = status;
                            if (status == "Rejected")
                            {
                                if (string.IsNullOrEmpty(objDLCLinkConfig.RejectReason7))
                                    objDLCLinkConfig.RejectReason7 = reason;
                                else
                                    objDLCLinkConfig.RejectReason7 = objDLCLinkConfig.RejectReason7 + " // " + reason;
                            }
                        }
                        if (statusid == 8)
                        {
                            objDLCLinkConfig.DLTStatus8 = status;
                            if (status == "Rejected")
                            {
                                if (string.IsNullOrEmpty(objDLCLinkConfig.RejectReason8))
                                    objDLCLinkConfig.RejectReason8 = reason;
                                else
                                    objDLCLinkConfig.RejectReason8 = objDLCLinkConfig.RejectReason8 + " // " + reason;
                            }
                        }

                        objDLCLinkConfig.UpdatedBy = loginid;
                        objDLCLinkConfig.UpdatedDate = DateTime.Now;
                        context.BOTS_TblDLCLinkConfig.AddOrUpdate(objDLCLinkConfig);
                        context.SaveChanges();
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateDLCLinkDLTStatus");
            }
            return result;
        }

        public BOTS_TblDLCLinkConfig GetDLCLinkDLTConfigById(int Id)
        {
            BOTS_TblDLCLinkConfig objDLCLinkConfig = new BOTS_TblDLCLinkConfig();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objDLCLinkConfig = context.BOTS_TblDLCLinkConfig.Where(x => x.Id == Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDLCLinkDLTConfigById");
            }
            return objDLCLinkConfig;
        }

        public bool SaveDLCLinkDLTConfig(BOTS_TblDLCLinkConfig objDLCLinkConfig)
        {
            bool result = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    context.BOTS_TblDLCLinkConfig.AddOrUpdate(objDLCLinkConfig);
                    context.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveDLCLinkDLTConfig");
            }
            return result;
        }

        public bool SaveEarnRule(BOTS_TblEarnRuleConfig objData, List<BOTS_TblSlabConfig> lstSlabs, List<BOTS_TblProductUpload> lstEarnProd, List<BOTS_TblProductUpload> lstBlockEatn)
        {
            bool result = false;
            using (var context = new CommonDBContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var oldEarnRule = context.BOTS_TblEarnRuleConfig.Where(x => x.GroupId == objData.GroupId).FirstOrDefault();
                        if (oldEarnRule != null)
                        {
                            objData.AddedBy = oldEarnRule.AddedBy;
                            objData.AddedDate = oldEarnRule.AddedDate;
                        }
                        context.BOTS_TblEarnRuleConfig.AddOrUpdate(objData);
                        context.SaveChanges();

                        //Slab
                        if (lstSlabs.Count > 0)
                        {
                            var oldSlabData = context.BOTS_TblSlabConfig.Where(x => x.GroupId == objData.GroupId).ToList();
                            foreach (var item in oldSlabData)
                            {
                                context.BOTS_TblSlabConfig.Remove(item);
                                context.SaveChanges();
                            }

                            foreach (var item in lstSlabs)
                            {
                                context.BOTS_TblSlabConfig.AddOrUpdate(item);
                                context.SaveChanges();
                            }
                        }

                        //Product Upload
                        if (lstEarnProd.Count > 0)
                        {
                            var oldProductData = context.BOTS_TblProductUpload.Where(x => x.GroupId == objData.GroupId && x.Type == "Product Earn").ToList();
                            foreach (var item in oldProductData)
                            {
                                context.BOTS_TblProductUpload.Remove(item);
                                context.SaveChanges();
                            }

                            foreach (var item in lstEarnProd)
                            {
                                context.BOTS_TblProductUpload.AddOrUpdate(item);
                                context.SaveChanges();
                            }
                        }

                        //Block For Earn
                        if (lstBlockEatn.Count > 0)
                        {
                            var oldBlockData = context.BOTS_TblProductUpload.Where(x => x.GroupId == objData.GroupId && x.Type == "Block Earn").ToList();
                            foreach (var item in oldBlockData)
                            {
                                context.BOTS_TblProductUpload.Remove(item);
                                context.SaveChanges();
                            }

                            foreach (var item in lstBlockEatn)
                            {
                                context.BOTS_TblProductUpload.AddOrUpdate(item);
                                context.SaveChanges();
                            }
                        }
                        transaction.Commit();
                        result = true;

                    }
                    catch (Exception ex)
                    {
                        newexception.AddException(ex, "SaveEarnRule");
                        transaction.Rollback();
                    }
                }
            }
            return result;
        }

        public BOTS_TblEarnRuleConfig GetEarnRuleConfig(string GroupId)
        {
            BOTS_TblEarnRuleConfig objData = new BOTS_TblEarnRuleConfig();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objData = context.BOTS_TblEarnRuleConfig.Where(x => x.GroupId == GroupId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetEarnRuleConfig");
            }
            return objData;
        }
        public List<BOTS_TblSlabConfig> GetEarnRuleSlabConfig(string GroupId)
        {
            List<BOTS_TblSlabConfig> objData = new List<BOTS_TblSlabConfig>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objData = context.BOTS_TblSlabConfig.Where(x => x.GroupId == GroupId).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetEarnRuleSlabConfig");
            }
            return objData;
        }

        public List<BOTS_TblProductUpload> GetProductUpload(string GroupId)
        {
            List<BOTS_TblProductUpload> objData = new List<BOTS_TblProductUpload>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objData = context.BOTS_TblProductUpload.Where(x => x.GroupId == GroupId).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetProductUpload");
            }
            return objData;
        }

        public BOTS_TblBurnRuleConfig GetBurnRuleConfig(string GroupId)
        {
            BOTS_TblBurnRuleConfig objData = new BOTS_TblBurnRuleConfig();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objData = context.BOTS_TblBurnRuleConfig.Where(x => x.GroupId == GroupId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetBurnRuleConfig");
            }
            return objData;
        }
        public bool SaveBurnRule(BOTS_TblBurnRuleConfig objData, List<BOTS_TblProductUpload> lstBurnBlock)
        {
            bool result = false;
            using (var context = new CommonDBContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var oldBurnRule = context.BOTS_TblBurnRuleConfig.Where(x => x.GroupId == objData.GroupId).FirstOrDefault();
                        if (oldBurnRule != null)
                        {
                            objData.AddedBy = oldBurnRule.AddedBy;
                            objData.AddedDate = oldBurnRule.AddedDate;
                            if (!objData.IsProductCodeBlocking)
                            {
                                objData.IsProductCodeBlocking = oldBurnRule.IsProductCodeBlocking;
                                objData.ProductCodeBlockingType = oldBurnRule.ProductCodeBlockingType;
                            }
                        }
                        context.BOTS_TblBurnRuleConfig.AddOrUpdate(objData);
                        context.SaveChanges();

                        if (lstBurnBlock.Count > 0)
                        {
                            var oldData = context.BOTS_TblProductUpload.Where(x => x.GroupId == objData.GroupId && x.Type == "Product Block Burn").ToList();
                            if (oldData != null)
                            {
                                foreach (var item in oldData)
                                {
                                    context.BOTS_TblProductUpload.Remove(item);
                                    context.SaveChanges();
                                }
                            }
                            foreach (var item in lstBurnBlock)
                            {
                                context.BOTS_TblProductUpload.AddOrUpdate(item);
                                context.SaveChanges();
                            }
                        }

                        transaction.Commit();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        newexception.AddException(ex, "SaveBurnRule");
                        transaction.Rollback();
                    }
                }
            }
            return result;
        }

        public ValidateApproval ValidateSendForApproval(string GroupId)
        {
            ValidateApproval ObjValidation = new ValidateApproval();

            try
            {
                using (var contex = new CommonDBContext())
                {
                    ObjValidation.Outletstatus = contex.BOTS_TblOutletMaster.Select(x => x.GroupId).Contains(GroupId);
                    ObjValidation.Earnstatus = contex.BOTS_TblEarnRuleConfig.Select(x => x.GroupId).Contains(GroupId);
                    ObjValidation.Burnstatus = contex.BOTS_TblBurnRuleConfig.Select(x => x.GroupId).Contains(GroupId);
                    ObjValidation.CommCount = contex.BOTS_TblCommunicationSet.Select(x => x.SetId).Count();
                    if(ObjValidation.CommCount > 0)
                    {
                        ObjValidation.CommounicationStatus = true;
                    }
                    var SMSList = contex.BOTS_TblSMSConfig.Select(x => x.SetId).Distinct().ToList();
                    var WAList = contex.BOTS_TblWAConfig.Select(x => x.SetId).Distinct().ToList();
                    var CountSMSWAset = SMSList.Union(WAList).Distinct().Count();
                    if (ObjValidation.CommCount == CountSMSWAset)
                    {
                        ObjValidation.CommSMSWAStatus = true;
                    }
                }
            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "ValidateSendForApproval Repository");
            }

            return ObjValidation;
        }

        public bool SendForApproval(string groupId, string LoginId)
        {
            bool status = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    var oldData = context.BOTS_TblGroupMaster.Where(x => x.GroupId == groupId).FirstOrDefault();
                    oldData.CustomerStatus = "Submit For Approval";
                    oldData.UpdatedDate = DateTime.Now;
                    oldData.UpdatedBy = LoginId;
                    context.BOTS_TblGroupMaster.AddOrUpdate(oldData);
                    context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SendForApproval");
            }

            return status;
        }

        public List<BOTS_TblSMSConfig> GetCommunicationSMSConfigByGroupId(string GroupId)
        {
            List<BOTS_TblSMSConfig> objData = new List<BOTS_TblSMSConfig>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objData = context.BOTS_TblSMSConfig.Where(x => x.GroupId == GroupId).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCommunicationSMSConfigByGroupId");
            }
            return objData;
        }
        public List<BOTS_TblWAConfig> GetCommunicationWAConfigByGroupId(string GroupId)
        {
            List<BOTS_TblWAConfig> objData = new List<BOTS_TblWAConfig>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objData = context.BOTS_TblWAConfig.Where(x => x.GroupId == GroupId).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCommunicationWAConfigByGroupId");
            }
            return objData;
        }

        public BOTS_TblDLCLinkConfig GetDLCLinkDLTConfigByGroupId(string GroupId)
        {
            BOTS_TblDLCLinkConfig objData = new BOTS_TblDLCLinkConfig();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objData = context.BOTS_TblDLCLinkConfig.Where(x => x.GroupId == GroupId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDLCLinkDLTConfigByGroupId");
            }
            return objData;
        }
        public List<BOTS_TblOutletMaster> GetOutletDetailsByGroupId(string GroupId)
        {
            List<BOTS_TblOutletMaster> objData = new List<BOTS_TblOutletMaster>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objData = context.BOTS_TblOutletMaster.Where(x => x.GroupId == GroupId).ToList();
                    foreach (var item in objData)
                    {
                        var city = COBR.GetCityById(Convert.ToInt32(item.City));
                        item.CityName = city.CityName;
                        var state = Convert.ToInt32(item.State);
                        item.StateName = context.tblStates.Where(x => x.StateId == state).Select(y => y.StateName).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOutletDetailsByGroupId");
            }

            return objData;
        }

        public List<BOTS_TblCampaignOtherConfig> GetCampaignOtherConfigByGroupId(string GroupId)
        {
            List<BOTS_TblCampaignOtherConfig> objData = new List<BOTS_TblCampaignOtherConfig>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objData = context.BOTS_TblCampaignOtherConfig.Where(x => x.GroupId == GroupId).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCampaignOtherConfigByGroupId");
            }

            return objData;
        }

        public List<BOTS_TblCampaignInactive> GetCampaignInactiveByGroupId(string GroupId)
        {
            List<BOTS_TblCampaignInactive> objData = new List<BOTS_TblCampaignInactive>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objData = context.BOTS_TblCampaignInactive.Where(x => x.GroupId == GroupId).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCampaignInactiveByGroupId");
            }

            return objData;
        }

        public bool UpdateConfigurationStatus(string GroupId, string Status, string LoginId, string RejectReason)
        {
            bool result = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    var oldData = context.BOTS_TblGroupMaster.Where(x => x.GroupId == GroupId).FirstOrDefault();
                    oldData.CustomerStatus = Status;
                    if (Status == "Rejected" || Status == "Rejected By Customer")
                    {
                        oldData.RejectReason = RejectReason;
                    }
                    oldData.UpdatedBy = LoginId;
                    oldData.UpdatedDate = DateTime.Now;

                    context.BOTS_TblGroupMaster.AddOrUpdate(oldData);
                    context.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateConfigurationStatus");
            }
            return result;
        }

        public List<BOTS_TblCommunicationSetAssignment> GetOutletsByAssignmentSetId(string groupId)
        {
            List<BOTS_TblCommunicationSetAssignment> lstData = new List<BOTS_TblCommunicationSetAssignment>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    lstData = context.BOTS_TblCommunicationSetAssignment.Where(x => x.GroupId == groupId).ToList();

                    foreach (var item in lstData)
                    {
                        var outletId = Convert.ToInt32(item.OutletId);
                        item.OutletName = context.BOTS_TblOutletMaster.Where(x => x.Id == outletId).Select(y => y.OutletName).FirstOrDefault();
                        //item.OutletName = outlet.OutletName;
                    }

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOutletsByAssignmentSetId");
            }

            return lstData;
        }

        public List<BOTS_TblRetailMaster> GetOutletsBrandId(string groupId)
        {
            List<BOTS_TblRetailMaster> lstData = new List<BOTS_TblRetailMaster>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    lstData = context.BOTS_TblRetailMaster.Where(x => x.GroupId == groupId).ToList();

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOutletsBrandId");
            }

            return lstData;
        }

        public int GetBulkUpload(string GroupId)
        {
            int objData = 0;
            try
            {
                using (var context = new CommonDBContext())
                {
                    objData = context.BOTS_TblBulkUpload.Where(x => x.GroupId == GroupId).Count();

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetBulkUpload");
            }

            return objData;
        }

        public NewCustDetails CreateCustomerDatabase(string GroupId)
        {
            NewCustDetails objData = new NewCustDetails();
            bool status = false;

            var connStr = ConfigurationManager.ConnectionStrings["CommonDBContext"].ToString();
            using (var context = new CommonDBContext())
            {
                var groupDetails = context.BOTS_TblGroupMaster.Where(x => x.GroupId == GroupId).FirstOrDefault();
                var DBName = groupDetails.GroupName.Replace(" ", "");
                SqlConnection _Con = new SqlConnection(connStr);
                string CreateDatabaseScript = "CREATE DATABASE " + DBName + " ; ";
                SqlCommand command = new SqlCommand(CreateDatabaseScript, _Con);

                try
                {
                    _Con.Open();
                    command.ExecuteNonQuery();

                    DataSet retVal = new DataSet();
                    SqlCommand cmdReport = new SqlCommand("BOTS_SpPushFromCommonToIndividualDB", _Con);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                    using (cmdReport)
                    {

                        SqlParameter param1 = new SqlParameter("pi_Date", DateTime.Today.ToString("yyyy-MM-dd"));
                        SqlParameter param2 = new SqlParameter("pi_LiveGroupName", DBName);
                        SqlParameter param3 = new SqlParameter("pi_LiveDBName", DBName);
                        SqlParameter param4 = new SqlParameter("pi_OnboardingGroupId", GroupId);
                        cmdReport.CommandType = CommandType.StoredProcedure;
                        cmdReport.Parameters.Add(param1);
                        cmdReport.Parameters.Add(param2);
                        cmdReport.Parameters.Add(param3);
                        cmdReport.Parameters.Add(param4);
                        daReport.Fill(retVal);
                        DataTable dt = retVal.Tables[1];

                        var response = retVal.Tables[0];
                        var BillingPartnerUrl = retVal.Tables[1];
                        var CounterDetail = retVal.Tables[2];

                        if (Convert.ToString(response.Rows[0]["ResponseCode"]) == "0")
                            objData.result = true;

                        if (objData.result)
                        {
                            objData.BillingPartnerUrl = Convert.ToString(BillingPartnerUrl.Rows[0][0]);
                            objData.DLCLink = Convert.ToString(BillingPartnerUrl.Rows[0][1]);
                            List<CounterIdDetails> lstCounterIdDetails = new List<CounterIdDetails>();
                            foreach (DataRow dr in CounterDetail.Rows)
                            {
                                CounterIdDetails objItem = new CounterIdDetails();
                                objItem.OutletName = Convert.ToString(dr["OutletName"]);
                                objItem.CounterId = Convert.ToString(dr["CounterId"]);
                                objItem.Securitykey = Convert.ToString(dr["Securitykey"]);
                                lstCounterIdDetails.Add(objItem);
                            }
                            objData.lstCounterIdDetails = lstCounterIdDetails;
                        }

                    }


                    //var result = context.Database.SqlQuery<DataSet>("BOTS_SpPushFromCommonToIndividualDB @pi_Date, @pi_LiveGroupName, @pi_LiveDBName, @pi_OnboardingGroupId",
                    //            new SqlParameter("@pi_Date", DateTime.Today.ToString("yyyy-MM-dd")),
                    //            new SqlParameter("@pi_LiveGroupName", DBName),
                    //            new SqlParameter("@pi_LiveDBName", DBName),
                    //            new SqlParameter("@pi_OnboardingGroupId", GroupId)).FirstOrDefault<DataSet>();
                    //var response = result.Tables[0];
                    //var BillingPartnerUrl= result.Tables[1];
                    //var CounterDetail = result.Tables[2];

                    //if (Convert.ToString(response.Rows[0]["ResponseCode"]) == "0")
                    //    objData.result = true;

                    //if(status)
                    //{
                    //    objData.BillingPartnerUrl = Convert.ToString(BillingPartnerUrl.Rows[0][0]);
                    //    objData.DLCLink = Convert.ToString(BillingPartnerUrl.Rows[0][1]);
                    //    List<CounterIdDetails> lstCounterIdDetails = new List<CounterIdDetails>();
                    //    foreach (DataRow dr in CounterDetail.Rows)
                    //    {
                    //        CounterIdDetails objItem = new CounterIdDetails();
                    //        objItem.OutletName = Convert.ToString(dr["OutletName"]);
                    //        objItem.CounterId = Convert.ToString(dr["CounterId"]);
                    //        objItem.Securitykey = Convert.ToString(dr["Securitykey"]);
                    //        lstCounterIdDetails.Add(objItem);                            
                    //    }
                    //    objData.lstCounterIdDetails = lstCounterIdDetails;
                    //}
                }
                catch (System.Exception ex)
                {
                    newexception.AddException(ex, "CreateCustomerDatabase");
                }
                finally
                {
                    if (_Con.State == ConnectionState.Open)
                    {
                        _Con.Close();
                    }
                }
            }
            return objData;
        }

        public string GetCSHeadEmailId()
        {
            string emailId = string.Empty;
            try
            {
                using (var context = new CommonDBContext())
                {
                    emailId = context.CustomerLoginDetails.Where(x => x.UserStatus.Value && x.LoginType == "6").Select(y => y.EmailId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCSHeadEmailId");
            }
            return emailId;
        }
        public string GetOnboardingGroupName(string groupid)
        {
            string groupName = string.Empty;
            try
            {
                using (var context = new CommonDBContext())
                {
                    groupName = context.BOTS_TblGroupMaster.Where(x => x.GroupId == groupid).Select(y => y.GroupName).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOnboardingGroupName");
            }
            return groupName;
        }

        public string GetOnboardingBrandName(string groupid, string brandId)
        {
            string groupName = string.Empty;
            try
            {
                using (var context = new CommonDBContext())
                {
                    groupName = context.BOTS_TblRetailMaster.Where(x => x.GroupId == groupid && x.BrandId == brandId).Select(y => y.BrandName).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOnboardingBrandName");
            }
            return groupName;
        }

        public string GetAssignedCSNameForOnboarding(string groupid)
        {
            string CSEmail = string.Empty;
            try
            {
                using (var context = new CommonDBContext())
                {
                    var RMAssignedId = context.BOTS_TblGroupMaster.Where(x => x.GroupId == groupid).Select(y => y.AssignedCS).FirstOrDefault();

                    CSEmail = context.CustomerLoginDetails.Where(x => x.LoginId == RMAssignedId).Select(y => y.EmailId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetAssignedCSNameForOnboarding");
            }
            return CSEmail;
        }

        public bool SaveOutletData(List<BOTS_TblOutletMaster> objOutletData)
        {
            bool result = false;
            using (var context = new CommonDBContext())
            {
                try
                {
                    foreach (var item in objOutletData)
                    {
                        context.BOTS_TblOutletMaster.AddOrUpdate(item);
                        context.SaveChanges();
                        result = true;
                    }

                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "SaveOutletData");

                }
            }
            return result;
        }

        public bool UploadBrandLogo(string groupid, string brandId, string LogoURL)
        {
            bool result = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    var brandDetails = context.BOTS_TblRetailMaster.Where(x => x.GroupId == groupid && x.BrandId == brandId).FirstOrDefault();
                    brandDetails.LogoPath = LogoURL;
                    context.BOTS_TblRetailMaster.AddOrUpdate(brandDetails);
                    context.SaveChanges();

                    result = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UploadBrandLogo");

            }
            return result;
        }

        public bool UploadOtherDocs(string groupid, string docName, string LogoURL, string addedBy)
        {
            bool result = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    BOTS_TblDocuments objData = new BOTS_TblDocuments();
                    objData.GroupId = groupid;
                    objData.DocumentName = docName;
                    objData.DocumentPath = LogoURL;
                    objData.AddedDate = DateTime.Now;
                    objData.AddedBy = addedBy;
                    context.BOTS_TblDocuments.AddOrUpdate(objData);
                    context.SaveChanges();

                    result = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UploadOtherDocs");

            }
            return result;
        }

        public List<BOTS_TblDocuments> GetOtherDocuments(string groupId)
        {
            List<BOTS_TblDocuments> lstData = new List<BOTS_TblDocuments>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    lstData = context.BOTS_TblDocuments.Where(x => x.GroupId == groupId).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOtherDocuments");

            }

            return lstData;
        }

        public bool SavePreferredLanguage(string groupId, string PreferredLanguage, string AddedBy)
        {
            bool result = false;
            using (var context = new CommonDBContext())
            {
                try
                {
                    var objData = context.BOTS_TblGroupMaster.Where(x => x.GroupId == groupId).FirstOrDefault();
                    if (objData != null)
                    {
                        objData.PreferredLanguage = PreferredLanguage;
                        objData.UpdatedBy = AddedBy;
                        objData.UpdatedDate = DateTime.Now;
                        context.BOTS_TblGroupMaster.AddOrUpdate(objData);
                        context.SaveChanges();
                        result = true;
                        AddTracking(groupId, "Preferred Language Add/Update Done", AddedBy);
                    }
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "SavePreferredLanguage" + groupId);
                }
            }
            return result;
        }
        public bool SaveProgramLanguage(string groupId, string ProgramLanguage, string AddedBy)
        {
            bool result = false;
            using (var context = new CommonDBContext())
            {
                try
                {
                    var objData = context.BOTS_TblOutletMaster.Where(x => x.GroupId == groupId).FirstOrDefault();
                    if (objData != null)
                    {
                        objData.ProgramLanguage = ProgramLanguage;
                        objData.UpdatedBy = AddedBy;
                        objData.UpdatedDate = DateTime.Now;
                        context.BOTS_TblOutletMaster.AddOrUpdate(objData);
                        context.SaveChanges();
                        result = true;
                        AddTracking(groupId, "Program Language Add/Update Done", AddedBy);
                    }
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "SaveProgramLanguage" + groupId);
                }
            }
            return result;
        }
        public void AddTracking(string groupId, string ActionTaken, string AddedBy)
        {
            using (var context = new CommonDBContext())
            {
                try
                {
                    BOTS_TblActionTracking objData = new BOTS_TblActionTracking();
                    objData.GroupId = groupId;
                    objData.ActionTaken = ActionTaken;
                    objData.AddedBy = AddedBy;
                    objData.AddedDate = DateTime.Now;
                    context.BOTS_TblActionTracking.AddOrUpdate(objData);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "AddTracking" + groupId);
                }
            }
        }

        public bool RecordIntroCall(string groupId, string AddedBy)
        {
            bool status = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    var existingData = context.BOTS_TblGroupMaster.Where(x => x.GroupId == groupId).FirstOrDefault();
                    existingData.IntroductionCall = true;
                    existingData.IntroductionCallDate = DateTime.Now;
                    existingData.UpdatedBy = AddedBy;
                    existingData.UpdatedDate = DateTime.Now;
                    context.BOTS_TblGroupMaster.AddOrUpdate(existingData);
                    context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "RecordIntroCall");

            }
            return status;
        }

        public tblStandardRulesSetting GetStandardConfigurationRules(int CategoryId)
        {
            tblStandardRulesSetting objData = new tblStandardRulesSetting();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objData = context.tblStandardRulesSettings.Where(x => x.CategoryId == CategoryId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetStandardConfigurationRules");

            }
            return objData;
        }

    }
}
