namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RedemptionValues")]
    public partial class RedemptionValue
    {
        [Key]
        public string Type { get; set; }
        public decimal? CaseIncentive { get; set; }
        public decimal? InfraStructure { get; set; }
        public decimal? Deposit { get; set; }
        public decimal? Promotion { get; set; }
        public DateTime? Datetime { get; set; }
        public string Status { get; set; }
    }
}
