namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblRenewalData")]
    public partial class tblRenewalData
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string GroupId { get; set; }

        [StringLength(150)]
        public string CustomerName { get; set; }

        [Column(TypeName = "date")]
        public DateTime RenewalDate { get; set; }

        public decimal? RenewalAmount { get; set; }

        public decimal? DiscountAmount { get; set; }
        public decimal? PaidAmount { get; set; }

        public bool IsPartPayment { get; set; }
        public decimal? PartPaymentAmount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NextPaymentDate { get; set; }

        [StringLength(50)]
        public string PaymentType { get; set; }

        [StringLength(50)]
        public string Frequency { get; set; }

        [Column(TypeName = "date")]
        public DateTime PaymentDate { get; set; }
        [StringLength(250)]
        public string Freebies { get; set; }
        [StringLength(500)]
        public string Comments { get; set; }

        [StringLength(150)]
        public string CSName { get; set; }
        [Required]
        [StringLength(50)]
        public string AddedBy { get; set; }

        [NotMapped]
        public string RenewalDateStr { get; set; }
        [NotMapped]
        public string PaymentDateStr { get; set; }
        [NotMapped]
        public string PartialPayment { get; set; }
        [NotMapped]
        public string PartialPaymentDateStr { get; set; }
    }
}
