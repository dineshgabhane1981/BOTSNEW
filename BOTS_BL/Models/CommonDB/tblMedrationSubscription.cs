namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblMedrationSubscription")]
    public partial class tblMedrationSubscription
    {
        [Key]
        public long SlNo { get; set; }

        [Required]
        [StringLength(250)]
        public string CustomerName { get; set; }

        public long ContactNo { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

       
        public string DOB { get; set; }

        [NotMapped]
        public DateTime DOBOriginal { get; set; }

        public bool IsPrimary { get; set; }

        public long PrimaryNo { get; set; }

        public int PlanId { get; set; }

        [Required]
        [StringLength(150)]
        public string PlanName { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
