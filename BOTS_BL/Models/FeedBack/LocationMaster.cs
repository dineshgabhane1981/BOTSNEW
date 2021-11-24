namespace BOTS_BL.Models.FeedBack
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LocationMaster")]
    public partial class LocationMaster
    {
        [Key]
        [StringLength(10)]
        public string LocationId { get; set; }

        [StringLength(100)]
        public string Location { get; set; }
    }
}
