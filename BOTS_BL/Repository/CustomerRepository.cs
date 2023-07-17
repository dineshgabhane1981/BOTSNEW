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
using DocumentFormat.OpenXml.InkML;
using System.Runtime.Remoting.Contexts;
using System.Data.Entity.Infrastructure;

namespace BOTS_BL.Repository
{
    public class CustomerRepository
    {
        Exceptions newexception = new Exceptions();

        public List<SelectListItem> GetRetailCategory()
        {
            List<SelectListItem> lstRetailCategory = new List<SelectListItem>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    var RetailCategory = context.tblCategories.Where(x => x.IsActive == true).ToList();

                    foreach (var item in RetailCategory)
                    {
                        lstRetailCategory.Add(new SelectListItem
                        {
                            Text = item.CategoryName,
                            Value = Convert.ToString(item.CategoryId)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetRetailCategory");
            }
            return lstRetailCategory;
        }
        public List<SelectListItem> GetCity()
        {
            List<SelectListItem> lstCity = new List<SelectListItem>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    var RetailCategory = context.tblCities.Where(x => x.IsActive == true).OrderBy(x => x.CityName).ToList();

                    foreach (var item in RetailCategory)
                    {
                        lstCity.Add(new SelectListItem
                        {
                            Text = item.CityName,
                            Value = Convert.ToString(item.CityId)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCity");
            }
            return lstCity;
        }

        public List<SelectListItem> GetStates()
        {
            List<SelectListItem> lstStates = new List<SelectListItem>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    var RetailCategory = context.tblStates.ToList();

                    foreach (var item in RetailCategory)
                    {
                        lstStates.Add(new SelectListItem
                        {
                            Text = item.StateName,
                            Value = Convert.ToString(item.StateId)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetStates");
            }
            return lstStates;
        }

        public List<SelectListItem> GetSourcedBy()
        {
            List<SelectListItem> lstSourcedBy = new List<SelectListItem>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    var RetailCategory = context.tblSourcedBies.Where(x => x.IsActive == true).ToList();

                    foreach (var item in RetailCategory)
                    {
                        lstSourcedBy.Add(new SelectListItem
                        {
                            Text = item.SourcedbyName,
                            Value = Convert.ToString(item.SourcedbyId)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetSourcedBy");
            }
            return lstSourcedBy;
        }
        public List<SelectListItem> GetRMAssigned()
        {
            List<SelectListItem> lstRMAssigned = new List<SelectListItem>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    var RetailCategory = context.tblRMAssigneds.Where(x => x.IsActive == true).ToList();

                    foreach (var item in RetailCategory)
                    {
                        lstRMAssigned.Add(new SelectListItem
                        {
                            Text = item.RMAssignedName,
                            Value = Convert.ToString(item.LoginId)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetRMAssigned");
            }
            return lstRMAssigned;
        }
        public List<SelectListItem> GetBillingPartner()
        {
            List<SelectListItem> lstBillingPartner = new List<SelectListItem>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    var RetailCategory = context.tblBillingPartners.Where(x => x.IsActive == true).ToList();

                    foreach (var item in RetailCategory)
                    {
                        lstBillingPartner.Add(new SelectListItem
                        {
                            Text = item.BillingPartnerName,
                            Value = Convert.ToString(item.BillingPartnerId)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetBillingPartner");
            }
            return lstBillingPartner;
        }
        public List<SelectListItem> GetBillingProduct(int BillingPartnerId)
        {
            List<SelectListItem> lstBillingProduct = new List<SelectListItem>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    var BillingProduct = context.BOTS_TblBillingPartnerProduct.Where(x => x.BillingPartnerProductId == BillingPartnerId && x.IsActive == true).ToList();

                    foreach (var item in BillingProduct)
                    {
                        lstBillingProduct.Add(new SelectListItem
                        {
                            Text = item.BillingPartnerProductName,
                            Value = Convert.ToString(item.BillingPartnerProductId)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetBillingProduct");
            }
            return lstBillingProduct;
        }

        public List<SelectListItem> GetAllGroups()
        {
            List<SelectListItem> lstAllFroup = new List<SelectListItem>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    var GroupDetails = context.tblGroupDetails.ToList();

                    foreach (var item in GroupDetails)
                    {
                        lstAllFroup.Add(new SelectListItem
                        {
                            Text = item.GroupName,
                            Value = Convert.ToString(item.GroupId)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetAllGroups");
            }
            return lstAllFroup;
        }

        public List<SelectListItem> GetAllActiveGroups()
        {
            List<SelectListItem> lstAllFroup = new List<SelectListItem>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    var GroupDetails = context.tblGroupDetails.Where(x => x.IsActive == true && x.IsLive == true).OrderBy(y => y.GroupName).ToList();

                    foreach (var item in GroupDetails)
                    {
                        lstAllFroup.Add(new SelectListItem
                        {
                            Text = item.GroupName,
                            Value = Convert.ToString(item.GroupId)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetAllActiveGroups");
            }
            return lstAllFroup;
        }

        public List<SelectListItem> GetAllRefferedCategory()
        {
            List<SelectListItem> lstAllRefferedCategory = new List<SelectListItem>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    var SourceTypes = context.tblSourceTypes.Where(x => x.IsActive == true).ToList();

                    foreach (var item in SourceTypes)
                    {
                        lstAllRefferedCategory.Add(new SelectListItem
                        {
                            Text = item.SourceTypeName,
                            Value = Convert.ToString(item.SourceTypeId)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetAllRefferedCategory");
            }
            return lstAllRefferedCategory;
        }

        public List<CustomerListing> GetAllCustomer(string loginId, string loginType)
        {
            List<CustomerListing> objGroupList = new List<CustomerListing>();
            try
            {
                List<tblGroupDetail> objGroupDetails = new List<tblGroupDetail>();
                using (var context = new CommonDBContext())
                {
                    //Commented By Dinesh, Dont remove it
                    //if (loginType == "7")
                    //{
                    //    var assigned = context.tblRMAssigneds.Where(x => x.LoginId == loginId).Select(y => y.RMAssignedId).FirstOrDefault();
                    //    objGroupDetails = context.tblGroupDetails.Where(x => x.IsActive.Value == true && x.IsLive == true && x.RMAssigned == assigned).ToList();
                    //}
                    //else
                    //{
                        objGroupDetails = context.tblGroupDetails.Where(x => x.IsActive.Value == true && x.IsLive == true).ToList();
                    //}
                    if (objGroupDetails != null)
                    {
                        foreach (var item in objGroupDetails)
                        {
                            CustomerListing objGroup = new CustomerListing();
                            objGroup.GroupId = item.GroupId;
                            objGroup.Product = item.ProductType;
                            objGroup.RetailName = item.RetailName;
                            objGroup.StartedOn = item.WentLiveDate;
                            objGroup.CustomerType = item.CustomerType;
                            objGroup.OutletCount = item.OutletCount;

                            var RetailCategoryName = context.tblCategories.Where(x => x.CategoryId == item.RetailCategory).Select(y => y.CategoryName).FirstOrDefault();
                            objGroup.RetailCategory = RetailCategoryName;
                            var CityName = context.tblCities.Where(x => x.CityId == item.City).Select(y => y.CityName).FirstOrDefault();
                            objGroup.City = CityName;
                            var SourcedByName = context.tblSourcedBies.Where(x => x.SourcedbyId == item.SourcedBy).Select(y => y.SourcedbyName).FirstOrDefault();
                            objGroup.SourcedBy = SourcedByName;
                            var RMTeamName = context.tblRMAssigneds.Where(x => x.RMAssignedId == item.RMAssigned).Select(y => y.RMAssignedName).FirstOrDefault();
                            objGroup.RMTeam = RMTeamName;
                            objGroup.BillingProductName = context.BOTS_TblBillingPartnerProduct.Where(x => x.BillingPartnerProductId == item.BillingProduct).Select(y => y.BillingPartnerProductName).FirstOrDefault();

                            if (item.WentLiveDate.HasValue)
                            {
                                var day = item.WentLiveDate.Value.Day;
                                var month = item.WentLiveDate.Value.Month;
                                var year = item.WentLiveDate.Value.Year;
                                var currentYear = DateTime.Today.Year;

                                DateTime nextRenewal = new DateTime(currentYear, month, day);
                                DateTime ProgramRenewalDate = new DateTime();
                                if (nextRenewal < DateTime.Today)
                                {
                                    ProgramRenewalDate = nextRenewal.AddYears(1);
                                }
                                else
                                {
                                    ProgramRenewalDate = nextRenewal;
                                }
                                objGroup.RenewalDate = ProgramRenewalDate.ToString("dd-MMM-yyyy");
                            }




                            objGroupList.Add(objGroup);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetAllCustomer");
            }
            return objGroupList;
        }

        public string GetCustomerConnString(string GroupId)
        {
            string ConnectionString = string.Empty;
            try
            {
                using (var context = new CommonDBContext())
                {
                    //var DBDetails = context.DatabaseDetails.Where(x => x.GroupId == GroupId).FirstOrDefault();
                    var DBDetails = context.tblDatabaseDetails.Where(x => x.GroupId == GroupId).FirstOrDefault(); 
                    if (DBDetails != null && GroupId != "1087")
                    {
                        newexception.AddDummyException("111");
                        ConnectionString = "Data Source = " + DBDetails.IPAddress + "; Initial Catalog = " + DBDetails.DBName + "; user id = " + DBDetails.DBId + "; password = " + DBDetails.DBPassword + "";
                    }
                    if (DBDetails != null && GroupId=="1087")
                    {
                        newexception.AddDummyException("222");
                        ConnectionString = "Data Source = " + DBDetails.IPAddress + "; Initial Catalog = MadhusudanTextiles_New; user id = " + DBDetails.DBId + "; password = " + DBDetails.DBPassword + "";
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCustomerConnString");
            }
            return ConnectionString;
        }

        public string GetRetailWebConnString(string CounterId)
        {
            string ConnectionString = string.Empty;
            try
            {
                using (var context = new CommonDBContext())
                {
                    var DBDetails = context.DatabaseDetails.Where(x => x.CounterId == CounterId).FirstOrDefault();
                    //CustomerConnString.ConnectionStringCustomer = DBDetails.DBName;
                    if (DBDetails != null)
                    {
                        ConnectionString = "Data Source = " + DBDetails.IPAddress + "; Initial Catalog = " + DBDetails.DBName + "; user id = " + DBDetails.DBId + "; password = " + DBDetails.DBPassword + "";
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetRetailWebConnString");
            }
            return ConnectionString;
        }

        public string GetCustomerName(string GroupId)
        {
            string CustomerName = string.Empty;
            try
            {
                using (var context = new CommonDBContext())
                {
                    var TgroupId = Convert.ToInt32(GroupId);
                    CustomerName = context.tblGroupDetails.Where(x => x.GroupId == TgroupId).Select(y => y.GroupName).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCustomerName");
            }
            return CustomerName;
        }

        public string GetCustomerLogo(string GroupId)
        {
            var conStr = GetCustomerConnString(GroupId);
            string CustomerLogo = string.Empty;
            try
            {
                using (var context = new BOTSDBContext(conStr))
                {
                    CustomerLogo = context.BrandDetails.Select(y => y.BrandLogoUrl).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCustomerLogo");
            }
            return CustomerLogo;
        }

        public bool GetIsFeedback(string GroupId)
        {
            bool IsFeedback = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    var TgroupId = Convert.ToInt32(GroupId);
                    var groupDetail = context.tblGroupDetails.Where(x => x.GroupId == TgroupId).Select(y => y.IsFeedback).FirstOrDefault();
                    if (groupDetail.HasValue)
                    {
                        if (groupDetail.Value)
                            IsFeedback = true;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetIsFeedback");
            }
            return IsFeedback;
        }
        public bool GetIsEvent(string EventId)
        {
            bool IsEvent = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    var TeventId = Convert.ToInt32(EventId);
                    var eventDetail = context.tblGroupDetails.Where(x => x.GroupId == TeventId).Select(y => y.IsEvent).FirstOrDefault();
                    if (eventDetail.HasValue)
                    {
                        if (eventDetail.Value)
                            IsEvent = true;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetIsEvent");
            }
            return IsEvent;
        }

        public tblGroupDetail GetGroupDetails(int GroupId)
        {
            tblGroupDetail objGroupDetail = new tblGroupDetail();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objGroupDetail = context.tblGroupDetails.Where(x => x.GroupId == GroupId).FirstOrDefault();
                }
                var connStr = GetCustomerConnString(Convert.ToString(GroupId));
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    var ticketSize = contextNew.TransactionMasters.Average(x => x.InvoiceAmt);
                    objGroupDetail.AverageTicket = Math.Round(Convert.ToDouble(ticketSize), 2);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetGroupDetails");
            }
            return objGroupDetail;
        }

        public BOTS_TblGroupMaster GetOnboardingGroupDetails(string GroupId)
        {
            BOTS_TblGroupMaster objGroupDetail = new BOTS_TblGroupMaster();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objGroupDetail = context.BOTS_TblGroupMaster.Where(x => x.GroupId == GroupId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOnboardingGroupDetails");
            }
            return objGroupDetail;
        }

        public tblModulesPayment GetModulesAndPayments(int GroupId)
        {
            tblModulesPayment objModulesPayment = new tblModulesPayment();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objModulesPayment = context.tblModulesPayments.Where(x => x.GroupId == GroupId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetModulesAndPayments");
            }
            return objModulesPayment;
        }

        public int AddGroupDetails(tblGroupDetail objGroupDetails)
        {
            int GroupId = 0;
            try
            {
                using (var context = new CommonDBContext())
                {
                    using (DbContextTransaction transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(objGroupDetails.OtherBillingPartner))
                            {
                                tblBillingPartner objBillingPartner = new tblBillingPartner();
                                objBillingPartner.BillingPartnerName = objGroupDetails.OtherBillingPartner;
                                context.tblBillingPartners.AddOrUpdate(objBillingPartner);
                                context.SaveChanges();
                                objGroupDetails.BillingPartner = objBillingPartner.BillingPartnerId;
                            }
                            if (!string.IsNullOrEmpty(objGroupDetails.OtherCity))
                            {
                                tblCity objCity = new tblCity();
                                objCity.CityName = objGroupDetails.OtherCity;
                                context.tblCities.AddOrUpdate(objCity);
                                context.SaveChanges();
                                objGroupDetails.City = objCity.CityId;
                            }
                            if (!string.IsNullOrEmpty(objGroupDetails.OtherRetailCategory))
                            {
                                tblCategory objCategory = new tblCategory();
                                objCategory.CategoryName = objGroupDetails.OtherRetailCategory;
                                context.tblCategories.AddOrUpdate(objCategory);
                                context.SaveChanges();
                                objGroupDetails.RetailCategory = objCategory.CategoryId;
                            }
                            if (!string.IsNullOrEmpty(objGroupDetails.OtherRMAssigned))
                            {
                                tblRMAssigned objRMAssigned = new tblRMAssigned();
                                objRMAssigned.RMAssignedName = objGroupDetails.OtherRMAssigned;
                                context.tblRMAssigneds.AddOrUpdate(objRMAssigned);
                                context.SaveChanges();
                                objGroupDetails.RMAssigned = objRMAssigned.RMAssignedId;
                            }
                            if (!string.IsNullOrEmpty(objGroupDetails.OtherSourcedBy))
                            {
                                tblSourcedBy objSourcedBy = new tblSourcedBy();
                                objSourcedBy.SourcedbyName = objGroupDetails.OtherSourcedBy;
                                context.tblSourcedBies.AddOrUpdate(objSourcedBy);
                                context.SaveChanges();
                                objGroupDetails.SourcedBy = objSourcedBy.SourcedbyId;
                            }

                            if (!string.IsNullOrEmpty(objGroupDetails.Logo))
                            {
                                //Upload Logo Image
                                var profilePhysicalURL = System.Configuration.ConfigurationManager.AppSettings["LogoPhysicalURL"];
                                string filePhysicalPath = System.IO.Path.Combine(profilePhysicalURL + "\\");
                                string base64String = Convert.ToString(objGroupDetails.LogoBase64);
                                byte[] newBytes = Convert.FromBase64String(base64String);
                                MemoryStream ms = new MemoryStream(newBytes, 0, newBytes.Length);
                                ms.Write(newBytes, 0, newBytes.Length);
                                var fileName = Convert.ToString(objGroupDetails.GroupId + ".jpg");
                                FileStream fileNew = new FileStream(filePhysicalPath + "\\" + fileName, FileMode.Create, FileAccess.Write);
                                ms.WriteTo(fileNew);
                                fileNew.Close();
                                ms.Close();
                                objGroupDetails.Logo = fileName;
                            }

                            string grpId = Convert.ToString(objGroupDetails.GroupId);
                            //Check Customer DB exist
                            var DBDetails = context.DatabaseDetails.Where(x => x.GroupId == grpId).FirstOrDefault();
                            if (DBDetails == null)
                            {
                                //Create Customer DB And Add Entry to DatabaseDetails Table
                                var LastGroupId = context.tblGroupDetails.OrderByDescending(x => x.GroupId).Take(1).Select(y => y.GroupId).FirstOrDefault();
                                objGroupDetails.GroupId = LastGroupId + 1;
                            }

                            context.tblGroupDetails.AddOrUpdate(objGroupDetails);
                            context.SaveChanges();

                            transaction.Commit();
                            GroupId = objGroupDetails.GroupId;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AddGroupDetails");
            }

            return GroupId;
        }

        public bool AddModulesAndPayments(tblModulesPayment objModulesPayment)
        {
            bool status = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    using (DbContextTransaction transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            context.tblModulesPayments.AddOrUpdate(objModulesPayment);
                            context.SaveChanges();
                            transaction.Commit();
                            status = true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AddModulesAndPayments");
            }
            return status;
        }

        public bool AddBrandAndOutlet(string GroupId, List<BrandDetail> lstBrand, List<OutletDetail> lstOutlet)
        {
            bool status = false;
            string ConnectionString = string.Empty;
            try
            {
                using (var context = new CommonDBContext())
                {
                    var DBDetails = context.DatabaseDetails.Where(x => x.GroupId == GroupId).FirstOrDefault();
                    ConnectionString = "Data Source = " + DBDetails.IPAddress + "; Initial Catalog = " + DBDetails.DBName + "; user id = " + DBDetails.DBId + "; password = " + DBDetails.DBPassword + "";
                }
                using (var contextNew = new BOTSDBContext(ConnectionString))
                {
                    using (DbContextTransaction transaction = contextNew.Database.BeginTransaction())
                    {
                        try
                        {
                            var oldOutlets = contextNew.OutletDetails.Where(x => x.GroupId == GroupId).ToList();
                            foreach (var item in oldOutlets)
                            {
                                contextNew.OutletDetails.Remove(item);
                                contextNew.SaveChanges();
                            }

                            var oldBrands = contextNew.BrandDetails.Where(x => x.GroupId == GroupId).ToList();
                            foreach (var item in oldBrands)
                            {
                                contextNew.BrandDetails.Remove(item);
                                contextNew.SaveChanges();
                            }

                            string brandId = string.Empty;
                            int sNoCount = 1;
                            var LastBrand = contextNew.BrandDetails.Where(m => m.GroupId == GroupId).OrderByDescending(x => x.BrandId).Select(y => y).FirstOrDefault();

                            if (LastBrand == null)
                            {
                                brandId = GroupId + sNoCount;
                            }
                            else
                            {
                                brandId = Convert.ToString(Convert.ToInt32(LastBrand.BrandId) + sNoCount);
                            }
                            int count = 0;
                            int outletcount = 1;

                            foreach (var item in lstBrand)
                            {
                                item.BrandId = Convert.ToString(Convert.ToInt32(brandId) + count);
                                contextNew.BrandDetails.AddOrUpdate(item);
                                contextNew.SaveChanges();

                                var OutletId = string.Empty;

                                if (sNoCount == 1)
                                {
                                    foreach (var outletItem in lstOutlet)
                                    {
                                        if ((outletItem.BrandId == item.BrandId) || outletItem.BrandId == "1")
                                        {
                                            var outletLast = contextNew.OutletDetails.Where(x => x.BrandId == item.BrandId).OrderByDescending(y => y.BrandId).FirstOrDefault();

                                            if (outletLast == null)
                                            {
                                                OutletId = item.BrandId + "00" + outletcount;
                                            }
                                            else
                                            {
                                                OutletId = Convert.ToString(Convert.ToDouble(outletLast.OutletId) + 1);
                                            }
                                            outletItem.BrandId = item.BrandId;
                                            outletItem.OutletId = OutletId;

                                            contextNew.OutletDetails.AddOrUpdate(outletItem);
                                            contextNew.SaveChanges();

                                            outletcount++;
                                        }
                                    }
                                }
                                if (sNoCount == 2)
                                {
                                    outletcount = 1;
                                    foreach (var outletItem in lstOutlet)
                                    {
                                        if ((outletItem.BrandId == item.BrandId) || outletItem.BrandId == "2")
                                        {
                                            var outletLast = contextNew.OutletDetails.Where(x => x.BrandId == item.BrandId).OrderByDescending(y => y.BrandId).FirstOrDefault();
                                            if (outletLast == null)
                                            {
                                                OutletId = item.BrandId + "00" + outletcount;
                                            }
                                            else
                                            {
                                                OutletId = Convert.ToString(Convert.ToDouble(outletLast.OutletId) + 1);
                                            }
                                            outletItem.BrandId = item.BrandId;
                                            outletItem.OutletId = OutletId;

                                            contextNew.OutletDetails.AddOrUpdate(outletItem);
                                            contextNew.SaveChanges();

                                            outletcount++;
                                        }
                                    }
                                }
                                count++;
                                sNoCount++;
                            }

                            transaction.Commit();
                            status = true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AddBrandAndOutlet");
            }
            return status;

        }

        public List<BrandDetail> GetAllBrandsByGroupId(string GroupId)
        {
            List<BrandDetail> lstBrands = new List<BrandDetail>();
            try
            {
                string ConnectionString = string.Empty;
                using (var context = new CommonDBContext())
                {
                    var DBDetails = context.DatabaseDetails.Where(x => x.GroupId == GroupId).FirstOrDefault();
                    ConnectionString = "Data Source = " + DBDetails.IPAddress + "; Initial Catalog = " + DBDetails.DBName + "; user id = " + DBDetails.DBId + "; password = " + DBDetails.DBPassword + "";
                }
                using (var contextNew = new BOTSDBContext(ConnectionString))
                {
                    lstBrands = contextNew.BrandDetails.Where(x => x.GroupId == GroupId).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetAllBrandsByGroupId");
            }
            return lstBrands;
        }

        public List<OutletDetail> GetAllOutletsByGroupId(string GroupId)
        {
            List<OutletDetail> lstOutlets = new List<OutletDetail>();
            try
            {
                string ConnectionString = string.Empty;

                using (var context = new CommonDBContext())
                {
                    var DBDetails = context.DatabaseDetails.Where(x => x.GroupId == GroupId).FirstOrDefault();
                    ConnectionString = "Data Source = " + DBDetails.IPAddress + "; Initial Catalog = " + DBDetails.DBName + "; user id = " + DBDetails.DBId + "; password = " + DBDetails.DBPassword + "";
                }
                using (var contextNew = new BOTSDBContext(ConnectionString))
                {
                    lstOutlets = contextNew.OutletDetails.Where(x => x.GroupId == GroupId).ToList();
                    foreach (var item in lstOutlets)
                    {
                        item.ProgramStartDate = contextNew.TransactionMasters.Where(x => x.CounterId.Contains(item.OutletId)).OrderBy(y => y.Datetime).Select(z => z.Datetime).FirstOrDefault();
                        item.ProgramRenewalDate = item.ProgramStartDate.Value.AddYears(1);
                        var day = item.ProgramStartDate.Value.Day;
                        var month = item.ProgramStartDate.Value.Month;
                        var year = item.ProgramStartDate.Value.Year;
                        var currentYear = DateTime.Today.Year;

                        DateTime nextRenewal = new DateTime(currentYear, month, day);
                        if (nextRenewal < DateTime.Today)
                        {
                            item.ProgramRenewalDate = nextRenewal.AddYears(1);
                        }
                        else
                        {
                            item.ProgramRenewalDate = nextRenewal;
                        }


                        var totalTransaction = contextNew.TransactionMasters.Where(x => x.CounterId.Contains(item.OutletId)).Count();
                        var FirstDate = contextNew.TransactionMasters.Where(x => x.CounterId.Contains(item.OutletId)).OrderBy(y => y.Datetime).Select(z => z.Datetime).FirstOrDefault();
                        var LastDate = contextNew.TransactionMasters.Where(x => x.CounterId.Contains(item.OutletId)).OrderByDescending(y => y.Datetime).Select(z => z.Datetime).FirstOrDefault();


                        var Days = (LastDate.Value - FirstDate.Value).TotalDays;
                        var Average = totalTransaction / Days;
                        var TransactionPerDay = Average;
                        item.TransactionPerDay = Math.Round(Convert.ToDouble(Average), 2);


                    }
                    //var ProgramStartDate = contextNew.TransactionMasters.Where(x => x.CounterId.Contains(OutletId)).OrderBy(y => y.Datetime).Select(z => z.Datetime).FirstOrDefault();
                    //var totalTransaction = contextNew.TransactionMasters.Where(x => x.CounterId.Contains(item.OutletId)).Count();


                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetAllOutletsByGroupId");
            }
            return lstOutlets;
        }

        public ProfilePage GetprofilePageData(string GroupId)
        {
            ProfilePage objprofilePage = new ProfilePage();
            //string connectionString = GetCustomerConnString(GroupId);
            //tblGroupDetail objgrpdetails = new tblGroupDetail();
            //tblCategory objtblcategory = new tblCategory();
            //SMSGatewayMaster objsmsgateway = new SMSGatewayMaster();

            //using (var context = new CommonDBContext())
            //{
            //    int gId = Convert.ToInt32(GroupId);
            //    objgrpdetails = context.tblGroupDetails.Where(x => x.GroupId == gId).FirstOrDefault();
            //    objtblcategory = context.tblCategories.Where(x => x.CategoryId == objgrpdetails.RetailCategory).FirstOrDefault();

            //}
            //SMSDetail objsMSDetail = new SMSDetail();
            //GroupDetail objgroupdetails = new GroupDetail();
            //using (var Context = new BOTSDBContext(connectionString))
            //{
            //    var objsms = Context.SMSDetails;
            //    string whatsAppbaln = null;
            //    string Smsbalance = null;
            //    // string SmsgatewayId = null;
            //    foreach (var sms in objsms)
            //    {
            //        //if (sms.SMSGatewayId == "2")
            //        //{
            //        if (!string.IsNullOrEmpty(sms.WhatsAppTokenId))
            //        {
            //            ServicePointManager.Expect100Continue = true;
            //            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
            //            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            //            WebClient client = new WebClient();
            //            client.Headers["Content-type"] = "application/json";
            //            client.Encoding = Encoding.UTF8;
            //            var uri = string.Concat("https://enotify.app/api/checkBal?token=", sms.WhatsAppTokenId);
            //            var result = client.DownloadString(uri);
            //            Dictionary<string, dynamic> whatsappbalance = (new JavaScriptSerializer()).Deserialize<Dictionary<string, dynamic>>(result);

            //            string whatsappexpirydt = whatsappbalance["data"]["expiryDate"];
            //            string whatsappdate = whatsappexpirydt.Substring(0, 19);
            //            // whatsexpdt = Convert.ToDateTime(whatsappdate).ToString("yyyy-MM-dd");
            //            whatsAppbaln = Convert.ToInt32(whatsappbalance["data"]["quota"]);

            //        }
            //        if (!string.IsNullOrEmpty(sms.TxnUserName) && !string.IsNullOrEmpty(sms.TxnPassword))
            //        {
            //            ServicePointManager.Expect100Continue = true;
            //            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
            //            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            //            WebClient client = new WebClient();
            //            client.Headers["Content-type"] = "application/json";
            //            client.Encoding = Encoding.UTF8;
            //            var uri = string.Concat("https://smsnotify.one/SMSApi/account/readstatus?userid=", sms.TxnUserName, "&password=", sms.TxnPassword, "&output=json");
            //            var result = client.DownloadString(uri);
            //            Dictionary<string, dynamic> smsbalance = (new JavaScriptSerializer()).Deserialize<Dictionary<string, dynamic>>(result);
            //            if (smsbalance["response"]["status"].Equals("success"))
            //            {
            //                Dictionary<string, dynamic> accountdata = smsbalance["response"]["account"];
            //                Smsbalance = Convert.ToInt32(accountdata["smsBalance"]);
            //            }

            //        }

            //        //}
            //        //else
            //        //{

            //        //}
            //        objprofilePage.GroupId = objgrpdetails.GroupId;
            //        objprofilePage.Logo = objgrpdetails.Logo;
            //        objprofilePage.LegalName = objgrpdetails.GroupName;
            //        objprofilePage.RetailName = objgrpdetails.GroupName;
            //        objprofilePage.OwnerName = objgrpdetails.OwnerName;
            //        objprofilePage.OwnerNumber = objgrpdetails.OwnerMobileNo;
            //        using (var Context1 = new BOTSDBContext(connectionString))
            //        {
            //            string gid = objgrpdetails.GroupId.ToString();
            //            objgroupdetails = Context1.GroupDetails.Where(x => x.GroupId == gid).FirstOrDefault();
            //        }
            //        objprofilePage.City = objgroupdetails.City;
            //        objprofilePage.RetailCategory = objtblcategory.CategoryName.ToString();
            //        objprofilePage.OutletEnrolled = objgrpdetails.OutletCount.ToString();
            //        using (var context = new CommonDBContext())
            //        {
            //            objsmsgateway = context.SMSGatewayMasters.Where(x => x.SMSGatewayId == sms.SMSGatewayId).FirstOrDefault();
            //        }
            //        objprofilePage.SMSGateway = objsmsgateway.SMSGatewayName;
            //        objprofilePage.GSTNo = objgrpdetails.GSTNO;
            //        objprofilePage.SMSBalance = Smsbalance;
            //        objprofilePage.WhatsAppBalance = whatsAppbaln;
            //        objprofilePage.NextPaymentDate = DateTime.Now.ToString();
            //        objprofilePage.NextPaymentAmount = "";
            //    }


            //}

            return objprofilePage;
        }

        public MobileAppOnceInMonthData GetMonthlySnapShotForMobileApp(string GroupId)
        {
            MobileAppOnceInMonthData objmobileappdata = new MobileAppOnceInMonthData();
            try
            {
                using (var context = new CommonDBContext())
                {
                    // int gId = Convert.ToInt32(GroupId);
                    objmobileappdata = context.MobileAppOnceInMonthData.Where(x => x.GroupId == GroupId).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetMonthlySnapShotForMobileApp");
            }
            return objmobileappdata;
        }

        public long GetMemberBase(string GroupId)
        {
            long MemberBase = 0;
            try
            {
                string ConnectionString = string.Empty;
                using (var context = new CommonDBContext())
                {
                    var DBDetails = context.DatabaseDetails.Where(x => x.GroupId == GroupId).FirstOrDefault();
                    ConnectionString = "Data Source = " + DBDetails.IPAddress + "; Initial Catalog = " + DBDetails.DBName + "; user id = " + DBDetails.DBId + "; password = " + DBDetails.DBPassword + "";
                    using (var contextNew = new BOTSDBContext(ConnectionString))
                    {
                        MemberBase = contextNew.CustomerDetails.Count();
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetMemberBase");
            }
            return MemberBase;
        }

        public GroupConfig GetGroupConfig(tblGroupDetail objGroupDetails)
        {
            GroupConfig objGroupConfig = new GroupConfig();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objGroupConfig.CityName = context.tblCities.Where(x => x.CityId == objGroupDetails.City).Select(y => y.CityName).FirstOrDefault();
                    objGroupConfig.CategoryName = context.tblCategories.Where(x => x.CategoryId == objGroupDetails.RetailCategory).Select(y => y.CategoryName).FirstOrDefault();
                    objGroupConfig.BillingPartnerName = context.tblBillingPartners.Where(x => x.BillingPartnerId == objGroupDetails.BillingPartner).Select(y => y.BillingPartnerName).FirstOrDefault();
                    objGroupConfig.BillingSystemName = context.BOTS_TblBillingPartnerProduct.Where(x => x.BillingPartnerProductId == objGroupDetails.BillingProduct).Select(y => y.BillingPartnerProductName).FirstOrDefault();
                    objGroupConfig.SourceByName = context.tblSourcedBies.Where(x => x.SourcedbyId == objGroupDetails.SourcedBy).Select(y => y.SourcedbyName).FirstOrDefault();
                    objGroupConfig.CSAssignedName = context.tblRMAssigneds.Where(x => x.RMAssignedId == objGroupDetails.RMAssigned).Select(y => y.RMAssignedName).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetGroupConfig");
            }
            return objGroupConfig;
        }

        public List<PointsRulesEarnConfig> GetPointsEarnConfig(string groupId)
        {
            List<PointsRulesEarnConfig> objData = new List<PointsRulesEarnConfig>();
            try
            {
                string ConnectionString = string.Empty;
                using (var context = new CommonDBContext())
                {
                    var DBDetails = context.DatabaseDetails.Where(x => x.GroupId == groupId).FirstOrDefault();
                    ConnectionString = "Data Source = " + DBDetails.IPAddress + "; Initial Catalog = " + DBDetails.DBName + "; user id = " + DBDetails.DBId + "; password = " + DBDetails.DBPassword + "";
                    using (var contextNew = new BOTSDBContext(ConnectionString))
                    {
                        var lstBrands = contextNew.BrandDetails.OrderBy(x => x.BrandId).ToList();
                        var lstEarnData = contextNew.EarnRules.OrderBy(x => x.RuleId).ToList();

                        int count = 0;
                        foreach (var item in lstBrands)
                        {
                            if (lstEarnData.Count >= (count + 1))
                            {
                                PointsRulesEarnConfig objItem = new PointsRulesEarnConfig();
                                var objEarnData = lstEarnData.ElementAt(count);
                                objItem.PointsAllocation = objEarnData.PointsAllocation.Value;
                                objItem.PointsExpiryVariableDate = objEarnData.PointsExpiryVariableDate.Value;
                                objItem.MinTxnAmt = objEarnData.MinTxnAmt.Value;
                                objItem.MaxPointsEarned = objEarnData.MaxPointsEarned.Value;
                                objItem.PointsProductORBase = item.PointsProductORBase;
                                objItem.PointsPrecentage = objEarnData.PointsPrecentage.Value;

                                objData.Add(objItem);
                                count++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetPointsEarnConfig");
            }
            return objData;
        }
        public List<PointsRulesBurnConfig> GetPointsBurnConfig(string groupId)
        {
            List<PointsRulesBurnConfig> objData = new List<PointsRulesBurnConfig>();
            string ConnectionString = string.Empty;
            try
            {
                using (var context = new CommonDBContext())
                {
                    var DBDetails = context.DatabaseDetails.Where(x => x.GroupId == groupId).FirstOrDefault();
                    ConnectionString = "Data Source = " + DBDetails.IPAddress + "; Initial Catalog = " + DBDetails.DBName + "; user id = " + DBDetails.DBId + "; password = " + DBDetails.DBPassword + "";
                    using (var contextNew = new BOTSDBContext(ConnectionString))
                    {
                        var lstBurnData = contextNew.BurnRules.OrderBy(x => x.RuleId).ToList();
                        var lstBrands = contextNew.BrandDetails.OrderBy(x => x.BrandId).ToList();
                        int count = 0;
                        foreach (var item in lstBrands)
                        {
                            if (lstBurnData.Count >= (count + 1))
                            {
                                PointsRulesBurnConfig objItem = new PointsRulesBurnConfig();
                                var objBurnData = lstBurnData.ElementAt(count);
                                objItem.MinThresholdPointsFirstTime = objBurnData.MinThresholdPointsFirstTime.Value;
                                objItem.MinThresholdPointsEveryTime = objBurnData.MinThresholdPointsEveryTime.Value;
                                objItem.MinTxnAmt = objBurnData.MinTxnAmt.Value;
                                objItem.EarnFullWhileBurnFlag = objBurnData.EarnFullWhileBurnFlag;

                                objData.Add(objItem);
                                count++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetPointsBurnConfig");
            }
            return objData;
        }

        public SMSDetail GetAllSMSDetails(string groupId)
        {
            SMSDetail SMSDetails = new SMSDetail();
            var connectionString = GetCustomerConnString(groupId);
            try
            {
                using (var contextNew = new BOTSDBContext(connectionString))
                {
                    var lstSMSDetails = contextNew.SMSDetails.ToList();
                    if (lstSMSDetails != null)
                    {
                        if (lstSMSDetails.Count == 1)
                        {
                            SMSDetails = lstSMSDetails.ElementAt(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetAllSMSDetails");
            }
            return SMSDetails;
        }

        public SMSConfig GetSMSEmailMasterDetails(string groupId)
        {
            SMSConfig objData = new SMSConfig();
            try
            {
                var connectionString = GetCustomerConnString(groupId);
                using (var contextNew = new BOTSDBContext(connectionString))
                {
                    var lstSMSEmail = contextNew.SMSEmailMasters.ToList();
                    if (lstSMSEmail != null)
                    {
                        objData.Enrollment = lstSMSEmail.Where(x => x.MessageId == "100").Select(y => y.SMS).FirstOrDefault();
                        objData.Earn = lstSMSEmail.Where(x => x.MessageId == "101").Select(y => y.SMS).FirstOrDefault();
                        objData.Burn = lstSMSEmail.Where(x => x.MessageId == "102").Select(y => y.SMS).FirstOrDefault();
                        objData.CancelEarn = lstSMSEmail.Where(x => x.MessageId == "103").Select(y => y.SMS).FirstOrDefault();
                        objData.CancelBurn = lstSMSEmail.Where(x => x.MessageId == "104").Select(y => y.SMS).FirstOrDefault();
                        objData.OTP = lstSMSEmail.Where(x => x.MessageId == "105").Select(y => y.SMS).FirstOrDefault();
                        objData.BalanceInquiry = lstSMSEmail.Where(x => x.MessageId == "106").Select(y => y.SMS).FirstOrDefault();
                        objData.EnrollmentAndEarn = lstSMSEmail.Where(x => x.MessageId == "107").Select(y => y.SMS).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetSMSEmailMasterDetails");
            }
            return objData;
        }

        public WAConfig GetWAEmailMasterDetails(string groupId)
        {
            WAConfig objData = new WAConfig();
            try
            {
                var connectionString = GetCustomerConnString(groupId);
                using (var contextNew = new BOTSDBContext(connectionString))
                {
                    bool exists = contextNew.Database
                         .SqlQuery<int?>(@"
                         SELECT 1 FROM sys.tables AS T
                         INNER JOIN sys.schemas AS S ON T.schema_id = S.schema_id
                         WHERE S.Name = 'dbo' AND T.Name = 'WhatsAppSMSMaster'")
                         .SingleOrDefault() != null;
                    if (exists)
                    {
                        var lstWAEmail = contextNew.WhatsAppSMSMasters.ToList();
                        if (lstWAEmail != null)
                        {
                            objData.Enrollment = lstWAEmail.Where(x => x.MessageId == "100").Select(y => y.SMS).FirstOrDefault();
                            objData.Earn = lstWAEmail.Where(x => x.MessageId == "101").Select(y => y.SMS).FirstOrDefault();
                            objData.Burn = lstWAEmail.Where(x => x.MessageId == "102").Select(y => y.SMS).FirstOrDefault();
                            objData.CancelEarn = lstWAEmail.Where(x => x.MessageId == "103").Select(y => y.SMS).FirstOrDefault();
                            objData.CancelBurn = lstWAEmail.Where(x => x.MessageId == "104").Select(y => y.SMS).FirstOrDefault();
                            objData.OTP = lstWAEmail.Where(x => x.MessageId == "105").Select(y => y.SMS).FirstOrDefault();
                            objData.BalanceInquiry = lstWAEmail.Where(x => x.MessageId == "106").Select(y => y.SMS).FirstOrDefault();
                            objData.EnrollmentAndEarn = lstWAEmail.Where(x => x.MessageId == "107").Select(y => y.SMS).FirstOrDefault();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetWAEmailMasterDetails");
            }
            return objData;
        }

        public List<MWP_Details> GetDLCDetails(string groupId)
        {
            List<MWP_Details> lstData = new List<MWP_Details>();
            try
            {
                var connectionString = GetCustomerConnString(groupId);
                using (var contextNew = new BOTSDBContext(connectionString))
                {
                    lstData = contextNew.MWP_Details.ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDLCDetails");
            }
            return lstData;
        }

        public List<MWPSourceMaster> GetMWPSourceMaster(string groupId)
        {
            List<MWPSourceMaster> objData = new List<MWPSourceMaster>();
            try
            {
                var connectionString = GetCustomerConnString(groupId);
                using (var contextNew = new BOTSDBContext(connectionString))
                {
                    objData = contextNew.MWPSourceMasters.ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetMWPSourceMaster");
            }
            return objData;
        }

        public List<tblUniquePoint> GetUniquePoints(string groupId)
        {
            List<tblUniquePoint> objData = new List<tblUniquePoint>();
            string ConnectionString = string.Empty;

            ConnectionString = GetCustomerConnString(groupId);
            try
            {
                using (var contextNew = new BOTSDBContext(ConnectionString))
                {
                    objData = contextNew.tblUniquePoints.ToList();
                }
            }

            catch (Exception ex)
            {
                newexception.AddException(ex, "GetUniquePoints");
            }
            return objData;
        }

        //This is specific to Caramella Franchisee Enquiry
        public bool AddFranchiseeEnquiry(string MobileNo, string Name, string Area)
        {
            bool status = false;
            try
            {
                tblFranchiseeEnquiry objData = new tblFranchiseeEnquiry();
                objData.CustomerName = Name;
                objData.MobileNo = MobileNo;
                objData.AreaOfFranchisee = Area;
                objData.AddedDate = DateTime.Now;
                var connectionString = GetCustomerConnString("1172");
                using (var contextNew = new BOTSDBContext(connectionString))
                {
                    contextNew.tblFranchiseeEnquiries.AddOrUpdate(objData);
                    contextNew.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AddFranchiseeEnquiry");
            }
            return status;
        }

        public List<MemberBaseAndTransaction> GetMemberBaseAndTransactions(string groupId)
        {
            List<MemberBaseAndTransaction> objData = new List<MemberBaseAndTransaction>();
            var conStr = GetCustomerConnString(groupId);
            try
            {
                if (!string.IsNullOrEmpty(conStr))
                {
               
                    using (var context = new BOTSDBContext(conStr))
                    {
                        MemberBaseAndTransaction objItem = new MemberBaseAndTransaction();
                        var FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month, 1);
                        var lastDay = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month);
                        var ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month, lastDay);
                        objItem.MemberType = "New Enrollment";
                        objItem.BaseCount = context.CustomerDetails.Where(x => x.DOJ >= FromDate && x.DOJ <= ToDate).Count();
                        objItem.TxnCount = context.TransactionMasters.Where(x => x.Datetime >= FromDate && x.Datetime <= ToDate).Count();
                        objItem.BizGen = context.TransactionMasters.Where(x => x.Datetime >= FromDate && x.Datetime <= ToDate).Select(y => y.InvoiceAmt).Sum();
                        objData.Add(objItem);

                        MemberBaseAndTransaction objItem1 = new MemberBaseAndTransaction();
                        objItem1.MemberType = "Existing Base - Till Date";
                        objItem1.BaseCount = context.CustomerDetails.Count();
                        objItem1.TxnCount = context.TransactionMasters.Count();
                        objItem1.BizGen = context.TransactionMasters.Select(y => y.InvoiceAmt).Sum();
                        objData.Add(objItem1);
                    }



                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetMemberBaseAndTransactions");
            }

            return objData;
        }

        public KeyMetrics GetKeyMetrics(string groupId)
        {
            KeyMetrics objData = new KeyMetrics();
            var conStr = GetCustomerConnString(groupId);
            try
            {
                if (!string.IsNullOrEmpty(conStr))
                {
                    using (var context = new BOTSDBContext(conStr))
                    {

                        string sqlString = "select C.MobileNo,C.Points,C.EnrollingOutlet,(Convert(varchar(10),cast(C.DOJ as date),103)) as EnrolledDate,C.CustomerName,Count(TM.MobileNo) as " +
                                "TxnCount,COUNT(CASE WHEN TM.TransType = '2' THEN 1 ELSE NULL END) as TotalBurnTxn,sum((case when TM.TransType = '2' then TM.PointsBurned else 0 " +
                                "end)) as TotalBurnPoints,COUNT(CASE WHEN TM.TransType = '1' THEN 1 ELSE NULL END) as EarnCount,COUNT(CASE WHEN TM.TransType = '2' THEN 1 ELSE " +
                                "NULL END) as BurnCount,Min(cast(TM.Datetime as date)) as FirstTxnDate,Max(cast(TM.Datetime as date)) as LastTxnDate,isnull(Sum(TM.PointsEarned) " +
                                ", 0) as PointsEarned,isnull(sum(TM.PointsBurned), 0) as PointsBurned,isnull(sum(TM.InvoiceAmt), 0) as InvoiceAmt,sum((case when TM.TransType = '2' " +
                                "then TM.InvoiceAmt else 0 end)) as BurnAmt from TransactionMaster TM right join CustomerDetails C on C.MobileNo = TM.MobileNo where " +
                                "((TM.TransType = '1') or(TM.TransType = '2')) and TM.InvoiceNo != 'B_Birthday' and TM.InvoiceNo != 'B_Anniversary' and TM.InvoiceNo != " +
                                "'B_ProfileUpdate' and TM.InvoiceNo != 'B_ReferralBonus' and TM.InvoiceNo != 'B_RefereePoints' and TM.InvoiceNo != 'B_GiftingPoints' and " +
                                "TM.InvoiceNo != 'Bonus' and TM.InvoiceNo != 'B_ReferralPoints' and TM.InvoiceNo != 'B_RefereeBonus' and TM.Status = '06' and " +
                                "((C.CustomerThrough = '2') or (C.CustomerThrough = '4') or (C.CustomerThrough = '5') or (C.CustomerThrough = '7')) and C.Status = '00' group by " +
                                "C.MobileNo,C.Points,C.CustomerName,C.EnrollingOutlet,C.DOJ";

                        var AllData = context.Database.SqlQuery<KeyMetricsData>(sqlString).ToList();
                        decimal? TotalRedeemPoints = 0;
                        decimal? TotalEarnPoints = 0;

                        TotalRedeemPoints = AllData.Sum(x => x.TotalBurnPoints);
                        if (TotalRedeemPoints == null)
                            TotalRedeemPoints = 0;
                        TotalEarnPoints = AllData.Sum(x => x.PointsEarned);
                        if (TotalEarnPoints == null)
                            TotalEarnPoints = 0;
                        if (TotalRedeemPoints != 0 && TotalEarnPoints != 0)
                        {
                            objData.RedemptionRate = decimal.Round(Convert.ToDecimal(((TotalRedeemPoints / TotalEarnPoints) * 100)), 2, MidpointRounding.AwayFromZero);
                        }
                        var TotalBurnInvoiceAmt = AllData.Sum(x => x.BurnAmt);
                        var TotalBurnPoints = AllData.Sum(x => x.TotalBurnPoints);
                        var PointAllocation = context.EarnRules.Select(x => x.PointsAllocation).FirstOrDefault();
                        if (TotalBurnPoints > 0)
                        {
                            objData.RedeemToInv = decimal.Round((Convert.ToDecimal(TotalBurnInvoiceAmt / (TotalBurnPoints * PointAllocation))), 2, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            objData.RedeemToInv = 0;
                        }
                        objData.OnlyOnceBase = AllData.Where(x => x.TxnCount == 1).Count();
                        objData.NonRedeemBase = AllData.Where(x => x.BurnCount == 1).Count();
                        var InactiveDate = DateTime.Now.AddMonths(-6);
                        objData.InactiveBase = AllData.Where(x => x.LastTxnDate < InactiveDate).Count();
                        objData.BulkImportBase = context.CustomerDetails.Where(x => x.CustomerThrough == "1").Count();

                        decimal? Issued = 0;
                        decimal? Redeemed = 0;

                        string query = "select TM.MobileNo,isnull(sum(TM.PointsEarned),0) as PointsEarned,isnull(sum(TM.PointsBurned),0) as PointsBurned from TransactionMaster(nolock) TM group by TM.MobileNo";
                        var PointSummaryData = context.Database.SqlQuery<KeyMetricsPointSummary>(query).ToList();


                        Issued = PointSummaryData.Sum(x => x.PointsEarned);
                        Redeemed = PointSummaryData.Sum(x => x.PointsBurned);
                        if (Issued == null)
                            Issued = 0;
                        if (Redeemed == null)
                            Redeemed = 0;

                        objData.Issued = Issued.Value;
                        objData.Redeemed = Redeemed.Value;

                        var ExpiredPointStr = "select sum(Points) from PointsExpiryDetails";
                        decimal? ExpiredPoint = 0;
                        ExpiredPoint = context.Database.SqlQuery<decimal?>(ExpiredPointStr).FirstOrDefault();
                        if (ExpiredPoint != null)
                            objData.Expired = ExpiredPoint.Value;
                        else
                            objData.Expired = 0;
                        objData.Available = objData.Issued - (objData.Redeemed + objData.Expired);

                    }

                }
            }

            catch (Exception ex)
            {
                newexception.AddException(ex, "GetKeyMetrics");
            }
            return objData;
        }

        public List<KeyInfoForNextMonth> GetKeyInfoForNextMonth(string groupId)
        {
            List<KeyInfoForNextMonth> lstData = new List<KeyInfoForNextMonth>();
            var conStr = GetCustomerConnString(groupId);
            try
            {
                if (!string.IsNullOrEmpty(conStr))
                {
                    using (var context = new BOTSDBContext(conStr))
                    {
                        var FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        var lastDay = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                        var ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, lastDay);

                        //var Birthday = context.CustomerDetails.Where(x => x.DOB >= FromDate && x.DOB <= ToDate).Count();
                        var Birthday = (from p in context.CustomerDetails where p.DOB.Value.Month == FromDate.Month && p.DOB.Value.Year != 1900 select p.DOB).Count();
                        //var Anniversary = context.CustomerDetails.Where(x => x.AnniversaryDate >= FromDate && x.AnniversaryDate <= ToDate).Count();
                        var Anniversary = (from p in context.CustomerDetails where p.AnniversaryDate.Value.Month == FromDate.Month && p.AnniversaryDate.Value.Year != 1900 select p.AnniversaryDate).Count();
                        var Expiry = context.PointsExpiries.Where(x => x.ExpiryDate >= FromDate && x.ExpiryDate <= ToDate).GroupBy(y => y.MobileNo).Count();

                        KeyInfoForNextMonth objItem = new KeyInfoForNextMonth();
                        objItem.Elements = "Birthday";
                        objItem.BaseCount = Birthday;
                        lstData.Add(objItem);

                        KeyInfoForNextMonth objItem1 = new KeyInfoForNextMonth();
                        objItem1.Elements = "Anniversary";
                        objItem1.BaseCount = Anniversary;
                        lstData.Add(objItem1);

                        KeyInfoForNextMonth objItem2 = new KeyInfoForNextMonth();
                        objItem2.Elements = "Expiry";
                        objItem2.BaseCount = Expiry;
                        lstData.Add(objItem2);
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetKeyInfoForNextMonth");
            }
            return lstData;
        }

        public List<tblFestival> GetFestivalDates()
        {
            List<tblFestival> lstData = new List<tblFestival>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    var FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    var lastDay = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                    var ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, lastDay);
                    lstData = context.tblFestivals.Where(x => x.Date >= FromDate && x.Date <= ToDate).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetFestivalDates");
            }
            return lstData;
        }

        public string GetWAGroupCode(string groupId)
        {
            string WAGroupCode = string.Empty;
            try
            {
                using (var context = new CommonDBContext())
                {
                    WAGroupCode = context.WAReports.Where(x => x.GroupId == groupId).Select(y => y.GroupCode).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetWAGroupCode");
            }
            return WAGroupCode;
        }
    }
}
