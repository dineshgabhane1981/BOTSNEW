using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.IndividualDBModels
{
    public class CustInformatonData
    {
        public string MobileNo { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime? AnniversaryDate { get; set; }
        public string Gender { get; set; }
        public DateTime? DOJ { get; set; }
        public int Age { get; set; }  

    }
}
