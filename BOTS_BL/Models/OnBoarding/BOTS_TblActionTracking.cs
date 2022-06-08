namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblActionTracking
    {
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string GroupId { get; set; }

        [StringLength(150)]
        public string ActionTaken { get; set; }

        [Required]
        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime AddedDate { get; set; }
    }
}
