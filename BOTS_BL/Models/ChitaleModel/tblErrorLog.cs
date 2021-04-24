namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblErrorLog")]
    public partial class tblErrorLog
    {
        [Key]
        public long Logid { get; set; }

        
        public string ExceptionMsg { get; set; }

        
        public string ExceptionType { get; set; }

        public string ExceptionSource { get; set; }

        
        public string ExceptionURL { get; set; }

        public DateTime? Logdate { get; set; }
    }
}
