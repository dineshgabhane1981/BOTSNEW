namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ReportForDownload")]
    public partial class ReportForDownload
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(200)]
        public string GroupName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }

        public string DownloadLink { get; set; }

        [StringLength(100)]
        public string ReportType { get; set; }

        public string Remarks { get; set; }
    }
}
