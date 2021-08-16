namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblInstallmentDetails
    {
        [Key]
        public long SINo { get; set; }

        [Required]
        [StringLength(4)]
        public string GroupId { get; set; }

        public int Installment { get; set; }

        [Column(TypeName = "date")]
        public DateTime PaymentDate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PaymentAmount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PaidAmount { get; set; }

        [StringLength(50)]
        public string PaymentType { get; set; }

        public long? ChequeNo { get; set; }

        [StringLength(150)]
        public string Bank { get; set; }

        [NotMapped]
        public string PaymentDateStr { get; set; }
    }
}
