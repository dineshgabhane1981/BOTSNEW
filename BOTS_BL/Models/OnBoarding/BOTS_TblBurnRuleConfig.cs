namespace BOTS_BL.Models.OnBoarding
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblBurnRuleConfig
    {
        [Key]
        public int RuleId { get; set; }

        [Required]
        [StringLength(50)]
        public string GroupId { get; set; }

        public int? MinInvoiceAmt { get; set; }

        public int? MinRedeemPts { get; set; }

        public int? MinThreshholdPtsFisttime { get; set; }

        public int? MinThreshholdPtsSubsequent { get; set; }

        public bool PartialEarn { get; set; }

        public bool? IsProductCodeBlocking { get; set; }

        [StringLength(50)]
        public string ProductCodeBlockingType { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime? AddedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
