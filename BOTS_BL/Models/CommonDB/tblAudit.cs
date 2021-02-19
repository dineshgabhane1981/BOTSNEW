namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblAudit")]
    public partial class tblAudit
    {
        [Key]
        public int RequestId { get; set; }

        [StringLength(50)]
        public string RequestedFor { get; set; }

        [StringLength(50)]
        public string RequestedEntity { get; set; }

        [StringLength(5)]
        public string GroupId { get; set; }

        [StringLength(150)]
        public string RequestedBy { get; set; }

        [StringLength(50)]
        public string RequestedOnForum { get; set; }

        public DateTime RequestedOn { get; set; }
    }
}
