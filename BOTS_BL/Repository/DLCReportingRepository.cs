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

namespace BOTS_BL.Repository
{
    public class DLCReportingRepository
    {
        Exceptions newexception = new Exceptions();
        CustomerRepository CR = new CustomerRepository();

        public List<DLCReporting> GetDLCReportings(string GroupId, string Month, string Year, string flag)
        {
            List<DLCReporting> objData = new List<DLCReporting>();
            try
            {
                var connstr = CR.GetCustomerConnString(GroupId);
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
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDLCReportings");
            }

            return objData;
        }
    }
}
