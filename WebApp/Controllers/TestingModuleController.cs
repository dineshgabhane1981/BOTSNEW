using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.App_Start;
using System.Web.Script.Serialization;
using WebApp.ViewModel;
using BOTS_BL.Models.OnBoarding;
using System.Data.OleDb;
using System.Data;
using System.Net;
using System.Xml;


namespace WebApp.Controllers
{
    public class TestingModuleController : Controller
    {
        CustomerRepository CR = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        // GET: TestingModule
        public ActionResult Index(string groupId)
        {
            TestingModuleViewModel TMV = new TestingModuleViewModel();
            var billingpartners = CR.GetBillingPartner();
            ViewBag.GroupId = groupId;
            ViewBag.BillingPartners = billingpartners;
            XmlDocument doc = new XmlDocument();
            doc.Load("C:\\Users\\91912\\source\\BOTS\\WebApp\\APIPackets.xml");
            XmlNode node = doc.DocumentElement.SelectSingleNode("/packets/Enrollnment");
            TMV.RequestPacket = node.InnerXml;
            
            return View(TMV);
        }
    }
}