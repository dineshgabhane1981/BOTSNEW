namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LogDetail
    {
        [Key]
        public long SlNo { get; set; }

        public string ReceivedData { get; set; }

        public DateTime? Datetime { get; set; }
    }
}
