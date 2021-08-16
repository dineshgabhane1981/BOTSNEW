namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblPaymentDetails
    {
        [Key]
        public long SINo { get; set; }

        [Required]
        [StringLength(4)]
        public string GroupId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TotalFees { get; set; }

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
