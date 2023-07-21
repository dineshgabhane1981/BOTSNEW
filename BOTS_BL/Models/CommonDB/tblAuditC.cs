namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblAuditCS")]
    public partial class tblAuditC
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string GroupId { get; set; }

        [StringLength(150)]
        public string RequestedFor { get; set; }

        [StringLength(50)]
        public string RequestedBy { get; set; }

        public DateTime RequestedDate { get; set; }
    }
}
