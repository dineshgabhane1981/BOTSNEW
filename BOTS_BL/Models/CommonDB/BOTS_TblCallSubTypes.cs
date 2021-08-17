namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblCallSubTypes
    {
        public int Id { get; set; }

        public int CallTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string CallSubType { get; set; }
    }
}
