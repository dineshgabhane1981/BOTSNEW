namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblDealDetails
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(4)]
        public string GroupId { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "numeric")]
        public decimal LoyaltyFees { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? WAPaidPackFees { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? SMSPaidPackFees { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? EcommIntegration { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AnyOtherFees { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TotalFeesA { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int GST { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TotalFeesB { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(20)]
        public string PaymentFrequency { get; set; }

        [StringLength(500)]
        public string AnyOtherFeesDesc { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AmountReceived { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TDSDeducted { get; set; }

        [StringLength(10)]
        public string PaymentMode { get; set; }

        [StringLength(10)]
        public string PaymentStatus { get; set; }

        [StringLength(10)]
        public string GSTRate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AdvanceAmount { get; set; }
    }
}
