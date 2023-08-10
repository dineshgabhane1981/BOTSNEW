namespace BOTS_BL.Models.IndividualDBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblPtsTransferDetail
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(50)]
        public string PtsFromMobileNo { get; set; }

        [StringLength(50)]
        public string PtsToMobileNo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PtsTransferred { get; set; }

        public DateTime? TxnDatetime { get; set; }

        [StringLength(50)]
        public string GroupId { get; set; }

        public bool? IsActive { get; set; }
        //public string MobileNoPtsId { get; set; }
    }
}
