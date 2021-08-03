namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GroupDetail
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }

        [StringLength(50)]
        public string GroupName { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(6)]
        public string PinCode { get; set; }

        [StringLength(15)]
        public string ContactNo { get; set; }

        [StringLength(50)]
        public string EmailId { get; set; }

        [StringLength(50)]
        public string AuthorisedPerson { get; set; }

        public int? NoOfBrands { get; set; }

        public DateTime? CreatedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpiryDate { get; set; }

        [StringLength(2)]
        public string Status { get; set; }
    }
}
