using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models.RetailerWeb;
using BOTS_BL.Models;
using System.Data.SqlClient;
using System.Data;

namespace BOTS_BL.Repository
{
    public class RetailerWebRepository
    {
        CustomerRepository CR = new CustomerRepository();
        public CustomerDetails GetCustomerDetails(string CounterId, string MobileNo)
        {
            CustomerDetails objData = new CustomerDetails();
            using (var context = new CommonDBContext())
            {
                string groupId = CounterId.Substring(0, 4);
                var conStr = CR.GetCustomerConnString(groupId);

                SqlConnection _Con = new SqlConnection(conStr);
                DataSet retVal = new DataSet();
                SqlCommand cmdReport = new SqlCommand("sp_Web_SearchCustomer", _Con);
                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);

                using (cmdReport)
                {
                    SqlParameter param2 = new SqlParameter("pi_CounterId", CounterId);
                    SqlParameter param3 = new SqlParameter("pi_SearchData", MobileNo);
                    SqlParameter param4 = new SqlParameter("pi_SearchType", "1");
                    SqlParameter param1 = new SqlParameter("pi_Datetime", DateTime.Today.ToString("yyyy-MM-dd"));
                    cmdReport.CommandType = CommandType.StoredProcedure;
                    cmdReport.Parameters.Add(param1);
                    cmdReport.Parameters.Add(param2);
                    cmdReport.Parameters.Add(param3);
                    cmdReport.Parameters.Add(param4);
                    daReport.Fill(retVal);

                    DataTable dt = retVal.Tables[0];

                    if (Convert.ToString(dt.Rows[0]["ResponseCode"]) == "00")
                    {
                        objData.ResponseCode = "00";
                        DataTable dt1 = retVal.Tables[1];
                        DataTable dt2 = retVal.Tables[2];
                        objData.MobileNo = Convert.ToString(dt2.Rows[0]["MobileNo"]);
                        objData.CustomerName = Convert.ToString(dt2.Rows[0]["CustomerName"]);
                        objData.PointBalance = Convert.ToString(dt2.Rows[0]["AvailablePoints"]);

                        objData.CardNo = Convert.ToString(dt1.Rows[0]["LoyaltyCard"]);
                        objData.TotalSpend = Convert.ToString(dt1.Rows[0]["TotalSpendText"]);
                        objData.LastTxnDate = Convert.ToString(dt1.Rows[0]["LastTxnText"]);
                    }
                    if (Convert.ToString(dt.Rows[0]["ResponseCode"]) == "06")
                    {
                        objData.ResponseCode = "06";
                    }
                }
            }
            return objData;
        }
    }
}
