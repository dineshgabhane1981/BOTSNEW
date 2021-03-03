using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models.ChitaleModel;

namespace BOTS_BL.Repository
{
    public class ChitaleDashboardRepository
    {
        public CustomerDetail GetCustomerDetail(string CustomerId)
        {
            CustomerDetail objCustomerDetail = new CustomerDetail();
            using (var context = new ChitaleDBContext())
            {
                objCustomerDetail = context.CustomerDetails.Where(x => x.CustomerId == CustomerId).FirstOrDefault();
            }
            return objCustomerDetail;
        }
    }
}
