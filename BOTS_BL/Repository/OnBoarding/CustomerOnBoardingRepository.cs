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
                                      join cl in context.CustomerLoginDetails on c.CreatedBy equals cl.LoginId into category from m in category.DefaultIfEmpty()
                                      select new CategoryDetails
                                      {
                                          CategoryId = c.CategoryId,
                                          CategoryName = c.CategoryName,
                                          CreatedBy = c.CreatedBy,
                                          CreatedDate = c.CreatedDate,
                                          UserName = m.UserName
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
                                      join cl in context.CustomerLoginDetails on s.CreatedBy equals cl.LoginId into source from m in source.DefaultIfEmpty()
                                      select new SourcedDetails
                                      {
                                          SourcedbyId = s.SourcedbyId,
                                          SourcedbyName = s.SourcedbyName,
                                          CreatedBy = s.CreatedBy,
                                          CreatedDate = s.CreatedDate,
                                          UserName = m.UserName
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

        public List<CityDetails> GetCityList()
        {
            List<CityDetails> objtblcity = new List<CityDetails>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    
                    objtblcity = (from c in context.tblCities
                                      join cl in context.CustomerLoginDetails on c.CreatedBy equals cl.LoginId into city from m in city.DefaultIfEmpty()
                                      select new CityDetails
                                      {
                                          CityId = c.CityId,
                                          CityName = c.CityName,
                                          CreatedBy = c.CreatedBy,
                                          CreatedDate = c.CreatedDate,
                                          UserName = m.UserName
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
                                  join cl in context.CustomerLoginDetails on r.CreatedBy equals cl.LoginId into RMAssigned from m in RMAssigned.DefaultIfEmpty()
                                    select new RMAssignedDetails
                                  {
                                      RMAssignedId = r.RMAssignedId,
                                      RMAssignedName = r.RMAssignedName,
                                      CreatedBy = r.CreatedBy,
                                      CreatedDate = r.CreatedDate,
                                      UserName = m.UserName
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

        public tblBillingPartner GetBillingPartnerById(int BillingpartnerId)
        {
            tblBillingPartner objbillingpartner = new tblBillingPartner();
            using (var context = new CommonDBContext())
            {
                objbillingpartner = context.tblBillingPartners.Where(x => x.BillingPartnerId == BillingpartnerId).FirstOrDefault();
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
                                        UserName = p.UserName
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
                                                from p in billingpartner.DefaultIfEmpty()
                                                select new BillingPartnerDetails
                                                {
                                                    BillingPartnerId = r.BillingPartnerId,
                                                    BillingPartnerName = r.BillingPartnerName,
                                                    CreatedBy = r.CreatedBy,
                                                    CreatedDate = r.CreatedDate,
                                                    UserName = p.UserName
                                                }).ToList();

                    foreach (var item in lstbillingpartner)
                    {
                        BillingPartnerItem.Add(new SelectListItem
                        {
                            Text = item.BillingPartnerName,
                            Value = Convert.ToString(item.BillingPartnerId)
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "onboarding_master");
            }
            return BillingPartnerItem;
        }

        public List<BOTS_TblBillingPartnerProduct> GetBillingPartnerProductById(int BillingpartnerId)
        {
            List<BOTS_TblBillingPartnerProduct> objbillingpartnerproduct = new List<BOTS_TblBillingPartnerProduct>();
            using (var context = new CommonDBContext())
            {
                objbillingpartnerproduct = context.BOTS_TblBillingPartnerProduct.Where(x => x.BillingPartnerId == BillingpartnerId).ToList();
                    
               
                //objbillingpartnerproduct = (from r in context.BOTS_TblBillingPartnerProduct                                           
                //                            join cl in context.CustomerLoginDetails on r.CreatedBy equals cl.LoginId
                //                            where r.BillingPartnerId == BillingpartnerId
                //                            select new BOTS_TblBillingPartnerProduct
                //                            {
                //                                BillingPartnerId = r.BillingPartnerId,
                //                                BillingPartnerProductId = r.BillingPartnerProductId,
                //                                BillingPartnerProductName = r.BillingPartnerProductName,
                //                                CreatedBy = r.CreatedBy,
                //                                CreatedDate = r.CreatedDate,
                //                                UserName = cl.UserName
                //                            }).ToList();


                //objbillingpartnerproduct = (from r in context.CustomerLoginDetails
                //                            join cl in context.BOTS_TblBillingPartnerProduct.Where(q => q.BillingPartnerId == BillingpartnerId)
                //                            on r.LoginId equals cl.CreatedBy into billingpartnerproduct
                //                            from p in billingpartnerproduct.DefaultIfEmpty()                                            
                //                            select new BOTS_TblBillingPartnerProduct
                //                            {
                //                                BillingPartnerId = p.BillingPartnerId,
                //                                BillingPartnerProductId = p.BillingPartnerProductId,
                //                                BillingPartnerProductName = p.BillingPartnerProductName,
                //                                CreatedBy = p.CreatedBy,
                //                                CreatedDate = p.CreatedDate,
                //                                UserName = r.UserName
                //                            }).ToList();

            }
           
            return objbillingpartnerproduct;
        }

    }
}
