namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderMaster")]
    public partial class OrderMaster
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string OrderNo { get; set; }

        public DateTime? OrderDatetime { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RavanaDate { get; set; }

        [StringLength(50)]
        public string CustomerId { get; set; }

        [StringLength(25)]
        public string CustomerType { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TotalAmt { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TotalDiscountAmt { get; set; }

        [StringLength(50)]
        public string PONumber { get; set; }

        [StringLength(10)]
        public string OrderStatus { get; set; }

        [StringLength(50)]
        public string RefOrderNo { get; set; }

        [StringLength(50)]
        public string ServiceProviderId { get; set; }

        [StringLength(25)]
        public string ServiceProviderType { get; set; }

        [StringLength(2)]
        public string Status { get; set; }
    }
}
