using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models;
using BOTS_BL.Models.CommonDB;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.IO;
using BOTS_BL;
using System.Data.Entity.Validation;
using System.Net;
using System.Web.Script.Serialization;
using System.Configuration;
using BOTS_BL.Models.SalesLead;


namespace BOTS_BL.Repository
{
    public class LeadReportsRepository
    {
        Exceptions newexception = new Exceptions();

        public List<MeetingMatrix> GetMeetingMatrixReport(string SalesManager, string FrmDate, string ToDate, string MeetingOrCall)
        {
            MeetingMatrix objMeetingMatrix = new MeetingMatrix();
            List<MeetingMatrix> lstMeetingMatrix = new List<MeetingMatrix>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    DateTime FromDate = Convert.ToDateTime(FrmDate);
                    DateTime ToDateNew = Convert.ToDateTime(ToDate);
                    var SMDetails = context.CustomerLoginDetails.Where(x => x.LoginType == "8").ToList();
                    foreach (var item in SMDetails)
                    {
                        MeetingMatrix objMeetingMatrixItem = new MeetingMatrix();
                        objMeetingMatrixItem.LoginId = item.LoginId;
                        objMeetingMatrixItem.SMName = item.UserName;
                        if (MeetingOrCall == "Meeting")
                        {
                            objMeetingMatrixItem.TotalMeeting = context.SALES_tblLeadTracking.Where(x => x.AssignedLead == item.LoginId && (x.ContactType == "OnlineMeeting" || x.ContactType == "PersonalMeeting")
                            && x.AddedDate >= FromDate && x.AddedDate <= ToDateNew).ToList().Count();
                        }
                        else
                        {
                            objMeetingMatrixItem.TotalMeeting = context.SALES_tblLeadTracking.Where(x => x.AssignedLead == item.LoginId && x.ContactType == "Call"
                            && x.AddedDate >= FromDate && x.AddedDate <= ToDateNew).ToList().Count();
                        }
                        if (MeetingOrCall == "Meeting")
                        {
                            objMeetingMatrixItem.TotalUniqueMeeting = context.SALES_tblLeadTracking.Where(x => x.AssignedLead == item.LoginId && x.MeetingType == "1stMeeting" && x.AddedDate >= FromDate && x.AddedDate <= ToDateNew).ToList().Count();
                        }
                        else
                        {
                            objMeetingMatrixItem.TotalUniqueMeeting = context.SALES_tblLeadTracking.Where(x => x.AssignedLead == item.LoginId && x.MeetingType == "1stcall" && x.AddedDate >= FromDate && x.AddedDate <= ToDateNew).ToList().Count();
                        }

                        if (objMeetingMatrixItem.TotalMeeting != 0)
                        {
                            decimal? uniquePer = (objMeetingMatrixItem.TotalUniqueMeeting * 100) / objMeetingMatrixItem.TotalMeeting;
                            if (uniquePer != null && uniquePer > 0)
                                objMeetingMatrixItem.TotalUniqueMeetingPer = Convert.ToString(uniquePer.Value.ToString("F")) + "%";
                            else
                                objMeetingMatrixItem.TotalUniqueMeetingPer = "0.00%";
                        }
                        else
                        {
                            objMeetingMatrixItem.TotalUniqueMeetingPer = "0.00%";
                        }

                        if (MeetingOrCall == "Meeting")
                        {
                            objMeetingMatrixItem.IntrestedCases = context.SALES_tblLeadTracking.Where(x => x.AssignedLead == item.LoginId && x.LeadStatus == "Interested" && (x.ContactType == "OnlineMeeting" || x.ContactType == "PersonalMeeting") && x.AddedDate >= FromDate && x.AddedDate <= ToDateNew).ToList().GroupBy(y => y.LeadId).Count();
                        }
                        else
                        {
                            objMeetingMatrixItem.IntrestedCases = context.SALES_tblLeadTracking.Where(x => x.AssignedLead == item.LoginId && x.LeadStatus == "Interested" && x.ContactType == "Call" && x.AddedDate >= FromDate && x.AddedDate <= ToDateNew).ToList().GroupBy(y => y.LeadId).Count();
                        }

                        if (objMeetingMatrixItem.TotalMeeting != 0)
                        {
                            decimal? intrestedCasesPer = (objMeetingMatrixItem.IntrestedCases * 100) / objMeetingMatrixItem.TotalMeeting;
                            if (intrestedCasesPer != null && intrestedCasesPer > 0)
                                objMeetingMatrixItem.IntrestedCasesPer = Convert.ToString(intrestedCasesPer.Value.ToString("F")) + "%";
                            else
                                objMeetingMatrixItem.IntrestedCasesPer = "0.00%";
                        }
                        else
                        {
                            objMeetingMatrixItem.IntrestedCasesPer = "0.00%";
                        }

                        if (MeetingOrCall == "Meeting")
                        {
                            objMeetingMatrixItem.PersonalMeeting = context.SALES_tblLeadTracking.Where(x => x.AssignedLead == item.LoginId && x.ContactType == "PersonalMeeting" && x.AddedDate >= FromDate && x.AddedDate <= ToDateNew).ToList().Count();
                            if (objMeetingMatrixItem.TotalMeeting != 0)
                            {
                                decimal? PersonalMeetingPer = (objMeetingMatrixItem.PersonalMeeting * 100) / objMeetingMatrixItem.TotalMeeting;
                                if (PersonalMeetingPer != null && PersonalMeetingPer > 0)
                                    objMeetingMatrixItem.PersonalMeetingPer = Convert.ToString(PersonalMeetingPer.Value.ToString("F")) + "%";
                                else
                                    objMeetingMatrixItem.PersonalMeetingPer = "0.00%";
                            }
                            else
                            {
                                objMeetingMatrixItem.PersonalMeetingPer = "0.00%";
                            }

                            objMeetingMatrixItem.ZoomMeeting = context.SALES_tblLeadTracking.Where(x => x.AssignedLead == item.LoginId && x.ContactType == "OnlineMeeting" && x.AddedDate >= FromDate && x.AddedDate <= ToDateNew).ToList().Count();
                            if (objMeetingMatrixItem.TotalMeeting != 0)
                            {
                                decimal? ZoomMeetingPer = (objMeetingMatrixItem.ZoomMeeting * 100) / objMeetingMatrixItem.TotalMeeting;
                                if (ZoomMeetingPer != null && ZoomMeetingPer > 0)
                                    objMeetingMatrixItem.ZoomMeetingPer = Convert.ToString(ZoomMeetingPer.Value.ToString("F")) + "%";
                                else
                                    objMeetingMatrixItem.ZoomMeetingPer = "0.00%";
                            }
                            else
                            {
                                objMeetingMatrixItem.ZoomMeetingPer = "0.00%";
                            }
                        }
                        else
                        {
                            objMeetingMatrixItem.PersonalMeeting = 0;
                            objMeetingMatrixItem.ZoomMeeting = 0;
                            objMeetingMatrixItem.PersonalMeetingPer = "0.00%";
                            objMeetingMatrixItem.ZoomMeetingPer = "0.00%";
                        }
                        int count = 0;
                        if (MeetingOrCall == "Meeting")
                        {
                            count = (from c in context.SALES_tblLeads
                                     join cc in context.SALES_tblLeadTracking on c.LeadId equals cc.LeadId
                                     where c.MeetingType == "Followup" && c.UpdatedDate == null 
                                     && (c.ContactType == "OnlineMeeting" || c.ContactType == "PersonalMeeting")
                                     && c.AddedDate >= FromDate && c.AddedDate <= ToDateNew
                                     && c.AssignedLead == item.LoginId
                                     select new SalesLead
                                     {
                                         LeadId = c.LeadId
                                     }).ToList().Count();
                        }
                        if (MeetingOrCall == "Calling")
                        {
                            count = (from c in context.SALES_tblLeads
                                     join cc in context.SALES_tblLeadTracking on c.LeadId equals cc.LeadId
                                     where c.MeetingType == "Followup" && c.UpdatedDate == null
                                     && c.ContactType == "Call"  && c.AddedDate >= FromDate && c.AddedDate <= ToDateNew
                                     && c.AssignedLead == item.LoginId
                                     select new SalesLead
                                     {
                                         LeadId = c.LeadId
                                     }).ToList().Count();
                        }

                        objMeetingMatrixItem.UntouchedFollowUp = count;
                        lstMeetingMatrix.Add(objMeetingMatrixItem);

                    }

                    if (!string.IsNullOrEmpty(SalesManager))
                    {
                        lstMeetingMatrix = lstMeetingMatrix.Where(x => x.LoginId == SalesManager).ToList();
                    }

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "MeetingMatrix");
            }
            return lstMeetingMatrix;
        }
    }
}
