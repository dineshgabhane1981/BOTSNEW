using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    context.tblCategories.Add(objtblcategory);
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
                    context.tblSourcedBies.Add(objtblSourceBy);
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
                    context.tblCities.Add(objtblcity);
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
                    context.tblRMAssigneds.Add(objtblRMAssigned);
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

    }
}
