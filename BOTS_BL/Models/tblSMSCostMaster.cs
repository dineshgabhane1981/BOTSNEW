namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblSMSCostMaster")]
    public partial class tblSMSCostMaster
    {
        [Key]
        public long SLno { get; set; }

        [StringLength(25)]
        public string SMSCost { get; set; }
        public Int64 Value { get; set; }
        public bool? Status { get; set; }
    }
}
