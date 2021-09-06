namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ListOfGroup
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(100)]
        public string GroupName { get; set; }

        [StringLength(50)]
        public string DBName { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }
    }
}
