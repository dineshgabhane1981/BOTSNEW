namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EventMemberDetail
    {
        [Key]
        [Column(Order = 0)]
        public int SLno { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int GroupId { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EventId { get; set; }

        [StringLength(10)]
        public string Mobileno { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        [StringLength(20)]
        public string Gender { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DOB { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DOA { get; set; }

        public string Address { get; set; }

        [StringLength(500)]
        public string EmailId { get; set; }

        [StringLength(10)]
        public string AlternateNo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PointsGiven { get; set; }

        public string Place { get; set; }

        public DateTime? DateOfRegistration { get; set; }

        [StringLength(10)]
        public string CustomerType { get; set; }
    }
}
