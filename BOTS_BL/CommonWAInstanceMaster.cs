namespace BOTS_BL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CommonWAInstanceMaster")]
    public partial class CommonWAInstanceMaster
    {
        [Key]
        public long SLno { get; set; }

        [StringLength(200)]
        public string GroupName { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }

        public string TokenId { get; set; }

        [StringLength(2)]
        public string Status { get; set; }

        [StringLength(100)]
        public string InstanceName { get; set; }
    }
}
