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
        public List<CustomerDetail> GetTop5Participant(string type, string CustomerId, string CustomerType)
        {
            List<CustomerDetail> objTop5Participant = new List<CustomerDetail>();
            using (var context = new ChitaleDBContext())
            {
                if (CustomerType == "Sales Executive")
                {
                    try
                    {
                        var IDs = context.SalesmanMappings.Where(x => x.SalesExecutive == CustomerId).Select(y => y.ParticipantId).ToList();
                        objTop5Participant = context.CustomerDetails.Where(a =>a.CustomerType== type && IDs.Contains(a.CustomerId)).OrderByDescending(x => x.Points).Take(5).ToList();
                        var count = IDs.Count;
                    }
                    catch(Exception ex)
                    {
                        newexception.AddException(ex);
                    }
                }
            }

            return objTop5Participant;
        }

        public List<CustomerDetail> Bottom5Participants(string type, string CustomerId, string CustomerType)
        {
            List<CustomerDetail> objTop5Participant = new List<CustomerDetail>();
            using (var context = new ChitaleDBContext())
            {
                if (CustomerType == "Sales Executive")
                {
                    try
                    {
                        var IDs = context.SalesmanMappings.Where(x => x.SalesExecutive == CustomerId).Select(y => y.ParticipantId).ToList();
                        objTop5Participant = context.CustomerDetails.Where(a => a.CustomerType == type && IDs.Contains(a.CustomerId)).OrderBy(x => x.Points).Take(5).OrderByDescending(y => y.Points).ToList();
                        var count = IDs.Count;
                    }
                    catch (Exception ex)
                    {
                        newexception.AddException(ex);
                    }
                }
            }

            return objTop5Participant;
        }
    }
}
