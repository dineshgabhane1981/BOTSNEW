using BOTS_BL.Models;
using System;
using System.Collections.Generic;
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

        public bool SaveEventData(string GroupId, string EventName, string EventDate, string EventPlace, string EventType, string EventStrDate, string EventEndDate, string BonusPoints, string PointsExp, string FirstRemaindScript, string FirstRemaindDate, string SecondRemaindScript, string SecondRemdDate, string Description)
        {
            bool status = false;

            try
            {

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveEventData");
            }
            return status;
        }
    }
    
}
