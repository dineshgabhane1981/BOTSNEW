namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblNPCDetail
    {
        [Key]
        public int Slno { get; set; }

        [StringLength(10)]
        public string CustomerMobileNo { get; set; }

        public string CustomerName { get; set; }

        public string EmployeeName { get; set; }

        public string CategoryName { get; set; }

        public string SubCategoryName { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NextVisitDay { get; set; }

        public string Remarks { get; set; }
    }
}
