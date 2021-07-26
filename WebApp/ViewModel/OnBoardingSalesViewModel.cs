using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BOTS_BL.Models;

namespace WebApp.ViewModel
{
    public class OnBoardingSalesViewModel
    {
        public BOTS_TblGroupMaster bots_TblGroupMaster { get; set; }
        public BOTS_TblRetailMaster bots_TblRetailMaster { get; set; }
    }
}