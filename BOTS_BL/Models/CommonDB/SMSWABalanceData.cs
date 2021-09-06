namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMSWABalanceData")]
    public partial class SMSWABalanceData
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string BrandName { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string OutletName { get; set; }

        [StringLength(50)]
        public string WABalance { get; set; }

        [StringLength(50)]
        public string WAExpiryDate { get; set; }

        [StringLength(50)]
        public string SMSBalance { get; set; }

        [StringLength(50)]
        public string VirtualSMSBalance { get; set; }

        public DateTime? Date { get; set; }

       

    }
    public class CommunicationsinglePageData
    {
        public List<SMSBalance> objSMSBalance { get; set; }
        public List<WhatsAppBalance> objWhatsAppBalance { get; set; }
        public List<VirtualSMSBalance> objVirtualSMSBalance { get; set; }
        public List<WhatsAppExpiryDate> objWhatsAppExpiryDate { get; set; }
    }
    public class SMSBalance
    {
        public string BrandName { get; set; }
        public string OutletName { get; set; }
        public string SmsBalance { get; set; }
    }
    public class WhatsAppBalance
    {
        public string BrandName { get; set; }
        public string OutletName { get; set; }
        public string WABalance { get; set; }

    }
    public class VirtualSMSBalance
    {
        public string BrandName { get; set; }
        public string OutletName { get; set; }
        public string VirtualsmsBalance { get; set; }
    }
    public class WhatsAppExpiryDate
    {
        public string BrandName { get; set; }
        public string OutletName { get; set; }
        public string WAExpiryDate { get; set; }

    }

}
