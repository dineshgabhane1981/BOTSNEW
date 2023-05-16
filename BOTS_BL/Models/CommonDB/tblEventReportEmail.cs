namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblEventReportEmail
    {
        [Key]
        public int SlNo { get; set; }

        public int GroupId { get; set; }

        [Required]
        [StringLength(250)]
        public string EmailId { get; set; }
    }
}
