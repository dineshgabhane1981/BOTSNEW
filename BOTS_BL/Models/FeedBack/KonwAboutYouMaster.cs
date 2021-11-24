namespace BOTS_BL.Models.FeedBack
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KonwAboutYouMaster")]
    public partial class KonwAboutYouMaster
    {
        [Key]
        [StringLength(50)]
        public string KnowAboutYouId { get; set; }

        [StringLength(100)]
        public string KnowAboutYou { get; set; }
    }
}
