namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblCustomerDataCollection")]
    public partial class tblCustomerDataCollection
    {
        [Key]
        public int SlNo { get; set; }

        [Required]
        [StringLength(250)]
        public string CustomerName { get; set; }

        [Required]
        [StringLength(50)]
        public string MobileNo { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        public DateTime AddedDate { get; set; }
    }
}
