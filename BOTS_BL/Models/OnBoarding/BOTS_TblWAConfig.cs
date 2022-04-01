namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblWAConfig
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string GroupId { get; set; }

        [StringLength(50)]
        public string BrandId { get; set; }
        public int SetId { get; set; }
        public bool IsWA { get; set; }

        [StringLength(50)]
        public string WAProvider { get; set; }

        [StringLength(150)]
        public string WANumber { get; set; }

        [StringLength(150)]
        public string WAUsername { get; set; }

        [StringLength(150)]
        public string WAPassword { get; set; }

        [StringLength(250)]
        public string WAlink { get; set; }

        public int? MessageId { get; set; }

        [StringLength(100)]
        public string TokenId { get; set; }

        public string WAScript { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime? AddedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
