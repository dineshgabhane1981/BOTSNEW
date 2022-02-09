namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DLCCreation")]
    public partial class DLCCreation
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(10)]
        public string GroupId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        [StringLength(100)]
        public string DescId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Points { get; set; }

        [StringLength(10)]
        public string MemberType { get; set; }

        public string SMS { get; set; }

        public string WhatsApp { get; set; }

        public string SMSRemainder { get; set; }

        public string WhatsAppRemainder { get; set; }

        public int? RemainderDays { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LinkExpiryDate { get; set; }

        [StringLength(100)]
        public string PointsExpiryType { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PointsExpiryDate { get; set; }

        public int? PointsExpiryVariableDays { get; set; }

        [StringLength(100)]
        public string DescMessage { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime? AddedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
