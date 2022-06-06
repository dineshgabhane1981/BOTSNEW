namespace BOTS_BL.Models.OnBoarding
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GroupIdMapping")]
    public partial class GroupIdMapping
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(100)]
        public string OnboardingGroupId { get; set; }

        [StringLength(500)]
        public string OnboardingGroupName { get; set; }

        [StringLength(100)]
        public string LiveGroupId { get; set; }

        [StringLength(500)]
        public string LiveGroupName { get; set; }

        [StringLength(50)]
        public string ActiceStatus { get; set; }

        [StringLength(100)]
        public string LiveDBName { get; set; }
    }
}
