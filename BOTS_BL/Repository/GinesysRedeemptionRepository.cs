using BOTS_BL.Models;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BOTS_BL.Repository
{
    public class GinesysRedeemptionRepository
    {
        CustomerRepository objCustRepo = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        public string GetCustomerConnString(string GroupId)
        {
            string ConnectionString = string.Empty;
            try
            {
                using (var context = new CommonDBContext())
                {                    
                    var DBDetails = context.tblDatabaseDetails.Where(x => x.GroupId == GroupId).FirstOrDefault(); 
                    if (DBDetails != null)
                    {                        
                        ConnectionString = "Data Source = " + DBDetails.IPAddress + "; Initial Catalog = " + DBDetails.DBName + "; user id = " + DBDetails.DBId + "; password = " + DBDetails.DBPassword + "";
                    }                   
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCustomerConnString");
            }
            return ConnectionString;
        }
        public  GinesysRedeemModel GetCustomerDetails(GinesysRedeemModel objData)
        {            
            string groupId = objData.StoreId.Substring(0, 4);
            string connStr = GetCustomerConnString(groupId);
            try
            {
                using (var context = new BOTSDBContext(connStr))
                {
                    objData.Points  = context.tblCustPointsMasters.Where(x => x.MobileNo == objData.MobileNo && x.IsActive == true).Sum(y => y.Points);
                    objData.PointsValue = context.tblRuleMasters.Select(x => x.PointsAllocation).FirstOrDefault();
                    objData.CustomerName = context.tblCustDetailsMasters.Where(x => x.MobileNo == objData.MobileNo).Select(y => y.Name).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCustomerDetails");
            }
            return objData;
        }        
    }
}
