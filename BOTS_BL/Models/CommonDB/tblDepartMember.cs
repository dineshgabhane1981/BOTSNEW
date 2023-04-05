namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblDepartMember")]
    public partial class tblDepartMember
    {
        [Key]
        [Column(Order = 0)]
        public long Slno { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string Department { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(200)]
        public string Members { get; set; }

        public string EmailId { get; set; }

        public string Role { get; set; }

        public string LoginId { get; set; }

        public string status { get; set; }
    }
}
