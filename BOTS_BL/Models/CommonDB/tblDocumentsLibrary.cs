namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblDocumentsLibrary")]
    public partial class tblDocumentsLibrary
    {
        [Key]
        public long SlNo { get; set; }

        public string GroupId { get; set; }

        public string GroupName { get; set; }

        [StringLength(100)]
        public string Department { get; set; }

        [StringLength(100)]
        public string DocumentType { get; set; }

        public string Path { get; set; }

        [StringLength(2)]
        public string Status { get; set; }

        [StringLength(100)]
        public string UploadedBy { get; set; }

        public DateTime? UploadDate { get; set; }

        public string Comments { get; set; }

        [StringLength(4000)]
        public string FileName { get; set; }
        [NotMapped]
        public string UploadDateStr { get; set; }
        [StringLength(100)]
        public string  Vendor { get; set; }
    }
}
