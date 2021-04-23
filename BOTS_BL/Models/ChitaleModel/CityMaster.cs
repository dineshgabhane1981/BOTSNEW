namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CityMaster")]
    public partial class CityMaster
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        public long? SubClusterId { get; set; }
    }
}
