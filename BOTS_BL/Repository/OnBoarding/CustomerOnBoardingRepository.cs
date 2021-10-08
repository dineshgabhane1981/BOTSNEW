using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL.Models.CommonDB;


namespace BOTS_BL.Repository
{
    public class CustomerOnBoardingRepository
    {
        Exceptions newexception = new Exceptions();

        public List<CategoryDetails> GetTblCategories()
        {
            List<CategoryDetails> objtblcategory = new List<CategoryDetails>();
            try
            {
                using (var context = new CommonDBContext())
                {

                    objtblcategory = (from c in context.tblCategories
                                      join cl in context.CustomerLoginDetails on c.CreatedBy equals cl.LoginId into category
                                      from m in category.DefaultIfEmpty()
                                      select new CategoryDetails
                                      {
                                          CategoryId = c.CategoryId,
                                          CategoryName = c.CategoryName,
                                          CreatedBy = c.CreatedBy,
                                          CreatedDate = c.CreatedDate,
                                          UserName = m.UserName,
                                          IsActive = c.IsActive
                                      }).ToList();

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "onboarding_master");
            }
            return objtblcategory;
        }

        public SPResponse AddCategory(tblCategory objtblcategory)
        {
            SPResponse result = new SPResponse();
            try
            {
                using (var context = new CommonDBContext())
                {
                    context.tblCategories.AddOrUpdate(objtblcategory);
                    context.SaveChanges();
                    result.ResponseCode = "00";
                    result.ResponseMessage = "Category Added Successfully";
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "onboarding_master");
            }
            return result;
        }

