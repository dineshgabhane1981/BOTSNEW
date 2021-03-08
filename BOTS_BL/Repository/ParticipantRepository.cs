using BOTS_BL.Models.ChitaleModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Repository
{
    public class ParticipantRepository
    {
        public List<ParticipantList> GetParticipantList(string CustomerId, string CustomerType)
        {
            ParticipantList objparticipantList = new ParticipantList();
            List<ParticipantList> lstparticipantLists = new List<ParticipantList>();
            try
            {
                DataSet retVal = new DataSet();
                SqlConnection sqlConn = new SqlConnection("Data Source=LAPTOP-SIDRDIDV;Initial Catalog=ChitaleLive;Integrated Security=True");
                SqlCommand cmdReport = new SqlCommand("sp_KYBLoadParticipant_MFC", sqlConn);
                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                using (cmdReport)
                {
                    SqlParameter param1 = new SqlParameter("@pi_LoginId", "");
                    SqlParameter param2 = new SqlParameter("@pi_Datetime", DateTime.Now.ToString("dd-MM-yyyy"));
                    SqlParameter param3 = new SqlParameter("@pi_DataType", "");
                    SqlParameter param4 = new SqlParameter("@pi_City", "");
                    SqlParameter param5 = new SqlParameter("@pi_Cluster", "");
                    SqlParameter param6 = new SqlParameter("@pi_SubCluster", "");
                    SqlParameter param7 = new SqlParameter("@pi_Id ", CustomerId);
                    SqlParameter param8 = new SqlParameter("@pi_ParticipantType", CustomerType);
                    cmdReport.CommandType = CommandType.StoredProcedure;
                    cmdReport.Parameters.Add(param1);
                    cmdReport.Parameters.Add(param2);
                    cmdReport.Parameters.Add(param3);
                    cmdReport.Parameters.Add(param4);
                    cmdReport.Parameters.Add(param5);
                    cmdReport.Parameters.Add(param6);
                    cmdReport.Parameters.Add(param7);
                    cmdReport.Parameters.Add(param8);
                    daReport.Fill(retVal);
                    DataTable dt = retVal.Tables[1];

                    foreach (DataRow dr in dt.Rows)
                    {
                        CustomerDetail objcustomerDetail = new CustomerDetail();
                        using (var context = new ChitaleDBContext())
                        {

                            string customerid = Convert.ToString(dr["Id"]);
                            objcustomerDetail = context.CustomerDetails.Where(x => x.CustomerId == customerid).FirstOrDefault();
                            objparticipantList.Id = Convert.ToString(dr["Id"]);
                            objparticipantList.Name = objcustomerDetail.CustomerName;
                            objparticipantList.participantType = objcustomerDetail.CustomerType;
                            objparticipantList.Rank = Convert.ToInt32(dr["CurrentRank"]);
                            objparticipantList.subcluster = objcustomerDetail.SubCluster;
                            objparticipantList.Totalpoints = (decimal)objcustomerDetail.Points;
                            objparticipantList.city = objcustomerDetail.City;
                            objparticipantList.cluster = objcustomerDetail.Cluster;

                        }

                        lstparticipantLists.Add(objparticipantList);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lstparticipantLists;
        }
    }
}
