namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OutletDetail
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(5)]
        public string BrandId { get; set; }

        [StringLength(8)]
        public string OutletId { get; set; }

        [StringLength(50)]
        public string OutletName { get; set; }

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

        [StringLength(6)]
        public string PinCode { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(2)]
        public string Status { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MemberEnrolmentPoints { get; set; }

        [StringLength(25)]
        public string InvoiceNo { get; set; }

        [StringLength(25)]
        public string Latitude { get; set; }

        [StringLength(25)]
        public string Longitude { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? FeedBackPoints { get; set; }

        [NotMapped]
        public DateTime? ProgramStartDate { get; set; }
        [NotMapped]
        public DateTime? ProgramRenewalDate { get; set; }
    }
}
