using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BOTS_BL.Models;

namespace WebApp.ViewModel
{
    public class CommunicationConfigViewModel
    {
        public List<BOTS_TblSMSConfig> SMSConfig { get; set; }
        public List<BOTS_TblWAConfig> WAConfig { get; set; }
        public BOTS_TblCommunicationSet objSetDetails { get; set; }
    }
}