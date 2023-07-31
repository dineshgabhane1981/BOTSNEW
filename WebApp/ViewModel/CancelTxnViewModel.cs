using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;

namespace WebApp.ViewModel
{
    public class CancelTxnViewModel
    {
        public CustomerDetail objCustomerDetail { get; set; }
        public CancelTxnModel objCancelTxnModel { get; set; }
        public List<CancelTxnModel> lstCancelTxnModel { get; set; }
        public MemberData objMemberData { get; set; }
    }
}