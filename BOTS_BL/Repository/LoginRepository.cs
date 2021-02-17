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

        public void AddException(tblErrorLog objerrorlog)
        {
            using (var context = new CommonDBContext())
            {
                context.tblErrorLogs.Add(objerrorlog);
                context.SaveChanges();

            }


        }

    }

    

}

