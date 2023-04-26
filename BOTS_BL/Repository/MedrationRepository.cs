using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;


namespace BOTS_BL.Repository
{
    public class MedrationRepository
    {
        Exceptions newexception = new Exceptions();
        CustomerRepository CR = new CustomerRepository();
        public bool AddCustomerDetails(List<tblMedrationSubscription> lstData)
        {
            var connStr = CR.GetCustomerConnString("1283");
            bool status = false;
            using (var context = new BOTSDBContext(connStr))
            {
                try
                {
                    foreach (var item in lstData)
                    {
                        item.DateAdded = DateTime.Now;
                        item.DOB = item.DOBOriginal.ToString("yyyy-MM-dd");
                        context.tblMedrationSubscriptions.AddOrUpdate(item);
                        context.SaveChanges();
                        status = true;
                    }
                }
                catch (Exception ex)
                {
                    status = false;
                    newexception.AddException(ex, "AddCustomerDetails");
                }
            }
            return status;
        }
    }
}
