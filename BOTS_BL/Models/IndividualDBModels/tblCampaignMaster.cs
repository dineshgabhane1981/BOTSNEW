namespace BOTS_BL.Models.IndividualDBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblCampaignMaster")]
    public partial class tblCampaignMaster
    {
        [Key]
        public long CampaignId { get; set; }

        [StringLength(500)]
        public string CampaignName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BonusPoints { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MinInvAmt { get; set; }

        [StringLength(200)]
        public string CampaignStatus { get; set; }

        public bool? BackEndStatus { get; set; }

        public DateTime? Datetime { get; set; }

        public long? ControlBase { get; set; }

        public long? CampaignBase { get; set; }

        public long? TotalTxnCount { get; set; }

        public long? UniqueTxnCount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BusinessGenerated { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Conversion { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TotalBonusPointsIssued { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TotalBonusPointsRedeemed { get; set; }

        public long? TotalCustCount { get; set; }

        [StringLength(50)]
        public string CommunicationMode { get; set; }

        public string CampaignScript { get; set; }
    }
}
