namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblReference
    {
        [Key]
        public int SlNo { get; set; }

        [Required]
        [StringLength(50)]
        public string ReferredMobileNo { get; set; }

        [Required]
        [StringLength(50)]
        public string RefereeMobileNo { get; set; }

        [Required]
        [StringLength(150)]
        public string ReferredName { get; set; }

        public DateTime Datetime { get; set; }
    }
}
