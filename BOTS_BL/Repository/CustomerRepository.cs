using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.IO;
using BOTS_BL;
using System.Data.Entity.Validation;

namespace BOTS_BL.Repository
{
    public class CustomerRepository
    {
        Exceptions newexception = new Exceptions();
        public List<SelectListItem> GetRetailCategory()
        {
            List<SelectListItem> lstRetailCategory = new List<SelectListItem>();
            using (var context = new CommonDBContext())
            {
                var RetailCategory = context.tblCategories.ToList();

                foreach (var item in RetailCategory)
                {
                    lstRetailCategory.Add(new SelectListItem
                    {
                        Text = item.CategoryName,
                        Value = Convert.ToString(item.CategoryId)
                    });
                }
            }
            return lstRetailCategory;
        }
        public List<SelectListItem> GetCity()
        {
            List<SelectListItem> lstCity = new List<SelectListItem>();
            using (var context = new CommonDBContext())
            {
                var RetailCategory = context.tblCities.ToList();

                foreach (var item in RetailCategory)
                {
                    lstCity.Add(new SelectListItem
                    {
                        Text = item.CityName,
                        Value = Convert.ToString(item.CityId)
                    });
                }
            }
            return lstCity;
        }
        public List<SelectListItem> GetSourcedBy()
        {
            List<SelectListItem> lstSourcedBy = new List<SelectListItem>();
            using (var context = new CommonDBContext())
            {
                var RetailCategory = context.tblSourcedBies.ToList();

                foreach (var item in RetailCategory)
                {
                    lstSourcedBy.Add(new SelectListItem
                    {
                        Text = item.SourcedbyName,
                        Value = Convert.ToString(item.SourcedbyId)
                    });
                }
            }
            return lstSourcedBy;
        }
        public List<SelectListItem> GetRMAssigned()
        {
            List<SelectListItem> lstRMAssigned = new List<SelectListItem>();
            using (var context = new CommonDBContext())
            {
                var RetailCategory = context.tblRMAssigneds.ToList();

                foreach (var item in RetailCategory)
                {
                    lstRMAssigned.Add(new SelectListItem
                    {
                        Text = item.RMAssignedName,
                        Value = Convert.ToString(item.RMAssignedId)
                    });
                }
            }
            return lstRMAssigned;
        }
        public List<SelectListItem> GetBillingPartner()
        {
            List<SelectListItem> lstBillingPartner = new List<SelectListItem>();
            using (var context = new CommonDBContext())
            {
                var RetailCategory = context.tblBillingPartners.ToList();

                foreach (var item in RetailCategory)
                {
                    lstBillingPartner.Add(new SelectListItem
                    {
                        Text = item.BillingPartnerName,
                        Value = Convert.ToString(item.BillingPartnerId)
                    });
                }
            }
            return lstBillingPartner;
        }

