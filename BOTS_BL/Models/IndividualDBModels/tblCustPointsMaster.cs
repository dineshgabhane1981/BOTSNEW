namespace BOTS_BL.Models.IndividualDBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblCustPointsMaster")]
    public partial class tblCustPointsMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SlNo { get; set; }

        [Required]
        [StringLength(50)]
        public string MobileNo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Points { get; set; }

        [StringLength(50)]
        public string PointsType { get; set; }

        [StringLength(50)]
        public string PointsDesc { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MinInvoiceAmtRequired { get; set; }

        
        
    }
}
