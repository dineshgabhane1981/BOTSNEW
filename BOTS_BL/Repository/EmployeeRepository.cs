using BOTS_BL.Models.ChitaleModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Repository
{
    public class EmployeeRepository
    {
        ChitaleException newexception = new ChitaleException();
        public List<CustomerDetail> GetTop5Participant(string type, string CustomerId, string CustomerType)
        {
            List<CustomerDetail> objTop5Participant = new List<CustomerDetail>();
            using (var context = new ChitaleDBContext())
            {
                if (CustomerType == "Sales Executive")
                {
                    try
                    {
                        var IDs = context.SalesmanMappings.Where(x => x.SalesExecutive == CustomerId).Select(y => y.ParticipantId).ToList();
                        objTop5Participant = context.CustomerDetails.Where(a =>a.CustomerType== type && IDs.Contains(a.CustomerId)).OrderByDescending(x => x.Points).Take(5).ToList();
                        var count = IDs.Count;
                    }
                    catch(Exception ex)
                    {
                        newexception.AddException(ex);
                    }
                }
            }

            return objTop5Participant;
        }

        public List<CustomerDetail> Bottom5Participants(string type, string CustomerId, string CustomerType)
        {
            List<CustomerDetail> objTop5Participant = new List<CustomerDetail>();
            using (var context = new ChitaleDBContext())
            {
                if (CustomerType == "Sales Executive")
                {
                    try
                    {
                        var IDs = context.SalesmanMappings.Where(x => x.SalesExecutive == CustomerId).Select(y => y.ParticipantId).ToList();
                        objTop5Participant = context.CustomerDetails.Where(a => a.CustomerType == type && IDs.Contains(a.CustomerId)).OrderBy(x => x.Points).Take(5).OrderByDescending(y => y.Points).ToList();
                        var count = IDs.Count;
                    }
                    catch (Exception ex)
                    {
                        newexception.AddException(ex);
                    }
                }
            }

            return objTop5Participant;
        }

        public List<Top5LostParticipants> GetTop5LostOppsParticipant(string type, string CustomerId, string CustomerType)
        {
            List<Top5LostParticipants> objTop5Participant = new List<Top5LostParticipants>();
            using (var context = new ChitaleDBContext())
            {
                objTop5Participant = context.Database.SqlQuery<Top5LostParticipants>("sp_GetTop5LostParticipantsEmployee @pi_CustomerType, @pi_CustomerId, @pi_Type", 
                    new SqlParameter("@pi_CustomerType", type),
                    new SqlParameter("@pi_CustomerId", CustomerId),
                    new SqlParameter("@pi_Type", CustomerType)).ToList<Top5LostParticipants>();
            }

            return objTop5Participant;
        }
        public List<Top5TgtVsAchPerformanceEmp> GetTop5TgtVsAchPerformanceEmp(string type, string CustomerId, string CustomerType)
        {
            List<Top5TgtVsAchPerformanceEmp> objTop5TgtVsAchPerformanceEmp = new List<Top5TgtVsAchPerformanceEmp>();
            using (var context = new ChitaleDBContext())
            {
                objTop5TgtVsAchPerformanceEmp = context.Database.SqlQuery<Top5TgtVsAchPerformanceEmp>("sp_GetTgtVsAchEmployee @pi_CustomerType, @pi_CustomerId, @pi_Type",
                    new SqlParameter("@pi_CustomerType", type),
                    new SqlParameter("@pi_CustomerId", CustomerId),
                    new SqlParameter("@pi_Type", CustomerType)).ToList<Top5TgtVsAchPerformanceEmp>();
            }

            return objTop5TgtVsAchPerformanceEmp;
        }
        public List<InvoiceToOrder> GetInvoiceToOrderData(string Cluster, string SubCluster, string City, string type, string FromDate, string Todate, string CustomerId, string customerType)
        {
            List<InvoiceToOrder> objData = new List<InvoiceToOrder>();
            using (var context = new ChitaleDBContext())
            {
                try
                {
                    objData = context.InvoiceToOrders.Where(x => x.CustomerId == CustomerId && x.CustomerType == customerType).ToList();
                    if (Cluster == "All" && SubCluster == "All" && City == "All" && FromDate == "" && Todate == "" && type == "0")
                    {
                        objData = context.InvoiceToOrders.Where(x => x.CustomerId == CustomerId && x.CustomerType == customerType).ToList();
                    }
                    else
                    {
                        if (Cluster != "All")
                        {
                            objData = objData.Where(x => x.Cluster == Cluster).ToList();
                        }
                        else if (SubCluster != "All")
                        {
                            objData = objData.Where(x => x.SubCluster == SubCluster).ToList();
                        }
                        else if (City != "All")
                        {
                            objData = objData.Where(x => x.City == City).ToList();
                        }
                        if (FromDate != "" && Todate != "")
                        {
                            objData = objData.Where(x => x.InvDate >= Convert.ToDateTime(FromDate) && x.InvDate <= Convert.ToDateTime(Todate)).ToList();
                        }
                        else
                        {
                            if (FromDate != "")
                            {
                                objData = objData.Where(x => x.InvDate >= Convert.ToDateTime(FromDate)).ToList();
                            }
                            if (Todate != "")
                            {
                                objData = objData.Where(x => x.InvDate >= Convert.ToDateTime(Todate)).ToList();
                            }
                        }
                    }
                    if (type != "0")
                    {
                        objData = objData.Where(x => x.CustomerType == type).ToList();
                    }
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex);
                }
            }


            foreach (var item in objData)
            {
                item.StrDate = item.InvDate.Value.ToString("dd-MM-yyyy");
            }

            return objData;
        }

    }
}
