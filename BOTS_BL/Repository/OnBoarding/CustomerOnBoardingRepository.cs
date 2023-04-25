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
                newexception.AddException(ex, "GetTblCategories");
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
                newexception.AddException(ex, "AddCategory");
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
                newexception.AddException(ex, "GetTblSourceBy");
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
                newexception.AddException(ex, "AddSource");
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
                newexception.AddException(ex, "GetTblSourceType");
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
                newexception.AddException(ex, "AddSourceType");
            }
            return result;
        }

        public tblSourceType GetSourceTypeById(int SourceTypeId)
        {
            tblSourceType objSourcetype = new tblSourceType();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objSourcetype = context.tblSourceTypes.Where(x => x.SourceTypeId == SourceTypeId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetSourceTypeById");
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
                newexception.AddException(ex, "GetCityList");
            }
            return objtblcity;
        }
        public List<ChannelPartnerDetails> GetChannelPartnerList()
        {
            List<ChannelPartnerDetails> objchannelpartner = new List<ChannelPartnerDetails>();
            try
            {
                using (var context = new CommonDBContext())
                {

                    objchannelpartner = (from c in context.tblChannelPartners
                                  join cl in context.CustomerLoginDetails on c.CreatedBy equals cl.LoginId into channelpartner
                                  from m in channelpartner.DefaultIfEmpty()
                                  select new ChannelPartnerDetails
                                  {
                                      ChannelPartnerId = c.CPId,
                                      ChannelPartnerName = c.CPartnerName,
                                      CreatedBy = c.CreatedBy,
                                      CreatedDate = c.CreatedDate,
                                      UserName = m.UserName,
                                      IsActive = c.IsActive
                                  }).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetChannelPartnerList");
            }
            return objchannelpartner;
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
                newexception.AddException(ex, "AddCity");
            }
            return result;
        }
        public SPResponse AddChannelPartner(tblChannelPartner objtblchannel)
        {
            SPResponse result = new SPResponse();
            try
            {
                using (var context = new CommonDBContext())
                {
                    context.tblChannelPartners.AddOrUpdate(objtblchannel);
                    context.SaveChanges();
                    result.ResponseCode = "00";
                    result.ResponseMessage = "Channel Partner Added Successfully";
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AddChannelPartner");
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
                newexception.AddException(ex, "GetRMList");
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
                newexception.AddException(ex, "AddRM");
            }
            return result;
        }

        public tblCategory GetCategoryById(int CategoryId)
        {
            tblCategory objcategory = new tblCategory();
            try
            {
               using (var context = new CommonDBContext())
               {
                  objcategory = context.tblCategories.Where(x => x.CategoryId == CategoryId).FirstOrDefault();
               }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCategoryById");
            }
            return objcategory;
        }

        public tblCity GetCityById(int CityId)
        {
            tblCity objcity = new tblCity();
            try
            { 
            using (var context = new CommonDBContext())
              {
                objcity = context.tblCities.Where(x => x.CityId == CityId).FirstOrDefault();
              }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCityById");
            }
            return objcity;
        }

        public tblChannelPartner GetchannelById(int ChannelPartnerid)
        {
            tblChannelPartner objchannel = new tblChannelPartner();
            try 
            {
                using (var context = new CommonDBContext())
                {
                    objchannel = context.tblChannelPartners.Where(x => x.CPId == ChannelPartnerid).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetchannelById");
            }
            return objchannel;
        }

        public tblRMAssigned GetRMById(int RMId)
        {
            tblRMAssigned objRM = new tblRMAssigned();
            try 
            {
                using (var context = new CommonDBContext())
                {
                    objRM = context.tblRMAssigneds.Where(x => x.RMAssignedId == RMId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetRMById");
            }
            return objRM;
        }

        public tblSourcedBy GetSourceById(int SourceId)
        {
            tblSourcedBy objsource = new tblSourcedBy();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objsource = context.tblSourcedBies.Where(x => x.SourcedbyId == SourceId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetSourceById");
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
                newexception.AddException(ex, "AddBillingPartner");
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
                newexception.AddException(ex, "AddBillingPartnerProduct");
            }
            return result;
        }

        public tblBillingPartner GetBillingPartnerById(int BillingpartnerId)
        {
            tblBillingPartner objbillingpartner = new tblBillingPartner();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objbillingpartner = context.tblBillingPartners.Where(x => x.BillingPartnerId == BillingpartnerId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetBillingPartnerById");
            }
            return objbillingpartner;
        }
        //for edit
        public BOTS_TblBillingPartnerProduct GetBillingPartnerProductByProductId(int BillingpartnerProductId)
        {
            BOTS_TblBillingPartnerProduct objbillingpartner = new BOTS_TblBillingPartnerProduct();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objbillingpartner = context.BOTS_TblBillingPartnerProduct.Where(x => x.BillingPartnerProductId == BillingpartnerProductId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetBillingPartnerProductByProductId");
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
                newexception.AddException(ex, "GetBillingPartnerList");
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
                newexception.AddException(ex, "GetBillingPartner");
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
                newexception.AddException(ex, "GetChannelPartner");
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
                newexception.AddException(ex, "GetBillingPartnerProductById");
            }
            return objbillingpartnerproduct;
        }

        public SPResponse ActiveInactiveBillingPartner(int BillingpartnerId)
        {
            tblBillingPartner objbillingpartner = new tblBillingPartner();
            SPResponse result = new SPResponse();
            try
            {
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
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ActiveInactiveBillingPartner");
            }
            return result;
        }

        public SPResponse ActiveInactiveCategory(int CategoryId)
        {
            tblCategory objcategory = new tblCategory();
            SPResponse result = new SPResponse();
            try
            {
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
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ActiveInactiveCategory");
            }
            return result;
        }

        public SPResponse ActiveInactiveCity(int CityId)
        {
            tblCity objcity = new tblCity();
            SPResponse result = new SPResponse();
            try
            {
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
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ActiveInactiveCity");
            }
            return result;
        }

        public SPResponse ActiveInactiveChannelPartner(int ChannelPartnerId)
        {
            tblChannelPartner objchannel = new tblChannelPartner();
            SPResponse result = new SPResponse();
            try
            {
                using (var context = new CommonDBContext())
                {

                    objchannel = context.tblChannelPartners.Where(x => x.CPId == ChannelPartnerId).FirstOrDefault();

                    if (objchannel.IsActive == true)
                    {
                        objchannel.IsActive = false;
                    }
                    else
                    {
                        objchannel.IsActive = true;
                    }

                    context.tblChannelPartners.AddOrUpdate(objchannel);
                    context.SaveChanges();
                    result.ResponseCode = "00";
                    result.ResponseMessage = "Channel Partner Updated Successfully";
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ActiveInactiveChannelPartner");
            }
            return result;
        }

        public SPResponse ActiveInactiveCS(int RmAssignedId)
        {
            tblRMAssigned objRM = new tblRMAssigned();
            SPResponse result = new SPResponse();
            try
            {
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
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ActiveInactiveCS");
            }
            return result;
        }

        public SPResponse ActiveInactiveSourceBy(int Sourcedbyid)
        {
            tblSourcedBy objsource = new tblSourcedBy();
            SPResponse result = new SPResponse();
            try
            {
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
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ActiveInactiveSourceBy");
            }
            return result;
        }

        public SPResponse ActiveInactiveSourceType(int SourceTypeid)
        {
            tblSourceType objsourcetype = new tblSourceType();
            SPResponse result = new SPResponse();
            try
            {
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
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ActiveInactiveSourceType");
            }
            return result;
        }

        public SPResponse ActiveInactiveBillingPartnerProduct(int BillingPartnerProductId)
        {
            BOTS_TblBillingPartnerProduct objbillingpartnerproduct = new BOTS_TblBillingPartnerProduct();
            SPResponse result = new SPResponse();
            try
            {
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
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ActiveInactiveBillingPartnerProduct");
            }
            return result;
        }

    }
}
