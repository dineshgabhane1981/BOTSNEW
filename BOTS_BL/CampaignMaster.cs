namespace BOTS_BL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CampaignMaster")]
    public partial class CampaignMaster
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(10)]
        public string CampaignId { get; set; }

        [StringLength(100)]
        public string LoginId { get; set; }

        public DateTime? Datetime { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }

        [StringLength(1000)]
        public string SegmentName { get; set; }

        [StringLength(100)]
        public string CampaignName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        [StringLength(50)]
        public string RewardType { get; set; }

        [StringLength(50)]
        public string RewardPointsType { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? RewardPoints { get; set; }

        public int? RewardNoOfTxn { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? RewardAmountSpend { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? RewardMinInvoiceAmt { get; set; }

        public long? ControlBase { get; set; }

        public long? CampaignBase { get; set; }

        [StringLength(100)]
        public string CampaignType { get; set; }

        [StringLength(100)]
        public string Status { get; set; }

        [StringLength(1)]
        public string BackEndStatus { get; set; }

        public long? UniqueMemberTransacted { get; set; }

        public long? Transactions { get; set; }

        public long? BizGenerated { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Conversion { get; set; }

        [StringLength(100)]
        public string CampaignBackEndType { get; set; }

        public string Script { get; set; }

        [StringLength(1)]
        public string CommunicationMode { get; set; }

        public string DLTScript { get; set; }

        [StringLength(50)]
        public string DLTStatus { get; set; }

        [StringLength(50)]
        public string DLTRejectReson { get; set; }

        [StringLength(100)]
        public string TemplateID { get; set; }

        [StringLength(250)]
        public string TemplateName { get; set; }

        [StringLength(100)]
        public string TemplateType { get; set; }
    }
}
