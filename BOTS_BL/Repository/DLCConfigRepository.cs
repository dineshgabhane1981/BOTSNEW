using BOTS_BL.Models;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
    }
}
