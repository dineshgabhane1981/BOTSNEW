namespace BOTS_BL.Models.ChitaleModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SalesmanMapping")]
    public partial class SalesmanMapping
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(200)]
        public string NationalHead { get; set; }

        [StringLength(200)]
        public string ZonalHead { get; set; }

        [StringLength(200)]
        public string StateHead { get; set; }

        [StringLength(200)]
        public string AreaSalesManager { get; set; }

        [StringLength(200)]
        public string SalesExecutive { get; set; }

        [StringLength(200)]
        public string SalesOfficer { get; set; }

        [StringLength(200)]
        public string SalesRepresentative { get; set; }

        [StringLength(50)]
        public string SalesmanId { get; set; }

        [StringLength(200)]
        public string SalesmanName { get; set; }

        [StringLength(50)]
        public string ParticipantId { get; set; }

        [StringLength(50)]
        public string ParticipantType { get; set; }

        [StringLength(2)]
        public string Status { get; set; }

        [StringLength(500)]
        public string ParticipantName { get; set; }
    }
}
