namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblCallTypes
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string CallType { get; set; }
    }
}
