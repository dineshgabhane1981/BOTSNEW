namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblRMAssigned")]
    public partial class tblRMAssigned
    {
        [Key]
        public int RMAssignedId { get; set; }

        [Required]
        [StringLength(250)]
        public string RMAssignedName { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
    }

    public class RMAssignedDetails
    {
        
        public int RMAssignedId { get; set; }
       
        public string RMAssignedName { get; set; }
        
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string UserName { get; set; }
    }
}
