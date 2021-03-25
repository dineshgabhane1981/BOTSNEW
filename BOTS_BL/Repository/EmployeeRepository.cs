using BOTS_BL.Models.ChitaleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Repository
{
    public class EmployeeRepository
    {
        ChitaleException newexception = new ChitaleException();
        public List<CustomerDetail> GetTop5Participant(string type)
        {
            List<CustomerDetail> objTop5Participant = new List<CustomerDetail>();
            using (var context = new ChitaleDBContext())
            {
                objTop5Participant = context.CustomerDetails.Where(a => a.CustomerType == type).OrderByDescending(x => x.Points).Take(5).ToList();
            }

            return objTop5Participant;
        }
    }
}
