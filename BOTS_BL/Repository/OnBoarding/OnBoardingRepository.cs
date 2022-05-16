using System;
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
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        newexception.AddException(ex, "OnBoarding");
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
                        lstGroups = context.BOTS_TblGroupMaster.ToList();
                    }
                    //Sales Head
                    if (userDetails.LoginType == "5")
                    {
                        lstGroups = context.BOTS_TblGroupMaster.Where(x => x.CustomerStatus == "Draft").ToList();
                    }
                    //CS Head
                    if (userDetails.LoginType == "6")
                    {
                        lstGroups = context.BOTS_TblGroupMaster.Where(x => x.CustomerStatus == "CS" || x.CustomerStatus == "CSUpdate").ToList();
                    }
                    //CS Success
                    if (userDetails.LoginType == "7")
                    {
                        lstGroups = context.BOTS_TblGroupMaster.Where(x => x.AssignedCS == userDetails.LoginId).ToList();
                    }
                    if (userDetails.LoginType != "1" && userDetails.LoginType != "5" && userDetails.LoginType != "6" && userDetails.LoginType != "7")
                    {
                        lstGroups = context.BOTS_TblGroupMaster.ToList();
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
                        onBoardingListings.Add(objItem);
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "OnBoarding");
            }
            return onBoardingListings;
        }

        public BOTS_TblGroupMaster GetGroupMasterDetails(string GroupId)
        {
            BOTS_TblGroupMaster objData = new BOTS_TblGroupMaster();
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
            return objData;
        }

        public BOTS_TblDealDetails GetDealMasterDetails(string GroupId)
        {
            BOTS_TblDealDetails objData = new BOTS_TblDealDetails();
            using (var context = new CommonDBContext())
            {
                objData = context.BOTS_TblDealDetails.Where(x => x.GroupId == GroupId).FirstOrDefault();
            }
            return objData;
        }

        public BOTS_TblPaymentDetails GetPaymentDetails(string GroupId)
        {
            BOTS_TblPaymentDetails objData = new BOTS_TblPaymentDetails();
            using (var context = new CommonDBContext())
            {
                objData = context.BOTS_TblPaymentDetails.Where(x => x.GroupId == GroupId).FirstOrDefault();
            }
            return objData;
        }

        public List<BOTS_TblRetailMaster> GetRetailDetails(string GroupId)
        {
            List<BOTS_TblRetailMaster> objData = new List<BOTS_TblRetailMaster>();
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
            return objData;
        }

        public List<BOTS_TblOutletMaster> GetOutletDetails(string GroupId)
        {
            List<BOTS_TblOutletMaster> objData = new List<BOTS_TblOutletMaster>();
            using (var context = new CommonDBContext())
            {
                objData = context.BOTS_TblOutletMaster.Where(x => x.GroupId == GroupId).ToList();
            }
            return objData;
        }

        public List<BOTS_TblInstallmentDetails> GetInstallmentDetails(string GroupId)
        {
            List<BOTS_TblInstallmentDetails> objData = new List<BOTS_TblInstallmentDetails>();
            using (var context = new CommonDBContext())
            {
                objData = context.BOTS_TblInstallmentDetails.Where(x => x.GroupId == GroupId).ToList();
                foreach (var item in objData)
                {
                    item.PaymentDateStr = item.PaymentDate.ToString("yyyy-MM-dd");
                }
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
            return lstBillingPartnerProduct;
        }

        public List<SelectListItem> GetRefferedName(string SourceType)
        {
            List<SelectListItem> lstRefferedName = new List<SelectListItem>();
            SelectListItem item1 = new SelectListItem();
            item1.Value = "0";
            item1.Text = "Please Select";
            lstRefferedName.Add(item1);
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
            return lstRefferedName;
        }

        public List<RetailDetails> GetRetailDetailsForEmail(string GroupId)
        {
            List<RetailDetails> objData = new List<RetailDetails>();
            using (var context = new CommonDBContext())
            {
                var data = context.BOTS_TblRetailMaster.Where(x => x.GroupId == GroupId).ToList();

                foreach (var item in data)
                {
                    RetailDetails item1 = new RetailDetails();
                    item1.CategoryName = item.CategoryName;
                    item1.NoOfOutlets = item.NoOfOutlets;
                    item1.NoOfEnrolled = item.NoOfEnrolled;
                    var id = Convert.ToInt32(item.BillingPartner);
                    item1.BillingPartner = context.tblBillingPartners.Where(x => x.BillingPartnerId == id).Select(y => y.BillingPartnerName).FirstOrDefault();
                    item1.BOProduct = item.BOProduct == "1" ? "Octa Plus" : "Octa XS";
                    objData.Add(item1);
                }
            }
            return objData;
        }

        public List<string> GetAllInternalEmailIds()
        {
            List<string> lstEmails = new List<string>();
            using (var context = new CommonDBContext())
            {
                var loginTypeList = new List<string> { "2", "3", "4" };
                lstEmails = context.CustomerLoginDetails.Where(x => x.EmailId != null && x.LoginType != null && !loginTypeList.Contains(x.LoginType)).Select(y => y.EmailId).ToList();
            }

            return lstEmails;
        }

        public OnBoardingCustomerDetails GetOnBoardingCustomerDetails(string groupId)
        {
            OnBoardingCustomerDetails objData = new OnBoardingCustomerDetails();
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

            return objData;
        }

        public List<BOTS_TblSMSConfig> GetCommunicationSMSConfigForDLT(string GroupId, int SetId)
        {
            List<BOTS_TblSMSConfig> objData = new List<BOTS_TblSMSConfig>();
            using (var context = new CommonDBContext())
            {
                objData = context.BOTS_TblSMSConfig.Where(x => x.GroupId == GroupId && x.SetId == SetId && x.DLTStatus != "" && x.DLTStatus != null).ToList();
            }

            return objData;
        }
        public List<BOTS_TblVariableWords> GetVariableWordsList()
        {
            List<BOTS_TblVariableWords> objData = new List<BOTS_TblVariableWords>();
            using (var context = new CommonDBContext())
            {
                objData = context.BOTS_TblVariableWords.ToList();
            }
            return objData;
        }

        public BOTS_TblSMSConfig GetCommunicationSMSConfigById(int Id)
        {
            BOTS_TblSMSConfig objSMSConfig = new BOTS_TblSMSConfig();
            using (var context = new CommonDBContext())
            {
                objSMSConfig = context.BOTS_TblSMSConfig.Where(x => x.Id == Id).FirstOrDefault();
            }
            return objSMSConfig;
        }
        public bool SaveIndividualSMSConfig(BOTS_TblSMSConfig objItem)
        {
            bool status = false;
            using (var context = new CommonDBContext())
            {
                context.BOTS_TblSMSConfig.AddOrUpdate(objItem);
                context.SaveChanges();
                status = true;
            }
            return status;
        }

        public bool UpdateStatusSMSConfig(int ItemId, string DLTStatus, string LoginId, string rejectReason, BOTS_TblSMSConfig objSMSConfig)
        {
            bool status = false;
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
                            objItem.RejectReason = objItem.RejectReason + " //// " + rejectReason;
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
                }
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

            return objData;
        }

        public BOTS_TblCommunicationSet GetSetDetails(int SetId)
        {
            BOTS_TblCommunicationSet objSetData = new BOTS_TblCommunicationSet();
            using (var context = new CommonDBContext())
            {
                objSetData = context.BOTS_TblCommunicationSet.Where(x => x.SetId == SetId).FirstOrDefault();
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
            using (var context = new CommonDBContext())
            {

                objDLCLinkConfig = context.BOTS_TblDLCLinkConfig.Where(x => x.GroupId == groupId).FirstOrDefault();
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
            using (var context = new CommonDBContext())
            {
                objData = context.BOTS_TblVelocityChecksConfig.Where(x => x.GroupId == groupId).OrderBy(y => y.VelocityType).ToList();
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
                objbulk.ColumnMappings.Add("CustId", "CustId");
                objbulk.ColumnMappings.Add("CustName", "CustName");
                objbulk.ColumnMappings.Add("MobileNo", "MobileNo");
                objbulk.ColumnMappings.Add("OutletId", "OutletId");
                objbulk.ColumnMappings.Add("Gender", "Gender");
                objbulk.ColumnMappings.Add("Status", "Status");
                objbulk.ColumnMappings.Add("DOB", "DOB");
                objbulk.ColumnMappings.Add("AOB", "AOB");
                objbulk.ColumnMappings.Add("EmailId", "EmailId");
                objbulk.ColumnMappings.Add("City", "City");
                objbulk.ColumnMappings.Add("Area", "Area");
                objbulk.ColumnMappings.Add("CustomerCategory", "CustomerCategory");
                objbulk.ColumnMappings.Add("CardNo", "CardNo");
                objbulk.ColumnMappings.Add("Points", "Points");

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
            using (var context = new CommonDBContext())
            {
                objData = context.BOTS_TblCampaignOtherConfig.Where(x => x.GroupId == groupId && x.CampaignType == type).FirstOrDefault();
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
            using (var context = new CommonDBContext())
            {
                lstSets = context.BOTS_TblCommunicationSet.Where(x => x.GroupId == GroupId).ToList();
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
            return lstCommunicationSet;
        }

        public List<SelectListItem> GetOutletListWithAssignment(string GroupId, string SetId)
        {
            List<SelectListItem> lstOutlets = new List<SelectListItem>();
            List<BOTS_TblOutletMaster> objData = new List<BOTS_TblOutletMaster>();
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
                newexception.AddException(ex, GroupId);
            }
            return status;
        }

        public bool AddVariableWord(string word)
        {
            bool status = false;
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

            return status;
        }

        public BOTS_TblCampaignOtherConfig GetCampaignOtherConfigForDLT(string groupId, string type)
        {
            BOTS_TblCampaignOtherConfig objData = new BOTS_TblCampaignOtherConfig();
            using (var context = new CommonDBContext())
            {
                objData = context.BOTS_TblCampaignOtherConfig.Where(x => x.GroupId == groupId && x.CampaignType == type).FirstOrDefault();
            }

            return objData;
        }
        public List<BOTS_TblCampaignInactive> GetCampaignAllInactiveForDLT(string groupId, string type)
        {
            List<BOTS_TblCampaignInactive> objData = new List<BOTS_TblCampaignInactive>();
            using (var context = new CommonDBContext())
            {
                objData = context.BOTS_TblCampaignInactive.Where(x => x.GroupId == groupId && x.InactiveType == type).ToList();
            }

            return objData;
        }
        public bool UpdateBADLTStatus(int id, int statusid, string status, string loginid, string reason)
        {
            bool result = false;
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
                            objData.RejectReason1 = reason;
                        }
                    }
                    if (statusid == 2)
                    {
                        objData.DLTStatus2 = status;
                        if (status == "Rejected")
                        {
                            objData.RejectReason2 = reason;
                        }
                    }
                    if (statusid == 3)
                    {
                        objData.DLTStatus3 = status;
                        if (status == "Rejected")
                        {
                            objData.RejectReason3 = reason;
                        }
                    }
                    if (statusid == 4)
                    {
                        objData.DLTStatus4 = status;
                        if (status == "Rejected")
                        {
                            objData.RejectReason4 = reason;
                        }
                    }
                    if (statusid == 5)
                    {
                        objData.DLTStatus5 = status;
                        if (status == "Rejected")
                        {
                            objData.RejectReason5 = reason;
                        }
                    }
                    if (statusid == 6)
                    {
                        objData.DLTStatus6 = status;
                        if (status == "Rejected")
                        {
                            objData.RejectReason6 = reason;
                        }
                    }

                    objData.UpdatedBy = loginid;
                    objData.UpdatedDate = DateTime.Now;
                    context.BOTS_TblCampaignOtherConfig.AddOrUpdate(objData);
                    context.SaveChanges();
                    result = true;
                }
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
                            existingObj.IntroScript1 = IntroDays;
                            existingObj.IntroScript1DLT = IntroDaysDLT;
                            existingObj.TemplateId1 = TemplateId;
                            existingObj.TemplateName1 = TemplateName;
                            existingObj.TemplateType1 = TemplateType;
                        }
                        if (statusid == 2)
                        {
                            existingObj.IntroScript2 = IntroDays;
                            existingObj.IntroScript2DLT = IntroDaysDLT;
                            existingObj.TemplateId2 = TemplateId;
                            existingObj.TemplateName2 = TemplateName;
                            existingObj.TemplateType2 = TemplateType;
                        }
                        if (statusid == 3)
                        {
                            existingObj.ReminderScript1 = IntroDays;
                            existingObj.ReminderScript1DLT = IntroDaysDLT;
                            existingObj.TemplateId3 = TemplateId;
                            existingObj.TemplateName3 = TemplateName;
                            existingObj.TemplateType3 = TemplateType;
                        }
                        if (statusid == 4)
                        {
                            existingObj.ReminderScript2 = IntroDays;
                            existingObj.ReminderScript2DLT = IntroDaysDLT;
                            existingObj.TemplateId4 = TemplateId;
                            existingObj.TemplateName4 = TemplateName;
                            existingObj.TemplateType4 = TemplateType;
                        }
                        if (statusid == 5)
                        {
                            existingObj.OnDayScriptPT = IntroDays;
                            existingObj.OnDayScriptPTDLT = IntroDaysDLT;
                            existingObj.TemplateId5 = TemplateId;
                            existingObj.TemplateName5 = TemplateName;
                            existingObj.TemplateType5 = TemplateType;
                        }
                        if (statusid == 6)
                        {
                            existingObj.OnDayScriptNPT = IntroDays;
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
                            objData.RejectReason1 = reason;
                        }
                    }
                    if (statusid == 2)
                    {
                        objData.DLTStatus2 = status;
                        if (status == "Rejected")
                        {
                            objData.RejectReason2 = reason;
                        }
                    }

                    objData.UpdatedBy = loginid;
                    objData.UpdatedDate = DateTime.Now;
                    context.BOTS_TblCampaignInactive.AddOrUpdate(objData);
                    context.SaveChanges();
                    result = true;
                }
            }

            return result;
        }

        public BOTS_TblCampaignInactive GetInactiveConfigById(int Id)
        {
            BOTS_TblCampaignInactive objInactiveConfig = new BOTS_TblCampaignInactive();
            using (var context = new CommonDBContext())
            {
                objInactiveConfig = context.BOTS_TblCampaignInactive.Where(x => x.Id == Id).FirstOrDefault();
            }
            return objInactiveConfig;
        }

        public bool SaveInactiveDLTConfig(BOTS_TblCampaignInactive objData)
        {
            bool result = false;
            using (var context = new CommonDBContext())
            {
                context.BOTS_TblCampaignInactive.AddOrUpdate(objData);
                context.SaveChanges();
                result = true;
            }
            return result;
        }

        public BOTS_TblCampaignOtherConfig GetCampaignRemainingForDLT(string groupId, string type)
        {
            BOTS_TblCampaignOtherConfig objData = new BOTS_TblCampaignOtherConfig();
            using (var context = new CommonDBContext())
            {
                objData = context.BOTS_TblCampaignOtherConfig.Where(x => x.GroupId == groupId && x.CampaignType == type).FirstOrDefault();
            }

            return objData;
        }

        public bool UpdateRemainingDLTStatus(int id, int statusid, string status, string loginid, string reason)
        {
            bool result = false;
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
                            objData.RejectReason1 = reason;
                        }
                    }
                    if (statusid == 2)
                    {
                        objData.DLTStatus2 = status;
                        if (status == "Rejected")
                        {
                            objData.RejectReason2 = reason;
                        }
                    }

                    objData.UpdatedBy = loginid;
                    objData.UpdatedDate = DateTime.Now;
                    context.BOTS_TblCampaignOtherConfig.AddOrUpdate(objData);
                    context.SaveChanges();
                    result = true;
                }
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
                            if (string.IsNullOrEmpty(objDLCLinkConfig.RejectReason1))
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
                            if (string.IsNullOrEmpty(objDLCLinkConfig.RejectReason1))
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
                            if (string.IsNullOrEmpty(objDLCLinkConfig.RejectReason1))
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
                            if (string.IsNullOrEmpty(objDLCLinkConfig.RejectReason1))
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
                            if (string.IsNullOrEmpty(objDLCLinkConfig.RejectReason1))
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
                            if (string.IsNullOrEmpty(objDLCLinkConfig.RejectReason1))
                                objDLCLinkConfig.RejectReason7 = reason;
                            else
                                objDLCLinkConfig.RejectReason7 = objDLCLinkConfig.RejectReason7 + " // " + reason;
                        }
                    }

                    objDLCLinkConfig.UpdatedBy = loginid;
                    objDLCLinkConfig.UpdatedDate = DateTime.Now;
                    context.BOTS_TblDLCLinkConfig.AddOrUpdate(objDLCLinkConfig);
                    context.SaveChanges();
                    result = true;
                }
            }

            return result;
        }

        public BOTS_TblDLCLinkConfig GetDLCLinkDLTConfigById(int Id)
        {
            BOTS_TblDLCLinkConfig objDLCLinkConfig = new BOTS_TblDLCLinkConfig();
            using (var context = new CommonDBContext())
            {
                objDLCLinkConfig = context.BOTS_TblDLCLinkConfig.Where(x => x.Id == Id).FirstOrDefault();
            }
            return objDLCLinkConfig;
        }

        public bool SaveDLCLinkDLTConfig(BOTS_TblDLCLinkConfig objDLCLinkConfig)
        {
            bool result = false;
            using (var context = new CommonDBContext())
            {
                context.BOTS_TblDLCLinkConfig.AddOrUpdate(objDLCLinkConfig);
                context.SaveChanges();
                result = true;
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
            using (var context = new CommonDBContext())
            {
                objData = context.BOTS_TblEarnRuleConfig.Where(x => x.GroupId == GroupId).FirstOrDefault();
            }
            return objData;
        }
        public List<BOTS_TblSlabConfig> GetEarnRuleSlabConfig(string GroupId)
        {
            List<BOTS_TblSlabConfig> objData = new List<BOTS_TblSlabConfig>();
            using (var context = new CommonDBContext())
            {
                objData = context.BOTS_TblSlabConfig.Where(x => x.GroupId == GroupId).ToList();
            }
            return objData;
        }

        public BOTS_TblBurnRuleConfig GetBurnRuleConfig(string GroupId)
        {
            BOTS_TblBurnRuleConfig objData = new BOTS_TblBurnRuleConfig();
            using (var context = new CommonDBContext())
            {
                objData = context.BOTS_TblBurnRuleConfig.Where(x => x.GroupId == GroupId).FirstOrDefault();
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
            catch(Exception ex)
            {
                newexception.AddException(ex, "SendForApproval");
            }

            return status;
        }

        public List<BOTS_TblSMSConfig> GetCommunicationSMSConfigByGroupId(string GroupId)
        {
            List<BOTS_TblSMSConfig> objData = new List<BOTS_TblSMSConfig>();
            using (var context = new CommonDBContext())
            {              
                    objData = context.BOTS_TblSMSConfig.Where(x => x.GroupId == GroupId).ToList();               
            }

            return objData;
        }
        public List<BOTS_TblWAConfig> GetCommunicationWAConfigByGroupId(string GroupId)
        {
            List<BOTS_TblWAConfig> objData = new List<BOTS_TblWAConfig>();
            using (var context = new CommonDBContext())
            {
                objData = context.BOTS_TblWAConfig.Where(x => x.GroupId == GroupId).ToList();
            }

            return objData;
        }

        public BOTS_TblDLCLinkConfig GetDLCLinkDLTConfigByGroupId(string GroupId)
        {
            BOTS_TblDLCLinkConfig objData = new BOTS_TblDLCLinkConfig();
            using (var context = new CommonDBContext())
            {
                objData = context.BOTS_TblDLCLinkConfig.Where(x => x.GroupId == GroupId).FirstOrDefault();
            }

            return objData;
        }
        public BOTS_TblOutletMaster GetOutletDetailsByGroupId(string GroupId)
        {
            BOTS_TblOutletMaster objData = new BOTS_TblOutletMaster();
            using (var context = new CommonDBContext())
            {
                objData = context.BOTS_TblOutletMaster.Where(x => x.GroupId == GroupId).FirstOrDefault();
            }

            return objData;
        }

        public BOTS_TblCampaignOtherConfig GetCampaignOtherConfigByGroupId(string GroupId)
        {
            BOTS_TblCampaignOtherConfig objData = new BOTS_TblCampaignOtherConfig();
            using (var context = new CommonDBContext())
            {
                objData = context.BOTS_TblCampaignOtherConfig.Where(x => x.GroupId == GroupId).FirstOrDefault();
            }

            return objData;
        }

        public BOTS_TblCampaignInactive GetCampaignInactiveByGroupId(string GroupId)
        {
            BOTS_TblCampaignInactive objData = new BOTS_TblCampaignInactive();
            using (var context = new CommonDBContext())
            {
                objData = context.BOTS_TblCampaignInactive.Where(x => x.GroupId == GroupId).FirstOrDefault();
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
                    if (Status == "Rejected")
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
    }
}
