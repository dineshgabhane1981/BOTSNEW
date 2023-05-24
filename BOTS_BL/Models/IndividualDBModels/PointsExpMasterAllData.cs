using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.IndividualDBModels
{
    public class PointsExpMasterAllData
    {
        //public string Mobileno { get; set; } 
        //public decimal ExpiredPoints { get; set; }
        //public DateTime? ExpiredDate { get; set; }
        //public decimal BalancePoints { get; set; }
        //public string PointsType { get; set; }
        //public string PointsCategory { get; set; }
        //public string Counterid { get; set; }
        //public string Outletid { get; set; }

        public string Mobileno { get; set; }
        public decimal Points { get; set; }
        public string PointsType { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