        public List<SourcedDetails> GetTblSourceBy()
        {
            List<SourcedDetails> objtblSourceBy = new List<SourcedDetails>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objtblSourceBy = (from s in context.tblSourcedBies
                                      join cl in context.CustomerLoginDetails on s.CreatedBy equals cl.LoginId into source
                                      from m in source.DefaultIfEmpty()
                                      select new SourcedDetails
                                      {
                                          SourcedbyId = s.SourcedbyId,
                                          SourcedbyName = s.SourcedbyName,
                                          CreatedBy = s.CreatedBy,
                                          CreatedDate = s.CreatedDate,
                                          UserName = m.UserName,
                                          IsActive = s.IsActive
                                      }).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "onboarding_master");
            }
            return objtblSourceBy;
        }

        public SPResponse AddSource(tblSourcedBy objtblSourceBy)
        {
            SPResponse result = new SPResponse();
            try
            {
                using (var context = new CommonDBContext())
                {
                    context.tblSourcedBies.AddOrUpdate(objtblSourceBy);
                    context.SaveChanges();
                    result.ResponseCode = "00";
                    result.ResponseMessage = "Source Added Successfully";
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "onboarding_master");
            }
            return result;
        }

        public List<SourcedTypeDetails> GetTblSourceType()
        {
            List<SourcedTypeDetails> objtblSourcetype = new List<SourcedTypeDetails>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objtblSourcetype = (from s in context.tblSourceTypes
                                        join cl in context.CustomerLoginDetails on s.CreatedBy equals cl.LoginId into source
                                        from m in source.DefaultIfEmpty()
                                        select new SourcedTypeDetails
                                        {
                                            SourceTypeId = s.SourceTypeId,
                                            SourceTypeName = s.SourceTypeName,
                                            CreatedBy = s.CreatedBy,
                                            CreatedDate = s.CreatedDate,
                                            UserName = m.UserName,
                                            IsActive = s.IsActive
                                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "onboarding_master");
            }
            return objtblSourcetype;
        }

        public SPResponse AddSourceType(tblSourceType objtblSourceType)
        {
            SPResponse result = new SPResponse();
            try
            {
                using (var context = new CommonDBContext())
                {
                    context.tblSourceTypes.AddOrUpdate(objtblSourceType);
                    context.SaveChanges();
                    result.ResponseCode = "00";
                    result.ResponseMessage = "Source Type Added Successfully";
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "onboarding_master");
            }
            return result;
        }

        public tblSourceType GetSourceTypeById(int SourceTypeId)
        {
            tblSourceType objSourcetype = new tblSourceType();
            using (var context = new CommonDBContext())
            {
                objSourcetype = context.tblSourceTypes.Where(x => x.SourceTypeId == SourceTypeId).FirstOrDefault();
            }
            return objSourcetype;
        }

        public List<CityDetails> GetCityList()
        {
            List<CityDetails> objtblcity = new List<CityDetails>();
            try
            {
                using (var context = new CommonDBContext())
                {

                    objtblcity = (from c in context.tblCities
                                  join cl in context.CustomerLoginDetails on c.CreatedBy equals cl.LoginId into city
                                  from m in city.DefaultIfEmpty()
                                  select new CityDetails
                                  {
                                      CityId = c.CityId,
                                      CityName = c.CityName,
                                      CreatedBy = c.CreatedBy,
                                      CreatedDate = c.CreatedDate,
                                      UserName = m.UserName,
                                      IsActive = c.IsActive
                                  }).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "onboarding_master");
            }
            return objtblcity;
        }

        public SPResponse AddCity(tblCity objtblcity)
        {
            SPResponse result = new SPResponse();
            try
            {
                using (var context = new CommonDBContext())
                {
                    context.tblCities.AddOrUpdate(objtblcity);
                    context.SaveChanges();
                    result.ResponseCode = "00";
                    result.ResponseMessage = "City Added Successfully";
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "onboarding_master");
            }
            return result;
        }

        public List<RMAssignedDetails> GetRMList()
        {
            List<RMAssignedDetails> objRMDetails = new List<RMAssignedDetails>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objRMDetails = (from r in context.tblRMAssigneds
                                    join cl in context.CustomerLoginDetails on r.CreatedBy equals cl.LoginId into RMAssigned
                                    from m in RMAssigned.DefaultIfEmpty()
                                    select new RMAssignedDetails
                                    {
                                        RMAssignedId = r.RMAssignedId,
                                        RMAssignedName = r.RMAssignedName,
                                        CreatedBy = r.CreatedBy,
                                        CreatedDate = r.CreatedDate,
                                        UserName = m.UserName,
                                        IsActive = r.IsActive
                                    }).ToList();



                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "onboarding_master");
            }
            return objRMDetails;
        }

        public SPResponse AddRM(tblRMAssigned objtblRMAssigned)
        {
            SPResponse result = new SPResponse();
            try
            {
                using (var context = new CommonDBContext())
                {
                    context.tblRMAssigneds.AddOrUpdate(objtblRMAssigned);
                    context.SaveChanges();
                    result.ResponseCode = "00";
                    result.ResponseMessage = "Customer Success Added Successfully";
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "onboarding_master");
            }
            return result;
        }

        public tblCategory GetCategoryById(int CategoryId)
        {
            tblCategory objcategory = new tblCategory();
            using (var context = new CommonDBContext())
            {
                objcategory = context.tblCategories.Where(x => x.CategoryId == CategoryId).FirstOrDefault();
            }
            return objcategory;
        }

        public tblCity GetCityById(int CityId)
        {
            tblCity objcity = new tblCity();
            using (var context = new CommonDBContext())
            {
                objcity = context.tblCities.Where(x => x.CityId == CityId).FirstOrDefault();
            }
            return objcity;
        }

        public tblRMAssigned GetRMById(int RMId)
        {
            tblRMAssigned objRM = new tblRMAssigned();
            using (var context = new CommonDBContext())
            {
                objRM = context.tblRMAssigneds.Where(x => x.RMAssignedId == RMId).FirstOrDefault();
            }
            return objRM;
        }

        public tblSourcedBy GetSourceById(int SourceId)
        {
            tblSourcedBy objsource = new tblSourcedBy();
            using (var context = new CommonDBContext())
            {
                objsource = context.tblSourcedBies.Where(x => x.SourcedbyId == SourceId).FirstOrDefault();
            }
            return objsource;
        }

        public SPResponse AddBillingPartner(tblBillingPartner objtblbillingpartner)
        {
            SPResponse result = new SPResponse();
            try
            {
                using (var context = new CommonDBContext())
                {
                    context.tblBillingPartners.AddOrUpdate(objtblbillingpartner);
                    context.SaveChanges();
                    result.ResponseCode = "00";
                    result.ResponseMessage = "Billing Partner Added Successfully";
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "onboarding_master");
            }
            return result;
        }

        public SPResponse AddBillingPartnerProduct(BOTS_TblBillingPartnerProduct objtblbillingpartnerproduct)
        {
            SPResponse result = new SPResponse();
            try
            {
                using (var context = new CommonDBContext())
                {
                    context.BOTS_TblBillingPartnerProduct.AddOrUpdate(objtblbillingpartnerproduct);
                    context.SaveChanges();
                    result.ResponseCode = "00";
                    result.ResponseMessage = "Billing Partner Product Added Successfully";
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "onboarding_master");
            }
            return result;
        }

        public tblBillingPartner GetBillingPartnerById(int BillingpartnerId)
        {
            tblBillingPartner objbillingpartner = new tblBillingPartner();
            using (var context = new CommonDBContext())
            {
                objbillingpartner = context.tblBillingPartners.Where(x => x.BillingPartnerId == BillingpartnerId).FirstOrDefault();
            }
            return objbillingpartner;
        }
        //for edit
        public BOTS_TblBillingPartnerProduct GetBillingPartnerProductByProductId(int BillingpartnerProductId)
        {
            BOTS_TblBillingPartnerProduct objbillingpartner = new BOTS_TblBillingPartnerProduct();
            using (var context = new CommonDBContext())
            {
                objbillingpartner = context.BOTS_TblBillingPartnerProduct.Where(x => x.BillingPartnerProductId == BillingpartnerProductId).FirstOrDefault();
            }
            return objbillingpartner;
        }

        public List<BillingPartnerDetails> GetBillingPartnerList()
        {
            List<BillingPartnerDetails> objbillingpartnerDetails = new List<BillingPartnerDetails>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objbillingpartnerDetails = (from r in context.tblBillingPartners
                                                join cl in context.CustomerLoginDetails on r.CreatedBy equals cl.LoginId into billingpartner
                                                from p in billingpartner.DefaultIfEmpty()
                                                select new BillingPartnerDetails
                                                {
                                                    BillingPartnerId = r.BillingPartnerId,
                                                    BillingPartnerName = r.BillingPartnerName,
                                                    CreatedBy = r.CreatedBy,
                                                    CreatedDate = r.CreatedDate,
                                                    UserName = p.UserName,
                                                    IsActive = r.IsActive
                                                }).ToList();



                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "onboarding_master");
            }
            return objbillingpartnerDetails;
        }

        public List<SelectListItem> GetBillingPartner()
        {
            List<SelectListItem> BillingPartnerItem = new List<SelectListItem>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    var lstbillingpartner = (from r in context.tblBillingPartners
                                             join cl in context.CustomerLoginDetails on r.CreatedBy equals cl.LoginId into billingpartner
                                             where r.IsActive == true
                                             select new BillingPartnerDetails
                                             {
                                                 BillingPartnerId = r.BillingPartnerId,
                                                 BillingPartnerName = r.BillingPartnerName,
                                                 CreatedBy = r.CreatedBy,
                                                 CreatedDate = r.CreatedDate,
                                                 IsActive = r.IsActive
                                             }).ToList();

                    foreach (var item in lstbillingpartner)
                    {
                        BillingPartnerItem.Add(new SelectListItem
                        {
                            Text = item.BillingPartnerName,
                            Value = Convert.ToString(item.BillingPartnerId)
                        });

                    }
                    var Billingselect = new SelectListItem()
                    {
                        Value = "0",
                        Text = "--Select Billing Partner--"
                    };
                    BillingPartnerItem.Insert(0, Billingselect);

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "onboarding_master");
            }
            return BillingPartnerItem;
        }


        public List<SelectListItem> GetChannelPartner()
        {
            List<SelectListItem> lstChannelPartner = new List<SelectListItem>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    var lstchannelpartner = (from r in context.tblChannelPartners
                                             join cl in context.CustomerLoginDetails on r.CreatedBy equals cl.LoginId into channelpartner
                                             where r.IsActive == true
                                             select new ChannelPartnerDetails
                                             {
                                                 ChannelPartnerId = r.CPId,
                                                 ChannelPartnerName = r.CPartnerName,
                                                 CreatedBy = r.CreatedBy,
                                                 CreatedDate = r.CreatedDate,
                                                 IsActive = r.IsActive
                                             }).ToList();

                    foreach (var item in lstchannelpartner)
                    {
                        lstChannelPartner.Add(new SelectListItem
                        {
                            Text = item.ChannelPartnerName,
                            Value = Convert.ToString(item.ChannelPartnerId)
                        });

                    }
                    var Billingselect = new SelectListItem()
                    {
                        Value = "0",
                        Text = "--Select Billing Partner--"
                    };
                    lstChannelPartner.Insert(0, Billingselect);

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "onboarding_master");
            }
            return lstChannelPartner;
        }



        //togetallproduct
        public List<BillingPartnerProductDetails> GetBillingPartnerProductById(int BillingpartnerId)
        {
            List<BillingPartnerProductDetails> objbillingpartnerproduct = new List<BillingPartnerProductDetails>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objbillingpartnerproduct = (from r in context.BOTS_TblBillingPartnerProduct
                                                join cl in context.CustomerLoginDetails on r.CreatedBy equals cl.LoginId
                                                where r.BillingPartnerId == BillingpartnerId
                                                select new BillingPartnerProductDetails
                                                {
                                                    BillingPartnerId = r.BillingPartnerId,
                                                    BillingPartnerProductId = r.BillingPartnerProductId,
                                                    BillingPartnerProductName = r.BillingPartnerProductName,
                                                    CreatedBy = r.CreatedBy,
                                                    CreatedDate = r.CreatedDate,
                                                    IsActive =r.IsActive,
                                                    UserName = cl.UserName
                                                }).ToList();

                    foreach (var item in objbillingpartnerproduct)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(item.CreatedDate)))
                            item.CreatedDateStr = item.CreatedDate.ToString("yyyy-MM-dd");
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objbillingpartnerproduct;
        }

        public SPResponse ActiveInactiveBillingPartner(int BillingpartnerId)
        {
            tblBillingPartner objbillingpartner = new tblBillingPartner();
            SPResponse result = new SPResponse();
            using (var context = new CommonDBContext())
            {

                objbillingpartner = context.tblBillingPartners.Where(x => x.BillingPartnerId == BillingpartnerId).FirstOrDefault();

                if (objbillingpartner.IsActive == true)
                {
                    objbillingpartner.IsActive = false;
                }
                else
                {
                    objbillingpartner.IsActive = true;
                }

                context.tblBillingPartners.AddOrUpdate(objbillingpartner);
                context.SaveChanges();
                result.ResponseCode = "00";
                result.ResponseMessage = "Billing Partner Updated Successfully";
            }
            return result;
        }

        public SPResponse ActiveInactiveCategory(int CategoryId)
        {
            tblCategory objcategory = new tblCategory();
            SPResponse result = new SPResponse();
            using (var context = new CommonDBContext())
            {

                objcategory = context.tblCategories.Where(x => x.CategoryId == CategoryId).FirstOrDefault();

                if (objcategory.IsActive == true)
                {
                    objcategory.IsActive = false;
                }
                else
                {
                    objcategory.IsActive = true;
                }

                context.tblCategories.AddOrUpdate(objcategory);
                context.SaveChanges();
                result.ResponseCode = "00";
                result.ResponseMessage = "Category Updated Successfully";
            }
            return result;
        }

        public SPResponse ActiveInactiveCity(int CityId)
        {
            tblCity objcity = new tblCity();
            SPResponse result = new SPResponse();
            using (var context = new CommonDBContext())
            {

                objcity = context.tblCities.Where(x => x.CityId == CityId).FirstOrDefault();

                if (objcity.IsActive == true)
                {
                    objcity.IsActive = false;
                }
                else
                {
                    objcity.IsActive = true;
                }

                context.tblCities.AddOrUpdate(objcity);
                context.SaveChanges();
                result.ResponseCode = "00";
                result.ResponseMessage = "City Updated Successfully";
            }
            return result;
        }

        public SPResponse ActiveInactiveCS(int RmAssignedId)
        {
            tblRMAssigned objRM = new tblRMAssigned();
            SPResponse result = new SPResponse();
            using (var context = new CommonDBContext())
            {

                objRM = context.tblRMAssigneds.Where(x => x.RMAssignedId == RmAssignedId).FirstOrDefault();

                if (objRM.IsActive == true)
                {
                    objRM.IsActive = false;
                }
                else
                {
                    objRM.IsActive = true;
                }

                context.tblRMAssigneds.AddOrUpdate(objRM);
                context.SaveChanges();
                result.ResponseCode = "00";
                result.ResponseMessage = "CS Updated Successfully";
            }
            return result;
        }

        public SPResponse ActiveInactiveSourceBy(int Sourcedbyid)
        {
            tblSourcedBy objsource = new tblSourcedBy();
            SPResponse result = new SPResponse();
            using (var context = new CommonDBContext())
            {

                objsource = context.tblSourcedBies.Where(x => x.SourcedbyId == Sourcedbyid).FirstOrDefault();

                if (objsource.IsActive == true)
                {
                    objsource.IsActive = false;
                }
                else
                {
                    objsource.IsActive = true;
                }

                context.tblSourcedBies.AddOrUpdate(objsource);
                context.SaveChanges();
                result.ResponseCode = "00";
                result.ResponseMessage = "SourceBy Updated Successfully";
            }
            return result;
        }

        public SPResponse ActiveInactiveSourceType(int SourceTypeid)
        {
            tblSourceType objsourcetype = new tblSourceType();
            SPResponse result = new SPResponse();
            using (var context = new CommonDBContext())
            {

                objsourcetype = context.tblSourceTypes.Where(x => x.SourceTypeId == SourceTypeid).FirstOrDefault();

                if (objsourcetype.IsActive == true)
                {
                    objsourcetype.IsActive = false;
                }
                else
                {
                    objsourcetype.IsActive = true;
                }

                context.tblSourceTypes.AddOrUpdate(objsourcetype);
                context.SaveChanges();
                result.ResponseCode = "00";
                result.ResponseMessage = "Source Type Updated Successfully";
            }
            return result;
        }

        public SPResponse ActiveInactiveBillingPartnerProduct(int BillingPartnerProductId)
        {
            BOTS_TblBillingPartnerProduct objbillingpartnerproduct = new BOTS_TblBillingPartnerProduct();
            SPResponse result = new SPResponse();
            using (var context = new CommonDBContext())
            {

                objbillingpartnerproduct = context.BOTS_TblBillingPartnerProduct.Where(x => x.BillingPartnerProductId == BillingPartnerProductId).FirstOrDefault();

                if (objbillingpartnerproduct.IsActive == true)
                {
                    objbillingpartnerproduct.IsActive = false;
                }
                else
                {
                    objbillingpartnerproduct.IsActive = true;
                }

                context.BOTS_TblBillingPartnerProduct.AddOrUpdate(objbillingpartnerproduct);
                context.SaveChanges();
                result.ResponseCode = "00";
                result.ResponseMessage = "Billing Partner product Updated Successfully";
            }
            return result;
        }

    }
}
