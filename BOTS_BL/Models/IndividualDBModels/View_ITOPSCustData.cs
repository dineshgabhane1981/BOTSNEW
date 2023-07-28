namespace BOTS_BL.Models.IndividualDBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class View_ITOPSCustData
    {
        [Key]
        [StringLength(50)]
        public string MobileNo { get; set; }

        [StringLength(100)]
        public string CustomerName { get; set; }

        [StringLength(50)]
        public string EnrolledOutlet { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DOJ { get; set; }

        [StringLength(50)]
        public string CardNo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Points { get; set; }

        [StringLength(20)]
        public string CustomerId { get; set; }
    }
}
