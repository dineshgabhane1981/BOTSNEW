namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblUniquePoint
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime? AddedDate { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }
    }
}
