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

        public MeetingMatrix GetMeetingMatrixReport(string SalesManager, string FrmDate, string ToDate,string MeetingOrCall)
        {
            MeetingMatrix objMeetingMatrix = new MeetingMatrix();
            List<MeetingMatrix> lstMeetingMatrix = new List<MeetingMatrix>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    if(!string.IsNullOrEmpty(SalesManager))
                    {
                        
                    }
                    else
                    {
                        
                        var SMDetails = context.CustomerLoginDetails.Where(x => x.LoginType == "8").ToList();
                        foreach (var item in SMDetails)
                        {
                            MeetingMatrix objMeetingMatrixItem = new MeetingMatrix();
                            objMeetingMatrixItem.LoginId = item.LoginId;
                            objMeetingMatrixItem.SMName = item.UserName;
                            objMeetingMatrixItem.TotalMeeting = context.SALES_tblLeads.Where(x => x.AssignedLead == item.LoginId).ToList().Count();
                            if (MeetingOrCall == "1stMeeting")
                            {
                                objMeetingMatrix.TotalUniqueMeeting = context.SALES_tblLeads.Where(x => x.AssignedLead == item.LoginId && x.MeetingType == "1stMeeting").ToList().Count();
                            }
                            else
                            {
                                objMeetingMatrix.TotalUniqueMeeting = context.SALES_tblLeads.Where(x => x.AssignedLead == item.LoginId && x.MeetingType == "1stcall").ToList().Count();
                            }
                            objMeetingMatrixItem.IntrestedCases = context.SALES_tblLeads.Where(x => x.AssignedLead == item.LoginId && x.LeadStatus == "Interested").ToList().Count();

                            decimal? uniquePer = (objMeetingMatrixItem.TotalUniqueMeeting * 100) / objMeetingMatrix.TotalMeeting;
                            if (uniquePer != null && uniquePer > 0)
                                objMeetingMatrixItem.TotalUniqueMeetingPer = Convert.ToString(uniquePer.Value.ToString("F")) + "%";

                        }                     

                     

                        objMeetingMatrix.IntrestedCases = context.SALES_tblLeads.Where(x => x.LeadStatus == "Interested").ToList().Count();
                        decimal? intPer = (objMeetingMatrix.IntrestedCases * 100) / objMeetingMatrix.TotalMeeting;
                        if (intPer != null && intPer > 0)
                            objMeetingMatrix.IntrestedCasesPer = Convert.ToString(intPer.Value.ToString("F")) + "%";
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "MeetingMatrix");
            }
            return objMeetingMatrix;
        }
    }
}
