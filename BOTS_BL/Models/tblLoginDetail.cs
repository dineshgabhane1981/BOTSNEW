namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblLoginDetail
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string LoginId { get; set; }

        [StringLength(100)]
        public string Password { get; set; }

        [StringLength(50)]
        public string GroupId { get; set; }

        [StringLength(50)]
        public string LevelIndicator { get; set; }

        [StringLength(50)]
        public string LoginType { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string MobileNo { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        public bool? IsActive { get; set; }
    }
}
