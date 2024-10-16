namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblCRDataset")]
    public partial class tblCRDataset
    {
        [Key]
        public int DSId { get; set; }

        [StringLength(250)]
        public string DSName { get; set; }

        public string DSCriteria { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime AddedDate { get; set; }

        public string DSCriteriaForQuery { get; set; }
        public string DSTxnQuery { get; set; }
        public string DSCumuQuery { get; set; }
        public string DSProdQuery { get; set; }

    }
}
