using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BOTS_BL.Models.ChitaleModel;

namespace Chitale.ViewModel
{
    public class TgtVsAchViewModel
    {

        public TgtvsAchMaster objOverAll { get; set; }
        public List<TgtvsAchMaster> objCategory { get; set; }
        public List<TgtvsAchMaster> objSubCategory { get; set; }
        public List<TgtvsAchMaster> objProducts { get; set; }
    }
}