namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblDLCUserDetail
    {
        [Key]
        public int SlNo { get; set; }

        [Required]
        [StringLength(50)]
        public string MobileNo { get; set; }

        [Required]
        [StringLength(150)]
        public string Password { get; set; }

        public DateTime AddedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
