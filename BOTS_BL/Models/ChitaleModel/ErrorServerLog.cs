namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ErrorServerLog")]
    public partial class ErrorServerLog
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
    }
}
