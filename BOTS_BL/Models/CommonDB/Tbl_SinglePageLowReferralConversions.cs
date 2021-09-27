namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_SinglePageLowReferralConversions
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string GroupName { get; set; }

        [StringLength(100)]
        public string CustomerVintage { get; set; }

        [StringLength(100)]
        public string RedemptionRate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Value { get; set; }

        public DateTime? Date { get; set; }
    }

    public class SinglepageLowerMetrics
    {
        
        public string GroupNamelessthan5 { get; set; }
        public double? Valuethanlessthan5 { get; set; }
        public string GroupNamelessthan10 { get; set; }
        public double? Valuelessthan10 { get; set; }
        public string GroupNamelessthan15 { get; set; }
        public double? Valuelessthan15 { get; set; }
        public string GroupNamelessthan30 { get; set; }
        public double? Valuelessthan30 { get; set; }
        public string GroupNamelessthan40 { get; set; }
        public double? Valuelessthan40 { get; set; }


    }
    //public class commonclass1
    //{
    //    public string GroupName { get; set; }
    //    public string Value { get; set; }
    //}
    //public class commonclass2
    //{
    //    public string GroupName { get; set; }
    //    public string Value { get; set; }
    //}
    //public class commonclass3
    //{
    //    public string GroupName { get; set; }
    //    public string Value { get; set; }
    //}
    //public class commonclass4
    //{
    //    public string GroupName { get; set; }
    //    public string Value { get; set; }
    //}
    //public class commonclass5
    //{
    //    public string GroupName { get; set; }
    //    public string Value { get; set; }
    //}
}
