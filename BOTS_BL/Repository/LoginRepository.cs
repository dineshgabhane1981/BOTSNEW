﻿using System;
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
using BOTS_BL.Common;

namespace BOTS_BL.Repository
{
    public class LoginRepository
    {
        Exceptions newexception = new Exceptions();
        CustomerRepository CR = new CustomerRepository();
        EncryptionDecryption encryptionDecryption = new EncryptionDecryption();
        public CustomerLoginDetail AuthenticateUser(LoginModel objLoginModel)
        {
            DatabaseDetail DBDetails = new DatabaseDetail();
            tblDatabaseDetail tblDatabase = new tblDatabaseDetail();
            CustomerLoginDetail userDetail = new CustomerLoginDetail();
            try
            {
                using (var context = new CommonDBContext())
                {
                    var ss = EncryptionDecryption.EncryptString(objLoginModel.Password);
                    //userDetail = context.CustomerLoginDetails.Where(a => a.LoginId == objLoginModel.LoginId && a.Password == objLoginModel.Password && a.UserStatus.Value == true).FirstOrDefault();
                    userDetail = context.CustomerLoginDetails.Where(a => a.LoginId == objLoginModel.LoginId && a.EncryptedPassword == ss && a.UserStatus.Value == true).FirstOrDefault();

                    if (userDetail != null)
                    {
                        if (userDetail.GroupId != null)
                        {

                            DBDetails = context.DatabaseDetails.Where(x => x.GroupId == userDetail.GroupId).FirstOrDefault();
                            //CustomerConnString.ConnectionStringCustomer = DBDetails.DBName;

                            if (DBDetails == null)
                            {
                                tblDatabase = context.tblDatabaseDetails.Where(x => x.GroupId == userDetail.GroupId).FirstOrDefault();
                                userDetail.connectionString = "Data Source = " + tblDatabase.IPAddress + "; Initial Catalog = " + tblDatabase.DBName + "; user id = " + tblDatabase.DBId + "; password = " + tblDatabase.DBPassword + "";
                            }
                            else
                            {
                                userDetail.connectionString = "Data Source = " + DBDetails.IPAddress + "; Initial Catalog = " + DBDetails.DBName + "; user id = " + DBDetails.DBId + "; password = " + DBDetails.DBPassword + "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AuthenticateUser");

            }
            return userDetail;

        }

        public string AuthenticateUserMobile(string LoginId, string Password)
        {
            DatabaseDetail DBDetails = new DatabaseDetail();
            CustomerLoginDetail userDetail = new CustomerLoginDetail();
            try
            {
                using (var context = new CommonDBContext())
                {
                    userDetail = context.CustomerLoginDetails.Where(a => a.LoginId == LoginId && a.Password == Password).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AuthenticateUserMobile");

            }
            return userDetail.GroupId;

        }

        public CustomerLoginDetail AuthenticateUserAPI(string LoginId, string Password)
        {
            DatabaseDetail DBDetails = new DatabaseDetail();
            CustomerLoginDetail userDetail = new CustomerLoginDetail();
            try
            {
                using (var context = new CommonDBContext())
                {
                    userDetail = context.CustomerLoginDetails.Where(a => a.LoginId == LoginId && a.Password == Password).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AuthenticateUserAPI");

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
            catch (Exception ex)
            {

            }
        }

        public CustomerLoginDetail CheckUserType(string LoginId)
        {
            CustomerLoginDetail userDetail = new CustomerLoginDetail();
            try
            {
                using (var context = new CommonDBContext())
                {
                    userDetail = context.CustomerLoginDetails.Where(a => a.LoginId == LoginId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "CheckUserType");

            }
            return userDetail;

        }
        public CustomerLoginDetail GetUserDetailsByLoginID(string LoginId)
        {
            CustomerLoginDetail userDetail = new CustomerLoginDetail();
            tblDatabaseDetail tblDatabase = new tblDatabaseDetail();
            DatabaseDetail DBDetails = new DatabaseDetail();
            try
            {
                using (var context = new CommonDBContext())
                {
                    userDetail = context.CustomerLoginDetails.Where(a => a.LoginId == LoginId).FirstOrDefault();
                    if (userDetail != null)
                    {
                        if (userDetail.GroupId != null)
                        {
                            DBDetails = context.DatabaseDetails.Where(x => x.GroupId == userDetail.GroupId).FirstOrDefault();
                            //CustomerConnString.ConnectionStringCustomer = DBDetails.DBName;

                            if (DBDetails == null)
                            {
                                tblDatabase = context.tblDatabaseDetails.Where(x => x.GroupId == userDetail.GroupId).FirstOrDefault();
                                userDetail.connectionString = "Data Source = " + tblDatabase.IPAddress + "; Initial Catalog = " + tblDatabase.DBName + "; user id = " + tblDatabase.DBId + "; password = " + tblDatabase.DBPassword + "";
                            }
                            else
                            {
                                userDetail.connectionString = "Data Source = " + DBDetails.IPAddress + "; Initial Catalog = " + DBDetails.DBName + "; user id = " + DBDetails.DBId + "; password = " + DBDetails.DBPassword + "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetUserDetailsByLoginID");

            }
            return userDetail;

        }

        public void AddLoginLog(tblLoginLog objLogData)
        {
            using (var context = new CommonDBContext())
            {
                try
                {
                    context.tblLoginLogs.AddOrUpdate(objLogData);
                    context.SaveChanges();
                }
                catch(Exception ex)
                {
                    newexception.AddException(ex, "Login Error");
                }
            }
        }

    }



}

