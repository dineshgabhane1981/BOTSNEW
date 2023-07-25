namespace BOTS_BL.Models.IndividualDBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblLogDetail
    {
        [Key]
        public long SlNo { get; set; }

        public string ReceivedData { get; set; }

        public DateTime? ServerDatetime { get; set; }

        [NotMapped]
        public string datetimestr { get; set; }
    }
}
