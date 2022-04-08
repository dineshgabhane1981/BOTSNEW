namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblLoginLog")]
    public partial class tblLoginLog
    {
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string LoginId { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        public DateTime LoggedInTime { get; set; }
    }
}
