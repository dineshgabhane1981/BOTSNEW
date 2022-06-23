namespace BOTS_BL.Models.OnBoarding
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblSlabConfig
    {
        [Key]
        public int SlabId { get; set; }

        public string GroupId { get; set; }        

        public int? SlabFrom { get; set; }

        public int? SlabTo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? SlabPercentage { get; set; }
       
    }
}
