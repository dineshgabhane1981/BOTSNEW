using BOTS_BL.Models;
using BOTS_BL.Models.FeedBack;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BOTS_BL.Models
{
    public partial class BOTSDBContext : DbContext
    {
        public BOTSDBContext()
            : base("name=BOTSDBContext")
        {
        }

        public BOTSDBContext(string connectionStringName)
            : base(connectionStringName)
        {
        }
        public virtual DbSet<tblEmployee> tblEmployees { get; set; }
        public virtual DbSet<tblNPCCategory> tblNPCCategories { get; set; }
        public virtual DbSet<tblNPCSubCategory> tblNPCSubCategories { get; set; }
        public virtual DbSet<tblNPCDetail> tblNPCDetails { get; set; }
        public virtual DbSet<GroupTestingLog> GroupTestingLogs { get; set; }
        public virtual DbSet<CampaignMaster> CampaignMasters { get; set; }
        public virtual DbSet<CampaignMemberDetail> CampaignMemberDetails { get; set; }
        public virtual DbSet<CelebrationMemberDetail> CelebrationMemberDetails { get; set; }
        public virtual DbSet<tblFranchiseeEnquiry> tblFranchiseeEnquiries { get; set; }
        public virtual DbSet<tblUniquePoint> tblUniquePoints { get; set; }
        public virtual DbSet<TelecallerTracking> TelecallerTrackings { get; set; }
        public virtual DbSet<MWPSourceMaster> MWPSourceMasters { get; set; }
        public virtual DbSet<MWP_Details> MWP_Details { get; set; }
        public virtual DbSet<DLCCreation> DLCCreations { get; set; }
        public virtual DbSet<WhatsAppSMSMaster> WhatsAppSMSMasters { get; set; }
        public virtual DbSet<BurnRule> BurnRules { get; set; }
        public virtual DbSet<EarnRule> EarnRules { get; set; }
        public virtual DbSet<BulkUploadCustList> BulkUploadCustLists { get; set; }
        public virtual DbSet<feedback_FeedbackMaster> feedback_FeedbackMaster { get; set; }
        public virtual DbSet<SMSEmailMaster> SMSEmailMasters { get; set; }
        public virtual DbSet<FeedBackMobileMaster> FeedBackMobileMasters { get; set; }
        public virtual DbSet<FeedBackMaster> FeedBackMasters { get; set; }
        public virtual DbSet<FeedBackQuestionMaster> FeedBackQuestionMasters { get; set; }
        public virtual DbSet<KonwAboutYouMaster> KonwAboutYouMasters { get; set; }
        public virtual DbSet<LocationMaster> LocationMasters { get; set; }
        public virtual DbSet<GroupDetail> GroupDetails { get; set; }
        public virtual DbSet<LoginDetail> LoginDetails { get; set; }
        public virtual DbSet<BrandDetail> BrandDetails { get; set; }
        public virtual DbSet<OutletDetail> OutletDetails { get; set; }
        public virtual DbSet<CustomerDetail> CustomerDetails { get; set; }        
        public virtual DbSet<OTPMaintenance> OTPMaintenances { get; set; }
        public virtual DbSet<TransactionMaster> TransactionMasters { get; set; }
        public virtual DbSet<TransactionTypeMaster> TransactionTypeMasters { get; set; }
        public virtual DbSet<StoreDetail> StoreDetails { get; set; }
        public virtual DbSet<TransferPointsDetail> TransferPointsDetails { get; set; }
        public virtual DbSet<PointsExpiry> PointsExpiries { get; set; }
        public virtual DbSet<SMSDetail> SMSDetails { get; set; }
        public virtual DbSet<LogDetailsRW> LogDetailsRWs { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<LoginDetail>()
                 .Property(e => e.UserId)
                 .IsUnicode(false);

            modelBuilder.Entity<LoginDetail>()
                .Property(e => e.LoginId)
                .IsUnicode(false);

            modelBuilder.Entity<LoginDetail>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<LoginDetail>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<LoginDetail>()
                .Property(e => e.LevelIndicator)
                .IsUnicode(false);

            modelBuilder.Entity<LoginDetail>()
                .Property(e => e.MobileNo)
                .IsUnicode(false);

            modelBuilder.Entity<LoginDetail>()
                .Property(e => e.LoginType)
                .IsUnicode(false);

            modelBuilder.Entity<BrandDetail>()
                .Property(e => e.GroupId)
                .IsUnicode(false);

            modelBuilder.Entity<BrandDetail>()
                .Property(e => e.BrandId)
                .IsUnicode(false);

            modelBuilder.Entity<BrandDetail>()
                .Property(e => e.BrandName)
                .IsUnicode(false);

            modelBuilder.Entity<BrandDetail>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<BrandDetail>()
                .Property(e => e.ContactNo)
                .IsUnicode(false);

            modelBuilder.Entity<BrandDetail>()
                .Property(e => e.EmailId)
                .IsUnicode(false);

            modelBuilder.Entity<BrandDetail>()
                .Property(e => e.AuthorisedPerson)
                .IsUnicode(false);

            modelBuilder.Entity<BrandDetail>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<BrandDetail>()
                .Property(e => e.State)
                .IsUnicode(false);

            modelBuilder.Entity<BrandDetail>()
                .Property(e => e.PinCode)
                .IsUnicode(false);

            modelBuilder.Entity<BrandDetail>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<BrandDetail>()
                .Property(e => e.MACStatus)
                .IsUnicode(false);

            modelBuilder.Entity<BrandDetail>()
                .Property(e => e.BrandLogoUrl)
                .IsUnicode(false);

            modelBuilder.Entity<BrandDetail>()
                .Property(e => e.SearchCriteriaType)
                .IsUnicode(false);

            modelBuilder.Entity<BrandDetail>()
                .Property(e => e.Category)
                .IsUnicode(false);

            modelBuilder.Entity<BrandDetail>()
                .Property(e => e.PointsProductORBase)
                .IsUnicode(false);

            modelBuilder.Entity<OutletDetail>()
                .Property(e => e.BrandId)
                .IsUnicode(false);

            modelBuilder.Entity<OutletDetail>()
                .Property(e => e.OutletId)
                .IsUnicode(false);

            modelBuilder.Entity<OutletDetail>()
                .Property(e => e.OutletName)
                .IsUnicode(false);

            modelBuilder.Entity<OutletDetail>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<OutletDetail>()
                .Property(e => e.ContactNo)
                .IsUnicode(false);

            modelBuilder.Entity<OutletDetail>()
                .Property(e => e.EmailId)
                .IsUnicode(false);

            modelBuilder.Entity<OutletDetail>()
                .Property(e => e.AuthorisedPerson)
                .IsUnicode(false);

            modelBuilder.Entity<OutletDetail>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<OutletDetail>()
                .Property(e => e.PinCode)
                .IsUnicode(false);

            modelBuilder.Entity<OutletDetail>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<OutletDetail>()
                .Property(e => e.InvoiceNo)
                .IsUnicode(false);

            modelBuilder.Entity<OutletDetail>()
                .Property(e => e.Latitude)
                .IsUnicode(false);

            modelBuilder.Entity<OutletDetail>()
                .Property(e => e.Longitude)
                .IsUnicode(false);

            modelBuilder.Entity<OutletDetail>()
                .Property(e => e.GroupId)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.MobileNo)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.CustomerId)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.CustomerName)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.CardNumber)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.EmailId)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.Gender)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.MaritalStatus)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.EnrollingOutlet)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.MemberGroupId)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.CustomerCategory)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.BillingCustomerId)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerDetail>()
                .Property(e => e.CustomerThrough)
                .IsUnicode(false);
        }
    }
}
