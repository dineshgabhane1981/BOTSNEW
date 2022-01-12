namespace BOTS_BL.Models.CommonDB
{
    using Newtonsoft.Json;
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
        public string Status { get; set; }
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
    public class User
    {
        public string userId { get; set; }
        public string userName { get; set; }
        public string userType { get; set; }
        public string emailId { get; set; }
        public string mobileNo { get; set; }
        public string domainName { get; set; }
        public string regDate { get; set; }
        public string userStatus { get; set; }
        public string enableCMS { get; set; }
        public string fullName { get; set; }
        public string postalAddress { get; set; }
        public string postalCity { get; set; }
        public string postalCountry { get; set; }
        public string postalRegion { get; set; }
        public string smppEnabled { get; set; }
        public string accountType { get; set; }
        public string smsBalance { get; set; }
        public string dltEntityId { get; set; }
        public string mmRouting { get; set; }
        public string expDate { get; set; }
    }

    public class UserList
    {
        public User user { get; set; }
    }

    public class Response
    {
        public string api { get; set; }
        public string action { get; set; }
        public string status { get; set; }
        public string msg { get; set; }
        public string code { get; set; }
        public int count { get; set; }
        public List<UserList> userList { get; set; }
    }

    public class Root
    {
        public Response response { get; set; }
    }

    
}
