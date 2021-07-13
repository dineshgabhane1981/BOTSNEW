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
using System.Data.Entity.Core.EntityClient;
using System.Configuration;

namespace BOTS_BL.Repository
{
    public class LoginRepository
    {
        public CustomerLoginDetail AuthenticateUser(LoginModel objLoginModel)
        {
            DatabaseDetail DBDetails = new DatabaseDetail();
            CustomerLoginDetail userDetail = new CustomerLoginDetail();
            using (var context = new CommonDBContext())
            {
                userDetail = context.CustomerLoginDetails.Where(a => a.LoginId == objLoginModel.LoginId && a.Password == objLoginModel.Password).FirstOrDefault();

                if (userDetail != null)
                {
                    if (userDetail.GroupId != null)
                    {
                        DBDetails = context.DatabaseDetails.Where(x => x.GroupId == userDetail.GroupId).FirstOrDefault();
                        //CustomerConnString.ConnectionStringCustomer = DBDetails.DBName;

                        userDetail.connectionString = "Data Source = " + DBDetails.IPAddress + "; Initial Catalog = " + DBDetails.DBName + "; user id = " + DBDetails.DBId + "; password = " + DBDetails.DBPassword + "";
                        
                    }
                }
            }
            return userDetail;

        }

        public string AuthenticateUserMobile(string LoginId, string Password)
        {
            DatabaseDetail DBDetails = new DatabaseDetail();
            CustomerLoginDetail userDetail = new CustomerLoginDetail();
            using (var context = new CommonDBContext())
            {
                userDetail = context.CustomerLoginDetails.Where(a => a.LoginId == LoginId && a.Password == Password).FirstOrDefault();
            }
            return userDetail.GroupId;

        }

        public CustomerLoginDetail AuthenticateUserAPI(string LoginId, string Password)
        {
            DatabaseDetail DBDetails = new DatabaseDetail();
            CustomerLoginDetail userDetail = new CustomerLoginDetail();
            using (var context = new CommonDBContext())
            {
                userDetail = context.CustomerLoginDetails.Where(a => a.LoginId == LoginId && a.Password == Password).FirstOrDefault();
            }
            return userDetail;

        }

        public void AddException(tblErrorLog objerrorlog)
        {
            try
            {
                using (var contextCommon = new CommonDBContext())
                {
                    contextCommon.tblErrorLogs.Add(objerrorlog);
                    contextCommon.SaveChanges();

                }
            }
            catch(Exception ex)
            {

            }
        }

        public string CheckUserType(string LoginId)
        {             
            CustomerLoginDetail userDetail = new CustomerLoginDetail();
            using (var context = new CommonDBContext())
            {
                userDetail = context.CustomerLoginDetails.Where(a => a.LoginId == LoginId).FirstOrDefault();
            }
            return userDetail.LoginType;

        }
        public CustomerLoginDetail GetUserDetailsByLoginID(string LoginId)
        {
            CustomerLoginDetail userDetail = new CustomerLoginDetail();
            DatabaseDetail DBDetails = new DatabaseDetail();
            using (var context = new CommonDBContext())
            {
                userDetail = context.CustomerLoginDetails.Where(a => a.LoginId == LoginId).FirstOrDefault();
                if (userDetail != null)
                {
                    if (userDetail.GroupId != null)
                    {
                        DBDetails = context.DatabaseDetails.Where(x => x.GroupId == userDetail.GroupId).FirstOrDefault();
                        //CustomerConnString.ConnectionStringCustomer = DBDetails.DBName;

                        userDetail.connectionString = "Data Source = " + DBDetails.IPAddress + "; Initial Catalog = " + DBDetails.DBName + "; user id = " + DBDetails.DBId + "; password = " + DBDetails.DBPassword + "";

                    }
                }
            }
            return userDetail;

        }

    }

    

}

