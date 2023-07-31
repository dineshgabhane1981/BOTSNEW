namespace BOTS_BL.Models.IndividualDBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class View_ITOPSCustTxnData
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string MobileNo { get; set; }

        public DateTime? TxnDatetime { get; set; }

        [StringLength(50)]
        public string InvoiceNo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? InvoiceAmt { get; set; }

        [StringLength(50)]
        public string TxnType { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PointsEarned { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string MobileNoInvId { get; set; }

        [StringLength(100)]
        public string OutletName { get; set; }
    }
}
