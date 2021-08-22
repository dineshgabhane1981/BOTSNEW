namespace BOTS_BL.Models.CommonDB
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

        [StringLength(100)]
        public string ExceptionType { get; set; }

        public string ExceptionSource { get; set; }

        [StringLength(100)]
        public string ExceptionURL { get; set; }

        public DateTime? Logdate { get; set; }

        [StringLength(50)]
        public string GroupId { get; set; }
    }
}
