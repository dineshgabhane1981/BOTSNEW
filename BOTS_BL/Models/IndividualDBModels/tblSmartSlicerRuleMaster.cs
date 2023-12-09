namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblSmartSlicerRuleMaster")]
    public partial class tblSmartSlicerRuleMaster
    {
        [Key]
        [StringLength(50)]
        public string GroupId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AvgTicketSize { get; set; }
    }
}
