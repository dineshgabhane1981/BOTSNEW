namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblRetailMaster
    {
        [Key]
        public long SINo { get; set; }

        [StringLength(50)]
        public string RetailId { get; set; }

        [StringLength(50)]
        public string CategoryId { get; set; }

        [StringLength(100)]
        public string CategoryName { get; set; }

        [StringLength(100)]
        public string BrandName { get; set; }

        public long? NoOfOutlets { get; set; }

        public long? NoOfEnrolled { get; set; }

        [StringLength(50)]
        public string BOProduct { get; set; }

        [StringLength(100)]
        public string BillingPartner { get; set; }

        [StringLength(100)]
        public string BillingProduct { get; set; }
    }
}