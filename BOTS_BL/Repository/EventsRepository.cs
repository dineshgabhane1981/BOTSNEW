using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using BOTS_BL.Models.EventModule;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

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
                        lstData = context.tblGroupDetails.Where(x => x.IsEvent == null || x.IsEvent == false).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetNeverOptForGroups");
            }
            return lstData;   
        }

        public bool EnableEventModule(string GroupId)
        {
            bool status = false;
            try
            {
                {
                    tblGroupDetail obj = new tblGroupDetail();
                    int varid = Convert.ToInt32(GroupId);
                    using (var context = new CommonDBContext())
                    {
                        var groupDetail = context.tblGroupDetails.Where(x => x.GroupId == varid).FirstOrDefault();
                        groupDetail.IsEvent = true;
                        context.SaveChanges();
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

        public bool DisableEventModule(string GroupId)
        {
            bool status = false;
            try
            {
                {
                    tblGroupDetail obj = new tblGroupDetail();
                    int varid = Convert.ToInt32(GroupId);
                    using (var context = new CommonDBContext())
                    {
                        var groupDetail = context.tblGroupDetails.Where(x => x.GroupId == varid).FirstOrDefault();
                        groupDetail.IsEvent = false;
                        context.SaveChanges();
                        status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "DisableEventModule");
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

            try
            {
                using (var context = new BOTSDBContext(connectionString))
                {
                    listEvent = context.EventDetails.Where(x => x.Status == null || x.Status != "Deleted").ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetListEvents");
            }

            return listEvent;
        }

        public bool EventDelete(string EventId, string GroupId, string connectionstring)
        {
            bool status = false;
            try
            {
                EventDetail ObjEventDetails = new EventDetail();
                using (var context = new BOTSDBContext(connectionstring))
                {
                    int varid = Convert.ToInt32(EventId);
                    ObjEventDetails = context.EventDetails.Where(x => x.EventId == varid).FirstOrDefault();
                    ObjEventDetails.Status = "Deleted";

                    context.EventDetails.AddOrUpdate(ObjEventDetails);
                    context.SaveChanges();
                }
                status = true;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "EventDelete");
            }
            return status;
        }

        public EventDetail GetEditEvents(string groupId, string eventid, string connectionString)
        {
            EventDetail obj = new EventDetail();

            int Id = Convert.ToInt32(eventid);
            using (var context = new BOTSDBContext(connectionString))
            {
                obj = context.EventDetails.Where(x => x.EventId == Id).FirstOrDefault();
            }

            return obj;
        }

        public EventModuleData GetCustomerDetails(string groupId, string Mobileno, string Place, string connectionString)
        {
            EventModuleData obj = new EventModuleData();
            try
            {
                using (var context = new BOTSDBContext(connectionString))
                {
                    var pointsexp = context.EarnRules.Select(e => e.PointsExpiryVariableDate).FirstOrDefault();
                    int PointExp = Convert.ToInt32(pointsexp);
                    obj = context.Database.SqlQuery<EventModuleData>("select C.Mobileno,Min(C.CustomerName) as CustomerName ,Min(C.Gender) as Gender,Min(C.DOB),Min(C.AnniversaryDate) as AnniversaryDate,Min(C.EmailId) as EmailId, Min(C.Points) as Points, Max(cast(T.Datetime as date)) as LastTxnDate, DATEADD(MONTH, @PointExp, Max(cast(T.Datetime as date))) as PointExp from CustomerDetails C inner join TransactionMaster T on C.MobileNo = T.MobileNo where T.MobileNo = @Mobileno Group by C.Mobileno", new SqlParameter("@Mobileno", Mobileno), new SqlParameter("@PointExp", PointExp)).FirstOrDefault();

                    if(obj == null)
                    {
                        obj = new EventModuleData();
                    }
                }
            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "GetCustomerDetails");
            }            
                return obj;
        }

        public bool SaveNewMemberData(EventMemberDetail objData, string connectionstring)
        {
            bool result = false;
            using (var context = new BOTSDBContext(connectionstring))
            {
                try
                {
                        context.EventMemberDetails.AddOrUpdate(objData);
                        context.SaveChanges();
                        result = true;              
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "SaveNewMemberData");
                }
            }
            return result;
        }
    }
    
}
