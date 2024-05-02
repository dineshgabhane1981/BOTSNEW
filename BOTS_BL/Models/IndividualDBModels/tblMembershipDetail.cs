namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblMembershipDetail
    {
        [Key]
        public int SlNo { get; set; }

        [Required]
        [StringLength(50)]
        public string MobileNo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PackageType { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PackageValidity { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "numeric")]
        public decimal? RemainingAmount { get; set; }

        public bool IsActive { get; set; }
    }
}
