namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CompetitionDetail
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(100)]
        public string StudentName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DOB { get; set; }

        [StringLength(500)]
        public string SchoolName { get; set; }

        [StringLength(50)]
        public string ClassStandard { get; set; }

        [StringLength(100)]
        public string ParentName { get; set; }

        [StringLength(50)]
        public string WhatsAppNo { get; set; }

        [StringLength(500)]
        public string EmailId { get; set; }

        [StringLength(1000)]
        public string HomeAddress { get; set; }
    }

    public class VinusResponse
    {
        public string ResponseCode { get; set; }

        public string ResponseMessage { get; set; }
    }
}
