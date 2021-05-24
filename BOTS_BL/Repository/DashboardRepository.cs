using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;

namespace BOTS_BL.Repository
{

    public class DashboardRepository
    {
        //string connstr = CustomerConnString.ConnectionStringCustomer;
        Exceptions newexception = new Exceptions();
        public ExecutiveSummary GetDashboardData(string GroupId, string connstr)
        {

            ExecutiveSummary dataDashboard = new ExecutiveSummary();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    
                    dataDashboard = context.Database.SqlQuery<ExecutiveSummary>("sp_Dashboard @pi_GroupId, @pi_Date", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString())).FirstOrDefault<ExecutiveSummary>();
                    
                    dataDashboard.lstOutletDetails = context.Database.SqlQuery<OutletDetails>("sp_OutletDashboard @pi_GroupId, @pi_Date", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd"))).ToList<OutletDetails>();
                    
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return dataDashboard;
        }

        public DashboardMemberSegment GetDashboardMemberSegmentData(string GroupId, string OutletId, string connstr)
        {

            DashboardMemberSegment dashboardMemberSegment = new DashboardMemberSegment();
            using (var context = new BOTSDBContext(connstr))
            {

                dashboardMemberSegment = context.Database.SqlQuery<DashboardMemberSegment>("sp_BOTS_DashboardMemberSegment @pi_GroupId, @pi_Date, @pi_OutletId", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd")), new SqlParameter("@pi_OutletId", OutletId)).FirstOrDefault<DashboardMemberSegment>();

                DateTime? FromDate;
                if (!string.IsNullOrEmpty(OutletId))
                {
                    OutletId = OutletId + "01";
                    FromDate = context.TransactionMasters.Where(x => x.CounterId == OutletId).OrderBy(y => y.Datetime).Select(z => z.Datetime).FirstOrDefault();

                }
                else
                {
                    FromDate = context.TransactionMasters.OrderBy(y => y.Datetime).Select(z => z.Datetime).FirstOrDefault();
                }
                if (FromDate.HasValue)
                {
                    dashboardMemberSegment.FromDate = FromDate.Value.ToString("dd-MM-yyyy");
                    dashboardMemberSegment.ToDate = DateTime.Now.ToString("dd-MM-yyyy");
                }
            }

            return dashboardMemberSegment;
        }
        public List<DashboardMemberSegmentTxn> GetDashboardMemberSegmentTxnData(string GroupId, string OutletId, string connstr)
        {

            List<DashboardMemberSegmentTxn> dashboardMemberSegmentTxn = new List<DashboardMemberSegmentTxn>();
            List<DashboardMemberSegmentTxnDB> dashboardMemberSegmentTxnDB = new List<DashboardMemberSegmentTxnDB>();
            using (var context = new BOTSDBContext(connstr))
            {

                dashboardMemberSegmentTxnDB = context.Database.SqlQuery<DashboardMemberSegmentTxnDB>("sp_BOTS_DashboardMemberSegment1 @pi_GroupId, @pi_Date, @pi_OutletId", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd")), new SqlParameter("@pi_OutletId", OutletId)).ToList<DashboardMemberSegmentTxnDB>();
                int count = 1;
                foreach (var item in dashboardMemberSegmentTxnDB)
                {
                    DashboardMemberSegmentTxn newItem = new DashboardMemberSegmentTxn();
                    if (count == 1)
                    {
                        newItem.Title = "No of Transactions";
                        newItem.Unit = "Nos";
                    }
                    if (count == 2)
                    {
                        newItem.Title = "Txns per Member";
                        newItem.Unit = "Nos";
                    }
                    if (count == 3)
                    {
                        newItem.Title = "Avg Days b/w Txns";
                        newItem.Unit = "Days";
                    }
                    if (count == 4)
                    {
                        newItem.Title = "Average Bill Size";
                        newItem.Unit = "Rs.";
                    }
                    if (count == 5)
                    {
                        newItem.Title = "Per Member Spends";
                        newItem.Unit = "Rs.";
                    }
                    if (count == 6)
                    {
                        newItem.Title = "Active Base (30 days)";
                        newItem.Unit = "%";
                    }
                    if (count == 7)
                    {
                        newItem.Title = "Non-Active Base";
                        newItem.Unit = "Nos";
                    }
                    if (count == 8)
                    {
                        newItem.Title = "Redeem to Invoice";
                        newItem.Unit = "Times";
                    }

                    newItem.TotalBase = item.TotalBase;
                    newItem.RepeatBase = item.RepeatBase;
                    newItem.OnlyOnce = item.OnlyOnce;
                    newItem.NeverRedeem = item.NeverRedeem;
                    newItem.RecentlyEnrolled = item.RecentlyEnrolled;
                    dashboardMemberSegmentTxn.Add(newItem);
                    count++;
                }

            }
            return dashboardMemberSegmentTxn;
        }

        public List<DashboardOutletEnrolment> GetDashboardOutletEnrolmentData(string GroupId, string monthFlag, string connstr)
        {

            List<DashboardOutletEnrolment> dashboardOutletEnrolment = new List<DashboardOutletEnrolment>();
            using (var context = new BOTSDBContext(connstr))
            {

                dashboardOutletEnrolment = context.Database.SqlQuery<DashboardOutletEnrolment>("sp_BOTS_DashboardOutletEnrolment @pi_GroupId, @pi_Date, @pi_Flag", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), new SqlParameter("@pi_Flag", monthFlag)).OrderByDescending(s => s.EnrollmentCount).ToList<DashboardOutletEnrolment>();

            }
            return dashboardOutletEnrolment;
        }

        public DashboardPointsSummary GetDashboardPointsSummaryData(string GroupId, string monthFlag, string connstr)
        {

            DashboardPointsSummary dashboardPointsSummary = new DashboardPointsSummary();
            using (var context = new BOTSDBContext(connstr))
            {

                dashboardPointsSummary = context.Database.SqlQuery<DashboardPointsSummary>("sp_BOTS_DashboardPointsSummary @pi_GroupId, @pi_Date, @pi_Flag", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), new SqlParameter("@pi_Flag", monthFlag)).FirstOrDefault<DashboardPointsSummary>();

            }
            return dashboardPointsSummary;
        }
        public DashboardMemberWebPage GetDashboardMemberWebPageData(string GroupId, string profileFlag, string connstr)
        {

            DashboardMemberWebPage dashboardMemberWebPage = new DashboardMemberWebPage();
            using (var context = new BOTSDBContext(connstr))
            {

                dashboardMemberWebPage = context.Database.SqlQuery<DashboardMemberWebPage>("sp_BOTS_DashboardMemberWebPage @pi_GroupId, @pi_Date, @pi_Flag", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), new SqlParameter("@pi_Flag", profileFlag)).FirstOrDefault<DashboardMemberWebPage>();

            }
            return dashboardMemberWebPage;
        }

