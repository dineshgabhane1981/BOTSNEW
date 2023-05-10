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
                    obj = context.Database.SqlQuery<EventModuleData>("select C.MobileNo,C.Points,C.CustomerName,C.Gender,C.DOB,C.AnniversaryDate,C.EmailId,min(CC.Address) as Address, CASE WHEN Max(cast(TM.Datetime as date)) = NULL THEN Max(cast(TM.Datetime as date)) ELSE Min(C.DOJ) END as LastTxnDate,DATEADD(MONTH, @PointExp, Max(cast(TM.Datetime as date))) as PointExp from CustomerDetails C Left join TransactionMaster TM on C.MobileNo = TM.MobileNo left join CustomerChild CC on CC.MobileNo = C.MobileNo and C.Status = '00' group by C.MobileNo, C.Points, C.CustomerName, C.EnrollingOutlet, C.Gender, C.DOB, C.AnniversaryDate, C.EmailId Having C.MobileNo = @Mobileno", new SqlParameter("@Mobileno", Mobileno), new SqlParameter("@PointExp", PointExp)).FirstOrDefault();

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

        public bool SaveNewMemberData(EventMemberDetail objData, CustomerDetail objCustomerDetail, CustomerChild objCustomerChild, TransactionMaster objTM, string connectionstring)
        {
            bool result = false;
            CustomerDetail TM2 = new CustomerDetail();
            CustomerChild objdata1 = new CustomerChild();
            OutletDetail objdata = new OutletDetail();
            TransactionMaster obj = new TransactionMaster();
            PointsExpiry obj1 = new PointsExpiry();
            using (var context = new BOTSDBContext(connectionstring))
            {
                try
                {
                     context.EventMemberDetails.AddOrUpdate(objData);
                     context.SaveChanges();
                     result = true;
                     var bonusPoints = context.EventDetails.Where(x => x.EventId == objData.EventId).Select(y=>y.BonusPoints).FirstOrDefault();
                     var existingCust  = context.CustomerDetails.Where(x => x.MobileNo == objCustomerDetail.MobileNo).FirstOrDefault();
                     var existingCust1 = context.CustomerChilds.Where(x => x.MobileNo == objCustomerChild.MobileNo).FirstOrDefault();
                     var ExpiryDays = context.EventDetails.Where(x => x.EventId == objData.EventId).Select(y => y.PointsExpiryDays).FirstOrDefault();
                    if (existingCust == null)
                    {
                        objData.CustomerType = "New";
                    }
                    else
                    {
                        objData.CustomerType = "Existing";
                    }
                    objData.PointsGiven = bonusPoints;
                    if (existingCust == null)
                    {
                        var CustomerId = context.CustomerDetails.OrderByDescending(x => x.CustomerId).Select(y => y.CustomerId).FirstOrDefault();
                        var AdminOutletId = context.OutletDetails.Where(x => x.OutletName.Contains("Admin")).Select(y => y.OutletId).FirstOrDefault();
                        TM2.MobileNo = objCustomerDetail.MobileNo;
                        TM2.CustomerName = objCustomerDetail.CustomerName;
                        TM2.CustomerId = Convert.ToString(Convert.ToInt64(CustomerId) + 1);
                        TM2.CardNumber = objCustomerDetail.CardNumber;
                        TM2.EmailId = objCustomerDetail.EmailId;
                        TM2.DOB = objCustomerDetail.DOB;
                        TM2.AnniversaryDate = objCustomerDetail.AnniversaryDate;
                        TM2.Gender = objCustomerDetail.Gender;
                        TM2.DOJ = DateTime.Now;
                        TM2.EnrollingOutlet = AdminOutletId;
                        TM2.MemberGroupId = "1000";
                        TM2.CustomerThrough = "1";
                        TM2.Status = "00";
                        TM2.OldMobileNo = objCustomerDetail.OldMobileNo;
                        TM2.Points = bonusPoints;
                        
                    }
                    else
                    {
                        TM2 = existingCust;
                        TM2.CustomerName = objCustomerDetail.CustomerName;
                        TM2.CardNumber = objCustomerDetail.CardNumber;
                        TM2.EmailId = objCustomerDetail.EmailId;
                        TM2.DOB = objCustomerDetail.DOB;
                        TM2.AnniversaryDate = objCustomerDetail.AnniversaryDate;
                        TM2.Gender = objCustomerDetail.Gender;
                        TM2.OldMobileNo = objCustomerDetail.OldMobileNo;
                        TM2.Points = TM2.Points + bonusPoints;
                    }
                    context.CustomerDetails.AddOrUpdate(TM2);
                    context.SaveChanges();

                    if (existingCust1 == null)
                    {
                        var CustomerId1 = context.CustomerDetails.OrderByDescending(x => x.CustomerId).Select(y => y.CustomerId).FirstOrDefault();
                        objdata1.MobileNo = objCustomerDetail.MobileNo;
                        objdata1.CustomerId = Convert.ToString(Convert.ToInt64(CustomerId1));
                        objdata1.ChildCount = objCustomerChild.ChildCount;
                        objdata1.Child1DOB = objCustomerChild.Child1DOB;
                        objdata1.Pincode = objCustomerChild.Pincode;
                        objdata1.Address = objCustomerChild.Address;
                        objdata1.PromotionalSMS = objCustomerChild.PromotionalSMS;
                        objdata1.Child2DOB = objCustomerChild.Child2DOB;
                        objdata1.Child3DOB = objCustomerChild.Child3DOB;
                        objdata1.City = objCustomerChild.City;
                        objdata1.LanguagePreferred = objCustomerChild.LanguagePreferred;
                        objdata1.Religion = objCustomerChild.Religion;
                        objdata1.Area = objCustomerChild.Area;
                    }
                    else
                    {
                        objdata1 = existingCust1;
                        objdata1.MobileNo = objCustomerChild.MobileNo;
                        objdata1.CustomerId = objCustomerChild.CustomerId;
                        objdata1.Address = objCustomerChild.Address;

                    }
                    context.CustomerChilds.AddOrUpdate(objdata1);
                    context.SaveChanges();

                    obj.CounterId = (TM2.EnrollingOutlet + "01");
                    obj.MobileNo = TM2.MobileNo;
                    obj.Datetime = DateTime.Now;
                    obj.TransType = "1";
                    obj.TransSource = "1";
                    obj.InvoiceNo = "Bonus";
                    obj.InvoiceAmt = 0;
                    obj.Status = "00";
                    obj.CustomerId = TM2.CustomerId;
                    obj.PointsEarned = bonusPoints;
                    obj.PointsBurned = 0;
                    obj.CampaignPoints = 0;
                    obj.TxnAmt = 0;
                    obj.CustomerPoints = TM2.Points;

                    context.TransactionMasters.AddOrUpdate(obj);
                    context.SaveChanges();

                    obj1.MobileNo = TM2.MobileNo;
                    obj1.CounterId = (TM2.EnrollingOutlet + "01");
                    obj1.EarnDate = DateTime.Now;
                    obj1.ExpiryDate = obj1.EarnDate.Value.AddDays(Convert.ToInt32(ExpiryDays));
                    obj1.Points = TM2.Points;
                    obj1.InvoiceNo = "Bonus";
                    obj1.Status = "00";
                    obj1.Datetime = DateTime.Now;
                    obj1.CustomerId = TM2.CustomerId;

                    context.PointsExpiries.AddOrUpdate(obj1);
                    context.SaveChanges();

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
