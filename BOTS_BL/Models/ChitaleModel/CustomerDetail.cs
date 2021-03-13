namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CustomerDetail
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string CustomerId { get; set; }

        [StringLength(100)]
        public string CustomerName { get; set; }

        [StringLength(25)]
        public string CustomerType { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Points { get; set; }

        [StringLength(16)]
        public string BOCustomerId { get; set; }

        [StringLength(2)]
        public string Status { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(100)]
        public string Cluster { get; set; }

        [StringLength(100)]
        public string SubCluster { get; set; }

        [StringLength(10)]
        public string MobileNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EnrolledDate { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(100)]
        public string Town { get; set; }

        [StringLength(100)]
        public string Taluka { get; set; }

        [StringLength(100)]
        public string District { get; set; }

        [StringLength(100)]
        public string State { get; set; }

        [StringLength(10)]
        public string Pincode { get; set; }

        [StringLength(20)]
        public string PanNo { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(100)]
        public string GSTNo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CashIncentive { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? InfraStructure { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Deposit { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Promotion { get; set; }

        [NotMapped]
        public string Type { get; set; }
        
        [NotMapped]
        public string CustomerCategory { get; set; }

        [NotMapped]
        public string CurrentRank { get; set; }

    }
}
