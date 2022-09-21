namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GroupDetails
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(200)]
        public string GroupName { get; set; }

        [StringLength(200)]
        public string IPAddress { get; set; }

        [StringLength(100)]
        public string DBName { get; set; }

        [StringLength(25)]
        public string DBPassword { get; set; }

        [StringLength(25)]
        public string DBId { get; set; }

        [StringLength(5)]
        public string GroupId { get; set; }

        [StringLength(25)]
        public string ActiveStatus { get; set; }
    }
}