        public DashboardBulkUpload GetDashboardBulkUpload(string GroupId, string connstr)
        {
            DashboardBulkUpload objDashboardBulkUpload = new DashboardBulkUpload();
            using (var context = new BOTSDBContext(connstr))
            {
                objDashboardBulkUpload = context.Database.SqlQuery<DashboardBulkUpload>("sp_BOTS_DashboardBulkUpload @pi_GroupId, @pi_Date, @pi_LoginId", new SqlParameter("@pi_GroupId", GroupId),
                    new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                    new SqlParameter("@pi_LoginId", "")).FirstOrDefault<DashboardBulkUpload>();
            }
            return objDashboardBulkUpload;
        }

        public DashboardRedemption GetDashboardRedemption(string GroupId, string Type, string connstr)
        {
            DashboardRedemption objDashboardRedemption = new DashboardRedemption();
            using (var context = new BOTSDBContext(connstr))
            {
                objDashboardRedemption = context.Database.SqlQuery<DashboardRedemption>("sp_BOTS_DashboardRedemption @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Type", new SqlParameter("@pi_GroupId", GroupId),
                    new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                    new SqlParameter("@pi_LoginId", ""),
                    new SqlParameter("@pi_Type", Type)).FirstOrDefault<DashboardRedemption>();
            }
            return objDashboardRedemption;
        }

        public bool UpdatePassword(string LoginId, string password)
        {
            bool status = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    CustomerLoginDetail userDetail = new CustomerLoginDetail();
                    userDetail = context.CustomerLoginDetails.Where(a => a.LoginId == LoginId).FirstOrDefault();
                    userDetail.Password = password;
                    context.CustomerLoginDetails.AddOrUpdate(userDetail);
                    context.SaveChanges();

                    status = true;
                }
            }
            catch (Exception ex)
            {

            }

            return status;
        }

        public bool InsertOTP(string emailId, int OTP)
        {
            bool status = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    OTPDetail objOTPDetail = new OTPDetail();
                    objOTPDetail.EmailId = emailId;
                    objOTPDetail.OTP = OTP;
                    objOTPDetail.SentDate = DateTime.Now;

                    context.OTPDetails.Add(objOTPDetail);
                    context.SaveChanges();

                    status = true;
                }
            }
            catch (Exception ex)
            {

            }
            return status;
        }

        public bool VerifyOTP(string emailId, int OTP)
        {
            bool status = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    OTPDetail objOTPDetail = new OTPDetail();
                    objOTPDetail = context.OTPDetails.Where(x => x.EmailId == emailId && x.OTP == OTP).OrderByDescending(y => y.SentDate).FirstOrDefault();
                    if (objOTPDetail != null)
                    {
                        status = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return status;
        }

        public bool ResetPassword(string emailId, string password)
        {
            bool status = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    CustomerLoginDetail userDetail = new CustomerLoginDetail();
                    userDetail = context.CustomerLoginDetails.Where(a => a.LoginId == emailId).FirstOrDefault();
                    userDetail.Password = password;
                    context.CustomerLoginDetails.AddOrUpdate(userDetail);
                    context.SaveChanges();

                    status = true;
                }
            }
            catch (Exception ex)
            {

            }
            return status;
        }
    }
}
