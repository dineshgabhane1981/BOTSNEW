using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace BOTS_BL.Repository
{
    public class HeatMapsRepository
    {
        //string connstr = CustomerConnString.ConnectionStringCustomer;
        
        Exceptions newexception = new Exceptions();

        public List<DaywiseHourwise> GetDaywiseHourwiseData(string GroupId,string outletId, string connstr)
        {
            List<DaywiseHourwise> objDaywiseHourwise = new List<DaywiseHourwise>();
            string DBName;
            DBName = "MadhusudanTextiles_New";
            try
            {
                //if(GroupId == "1087")
                //{
                //    using (var context = new CommonDBContext())
                //    {
                //        objDaywiseHourwise = context.Database.SqlQuery<DaywiseHourwise>("sp_HeatMap @pi_GroupId, @pi_Date, @pi_LoginId, @pi_OutletId,@pi_DBName ", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), new SqlParameter("@pi_LoginId", ""), new SqlParameter("@pi_OutletId", outletId), new SqlParameter("@pi_DBName", DBName)).ToList<DaywiseHourwise>();
                //    }
                //}
                //else
                //{
                    using (var context = new BOTSDBContext(connstr))
                    {
                        objDaywiseHourwise = context.Database.SqlQuery<DaywiseHourwise>("sp_BOTS_HeatMap @pi_GroupId, @pi_Date, @pi_LoginId, @pi_OutletId", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), new SqlParameter("@pi_LoginId", ""), new SqlParameter("@pi_OutletId", outletId)).ToList<DaywiseHourwise>();

                    }
                //}
                
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDaywiseHourwiseData");
            }

            return objDaywiseHourwise;
        }
    }
}
