namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblDiscussionCustomerData")]
    public partial class tblDiscussionCustomerData
    {
        [Key]
        public int SlNo { get; set; }

        [StringLength(50)]
        public string MobileNo { get; set; }

        [StringLength(250)]
        public string CustomerName { get; set; }
    }
}
