namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblErrorServerLog")]
    public partial class tblErrorServerLog
    {
        [Key]
        public long SlNo { get; set; }

        public string ErrorNumber { get; set; }

        public DateTime? Date_time { get; set; }

        public string ErrorProcedure { get; set; }

        public string ErrorSeverity { get; set; }

        public string ErrorState { get; set; }

        public string ErrorLine { get; set; }

        public string ErrorMessage { get; set; }

        public string ReceivedData { get; set; }

        public DateTime? INDDatetime { get; set; }
    }
}
