namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblRedemptionRequest")]
    public partial class tblRedemptionRequest
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string CustomerId { get; set; }

        [StringLength(50)]
        public string RedemptionType { get; set; }

        [Required]
        [StringLength(50)]
        public string Points { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(25)]
        public string CustomerType { get; set; }
    }
}
