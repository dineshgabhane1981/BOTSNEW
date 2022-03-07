namespace BOTS_BL.Models.OnBoarding
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblVelocityChecksConfig
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string GroupId { get; set; }

        public int VelocityType { get; set; }

        public int? CountFrom { get; set; }

        public int? CountTo { get; set; }

        public int? LastDays { get; set; }

        [StringLength(50)]
        public string Action { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime? AddedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
