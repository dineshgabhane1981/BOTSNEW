using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class DocumentLibraryController : Controller
    {
        DocumentLibraryRepository DLR = new DocumentLibraryRepository();
        // GET: DocumentLibrary
        public ActionResult Index()
        {
            return View();
        }
        public bool UploadDocument(string fileData)
        {
            bool status = false;

            status = DLR.UploadDocumentToS3(fileData);
            return status;
        }
    }
}