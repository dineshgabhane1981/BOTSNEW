namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SubClusterMaster")]
    public partial class SubClusterMaster
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(100)]
        public string SubCluster { get; set; }

        public long? ClusterId { get; set; }
    }
}
