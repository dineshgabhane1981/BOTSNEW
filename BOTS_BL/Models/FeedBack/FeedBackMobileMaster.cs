namespace BOTS_BL.Models.FeedBack
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FeedBackMobileMaster")]
    public partial class FeedBackMobileMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(10)]
        public string MobileNo { get; set; }

        [StringLength(50)]
        public string MessageId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Datetime { get; set; }
    }
}
