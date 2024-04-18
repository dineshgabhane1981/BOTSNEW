namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblDeactivatedGroupList")]
    public partial class tblDeactivatedGroupList
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(100)]
        public string GroupName { get; set; }

        [StringLength(50)]
        public string DBName { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DeactivationDate { get; set; }
    }
}
