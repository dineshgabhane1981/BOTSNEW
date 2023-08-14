namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CommonOTPCredentialMaster")]
    public partial class CommonOTPCredentialMaster
    {
        [Key]
        public long SLno { get; set; }

        [StringLength(100)]
        public string LoginId { get; set; }

        [StringLength(100)]
        public string Password { get; set; }

        [StringLength(2)]
        public string LevelIndicator { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedDate { get; set; }

        [StringLength(15)]
        public string LoginType { get; set; }

        [StringLength(1)]
        public string Status { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }

        [StringLength(10)]
        public string DBStatus { get; set; }
    }
}
