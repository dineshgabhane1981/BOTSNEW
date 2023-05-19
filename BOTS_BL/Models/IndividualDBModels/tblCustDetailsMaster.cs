namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblCustDetailsMaster")]
    public partial class tblCustDetailsMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SlNo { get; set; }

        [Key]
        [StringLength(50)]
        public string MobileNo { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DOB { get; set; }

        [Column(TypeName = "date")]
        public DateTime? AnniversaryDate { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Category { get; set; }

        [StringLength(50)]
        public string CardNo { get; set; }

        [StringLength(50)]
        public string Gender { get; set; }

        [StringLength(50)]
        public string EnrolledOutlet { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DOJ { get; set; }

        [StringLength(50)]
        public string Tier { get; set; }

        public bool? IsActive { get; set; }

        public bool? DisableTxn { get; set; }

        public bool? DisableSMSWA { get; set; }

        [StringLength(50)]
        public string EnrolledBy { get; set; }

        [StringLength(50)]
        public string CountryCode { get; set; }

        [StringLength(50)]
        public string CurrentEnrolledOutlet { get; set; }
    }
}
