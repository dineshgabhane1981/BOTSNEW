namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MWP_Details
    {
        [Key]
        [StringLength(2)]
        public string MWP_Id { get; set; }

        [StringLength(1000)]
        public string MWP_Name { get; set; }

        [StringLength(5)]
        public string BrandId { get; set; }

        [StringLength(2)]
        public string Status { get; set; }
    }
}
