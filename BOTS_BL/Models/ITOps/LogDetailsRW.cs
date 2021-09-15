namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LogDetailsRW")]
    public partial class LogDetailsRW
    {
        [Key]
        public long SlNo { get; set; }

        public string ReceivedData { get; set; }

        public string SendData { get; set; }

        public DateTime? Datetime { get; set; }

        [NotMapped]
        public string datetimestr { get; set; }
    }
}
