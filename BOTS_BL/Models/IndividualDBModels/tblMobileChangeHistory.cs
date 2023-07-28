namespace BOTS_BL.Models.IndividualDBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblMobileChangeHistory")]
    public partial class tblMobileChangeHistory
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string OldMobileNo { get; set; }

        [StringLength(50)]
        public string NewMobileNo { get; set; }

        [StringLength(50)]
        public string GroupId { get; set; }

        public DateTime? TxnDatetime { get; set; }
    }
}
