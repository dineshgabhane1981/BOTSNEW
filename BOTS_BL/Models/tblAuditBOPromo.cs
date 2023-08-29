namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblAuditBOPromo")]
    public partial class tblAuditBOPromo
    {
        public long Id { get; set; }

        public string RequestedFor { get; set; }

        public string RequestedBy { get; set; }

        public DateTime? RequestedDate { get; set; }

        [StringLength(100)]
        public string Type { get; set; }
    }
}
