using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Repository;
using BOTS_BL.Models;
using System.Web.Script.Serialization;
using NPC.ViewModels;
using BOTS_BL;

namespace NPC.Controllers
{
    public class LoginController : Controller
    {
        Exceptions newexception = new Exceptions();
        NPCRepository NPCR = new NPCRepository();
        // GET: Login
        public ActionResult Index()
        {
            NPCLoginDetail objData = new NPCLoginDetail();

            return View(objData);
        }

        public ActionResult Authenticate(NPCLoginDetail objData)
        {
            try
            {                
                var groupId = NPCR.CheckLogin(objData);               
                if (!string.IsNullOrEmpty(groupId))
                {                    
                    Session["GroupId"] = groupId;
                    return RedirectToAction("NPCPage");
                }
                else
                {
                    return RedirectToAction("Index");
                }
               
            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "NPC Login");
            }
            return RedirectToAction("Index");
        }

        public ActionResult NPCPage()
        {
            newexception.AddDummyException("Inside NPC");
            var groupId = Convert.ToString(Session["GroupId"]);
            NPCViewModel objdata = new NPCViewModel();
            objdata.lstNPCCategory = NPCR.GetNPCCategory(groupId);
            objdata.lstNPCSubCategory = NPCR.GetNPCSubCategory(groupId);
            objdata.lstNPCEmployees = NPCR.GetNPCEmployees(groupId);
            objdata.lstOutlets = NPCR.GetOutlets(groupId);
            objdata.Logo = NPCR.GetLogo(groupId);
            return View(objdata);           
        }


        public ActionResult SaveNPC(string jsonData)
        {
            bool result = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];

            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            tblNPCDetail objOutletData = new tblNPCDetail();
            foreach (Dictionary<string, object> item in objData)
            {

                objOutletData.Outlet = Convert.ToString(item["OutletName"]);
                objOutletData.CustomerMobileNo = Convert.ToString(item["CustomerMobileNo"]);
                objOutletData.CustomerName = Convert.ToString(item["CustomerName"]);
                objOutletData.EmployeeName = Convert.ToString(item["EmployeeName"]);
                objOutletData.CategoryName = Convert.ToString(item["CategoryName"]);
                objOutletData.SubCategoryName = Convert.ToString(item["SubCategoryName"]);
                objOutletData.NextVisitDay = Convert.ToDecimal(item["NextVisitDay"]);
                objOutletData.Remarks = Convert.ToString(item["Remarks"]);

            }
            result = NPCR.SaveNPCData(objOutletData);

            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };

        }

    }
}