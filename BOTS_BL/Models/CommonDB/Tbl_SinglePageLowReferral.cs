namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_SinglePageLowReferral
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string GroupName { get; set; }

        [StringLength(100)]
        public string CustomerVintage { get; set; }

        [StringLength(100)]
        public string RedemptionRate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Value { get; set; }

        public DateTime? Date { get; set; }
    }
}