        public List<CustomerListing> GetAllCustomer()
        {
            List<CustomerListing> objGroupList = new List<CustomerListing>();
            try
            {
                List<tblGroupDetail> objGroupDetails = new List<tblGroupDetail>();
                using (var context = new CommonDBContext())
                {                    
                    objGroupDetails = context.tblGroupDetails.ToList();                    
                    if (objGroupDetails != null)
                    {
                        foreach (var item in objGroupDetails)
                        {                            
                            CustomerListing objGroup = new CustomerListing();
                            objGroup.GroupId = item.GroupId;
                            objGroup.Product = item.ProductType;
                            objGroup.RetailName = item.RetailName;
                            objGroup.StartedOn = item.WentLiveDate;

                            var RetailCategoryName = context.tblCategories.Where(x => x.CategoryId == item.RetailCategory).Select(y => y.CategoryName).FirstOrDefault();
                            objGroup.RetailCategory = RetailCategoryName;
                            var CityName = context.tblCities.Where(x => x.CityId == item.City).Select(y => y.CityName).FirstOrDefault();
                            objGroup.City = CityName;
                            //SMSBalCount
                            //RenewalOn
                            var SourcedByName = context.tblSourcedBies.Where(x => x.SourcedbyId == item.SourcedBy).Select(y => y.SourcedbyName).FirstOrDefault();
                            objGroup.SourcedBy = SourcedByName;
                            var RMTeamName = context.tblRMAssigneds.Where(x => x.RMAssignedId == item.RMAssigned).Select(y => y.RMAssignedName).FirstOrDefault();
                            objGroup.RMTeam = RMTeamName;

                            objGroupList.Add(objGroup);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex,"");
            }
            return objGroupList;
        }

        public string GetCustomerConnString(string GroupId)
        {
            string ConnectionString = string.Empty;
            using (var context = new CommonDBContext())
            {
                var DBDetails = context.DatabaseDetails.Where(x => x.GroupId == GroupId).FirstOrDefault();
                //CustomerConnString.ConnectionStringCustomer = DBDetails.DBName;

                ConnectionString = "Data Source = " + DBDetails.IPAddress + "; Initial Catalog = " + DBDetails.DBName + "; user id = " + DBDetails.DBId + "; password = " + DBDetails.DBPassword + "";
            }
            return ConnectionString;
        }

        public string GetCustomerName(string GroupId)
        {
            string CustomerName = string.Empty;
            using (var context = new CommonDBContext())
            {
                CustomerName = context.CustomerLoginDetails.Where(x => x.GroupId == GroupId).Select(y => y.UserName).FirstOrDefault();
            }
            return CustomerName;
        }

        public tblGroupDetail GetGroupDetails(int GroupId)
        {
            tblGroupDetail objGroupDetail = new tblGroupDetail();
            using (var context = new CommonDBContext())
            {
                objGroupDetail = context.tblGroupDetails.Where(x => x.GroupId == GroupId).FirstOrDefault();
            }
            return objGroupDetail;
        }

        public tblModulesPayment GetModulesAndPayments(int GroupId)
        {
            tblModulesPayment objModulesPayment = new tblModulesPayment();
            using (var context = new CommonDBContext())
            {
                objModulesPayment = context.tblModulesPayments.Where(x => x.GroupId == GroupId).FirstOrDefault();
            }
            return objModulesPayment;
        }

        public int AddGroupDetails(tblGroupDetail objGroupDetails)
        {
            int GroupId = 0;
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

            return GroupId;
        }

        public bool AddModulesAndPayments(tblModulesPayment objModulesPayment)
        {
            bool status = false;
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
            return status;
        }

        public bool AddBrandAndOutlet(string GroupId, List<BrandDetail> lstBrand, List<OutletDetail> lstOutlet)
        {
            bool status = false;
            string ConnectionString = string.Empty;
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
            return status;

        }

        public List<BrandDetail> GetAllBrandsByGroupId(string GroupId)
        {
            List<BrandDetail> lstBrands = new List<BrandDetail>();
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
            return lstBrands;
        }

        public List<OutletDetail> GetAllOutletsByGroupId(string GroupId)
        {
            List<OutletDetail> lstOutlets = new List<OutletDetail>();
            string ConnectionString = string.Empty;
            using (var context = new CommonDBContext())
            {
                var DBDetails = context.DatabaseDetails.Where(x => x.GroupId == GroupId).FirstOrDefault();
                ConnectionString = "Data Source = " + DBDetails.IPAddress + "; Initial Catalog = " + DBDetails.DBName + "; user id = " + DBDetails.DBId + "; password = " + DBDetails.DBPassword + "";
            }
            using (var contextNew = new BOTSDBContext(ConnectionString))
            {
                lstOutlets = contextNew.OutletDetails.Where(x => x.GroupId == GroupId).ToList();
            }
            return lstOutlets;
        }

        public ProfilePage GetprofilePageData(string GroupId)
        {
            ProfilePage objprofilePage = new ProfilePage();
            string connectionString = GetCustomerConnString(GroupId);
            tblGroupDetail objgrpdetails = new tblGroupDetail();
            using (var context = new CommonDBContext())
            {
                objgrpdetails = context.tblGroupDetails.Where(x => x.GroupId == Convert.ToInt32(GroupId)).FirstOrDefault();

            }
            SMSDetail objsMSDetail = new SMSDetail();
            using (var Context = new BOTSDBContext(connectionString))
            {
                var objsms = Context.SMSDetails;
                foreach(var sms in objsms)
                {
                   if(sms.SMSGatewayId == "2")
                   {
                        if(!string.IsNullOrEmpty(sms.WhatsappTokenId))
                        {

                        }
                        if(!string.IsNullOrEmpty(sms.TxnUserName) && !string.IsNullOrEmpty(sms.TxnPassword))
                        {

                        }
                   }
                   else
                   {

                   }
                }

            }

            return objprofilePage;
        }

    }
}
