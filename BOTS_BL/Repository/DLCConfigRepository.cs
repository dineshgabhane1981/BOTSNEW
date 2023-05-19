using BOTS_BL.Models;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Repository
{
    public class DLCConfigRepository
    {
        CustomerRepository objCustRepo = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        public bool SaveDLCDashboardConfig(string GroupId, tblDLCDashboardConfig objDashboard)
        {
            bool result = false;
            string connStr = objCustRepo.GetCustomerConnString(GroupId);
            try
            {
                using (var context = new BOTSDBContext(connStr))
                {
                    context.tblDLCDashboardConfigs.AddOrUpdate(objDashboard);
                    context.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveDLCDashboardConfig");
            }
            return result;
        }
        public tblDLCDashboardConfig GetDLCDashboardConfig(string GroupId)
        {
            tblDLCDashboardConfig objData = new tblDLCDashboardConfig();
            string connStr = objCustRepo.GetCustomerConnString(GroupId);
            try
            {
                using (var context = new BOTSDBContext(connStr))
                {
                    objData = context.tblDLCDashboardConfigs.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDLCDashboardConfig");
            }

            return objData;
        }
        public bool ProfileDataInsert(string Groupid, string Name, string NameMandStat, string Gender, string GenderMandStat, string BirthDate, string BirthMandStat, string Marrital, string MargMandStat, string Area, string AreaMandStat, string City, string CityMandStat, string Pincode, string PinMandStat, string Email, string MailMandStat)
        {
            bool status = false;
            var connstr = objCustRepo.GetCustomerConnString(Groupid);
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    SqlConnection _Con = new SqlConnection(connstr);
                    DataSet retVal = new DataSet();
                    SqlCommand cmdReport = new SqlCommand("sp_DLCProfileUpdateInsert", _Con);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);

                    using (cmdReport)
                    {
                        SqlParameter param1 = new SqlParameter("pi_Groupid", Groupid);
                        SqlParameter param2 = new SqlParameter("pi_Name", Name);
                        SqlParameter param3 = new SqlParameter("pi_NameMandStat", NameMandStat);
                        SqlParameter param4 = new SqlParameter("pi_Gender", Gender);
                        SqlParameter param5 = new SqlParameter("pi_GenderMandStat", GenderMandStat);
                        SqlParameter param6 = new SqlParameter("pi_BirthDate", BirthDate);
                        SqlParameter param7 = new SqlParameter("pi_BirthMandStat", BirthMandStat);
                        SqlParameter param8 = new SqlParameter("pi_Marrital", Marrital);
                        SqlParameter param9 = new SqlParameter("pi_MargMandStat", MargMandStat);
                        SqlParameter param10 = new SqlParameter("pi_Area", Area);
                        SqlParameter param11 = new SqlParameter("pi_AreaMandStat", AreaMandStat);
                        SqlParameter param12 = new SqlParameter("pi_City", City);
                        SqlParameter param13 = new SqlParameter("pi_CityMandStat", CityMandStat);
                        SqlParameter param14 = new SqlParameter("pi_Pincode", Pincode);
                        SqlParameter param15 = new SqlParameter("pi_PinMandStat", PinMandStat);
                        SqlParameter param16 = new SqlParameter("pi_Email", Email);
                        SqlParameter param17 = new SqlParameter("pi_MailMandStat", MailMandStat);

                        cmdReport.CommandType = CommandType.StoredProcedure;
                        cmdReport.Parameters.Add(param1);
                        cmdReport.Parameters.Add(param2);
                        cmdReport.Parameters.Add(param3);
                        cmdReport.Parameters.Add(param4);
                        cmdReport.Parameters.Add(param5);
                        cmdReport.Parameters.Add(param6);
                        cmdReport.Parameters.Add(param7);
                        cmdReport.Parameters.Add(param8);
                        cmdReport.Parameters.Add(param9);
                        cmdReport.Parameters.Add(param10);
                        cmdReport.Parameters.Add(param11);
                        cmdReport.Parameters.Add(param12);
                        cmdReport.Parameters.Add(param13);
                        cmdReport.Parameters.Add(param14);
                        cmdReport.Parameters.Add(param15);
                        cmdReport.Parameters.Add(param16);
                        cmdReport.Parameters.Add(param17);

                        daReport.Fill(retVal);

                        DataTable dt = retVal.Tables[0];
                        if (Convert.ToString(dt.Rows[0]["Result"]) == "1")
                        {
                            status = true;
                        }
                        else
                        {
                            status = false;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                newexception.AddException(ex, "ProfileDataInsert");
            }


            return status;
        }

        public bool PublishDLCDashboardConfig(CustomerLoginDetail userDetails)
        {
            bool status = false;
            var connstr = objCustRepo.GetCustomerConnString(userDetails.GroupId);
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    var configData = context.tblDLCDashboardConfigs.FirstOrDefault();

                    var configPData = context.tblDLCDashboardConfig_Publish.FirstOrDefault();
                    tblDLCDashboardConfig_Publish objData = new tblDLCDashboardConfig_Publish();
                    if (configPData != null)
                        objData.SlNo = configPData.SlNo;

                    objData.LoginWithOTP = configData.LoginWithOTP;
                    objData.RedirectToPage = configData.RedirectToPage;
                    objData.AddPersonalDetails = configData.AddPersonalDetails;
                    objData.AddGiftPoints = configData.AddGiftPoints;
                    objData.AddReferFriend = configData.AddReferFriend;
                    objData.ShowLogoToFooter = configData.ShowLogoToFooter;
                    objData.CollectPersonalDataRandomly = configData.CollectPersonalDataRandomly;
                    objData.UseLogo = configData.UseLogo;
                    objData.UseLogoURL = configData.UseLogoURL;
                    objData.PrefferedLanguage = configData.PrefferedLanguage;
                    objData.HeaderColor = configData.HeaderColor;
                    objData.AddedBy = userDetails.UserId;
                    objData.AddedDate = DateTime.Now;
                    context.tblDLCDashboardConfig_Publish.AddOrUpdate(objData);
                    context.SaveChanges();

                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "PublishDLCDashboardConfig");
            }
            return status;
        }

        public bool PublishDLCProfileUpdate(string Groupid)
        {
            bool status = false;
            var connstr = objCustRepo.GetCustomerConnString(Groupid);
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    SqlConnection _Con = new SqlConnection(connstr);
                    DataSet retVal = new DataSet();
                    SqlCommand cmdReport = new SqlCommand("sp_DLCProfileUpdatePublish", _Con);
                    SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);

                    using (cmdReport)
                    {
                        SqlParameter param1 = new SqlParameter("pi_Groupid", Groupid);

                        cmdReport.CommandType = CommandType.StoredProcedure;
                        cmdReport.Parameters.Add(param1);

                        daReport.Fill(retVal);

                        DataTable dt = retVal.Tables[0];
                        if (Convert.ToString(dt.Rows[0]["Result"]) == "1")
                        {
                            status = true;
                        }
                        else
                        {
                            status = false;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                newexception.AddException(ex, "PublishDLCProfileUpdate");
            }


            return status;
        }

        public tblDLCDashboardConfig_Publish GetPublishDLCDashboardConfig(string groupId)
        {
            tblDLCDashboardConfig_Publish objData = new tblDLCDashboardConfig_Publish();
            string connStr = objCustRepo.GetCustomerConnString(groupId);
            try
            {
                using (var context = new BOTSDBContext(connStr))
                {
                    objData = context.tblDLCDashboardConfig_Publish.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetPublishDLCDashboardConfig");
            }
            return objData;
        }
        public List<tblDLCFrontEndPageData> GetDLCFrontEndPageData(string groupId)
        {
            List<tblDLCFrontEndPageData> objData = new List<tblDLCFrontEndPageData>();
            string connStr = objCustRepo.GetCustomerConnString(groupId);
            try
            {
                using (var context = new BOTSDBContext(connStr))
                {
                    objData = context.tblDLCFrontEndPageDataNews.ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDLCFrontEndPageData");
            }
            return objData;
        }
        public List<tblCountyCode> GetCountryCodes()
        {
            List<tblCountyCode> lstCodes = new List<tblCountyCode>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    lstCodes = context.tblCountyCodes.ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCountryCodes");
            }
            return lstCodes;
        }

        public string CheckUserAndSendOTP(string groupId, string mobileNo, bool IsOTP)
        {
            string status = string.Empty;
            string connStr = objCustRepo.GetCustomerConnString(groupId);
            try
            {
                using (var context = new BOTSDBContext(connStr))
                {
                    if (IsOTP)
                    {
                        //Send OTP
                        status = "OTP";
                    }
                    else
                    {
                        //Check whether Password is present of not
                        string Password = context.CustomerDetails.Where(x => x.MobileNo == mobileNo).Select(y => y.Password).FirstOrDefault();
                        if(!string.IsNullOrEmpty(Password))
                        {
                            status = "Password";
                        }
                        else
                        {
                            status = "OTP";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "CheckUserAndSendOTP");
            }

            return status;
        }



    }
}
