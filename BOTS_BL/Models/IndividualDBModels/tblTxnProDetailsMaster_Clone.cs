namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblTxnProDetailsMaster_Clone
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string MobileNo { get; set; }

        [StringLength(50)]
        public string InvoiceNo { get; set; }

        [StringLength(50)]
        public string OutletId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ProductAmt { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ProductRate { get; set; }

        [StringLength(50)]
        public string ProductQty { get; set; }

        [StringLength(50)]
        public string ProductGST { get; set; }

        [StringLength(50)]
        public string ProductCode { get; set; }

        [StringLength(50)]
        public string CategoryCode { get; set; }

        [StringLength(50)]
        public string SubCategoryCode { get; set; }

        [StringLength(50)]
        public string BrandCode { get; set; }

        [StringLength(50)]
        public string DepartCode { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PointsEarned { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PointsBurned { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ProductAmtWithoutGST { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Carret { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ProductMakingCharges { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ProductWastageAmount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ProductOtherCharges { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ProductTotalOfmakingWastageOtherC { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ProductDiscount { get; set; }
    }
}
