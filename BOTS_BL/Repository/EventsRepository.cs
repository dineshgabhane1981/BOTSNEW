using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Repository
{
    public class EventsRepository
    {
        Exceptions newexception = new Exceptions();
        public List<tblGroupDetail> GetNeverOptForGroups(bool status)
        {
            List<tblGroupDetail> lstData = new List<tblGroupDetail>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    if (status)
                        lstData = context.tblGroupDetails.Where(x => x.IsEvent == true).ToList();
                    else
                        lstData = context.tblGroupDetails.Where(x => x.IsEvent == null).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetNeverOptForGroups");
            }
            return lstData;   
        }

        public bool EnableEventModule(string GroupId, List<tblGroupDetail> contentData)
        {
            bool status = false;
            try
            {
                {
                    tblGroupDetail obj = new tblGroupDetail();
                    int varid = Convert.ToInt32(GroupId);
                    using (var context = new CommonDBContext())
                    {                        
                        var groupDetails = context.tblGroupDetails.Where(x => x.GroupId == varid).FirstOrDefault();
                        groupDetails.IsEvent = true;                        
                        context.SaveChanges();
                        if (contentData != null)
                        {
                            foreach (var item in contentData)
                            {
                                context.tblGroupDetails.Add(item);
                                context.SaveChanges();
                            }
                        }
                        status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "EnableEventModule");
            }
            return status;
        }

        public bool SaveEventData(EventDetail Obj,string connectionstring)
        {
            bool status = false;

            try
            {
                using (var context = new BOTSDBContext(connectionstring))
                {
                    context.EventDetails.AddOrUpdate(Obj);
                    context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveEventData");
            }
            return status;
        }

        public List<EventDetail> GetListEvents(string GroupId,string connectionString)
        {
            List<EventDetail> listEvent = new List<EventDetail>();

            return listEvent;
        }
    }
    
}
