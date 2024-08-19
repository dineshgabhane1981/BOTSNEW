using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.App_Start;
using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using System.Configuration;
using WebApp.ViewModel;
using Newtonsoft.Json.Linq;

namespace WebApp.Controllers.ITOPS
{
    public class EnableWhatsAppReportController : Controller
    {
        // GET: EnableWhatsAppReport
        Exceptions newexception = new Exceptions();
        ITOPSNEWRepository objITOPSRepo = new ITOPSNEWRepository();
        EventsRepository EvtRepo = new EventsRepository();

        public ActionResult Index()
        {
            var groupId = (string)Session["GroupId"];
            string GoupAPILink = ConfigurationManager.AppSettings["GroupAPILink"].ToString();
            ITOPSAddWAReportViewModel ObjViewModel = new ITOPSAddWAReportViewModel();
            ListWAGroupDetailsModel objDetail = new ListWAGroupDetailsModel();
            try
            {
                var WAReportDetails = objITOPSRepo.GetWAReportData(groupId, GoupAPILink);

                ObjViewModel.objWADetails = new WAReportDetailsViewModel();
                if(WAReportDetails.ObjWADetails.Groupid == null)
                {
                    ObjViewModel.objWADetails.Groupid = groupId;
                }
                else
                {
                    ObjViewModel.objWADetails.Groupid = WAReportDetails.ObjWADetails.Groupid;
                }
                ObjViewModel.objWADetails.GroupName = WAReportDetails.ObjWADetails.GroupName;
                ObjViewModel.objWADetails.WAGroupName = WAReportDetails.ObjWADetails.WAGroupName;
                ObjViewModel.objWADetails.GroupCode = WAReportDetails.ObjWADetails.GroupCode;
                ObjViewModel.objWADetails.Status = WAReportDetails.ObjWADetails.Status;
                JObject jobj = JObject.Parse(WAReportDetails.ObjWADetails.APIData);
                JToken jToken = jobj.Last.Last();
                foreach (JToken child in jToken.Children())
                {
                    ListWAGroupDetailsViewModel obj = new ListWAGroupDetailsViewModel();
                    if (child.HasValues)
                    {
                        obj.id = Convert.ToString(child.First.First);
                        obj.name = Convert.ToString(child.First.Next.First);
                        ObjViewModel.LstWAAPIDetails.Add(obj);
                        if (ObjViewModel.objWADetails.GroupCode == obj.id)
                        {
                            ObjViewModel.objWADetails.WAGroupName = obj.name;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "Index");
            }   
            return View(ObjViewModel);
        }

        [HttpPost]
        public ActionResult SaveReportDetails(string Groupid,string Groupcode)
        {
            bool result = default;
            tblAudit objAudit = new tblAudit();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];

            try
            {
                objAudit.GroupId = userDetails.GroupId;
                objAudit.RequestedFor = userDetails.UserName;
                objAudit.AddedBy = userDetails.LoginId;
                objAudit.AddedDate = EvtRepo.IndianDatetime();
                objAudit.RequestedOn = EvtRepo.IndianDatetime();

                result = objITOPSRepo.InsertGroupCode(Groupid, Groupcode, objAudit);
            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "SaveReportDetails");
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}