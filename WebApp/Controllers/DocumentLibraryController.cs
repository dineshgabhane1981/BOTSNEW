using BOTS_BL.Models;
using BOTS_BL.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.ViewModel;

namespace WebApp.Controllers
{
    public class DocumentLibraryController : Controller
    {
        DocumentLibraryRepository DLR = new DocumentLibraryRepository();
        // GET: DocumentLibrary
        public ActionResult Index()
        {
            DocumentLibraryViewModel objdata = new DocumentLibraryViewModel();
            objdata.lstGroupDetails = DLR.GetGroupDetails();
            return View(objdata);
        }
        public bool UploadDocument(string fileData, string fileName, string Groupid, string GroupName, string Comment)
        {
            bool status = false;

            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            string Addedby = userDetails.MobileNo;
            string Addeddate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            status = DLR.UploadDocumentToS3(fileData, fileName, Groupid, GroupName, Comment, Addedby, Addeddate);
            return status;
        }
    }
}