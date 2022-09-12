namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblEmployee
    {
        public int Id { get; set; }

        [StringLength(150)]
        public string EmpName { get; set; }

        [StringLength(12)]
        public string EmpMobileNo { get; set; }
    }
}
