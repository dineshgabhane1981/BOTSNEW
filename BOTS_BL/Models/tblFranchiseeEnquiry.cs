namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblFranchiseeEnquiry")]
    public partial class tblFranchiseeEnquiry
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string MobileNo { get; set; }

        [StringLength(250)]
        public string CustomerName { get; set; }

        [StringLength(50)]
        public string AreaOfFranchisee { get; set; }

        public DateTime AddedDate { get; set; }
    }
}
