using BOTS_BL.Models.CommonDB;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BOTS_BL.Models
{
    public partial class CommonDBContext : DbContext
    {
        public CommonDBContext()
            : base("name=CommonDBContext")
        {
        }    
        
        public virtual DbSet<CommonReferralURL> CommonReferralURLs { get; set; }
        public virtual DbSet<DailyReport> DailyReports { get; set; }
        public virtual DbSet<DailySM> DailySMS { get; set; }
        public virtual DbSet<DailyWhatsAppSM> DailyWhatsAppSMS { get; set; }
        public virtual DbSet<DashboardReport> DashboardReports { get; set; }
        public virtual DbSet<DatabaseDetail> DatabaseDetails { get; set; }
        public virtual DbSet<MobileAppCommSecurityKey> MobileAppCommSecurityKeys { get; set; }
        public virtual DbSet<MonthlyReport> MonthlyReports { get; set; }
        public virtual DbSet<NPCSM> NPCSMS { get; set; }
        public virtual DbSet<PODetail> PODetails { get; set; }
        public virtual DbSet<POItem> POItems { get; set; }
        public virtual DbSet<ReferralBonusSM> ReferralBonusSMS { get; set; }
        public virtual DbSet<tblErrorLog> tblErrorLogs { get; set; }
        public virtual DbSet<CustomerLoginDetail> CustomerLoginDetails { get; set; }
        public virtual DbSet<OTPDetail> OTPDetails { get; set; }
        public virtual DbSet<tblGroupDetail> tblGroupDetails { get; set; }
        public virtual DbSet<tblBillingPartner> tblBillingPartners { get; set; }
        public virtual DbSet<tblCategory> tblCategories { get; set; }
        public virtual DbSet<tblCity> tblCities { get; set; }
        public virtual DbSet<tblRMAssigned> tblRMAssigneds { get; set; }
        public virtual DbSet<tblSourcedBy> tblSourcedBies { get; set; }
        public virtual DbSet<tblModulesPayment> tblModulesPayments { get; set; }
        public virtual DbSet<ReferAndEarn> ReferAndEarns { get; set; }

        public virtual DbSet<tblAudit> tblAudits { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommonReferralURL>()
                .Property(e => e.CounterId)
                .IsUnicode(false);

            modelBuilder.Entity<CommonReferralURL>()
                .Property(e => e.DataSource)
                .IsUnicode(false);

            modelBuilder.Entity<CommonReferralURL>()
                .Property(e => e.Data_Base)
                .IsUnicode(false);

            modelBuilder.Entity<CommonReferralURL>()
                .Property(e => e.DBPassword)
                .IsUnicode(false);

            modelBuilder.Entity<CommonReferralURL>()
                .Property(e => e.UserId)
                .IsUnicode(false);

            modelBuilder.Entity<DailyReport>()
                .Property(e => e.GroupName)
                .IsUnicode(false);

            modelBuilder.Entity<DailyReport>()
                .Property(e => e.IPAddress)
                .IsUnicode(false);

            modelBuilder.Entity<DailyReport>()
                .Property(e => e.DBName)
                .IsUnicode(false);

            modelBuilder.Entity<DailyReport>()
                .Property(e => e.DBId)
                .IsUnicode(false);

            modelBuilder.Entity<DailyReport>()
                .Property(e => e.DBPassword)
                .IsUnicode(false);

            modelBuilder.Entity<DailyReport>()
                .Property(e => e.ReportStatus)
                .IsUnicode(false);

            modelBuilder.Entity<DailyReport>()
                .Property(e => e.EmailId)
                .IsUnicode(false);

            modelBuilder.Entity<DailyReport>()
                .Property(e => e.BrandId)
                .IsUnicode(false);

            modelBuilder.Entity<DailyReport>()
                .Property(e => e.GroupId)
                .IsUnicode(false);

            modelBuilder.Entity<DailyReport>()
                .Property(e => e.ComReportFlag)
                .IsUnicode(false);

            modelBuilder.Entity<DailyReport>()
                .Property(e => e.BOEmailId)
                .IsUnicode(false);

            modelBuilder.Entity<DailySM>()
                .Property(e => e.GroupName)
                .IsUnicode(false);

            modelBuilder.Entity<DailySM>()
                .Property(e => e.IPAddress)
                .IsUnicode(false);

            modelBuilder.Entity<DailySM>()
                .Property(e => e.DBName)
                .IsUnicode(false);

            modelBuilder.Entity<DailySM>()
                .Property(e => e.DBId)
                .IsUnicode(false);

            modelBuilder.Entity<DailySM>()
                .Property(e => e.DBPassword)
                .IsUnicode(false);

            modelBuilder.Entity<DailySM>()
                .Property(e => e.SMSStatus)
                .IsUnicode(false);

            modelBuilder.Entity<DailySM>()
                .Property(e => e.EmailId)
                .IsUnicode(false);

            modelBuilder.Entity<DailySM>()
                .Property(e => e.BrandId)
                .IsUnicode(false);

            modelBuilder.Entity<DailySM>()
                .Property(e => e.GroupId)
                .IsUnicode(false);

            modelBuilder.Entity<DailyWhatsAppSM>()
                .Property(e => e.GroupName)
                .IsUnicode(false);

            modelBuilder.Entity<DailyWhatsAppSM>()
                .Property(e => e.IPAddress)
                .IsUnicode(false);

            modelBuilder.Entity<DailyWhatsAppSM>()
                .Property(e => e.DBName)
                .IsUnicode(false);

            modelBuilder.Entity<DailyWhatsAppSM>()
                .Property(e => e.DBId)
                .IsUnicode(false);

            modelBuilder.Entity<DailyWhatsAppSM>()
                .Property(e => e.DBPassword)
                .IsUnicode(false);

            modelBuilder.Entity<DailyWhatsAppSM>()
                .Property(e => e.SMSStatus)
                .IsUnicode(false);

            modelBuilder.Entity<DailyWhatsAppSM>()
                .Property(e => e.EmailId)
                .IsUnicode(false);

            modelBuilder.Entity<DailyWhatsAppSM>()
                .Property(e => e.BrandId)
                .IsUnicode(false);

            modelBuilder.Entity<DailyWhatsAppSM>()
                .Property(e => e.GroupId)
                .IsUnicode(false);

            modelBuilder.Entity<DashboardReport>()
                .Property(e => e.GroupName)
                .IsUnicode(false);

            modelBuilder.Entity<DashboardReport>()
                .Property(e => e.IPAddress)
                .IsUnicode(false);

            modelBuilder.Entity<DashboardReport>()
                .Property(e => e.DBName)
                .IsUnicode(false);

            modelBuilder.Entity<DashboardReport>()
                .Property(e => e.DBId)
                .IsUnicode(false);

            modelBuilder.Entity<DashboardReport>()
                .Property(e => e.DBPassword)
                .IsUnicode(false);

            modelBuilder.Entity<DashboardReport>()
                .Property(e => e.ReportStatus)
                .IsUnicode(false);

            modelBuilder.Entity<DashboardReport>()
                .Property(e => e.EmailId)
                .IsUnicode(false);

            modelBuilder.Entity<DashboardReport>()
                .Property(e => e.BrandId)
                .IsUnicode(false);

            modelBuilder.Entity<DashboardReport>()
                .Property(e => e.GroupId)
                .IsUnicode(false);

            modelBuilder.Entity<DashboardReport>()
                .Property(e => e.ComReportFlag)
                .IsUnicode(false);

            modelBuilder.Entity<DashboardReport>()
                .Property(e => e.BOEmailId)
                .IsUnicode(false);

            modelBuilder.Entity<DatabaseDetail>()
                .Property(e => e.CounterId)
                .IsUnicode(false);

            modelBuilder.Entity<DatabaseDetail>()
                .Property(e => e.SecurityKey)
                .IsUnicode(false);

            modelBuilder.Entity<DatabaseDetail>()
                .Property(e => e.IPAddress)
                .IsUnicode(false);

            modelBuilder.Entity<DatabaseDetail>()
                .Property(e => e.DBName)
                .IsUnicode(false);

            modelBuilder.Entity<DatabaseDetail>()
                .Property(e => e.DBPassword)
                .IsUnicode(false);

            modelBuilder.Entity<DatabaseDetail>()
                .Property(e => e.DBId)
                .IsUnicode(false);

            modelBuilder.Entity<DatabaseDetail>()
                .Property(e => e.EncryptionStatus)
                .IsUnicode(false);

            modelBuilder.Entity<DatabaseDetail>()
                .Property(e => e.LoginId)
                .IsUnicode(false);

            modelBuilder.Entity<MobileAppCommSecurityKey>()
                .Property(e => e.SecurityKey)
                .IsUnicode(false);

            modelBuilder.Entity<MobileAppCommSecurityKey>()
                .Property(e => e.BrandId)
                .IsUnicode(false);

            modelBuilder.Entity<MobileAppCommSecurityKey>()
                .Property(e => e.WebServiceLink)
                .IsUnicode(false);

            modelBuilder.Entity<MobileAppCommSecurityKey>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<MonthlyReport>()
                .Property(e => e.GroupName)
                .IsUnicode(false);

            modelBuilder.Entity<MonthlyReport>()
                .Property(e => e.IPAddress)
                .IsUnicode(false);

            modelBuilder.Entity<MonthlyReport>()
                .Property(e => e.DBName)
                .IsUnicode(false);

            modelBuilder.Entity<MonthlyReport>()
                .Property(e => e.DBId)
                .IsUnicode(false);

            modelBuilder.Entity<MonthlyReport>()
                .Property(e => e.DBPassword)
                .IsUnicode(false);

            modelBuilder.Entity<MonthlyReport>()
                .Property(e => e.ReportStatus)
                .IsUnicode(false);

            modelBuilder.Entity<MonthlyReport>()
                .Property(e => e.EmailId)
                .IsUnicode(false);

            modelBuilder.Entity<MonthlyReport>()
                .Property(e => e.BrandId)
                .IsUnicode(false);

            modelBuilder.Entity<MonthlyReport>()
                .Property(e => e.GroupId)
                .IsUnicode(false);

            modelBuilder.Entity<NPCSM>()
                .Property(e => e.GroupName)
                .IsUnicode(false);

            modelBuilder.Entity<NPCSM>()
                .Property(e => e.IPAddress)
                .IsUnicode(false);

            modelBuilder.Entity<NPCSM>()
                .Property(e => e.DBName)
                .IsUnicode(false);

            modelBuilder.Entity<NPCSM>()
                .Property(e => e.DBId)
                .IsUnicode(false);

            modelBuilder.Entity<NPCSM>()
                .Property(e => e.DBPassword)
                .IsUnicode(false);

            modelBuilder.Entity<NPCSM>()
                .Property(e => e.ReportStatus)
                .IsUnicode(false);

            modelBuilder.Entity<NPCSM>()
                .Property(e => e.EmailId)
                .IsUnicode(false);

            modelBuilder.Entity<NPCSM>()
                .Property(e => e.BrandId)
                .IsUnicode(false);

            modelBuilder.Entity<NPCSM>()
                .Property(e => e.GroupId)
                .IsUnicode(false);

            modelBuilder.Entity<PODetail>()
                .Property(e => e.PoNo)
                .IsUnicode(false);

            modelBuilder.Entity<PODetail>()
                .Property(e => e.SupplierCode)
                .IsUnicode(false);

            modelBuilder.Entity<PODetail>()
                .Property(e => e.Currency)
                .IsUnicode(false);

            modelBuilder.Entity<PODetail>()
                .Property(e => e.Address_)
                .IsUnicode(false);

            modelBuilder.Entity<PODetail>()
                .Property(e => e.Shipment_Mode)
                .IsUnicode(false);

            modelBuilder.Entity<PODetail>()
                .Property(e => e.Location)
                .IsUnicode(false);

            modelBuilder.Entity<PODetail>()
                .Property(e => e.Payment_Terms)
                .IsUnicode(false);

            modelBuilder.Entity<POItem>()
                .Property(e => e.Item)
                .IsUnicode(false);

            modelBuilder.Entity<POItem>()
                .Property(e => e.Grade)
                .IsUnicode(false);

            modelBuilder.Entity<POItem>()
                .Property(e => e.Location)
                .IsUnicode(false);

            modelBuilder.Entity<POItem>()
                .Property(e => e.Item_Name)
                .IsUnicode(false);

            modelBuilder.Entity<POItem>()
                .Property(e => e.PoNo)
                .IsUnicode(false);

            modelBuilder.Entity<ReferralBonusSM>()
                .Property(e => e.BrandId)
                .IsUnicode(false);

            modelBuilder.Entity<ReferralBonusSM>()
                .Property(e => e.SMSStatus)
                .IsUnicode(false);

            modelBuilder.Entity<ReferralBonusSM>()
                .Property(e => e.TokenId)
                .IsUnicode(false);

            modelBuilder.Entity<ReferralBonusSM>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<ReferralBonusSM>()
                .Property(e => e.Mobileno)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerLoginDetail>()
                .Property(e => e.UserId)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerLoginDetail>()
                .Property(e => e.LoginId)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerLoginDetail>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerLoginDetail>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerLoginDetail>()
                .Property(e => e.LevelIndicator)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerLoginDetail>()
                .Property(e => e.MobileNo)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerLoginDetail>()
                .Property(e => e.LoginType)
                .IsUnicode(false);

            modelBuilder.Entity<tblModulesPayment>()
               .Property(e => e.AnnualFees)
               .HasPrecision(18, 0);

            modelBuilder.Entity<tblModulesPayment>()
                .Property(e => e.PrepaidSMSAmount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblModulesPayment>()
                .Property(e => e.IntegrationAmount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblModulesPayment>()
                .Property(e => e.WAPrepaidPackAmount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblModulesPayment>()
                .Property(e => e.MemberWebpageAnnualFee)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblModulesPayment>()
                .Property(e => e.NPCAnnualCharge)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblModulesPayment>()
                .Property(e => e.MobileAppAnnualCharge)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblModulesPayment>()
                .Property(e => e.SProfileUpdateAnnualCharge)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblModulesPayment>()
                .Property(e => e.OtherOneFees)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblModulesPayment>()
                .Property(e => e.OtherTwoFees)
                .HasPrecision(18, 0);

        }
    }
}
