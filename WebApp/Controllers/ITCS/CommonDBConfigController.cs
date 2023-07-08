using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Models.RetailerWeb;
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
        CustomerRepository CR = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        // GET: CommonDBConfig
        public ActionResult ChangeCSName()
        {
            ProgrammeViewModel objData = new ProgrammeViewModel();
            objData.lstGroupDetails = ITCSR.GetGroupDetails();
            objData.lstRMAssigned = ITCSR.GetRMAssignedList();
            return View(objData);
        }
        public ActionResult EnableProgramme()
        {
            //var userDetails = (CustomerLoginDetail)Session["UserSession"];
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

        public ActionResult GetCSNameData(string GroupId)
        {
            GroupData objCustomerDetail = new GroupData();
            var groupId = (string)Session["GroupId"];
            try
            {
                if (!string.IsNullOrEmpty(GroupId))
                {
                    objCustomerDetail = ITCSR.GetCSNameByGroupId(GroupId);
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCSNameData");
            }
            return Json(objCustomerDetail, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveCSName(string jsonData)
        {
            var result = false;
            string GroupId, RMAssignedId;
            GroupId = string.Empty;
            RMAssignedId = string.Empty;
            try
            {

                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                foreach (Dictionary<string, object> item in objData)
                {
                    GroupId = Convert.ToString(item["GroupID"]);
                    RMAssignedId = Convert.ToString(item["RMAssignedID"]);

                }
                result = ITCSR.SaveCSData(GroupId, RMAssignedId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveCSName");
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

    }
}