using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BOTS_BL.Repository
{
  public class DashBoardCustomerLoginRepository
    {
        CustomerRepository objCustRepo = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        public bool AddDashboardCustomerLogin(DashboardCustomerLogin objdashboardlogin)
        {
            bool status = false;
            CustomerLoginDetail objdashboardlogindeatils = new CustomerLoginDetail();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objdashboardlogindeatils.UserId = objdashboardlogin.GroupId;
                    objdashboardlogindeatils.LoginId = objdashboardlogin.MobileNo;
                    objdashboardlogindeatils.Password = "123";
                    objdashboardlogindeatils.UserName = objdashboardlogin.CustomerName;
                    if (objdashboardlogin.LoginType == "02")
                    {
                        objdashboardlogindeatils.LevelIndicator = "02";
                        objdashboardlogindeatils.CreatedDate = DateTime.UtcNow.Date;
                        objdashboardlogindeatils.LoginStatus = true;
                        objdashboardlogindeatils.UserStatus = true;
                        

                    }
                    else
                    {
                        objdashboardlogindeatils.LevelIndicator = "04";
                        objdashboardlogindeatils.CreatedDate = DateTime.UtcNow.Date;
                        objdashboardlogindeatils.LoginStatus = true;
                        objdashboardlogindeatils.UserStatus = true;
                        objdashboardlogindeatils.OutletOrBrandId = objdashboardlogin.OutletOrBrandId;
                    }
                    objdashboardlogindeatils.GroupId = objdashboardlogin.GroupId;
                    objdashboardlogindeatils.EmailId = null;
                    objdashboardlogindeatils.MobileNo = null;
                    objdashboardlogindeatils.LoginType = null;

                    context.CustomerLoginDetails.AddOrUpdate(objdashboardlogindeatils);
                    context.SaveChanges();

                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, objdashboardlogin.GroupId);
            }

            return status;
        }
        
        public List<SelectListItem> GetLoginType()
        {
            List<SelectListItem> lstlogintype = new List<SelectListItem>();
            using (var context = new CommonDBContext())
            {
                // var RetailCategory = context.tblCategories.ToList();

                 lstlogintype.Add(new SelectListItem { Selected = true, Text = "---Select---", Value = "0" });
                lstlogintype.Add(new SelectListItem { Text = "Admin Login", Value = "Admin" });
                lstlogintype.Add(new SelectListItem { Text = "Outlet Login", Value = "Outlet" });

            }
            return lstlogintype;
        }

        public List<DashboardCustomerLogin> GetDashboardcustomerlogin(string GroupId)
        {
            List<DashboardCustomerLogin> objdashboard = new List<DashboardCustomerLogin>();
          //  string connStr = objCustRepo.GetCustomerConnString(GroupId);
            using (var context = new CommonDBContext())
            {
                objdashboard = (from c in context.CustomerLoginDetails                          
                           where c.GroupId == GroupId
                           select new DashboardCustomerLogin
                           {
                               SlNo = c.SlNo,
                               OutletOrBrandId = c.OutletOrBrandId,
                               CustLoginType= c.OutletOrBrandId !=null ? "OutletWise":"Admin",                        
                               GroupId = c.GroupId,
                               LoginType = c.LoginType,
                               MobileNo = c.MobileNo,
                               LoginStatus = c.LoginStatus,
                               CreatedDate = c.CreatedDate.ToString(),
                               LevelIndicator = c.LevelIndicator,
                               UserName = c.UserName,
                               Password = c.Password,
                               LoginId = c.LoginId,
                               UserId =c.GroupId,
                               CustomerName =c.UserName,

                           }).OrderByDescending(x => x.CreatedDate).ToList();

            }
            return objdashboard;
        }

    }
}
