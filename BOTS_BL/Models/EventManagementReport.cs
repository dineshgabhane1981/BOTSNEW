namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EventManagementReport")]
    public partial class EventManagementReport
    {
        [Key]
        [Column(Order = 0)]
        public long SLno { get; set; }

        [StringLength(100)]
        public string GroupName { get; set; }

        [Key]
        [Column(Order = 1)]
        public bool ReportStatus { get; set; }

        public string Description { get; set; }

        [StringLength(200)]
        public string DBName { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }
    }
}
