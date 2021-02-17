using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class CustomerConnString
    {
        public string _connString;

        /// <summary>
        /// Get or set the static important data.
        /// </summary>
        public string ConnectionStringCustomer
        {
            get
            {
                return _connString;
            }
            set
            {
                _connString = value;
            }
        }

    }
}
