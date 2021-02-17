namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BrandDetail
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }

        [StringLength(5)]
        public string BrandId { get; set; }

        [StringLength(50)]
        public string BrandName { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(15)]
        public string ContactNo { get; set; }

        [StringLength(50)]
        public string EmailId { get; set; }

        [StringLength(50)]
        public string AuthorisedPerson { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(6)]
        public string PinCode { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(2)]
        public string Status { get; set; }

        [StringLength(1)]
        public string MACStatus { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Slab1InvoiceAmt { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Slab2InvoiceAmt { get; set; }

        [StringLength(100)]
        public string BrandLogoUrl { get; set; }

        [StringLength(1)]
        public string SearchCriteriaType { get; set; }

        [StringLength(50)]
        public string Category { get; set; }

        [StringLength(1)]
        public string PointsProductORBase { get; set; }
    }
}
