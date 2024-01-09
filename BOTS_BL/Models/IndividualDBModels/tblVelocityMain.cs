namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblVelocityMain")]
    public partial class tblVelocityMain
    {
        [Key]
        public int SlNo { get; set; }

        public int Days { get; set; }

        public int NoOfTxn { get; set; }

        public int NoOfCustomers { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateCalculated { get; set; }
    }
}
