namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblStoreMaster")]
    public partial class tblStoreMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SlNo { get; set; }

        [Key]
        [StringLength(50)]
        public string CounterId { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CounterType { get; set; }

        [StringLength(50)]
        public string Securitykey { get; set; }

        [StringLength(50)]
        public string OutletId { get; set; }
    }
}
