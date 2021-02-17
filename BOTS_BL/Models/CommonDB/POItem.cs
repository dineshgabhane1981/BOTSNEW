namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class POItem
    {
        [Key]
        public long SlNo { get; set; }

        public string Item { get; set; }

        [StringLength(25)]
        public string Grade { get; set; }

        [StringLength(10)]
        public string Location { get; set; }

        public int? ItemPrice { get; set; }

        public int? DiscAmt { get; set; }

        public int? Total { get; set; }

        public string Item_Name { get; set; }

        public int? Quantity { get; set; }

        public int? Rate { get; set; }

        public string PoNo { get; set; }
    }
}
