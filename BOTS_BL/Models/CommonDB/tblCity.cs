namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblCity")]
    public partial class tblCity
    {
        [Key]
        public int CityId { get; set; }

        [Required]
        [StringLength(250)]
        public string CityName { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
    public class CityDetails
    {
        
        public int CityId { get; set; }
        
        public string CityName { get; set; }
       
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string UserName { get; set; }
    }
}
