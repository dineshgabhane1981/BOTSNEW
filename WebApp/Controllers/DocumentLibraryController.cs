using BOTS_BL.Models;
using BOTS_BL.Models.CommonDB;
using BOTS_BL.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.ViewModel;
using System.Web.Script.Serialization;

namespace WebApp.Controllers
{
    public class DocumentLibraryController : Controller
    {
        DocumentLibraryRepository DLR = new DocumentLibraryRepository();
        // GET: DocumentLibrary
        public ActionResult Index()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            DocumentLibraryViewModel objdata = new DocumentLibraryViewModel();
            List<tblDocumentsLibrary> ObjList = new List<tblDocumentsLibrary>();
            objdata.lstGroupDetails = DLR.GetGroupDetails();

            return View(objdata);
        }
        public bool UploadDocument(string fileData, string fileName, string Groupid, string GroupName, string Comment, string DocType)
        {
            bool status = false;

            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            string Addedby = userDetails.LoginId;
            string Addeddate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            status = DLR.UploadDocumentToS3(fileData, fileName, Groupid, GroupName, Comment, DocType, Addedby, Addeddate);
            return status;
        }

        [HttpPost]
        public JsonResult ListDocuments(string jsonData)
        {
            List<tblDocumentsLibrary> Doclist = new List<tblDocumentsLibrary>();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);

            foreach (Dictionary<string, object> item in objData)
            {
                string GroupId = Convert.ToString(item["GroupId"]);

                Doclist = DLR.GetDocLibData(GroupId);
            }

                return new JsonResult() { Data = Doclist, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetDownloadDetails(string jsonData)
        {
            string Filename = string.Empty;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in objData)
            {
                string SLno = Convert.ToString(item["SLno"]);

                 Filename  = DLR.GetDownloadData(SLno);
            }
            return new JsonResult() { Data = Filename, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}