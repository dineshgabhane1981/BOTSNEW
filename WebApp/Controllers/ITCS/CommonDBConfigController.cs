using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApp.ViewModel;

namespace WebApp.Controllers.ITCS
{
    public class CommonDBConfigController : Controller
    {
        ITCSRepository ITCSR = new ITCSRepository();
        Exceptions newexception = new Exceptions();
        // GET: CommonDBConfig
        public ActionResult ChangeCSName()
        {
            return View();
        }
        public ActionResult EnableProgramme()
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.lstNotActive = ITCSR.GetNeverOptForGroups(false);
            objData.lstActive = ITCSR.GetNeverOptForGroups(true);
            return View(objData);
        }
        public ActionResult DisableProgrammeDetails(string jsonData)
        {
            bool status = false;
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                    string GroupId = Convert.ToString(item["GroupId"]);
                    status = ITCSR.DisableProgrammeDetails(GroupId);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "DisableProgrammeDetails");
            }

            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult EnableProgrammeDetails(string jsonData)
        {
            bool status = false;
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                    string GroupId = Convert.ToString(item["GroupId"]);
                    status = ITCSR.EnableProgrammeDetails(GroupId);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "EnableProgrammeDetails");
            }

            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult ChangeWAScript()
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.lstGroupDetails = ITCSR.GetGroupDetails();
            return View(objData);
        }
        public ActionResult GetWAScripts(int GroupId,string GroupName,string MessageType)
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.objWhatsAppSMSMaster = ITCSR.GetWAScripts(GroupId,GroupName,MessageType);
            return Json(objData, JsonRequestBehavior.AllowGet);
        }

        
        
    }
}