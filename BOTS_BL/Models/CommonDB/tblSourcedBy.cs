namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblSourcedBy")]
    public partial class tblSourcedBy
    {
        [Key]
        public int SourcedbyId { get; set; }

        [Required]
        [StringLength(250)]
        public string SourcedbyName { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        
    }
    public  class SourcedDetails
    {
        
        public int SourcedbyId { get; set; }        
        
        public string SourcedbyName { get; set; }
        
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string UserName { get; set; }


    }
}
