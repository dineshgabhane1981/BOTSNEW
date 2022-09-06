namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CommonSMSGateWayMaster")]
    public partial class CommonSMSGateWayMaster
    {
        [Key]
        public long Slno { get; set; }

        [StringLength(100)]
        public string SMSVendor { get; set; }

        [StringLength(150)]
        public string UserName { get; set; }

        [StringLength(150)]
        public string Password { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }

        [StringLength(2)]
        public string Status { get; set; }

        [StringLength(100)]
        public string GroupName { get; set; }

        [StringLength(6)]
        public string BOCode { get; set; }

        [NotMapped]
        public string smsBalance { get; set; }
    }
}
