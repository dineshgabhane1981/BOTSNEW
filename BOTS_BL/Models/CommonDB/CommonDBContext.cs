using BOTS_BL.Models.CommonDB;
using BOTS_BL.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using BOTS_BL.Models.SalesLead;
using BOTS_BL.Models.FeedbackModule;
using BOTS_BL.Models.OnBoarding;

namespace BOTS_BL.Models
{
    public partial class CommonDBContext : DbContext
    {
        public CommonDBContext()
            : base("name=CommonDBContext")
        {
        }
        public virtual DbSet<tblReference> tblReferences { get; set; }
        public virtual DbSet<CommonSMSGateWayMaster> CommonSMSGateWayMasters { get; set; }
        public virtual DbSet<NPCLoginDetail> NPCLoginDetails { get; set; }
        public virtual DbSet<BOTS_TblDocuments> BOTS_TblDocuments { get; set; }
        public virtual DbSet<BOTS_TblActionTracking> BOTS_TblActionTracking { get; set; }
        public virtual DbSet<GroupIdMapping> GroupIdMappings { get; set; }
        public virtual DbSet<BOTS_TblBurnRuleConfig> BOTS_TblBurnRuleConfig { get; set; }
        public virtual DbSet<BOTS_TblProductUpload> BOTS_TblProductUpload { get; set; }
        public virtual DbSet<BOTS_TblEarnRuleConfig> BOTS_TblEarnRuleConfig { get; set; }
        public virtual DbSet<BOTS_TblSlabConfig> BOTS_TblSlabConfig { get; set; }
        public virtual DbSet<ReportForDownload> ReportForDownloads { get; set; }
        public virtual DbSet<BOTS_TblVariableWords> BOTS_TblVariableWords { get; set; }
        public virtual DbSet<tblLoginLog> tblLoginLogs { get; set; }
        public virtual DbSet<BOTS_TblCommunicationSet> BOTS_TblCommunicationSet { get; set; }
        public virtual DbSet<BOTS_TblCommunicationSetAssignment> BOTS_TblCommunicationSetAssignment { get; set; }        
        public virtual DbSet<BOTS_TblCampaignInactive> BOTS_TblCampaignInactive { get; set; }
        public virtual DbSet<BOTS_TblCampaignOtherConfig> BOTS_TblCampaignOtherConfig { get; set; }
        public virtual DbSet<BOTS_TblBulkUpload> BOTS_TblBulkUpload { get; set; }
        public virtual DbSet<BOTS_TblVelocityChecksConfig> BOTS_TblVelocityChecksConfig { get; set; }
        public virtual DbSet<BOTS_TblDLCLinkConfig> BOTS_TblDLCLinkConfig { get; set; }        
        public virtual DbSet<Feedback_SMSNumbers> Feedback_SMSNumbers { get; set; }
        public virtual DbSet<Feedback_Content> Feedback_Content { get; set; }
        public virtual DbSet<Feedback_ContentMaster> Feedback_ContentMaster { get; set; }
        public virtual DbSet<Feedback_KnowAboutYou> Feedback_KnowAboutYou { get; set; }
        public virtual DbSet<Feedback_PointsAndMessages> Feedback_PointsAndMessages { get; set; }
        public virtual DbSet<Feedback_Headings> Feedback_Headings { get; set; }
        public virtual DbSet<Feedback_Questions> Feedback_Questions { get; set; }
        public virtual DbSet<Feedback_FeedbackConfig> Feedback_FeedbackConfig { get; set; }
        public virtual DbSet<Tbl_SinglePageHigherOnlyonceAndInactive> Tbl_SinglePageHigherOnlyonceAndInactive { get; set; }
        public virtual DbSet<Tbl_SinglePageLowerRedemptionRate> Tbl_SinglePageLowerRedemptionRate { get; set; }
        public virtual DbSet<Tbl_SinglePageLowProfileUpdates> Tbl_SinglePageLowProfileUpdates { get; set; }
        public virtual DbSet<Tbl_SinglePageLowReferral> Tbl_SinglePageLowReferral { get; set; }
        public virtual DbSet<Tbl_SinglePageLowReferralConversions> Tbl_SinglePageLowReferralConversions { get; set; }
        public virtual DbSet<BOTS_TblSubDiscussionData> BOTS_TblSubDiscussionData { get; set; }
        public virtual DbSet<BOTS_TblOutletMaster> BOTS_TblOutletMaster { get; set; }
        public virtual DbSet<SMSWABalanceData> SMSWABalanceData { get; set; }
        public virtual DbSet<ListOfGroup> ListOfGroups { get; set; }
        public virtual DbSet<Tbl_SinglePageSummaryTable> Tbl_SinglePageSummaryTable { get; set; }
        public virtual DbSet<Tbl_SinglePageLowTransactingOutlet> Tbl_SinglePageLowTransactingOutlet { get; set; }
        public virtual DbSet<Tbl_SinglePageNonTransactingGroup> Tbl_SinglePageNonTransactingGroup { get; set; }
        public virtual DbSet<Tbl_SinglePageNonTransactingOutlet> Tbl_SinglePageNonTransactingOutlet { get; set; }
        public virtual DbSet<tblSourceType> tblSourceTypes { get; set; }
        public virtual DbSet<BOTS_TblBillingPartnerProduct> BOTS_TblBillingPartnerProduct { get; set; }
        public virtual DbSet<SMSGatewayMaster> SMSGatewayMasters { get; set; }
        public virtual DbSet<MobileAppOnceInMonthData> MobileAppOnceInMonthData { get; set; }
        public virtual DbSet<BOTS_TblDealDetails> BOTS_TblDealDetails { get; set; }
        public virtual DbSet<BOTS_TblGroupMaster> BOTS_TblGroupMaster { get; set; }
        public virtual DbSet<BOTS_TblInstallmentDetails> BOTS_TblInstallmentDetails { get; set; }
        public virtual DbSet<BOTS_TblPaymentDetails> BOTS_TblPaymentDetails { get; set; }
        public virtual DbSet<BOTS_TblRetailCategoryMaster> BOTS_TblRetailCategoryMaster { get; set; }
        public virtual DbSet<BOTS_TblRetailMaster> BOTS_TblRetailMaster { get; set; }
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
        public virtual DbSet<BOTS_TblDiscussion> BOTS_TblDiscussion { get; set; }
        public virtual DbSet<BOTS_TblCallSubTypes> BOTS_TblCallSubTypes { get; set; }
        public virtual DbSet<BOTS_TblCallTypes> BOTS_TblCallTypes { get; set; }
        public virtual DbSet<tblState> tblStates { get; set; }
        public virtual DbSet<tblChannelPartner> tblChannelPartners { get; set; }

        public virtual DbSet<SALES_tblLeads> SALES_tblLeads { get; set; }
        public virtual DbSet<SALES_tblLeadTracking> SALES_tblLeadTracking { get; set; }
        public virtual DbSet<BOTS_TblSMSConfig> BOTS_TblSMSConfig { get; set; }
        public virtual DbSet<BOTS_TblWAConfig> BOTS_TblWAConfig { get; set; }
        public virtual DbSet<tblStandardRulesSetting> tblStandardRulesSettings { get; set; }

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
