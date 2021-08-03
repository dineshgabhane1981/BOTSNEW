namespace BOTS_BL.Models.OnBoarding
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblInstallmentDetails
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
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Installment { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "date")]
        public DateTime PaymentDate { get; set; }

        [Key]
        [Column(Order = 4, TypeName = "numeric")]
        public decimal PaymentAmount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PaidAmount { get; set; }

        [StringLength(50)]
        public string PaymentType { get; set; }

        public long? ChequeNo { get; set; }

        [StringLength(150)]
        public string Bank { get; set; }
    }
}
