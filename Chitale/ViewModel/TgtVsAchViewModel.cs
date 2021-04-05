using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models.ChitaleModel;

namespace Chitale.ViewModel
{
    public class TgtVsAchViewModel
    {

        public TgtvsAchMaster objOverAll { get; set; }
        public List<TgtvsAchMaster> objCategory { get; set; }
        public List<TgtvsAchMaster> objSubCategory { get; set; }
        public List<TgtvsAchMaster> objProducts { get; set; }
        public string ParticipantName { get; set; }
        public List<SelectListItem> MonthItems { get; set; }
        public List<SelectListItem> YearItems { get; set; }
    }
}