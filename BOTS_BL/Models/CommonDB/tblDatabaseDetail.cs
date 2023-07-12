namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblDatabaseDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SlNo { get; set; }

        [Key]
        [StringLength(50)]
        public string CounterId { get; set; }

        [StringLength(50)]
        public string SecurityKey { get; set; }

        [StringLength(50)]
        public string IPAddress { get; set; }

        [StringLength(100)]
        public string DBName { get; set; }

        [StringLength(200)]
        public string DBPassword { get; set; }

        [StringLength(50)]
        public string DBId { get; set; }

        [StringLength(50)]
        public string GroupId { get; set; }

        public bool? IsActive { get; set; }
    }
}
