namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblGroupMaster")]
    public partial class tblGroupMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SlNo { get; set; }

        [Key]
        [StringLength(50)]
        public string GroupId { get; set; }

        [StringLength(100)]
        public string GroupName { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(10)]
        public string CountryCode { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(100)]
        public string State { get; set; }

        [StringLength(100)]
        public string Country { get; set; }

        [StringLength(100)]
        public string Area { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedDate { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(100)]
        public string AuthorisedPerson { get; set; }

        public int? NoOfBrands { get; set; }

        public bool? IsActive { get; set; }
    }
}
