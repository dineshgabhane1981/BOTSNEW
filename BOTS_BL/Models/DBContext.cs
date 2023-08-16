using BOTS_BL.Models;
using BOTS_BL.Models.CommonDB;
using BOTS_BL.Models.FeedBack;
using BOTS_BL.Models.IndividualDBModels;
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
        public virtual DbSet<tblTierMaster> tblTierMasters { get; set; }
        public virtual DbSet<tblCustomerDataCollection> tblCustomerDataCollections { get; set; }
        public virtual DbSet<tblBurnPtsSoftBlock> tblBurnPtsSoftBlocks { get; set; }
        public virtual DbSet<tblOTPSMSWhatsAppCredential> tblOTPSMSWhatsAppCredentials { get; set; }
        public virtual DbSet<View_ITOPSCustTxnData> View_ITOPSCustTxnData { get; set; } 
        public virtual DbSet<tblMobileChangeHistory> tblMobileChangeHistories { get; set; }
        public virtual DbSet<tblPtsTransferDetail> tblPtsTransferDetails { get; set; }
        public virtual DbSet<tblLoginDetail> tblLoginDetails { get; set; }
        public virtual DbSet<tblLogDetail> tblLogDetails { get; set; }
        public virtual DbSet<View_ITOPSCustData> View_ITOPSCustData { get; set; }
        public virtual DbSet<tblGroupOwnerInfo> tblGroupOwnerInfoes { get; set; }
        public virtual DbSet<tblProductMaster> tblProductMasters { get; set; }
        public virtual DbSet<ProductMaster> ProductMasters { get; set; }
        public virtual DbSet<tblDLCProfileUpdateConfig_Publish> tblDLCProfileUpdateConfig_Publish { get; set; }
        public virtual DbSet<MWP_ReferTNC> MWP_ReferTNC { get; set; }
        public virtual DbSet<MWP_TNC> MWP_TNC { get; set; }
        public virtual DbSet<tblDLCUserDetail> tblDLCUserDetails { get; set; }
        public virtual DbSet<tblDLCProfileUpdateConfig> tblDLCProfileUpdateConfigs { get; set; }
        public virtual DbSet<CustomerChild> CustomerChilds { get; set; }
        public virtual DbSet<EventDetail> EventDetails { get; set; }
        public virtual DbSet<EventMemberDetail> EventMemberDetails { get; set; }
        public virtual DbSet<tblDLCFrontEndPageDataNew> tblDLCFrontEndPageDataNews { get; set; }
        public virtual DbSet<tblDLCDashboardConfig_Publish> tblDLCDashboardConfig_Publish { get; set; }
        public virtual DbSet<tblDLCDashboardConfig> tblDLCDashboardConfigs { get; set; }
        public virtual DbSet<tblMedrationSubscription> tblMedrationSubscriptions { get; set; }
        public virtual DbSet<SMSOutletMapping> SMSOutletMappings { get; set; }
        public virtual DbSet<CompetitionDetail> CompetitionDetails { get; set; }
        public virtual DbSet<tblEmployee> tblEmployees {    get; set; }
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

        //New DB Structure Tables
        public virtual DbSet<tblBrandMaster> tblBrandMasters { get; set; }
        public virtual DbSet<tblBurnPtsProrataInvAmt> tblBurnPtsProrataInvAmts { get; set; }
        public virtual DbSet<tblCampaignMemberDetail> tblCampaignMemberDetails { get; set; }
        public virtual DbSet<tblCampaignTxnDetail> tblCampaignTxnDetails { get; set; }
        public virtual DbSet<tblCustDetailsMaster> tblCustDetailsMasters { get; set; }
        public virtual DbSet<tblCustInfo> tblCustInfoes { get; set; }
        public virtual DbSet<tblCustPointsMaster> tblCustPointsMasters { get; set; }
        public virtual DbSet<tblCustTxnSummaryMaster> tblCustTxnSummaryMasters { get; set; }
        public virtual DbSet<tblDLCReporting> tblDLCReportings { get; set; }
        public virtual DbSet<tblGroupMaster> tblGroupMasters { get; set; }
        public virtual DbSet<tblOutletMaster> tblOutletMasters { get; set; }
        public virtual DbSet<tblPromoBlastMemberDetail> tblPromoBlastMemberDetails { get; set; }
        public virtual DbSet<tblRuleMaster> tblRuleMasters { get; set; }
        public virtual DbSet<tblSalesReturnMaster> tblSalesReturnMasters { get; set; }
        public virtual DbSet<tblSMSWhatsAppCredential> tblSMSWhatsAppCredentials { get; set; }
        public virtual DbSet<tblSMSWhatsAppScriptMaster> tblSMSWhatsAppScriptMasters { get; set; }
        public virtual DbSet<tblStoreMaster> tblStoreMasters { get; set; }
        public virtual DbSet<tblTxnBonusMaster> tblTxnBonusMasters { get; set; }
        public virtual DbSet<tblTxnDetailsMaster> tblTxnDetailsMasters { get; set; }
        public virtual DbSet<tblTxnDetailsMaster_Clone> tblTxnDetailsMaster_Clone { get; set; }
        public virtual DbSet<tblBulkCustList> tblBulkCustLists { get; set; }
        public virtual DbSet<tblBurnPtsProrataInvAmt_Clone> tblBurnPtsProrataInvAmt_Clone { get; set; }
        public virtual DbSet<tblCampaignMaster> tblCampaignMasters { get; set; }
        public virtual DbSet<tblCampaignMemberDetails_Clone> tblCampaignMemberDetails_Clone { get; set; }
        public virtual DbSet<tblCampaignTxnDetails_Clone> tblCampaignTxnDetails_Clone { get; set; }
        public virtual DbSet<tblCelebrationMaster> tblCelebrationMasters { get; set; }
        public virtual DbSet<tblCelebrationMemberDetail> tblCelebrationMemberDetails { get; set; }
        public virtual DbSet<tblCelebrationTxnDetail> tblCelebrationTxnDetails { get; set; }
        public virtual DbSet<tblDLCFrontEndPageData> tblDLCFrontEndPageDatas { get; set; }
        public virtual DbSet<tblDLCFrontEndPageDataReferTNC> tblDLCFrontEndPageDataReferTNCs { get; set; }
        public virtual DbSet<tblDLCFrontEndPageDataTNC> tblDLCFrontEndPageDataTNCs { get; set; }
        public virtual DbSet<tblDLCGiftPtsDetail> tblDLCGiftPtsDetails { get; set; }
        public virtual DbSet<tblDLCNewRegMaster> tblDLCNewRegMasters { get; set; }
        public virtual DbSet<tblDLCProfileUpdatedList> tblDLCProfileUpdatedLists { get; set; }
        public virtual DbSet<tblDLCRegMemberDetail> tblDLCRegMemberDetails { get; set; }
        public virtual DbSet<tblDLCRuleMaster> tblDLCRuleMasters { get; set; }
        public virtual DbSet<tblDLCSMSWAScriptMaster> tblDLCSMSWAScriptMasters { get; set; }
        public virtual DbSet<tblErrorServerLog> tblErrorServerLogs { get; set; }
        public virtual DbSet<tblExpiredPointsMaster> tblExpiredPointsMasters { get; set; }
        public virtual DbSet<tblInActiveMaster> tblInActiveMasters { get; set; }
        public virtual DbSet<tblInActiveMemberDetail> tblInActiveMemberDetails { get; set; }
        public virtual DbSet<tblOTPDetail> tblOTPDetails { get; set; }
        public virtual DbSet<tblPointsExpiryMaster> tblPointsExpiryMasters { get; set; }
        public virtual DbSet<tblPointsExpiryMemberDetail> tblPointsExpiryMemberDetails { get; set; }
        public virtual DbSet<tblProductRuleMaster> tblProductRuleMasters { get; set; }
        public virtual DbSet<tblProfileUpdateMaster> tblProfileUpdateMasters { get; set; }
        public virtual DbSet<tblPromoBlastMaster> tblPromoBlastMasters { get; set; }
        public virtual DbSet<tblPromoBlastMemberDetails_Clone> tblPromoBlastMemberDetails_Clone { get; set; }
        public virtual DbSet<tblSalesReturnMaster_Clone> tblSalesReturnMaster_Clone { get; set; }
        public virtual DbSet<tblSalesReturnProDetailsMaster> tblSalesReturnProDetailsMasters { get; set; }
        public virtual DbSet<tblSalesReturnProDetailsMaster_Clone> tblSalesReturnProDetailsMaster_Clone { get; set; }
        public virtual DbSet<tblTxnProDetailsMaster> tblTxnProDetailsMasters { get; set; }
        public virtual DbSet<tblTxnProDetailsMaster_Clone> tblTxnProDetailsMaster_Clone { get; set; }
        



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
