namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ClusterMaster")]
    public partial class ClusterMaster
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(100)]
        public string Cluster { get; set; }
    }
}
