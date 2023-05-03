namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EventDetail
    {
        [Key]
        [Column(Order = 0)]
        public long EventId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int GroupId { get; set; }

        public string EventName { get; set; }

        [StringLength(500)]
        public string Place { get; set; }

        [StringLength(500)]
        public string EventType { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EventStartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EventEndDate { get; set; }

        public int? BonusPoints { get; set; }

        public int? PointsExpiryDays { get; set; }

        [Column("1stRemBefore")]
        public int? C1stRemBefore { get; set; }

        [Column("1stReminderScript")]
        public string C1stReminderScript { get; set; }

        [Column("2ndRemBefore")]
        public int? C2ndRemBefore { get; set; }

        [Column("2ndReminderScript")]
        public string C2ndReminderScript { get; set; }

        public string Desciption { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string AddedBy { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "date")]
        public DateTime Addeddate { get; set; }
    }
}
