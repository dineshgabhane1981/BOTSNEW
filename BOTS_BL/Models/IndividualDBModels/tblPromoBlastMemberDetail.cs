namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblPromoBlastMemberDetail
    {
        [Key]
        public long SlNo { get; set; }

        public long? CampaignId { get; set; }

        [StringLength(50)]
        public string MobileNo { get; set; }

        [StringLength(50)]
        public string CustType { get; set; }

        public long? TotalTxnCount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BusinessGenerated { get; set; }
    }
}
