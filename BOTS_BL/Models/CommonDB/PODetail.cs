namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PODetail
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(10)]
        public string PoNo { get; set; }

        [StringLength(25)]
        public string SupplierCode { get; set; }

        [StringLength(10)]
        public string Currency { get; set; }

        public DateTime? DocDate { get; set; }

        public string Address_ { get; set; }

        public string Shipment_Mode { get; set; }

        public string Location { get; set; }

        public string Payment_Terms { get; set; }
    }
}
