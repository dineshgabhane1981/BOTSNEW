using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models.ChitaleModel;


namespace BOTS_BL.Repository
{
    public class ChitaleDashboardRepository
    {
        public CustomerDetail GetCustomerDetail(string CustomerId)
        {
            CustomerDetail objCustomerDetail = new CustomerDetail();
            using (var context = new ChitaleDBContext())
            {
                objCustomerDetail = context.CustomerDetails.Where(x => x.CustomerId == CustomerId).FirstOrDefault();
            }
            return objCustomerDetail;
        }

        public List<TransactionMaster> GetTransactionDetails(string CustomerId,string CustomerType,string startdt,string enddt)
        {
            List<TransactionMaster> objtransactionmaster = new List<TransactionMaster>();
            using(var context = new ChitaleDBContext())
            {
                objtransactionmaster = context.Database.SqlQuery<TransactionMaster>("sp_GetTransactionMasterDetailsByDate @customerId, @customerType, @startDt, @endDt",
                        new SqlParameter("@customerId", CustomerId),
                        new SqlParameter("@customerType", CustomerType),
                        new SqlParameter("@startDt", startdt),
                        new SqlParameter("@endDt", enddt)).ToList<TransactionMaster>();

                
            }

            return objtransactionmaster;
        }

        public decimal GetOrderPointsPurchase(string CustomerId, string CustomerType, string startdt, string enddt)
        {
            List<TransactionMaster> objtransactionmaster = new List<TransactionMaster>();
            // List<Dashboardsummary> objdashboardsummary = new List<Dashboardsummary>();
            decimal sumorder =0;
            using (var context = new ChitaleDBContext())
            {
                objtransactionmaster = context.Database.SqlQuery<TransactionMaster>("sp_GetTransactionMasterDetailsByDate @customerId, @customerType, @startDt, @endDt",
                        new SqlParameter("@customerId", CustomerId),
                        new SqlParameter("@customerType", CustomerType),
                        new SqlParameter("@startDt", startdt),
                        new SqlParameter("@endDt", enddt)).ToList<TransactionMaster>();

                sumorder = (decimal)objtransactionmaster.Where(x => x.TxnType == "Purchase").Sum(x => x.NormalPoints);
            }

            return sumorder;
        }

        public decimal GetRedeemPoints(string CustomerId, string CustomerType, string startdt, string enddt)
        {
            List<TransactionMaster> objtransactionmaster = new List<TransactionMaster>();
            // List<Dashboardsummary> objdashboardsummary = new List<Dashboardsummary>();
            var sumorder = 0;
            using (var context = new ChitaleDBContext())
            {
                objtransactionmaster = context.Database.SqlQuery<TransactionMaster>("sp_GetTransactionMasterDetailsByDate @customerId, @customerType, @startDt, @endDt",
                        new SqlParameter("@customerId", CustomerId),
                        new SqlParameter("@customerType", CustomerType),
                        new SqlParameter("@startDt", startdt),
                        new SqlParameter("@endDt", enddt)).ToList<TransactionMaster>();

                sumorder = (int)(decimal)objtransactionmaster.Where(x => x.TxnType == "Sale").Sum(x => x.NormalPoints);
            }

            return sumorder;
        }
        public decimal GetAddOnPoints(string CustomerId, string CustomerType, string startdt, string enddt)
        {
            List<TransactionMaster> objtransactionmaster = new List<TransactionMaster>();
            // List<Dashboardsummary> objdashboardsummary = new List<Dashboardsummary>();
            var sumorder = 0;
            using (var context = new ChitaleDBContext())
            {
                objtransactionmaster = context.Database.SqlQuery<TransactionMaster>("sp_GetTransactionMasterDetailsByDate @customerId, @customerType, @startDt, @endDt",
                        new SqlParameter("@customerId", CustomerId),
                        new SqlParameter("@customerType", CustomerType),
                        new SqlParameter("@startDt", startdt),
                        new SqlParameter("@endDt", enddt)).ToList<TransactionMaster>();

                sumorder = (int)(decimal)objtransactionmaster.Sum(x => x.AddOnPoints);
            }

            return sumorder;
        }
        public decimal GetLostPoints(string CustomerId, string CustomerType, string startdt, string enddt)
        {
            List<TransactionMaster> objtransactionmaster = new List<TransactionMaster>();
            // List<Dashboardsummary> objdashboardsummary = new List<Dashboardsummary>();
            var sumorder = 0;
            using (var context = new ChitaleDBContext())
            {
                objtransactionmaster = context.Database.SqlQuery<TransactionMaster>("sp_GetTransactionMasterDetailsByDate @customerId, @customerType, @startDt, @endDt",
                        new SqlParameter("@customerId", CustomerId),
                        new SqlParameter("@customerType", CustomerType),
                        new SqlParameter("@startDt", startdt),
                        new SqlParameter("@endDt", enddt)).ToList<TransactionMaster>();

                sumorder = (int)(decimal)objtransactionmaster.Sum(x => x.PenaltyPoints);
            }

            return sumorder;
        }
        //public decimal GetTotalPoints(string CustomerId, string CustomerType, string startdt, string enddt)
        //{
        //    CustomerDetail objCustomerDetail = new CustomerDetail();
        //    List<CustomerDetail> objtransactionmaster = new List<CustomerDetail>();
        //    // List<Dashboardsummary> objdashboardsummary = new List<Dashboardsummary>();
        //    var sumorder = 0;
        //    using (var context = new ChitaleDBContext())
        //    {
        //        objtransactionmaster = context.Database.SqlQuery<CustomerDetail>("sp_GetTransactionMasterDetailsByDate @customerId, @customerType, @startDt, @endDt",
        //                new SqlParameter("@customerId", CustomerId),
        //                new SqlParameter("@customerType", CustomerType),
        //                new SqlParameter("@startDt", startdt),
        //                new SqlParameter("@endDt", enddt)).ToList<TransactionMaster>();

        //        sumorder = (int)(decimal)objtransactionmaster.Sum(x => x.PenaltyPoints);
        //    }

        //    return sumorder;
        //}

    }
}
