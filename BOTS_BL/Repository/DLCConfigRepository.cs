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

namespace BOTS_BL
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
            catch(Exception ex)
            {
                newexception.AddException(ex, GroupId);
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
                newexception.AddException(ex, GroupId);
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
                newexception.AddException(ex, Groupid);
            }


            return status;
        }
    }
}
