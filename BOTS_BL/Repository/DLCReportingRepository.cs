using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;
using System.Threading;
using System.Web;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using BOTS_BL.Models.Reports;

namespace BOTS_BL.Repository
{
    public class DLCReportingRepository
    {
        Exceptions newexception = new Exceptions();
        CustomerRepository CR = new CustomerRepository();

        public List<DLCReporting> GetDLCReportings(string GroupId, string Month, string Year, string flag)
        {
            List<DLCReporting> objData = new List<DLCReporting>();
            string DBName = string.Empty;
            try
            {
                var connstr = CR.GetCustomerConnString(GroupId);

                if(GroupId == "1087")
                {
                    DBName = "MadhusudanTextiles_New";
                    using (var context = new CommonDBContext())
                    {
                        if (Month == "0")
                        {
                            objData = context.Database.SqlQuery<DLCReporting>("sp_DLCReporting @pi_GroupId,@pi_Month,@pi_Year,@pi_INDDatetime,@pi_SelectedCriteria,@pi_DBName",
                                new SqlParameter("@pi_GroupId", GroupId),
                                new SqlParameter("@pi_Month", DateTime.Today.Month),
                                new SqlParameter("@pi_Year", DateTime.Today.Year),
                                new SqlParameter("@pi_INDDatetime", DateTime.Now),
                                new SqlParameter("@pi_SelectedCriteria", flag),
                                new SqlParameter("@pi_DBName", DBName)).ToList<DLCReporting>();
                        }
                        else
                        {
                            objData = context.Database.SqlQuery<DLCReporting>("sp_DLCReporting @pi_GroupId,@pi_Month,@pi_Year,@pi_INDDatetime,@pi_SelectedCriteria,@pi_DBName",
                                new SqlParameter("@pi_GroupId", GroupId),
                                new SqlParameter("@pi_Month", Month),
                                new SqlParameter("@pi_Year", Year),
                                new SqlParameter("@pi_INDDatetime", DateTime.Now),
                                new SqlParameter("@pi_SelectedCriteria", flag),
                                new SqlParameter("@pi_DBName", DBName)).ToList<DLCReporting>();
                        }
                    }

                }
                else
                {
                    using (var context = new BOTSDBContext(connstr))
                    {
                        if (Month == "0")
                        {
                            objData = context.Database.SqlQuery<DLCReporting>("sp_BOTS_DLC_Reporting @pi_GroupId,@pi_Month,@pi_Year,@pi_INDDatetime,@pi_SelectedCriteria",
                                new SqlParameter("@pi_GroupId", GroupId),
                                new SqlParameter("@pi_Month", DateTime.Today.Month),
                                new SqlParameter("@pi_Year", DateTime.Today.Year),
                                new SqlParameter("@pi_INDDatetime", DateTime.Now),
                                new SqlParameter("@pi_SelectedCriteria", flag)).ToList<DLCReporting>();
                        }
                        else
                        {
                            objData = context.Database.SqlQuery<DLCReporting>("sp_BOTS_DLC_Reporting @pi_GroupId,@pi_Month,@pi_Year,@pi_INDDatetime,@pi_SelectedCriteria",
                                new SqlParameter("@pi_GroupId", GroupId),
                                new SqlParameter("@pi_Month", Month),
                                new SqlParameter("@pi_Year", Year),
                                new SqlParameter("@pi_INDDatetime", DateTime.Now),
                                new SqlParameter("@pi_SelectedCriteria", flag)).ToList<DLCReporting>();
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDLCReportings");
            }

            return objData;
        }

        public List<DLCNewReg> GetDLCNew(string GroupId)
        {
            List<DLCNewReg> ObjDLCdata = new List<DLCNewReg>();
            string DBName = string.Empty;
            try
            {
                var connstr = CR.GetCustomerConnString(GroupId);
                DBName = "MadhusudanTextiles_New";
                using (var context = new CommonDBContext())
                {
                    ObjDLCdata = context.Database.SqlQuery<DLCNewReg>("sp_DLCReportingSummary @pi_GroupId,@pi_Date,@pi_LoginId,@pi_DBName",
                                new SqlParameter("@pi_GroupId", GroupId),
                                new SqlParameter("@pi_Date", DateTime.Now),
                                new SqlParameter("@pi_LoginId", ""),
                                new SqlParameter("@pi_DBName", DBName)).ToList<DLCNewReg>();
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDLCNew");
            }
            return ObjDLCdata;
        }

        public List<DLCNewRegDetail> GetDLCDataDetail(string GroupId,string SourceId)
        {
            List<DLCNewRegDetail> objDLCDetail = new List<DLCNewRegDetail>();
            string DBName = string.Empty;
            try
            {
                var connstr = CR.GetCustomerConnString(GroupId);
                DBName = "MadhusudanTextiles_New";
                using (var context = new CommonDBContext())
                {
                    objDLCDetail = context.Database.SqlQuery<DLCNewRegDetail>("sp_DLCReportingDetailed @pi_GroupId,@pi_Date,@pi_LoginId,@pi_Source,@pi_DBName",
                                new SqlParameter("@pi_GroupId", GroupId),
                                new SqlParameter("@pi_Date", DateTime.Now),
                                new SqlParameter("@pi_LoginId", ""),
                                new SqlParameter("@pi_Source", SourceId),
                                new SqlParameter("@pi_DBName", DBName)).ToList<DLCNewRegDetail>();
                }
            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "GetDLCDataDetail");
            }

            return objDLCDetail;
        }
    }
}
