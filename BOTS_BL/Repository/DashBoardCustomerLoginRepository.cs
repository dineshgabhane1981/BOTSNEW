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
                    CustomerLoginDetail objcustexist = new CustomerLoginDetail();
                    objdashboardlogindeatils = context.CustomerLoginDetails.Where(x => x.LoginId == objdashboardlogin.MobileNo && x.GroupId == objdashboardlogin.GroupId).FirstOrDefault();
                    if (objdashboardlogindeatils == null)
                    {

                        objcustexist.UserId = objdashboardlogin.GroupId;
                        objcustexist.LoginId = objdashboardlogin.MobileNo;
                        objcustexist.Password = "123";
                        objcustexist.UserName = objdashboardlogin.CustomerName;
                        if (objdashboardlogin.LoginType == "02")
                        {
                            objcustexist.LevelIndicator = "02";
                            objcustexist.CreatedDate = DateTime.UtcNow.Date;
                            objcustexist.LoginStatus = true;
                            objcustexist.UserStatus = true;


                        }
                        else
                        {
                            objcustexist.LevelIndicator = "04";
                            objcustexist.CreatedDate = DateTime.UtcNow.Date;
                            objcustexist.LoginStatus = true;
                            objcustexist.UserStatus = true;
                            objcustexist.OutletOrBrandId = objdashboardlogin.OutletOrBrandId;
                        }
                        objcustexist.GroupId = objdashboardlogin.GroupId;
                        objcustexist.EmailId = null;
                        objcustexist.MobileNo = null;
                        objcustexist.LoginType = null;

                        context.CustomerLoginDetails.AddOrUpdate(objcustexist);
                        context.SaveChanges();

                        status = true;
                    }
                    else
                    {
                        status = false;
                    }
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
                                    CustLoginType = c.OutletOrBrandId != null ? "OutletWise" : "Admin",
                                    GroupId = c.GroupId,
                                    LoginType = c.LoginType,
                                    MobileNo = c.MobileNo,
                                    LoginStatus = c.LoginStatus,
                                    CreatedDate = c.CreatedDate,
                                    LevelIndicator = c.LevelIndicator,
                                    UserName = c.UserName,
                                    Password = c.Password,
                                    LoginId = c.LoginId,
                                    UserId = c.GroupId,
                                    CustomerName = c.UserName,

                                }).OrderByDescending(x => x.CreatedDate).ToList();

                foreach (var item in objdashboard)
                {
                    item.CreatedDateStr = item.CreatedDate.Value.ToString("MM/dd/yyyy");
                }

            }
            return objdashboard;
        }

        public bool UpdateMaskedValue(int GroupId, string value)
        {
            bool status = false;
            
            using (var context = new CommonDBContext())
            {
                try
                {
                    var groupdetails = context.tblGroupDetails.Where(x => x.GroupId == GroupId).FirstOrDefault();
                    groupdetails.IsMasked = Convert.ToBoolean(value);
                    context.tblGroupDetails.AddOrUpdate(groupdetails);
                    context.SaveChanges();
                    status = true;
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "UpdateMaskedValue");
                }
            }

            return status;
        }
    }
}
