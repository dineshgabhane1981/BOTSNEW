namespace BOTS_BL.Models.OnBoarding
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblProductUploadConfig
    {
        [Key]
        public long SrNo { get; set; }

        [StringLength(4)]
        public string GroupId { get; set; }

        [StringLength(5)]
        public string BrandId { get; set; }

        [StringLength(4)]
        public string CategoryId { get; set; }

        [StringLength(50)]
        public string DepartmentId { get; set; }

        [StringLength(50)]
        public string BrandCode { get; set; }

        [StringLength(50)]
        public string ProductCode { get; set; }

        [StringLength(50)]
        public string ProductSubCategoryCode { get; set; }

        [StringLength(50)]
        public string ProductCategoryCode { get; set; }

        [StringLength(50)]
        public string Carret { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? LoyaltyPercentage { get; set; }

        [StringLength(50)]
        public string UploadType { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime? AddedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
