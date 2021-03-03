namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CustomerMapping")]
    public partial class CustomerMapping
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string CustomerId { get; set; }

        [StringLength(50)]
        public string CustomerType { get; set; }

        [StringLength(50)]
        public string MappedCustomerId { get; set; }

        [StringLength(50)]
        public string MappedCustomerType { get; set; }

        [StringLength(500)]
        public string CustomerName { get; set; }

        [StringLength(500)]
        public string MappedCustomerName { get; set; }

        [StringLength(2)]
        public string Status { get; set; }
    }
}
