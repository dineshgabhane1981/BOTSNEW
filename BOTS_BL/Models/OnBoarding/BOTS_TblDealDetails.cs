namespace BOTS_BL.Models.OnBoarding
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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long SINo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(4)]
        public string GroupId { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "numeric")]
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
        [Column(Order = 3)]
        public bool GST { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TotalFeesB { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(20)]
        public string PaymentFrequency { get; set; }
    }
}
