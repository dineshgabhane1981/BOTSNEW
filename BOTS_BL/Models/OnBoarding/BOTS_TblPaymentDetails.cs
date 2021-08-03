namespace BOTS_BL.Models.OnBoarding
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblPaymentDetails
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long SINo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(4)]
        public string GroupId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TotalFees { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(150)]
        public string PaymentCleared { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AmountReceived { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AmountBalance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NoOfInstallments { get; set; }
    }
}
