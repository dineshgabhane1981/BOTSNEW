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
                            context.BOTS_TblGroupMaster.AddOrUpdate(objGroup);
                            context.SaveChanges();

                            var lstRetail = context.BOTS_TblRetailMaster.Where(x => x.GroupId == objGroup.GroupId).ToList();
                            foreach (var item in lstRetail)
                            {
                                context.BOTS_TblRetailMaster.Remove(item);
                                context.SaveChanges();
                            }

                            foreach (var item in objLstRetail)
                            {
                                var id = Convert.ToInt32(item.CategoryId);
                                var categoryName = context.tblCategories.Where(x => x.CategoryId == id).Select(y => y.CategoryName).FirstOrDefault();
                                item.CategoryName = categoryName;
                                item.GroupId = Convert.ToString(objGroup.GroupId);
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


        public List<OnBoardingListing> GetOnBoardingListings(string loginType)
        {
            List<OnBoardingListing> onBoardingListings = new List<OnBoardingListing>();
            List<BOTS_TblGroupMaster> lstGroups = new List<BOTS_TblGroupMaster>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    //SuperAdmin
                    if (loginType == "1")
                    {
                        lstGroups = context.BOTS_TblGroupMaster.ToList();
                    }
                    //Sales
                    if (loginType == "5")
                    {
                        lstGroups = context.BOTS_TblGroupMaster.Where(x => x.CustomerStatus == "Draft").ToList();
                    }
                    //Customer Success
                    if (loginType == "6")
                    {
                        lstGroups = context.BOTS_TblGroupMaster.Where(x => x.CustomerStatus == "CS" || x.CustomerStatus == "CSUpdate").ToList();
                    }
                    //CS Head
                    if (loginType == "7")
                    {

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
                        var bId = Convert.ToInt32(BPId);

                        objItem.BillingPartnerName = context.tblBillingPartners.Where(x => x.BillingPartnerId == bId).Select(y => y.BillingPartnerName).FirstOrDefault();
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
                if (SourceType == "3")
                {

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
                lstEmails = context.CustomerLoginDetails.Where(x => x.EmailId != null && x.LoginType !=null && !loginTypeList.Contains(x.LoginType)).Select(y => y.EmailId).ToList();
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

    }
}
