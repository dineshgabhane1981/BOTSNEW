using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApp.App_Start;

namespace WebApp.Controllers.ITOPS
{
    public class AddMemberController : Controller
    {
        // GET: AddMember

        ITOpsRepository ITOPS = new ITOpsRepository();
        ReportsRepository RR = new ReportsRepository();
        CustomerRepository objCustRepo = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        public ActionResult Index()
        {
            return View();
        }
       


        public ActionResult BulkImport()
        {
            var groupId = (string)Session["GroupId"];
            try
            {
                
                
                string connStr = objCustRepo.GetCustomerConnString(groupId);
                var lstOutlet = RR.GetOutletList(groupId, connStr);
                //var lstBrand = RR.GetBrandList(groupId, connStr);
                var GroupDetails = objCustRepo.GetGroupDetails(Convert.ToInt32(groupId));
                ViewBag.OutletList = lstOutlet;
                //ViewBag.BranchList = lstBrand;
                ViewBag.GroupId = groupId;
                //ViewBag.GroupName = GroupDetails.RetailName;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, groupId);
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddSingleMember(string jsonData)
        {
            SPResponse result = new SPResponse();
            string GroupId = "";
            
            try
            {
                var groupId = (string)Session["GroupId"];
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                tblAudit objAudit = new tblAudit();
                CustomerDetail objCustomer = new CustomerDetail();


                foreach (Dictionary<string, object> item in objData)
                {
                    
                    //GroupId = Convert.ToString(item["GroupID"]);
                    objCustomer.MobileNo = Convert.ToString(item["MobileNo"]);
                    objCustomer.CardNumber = Convert.ToString(item["CardNo"]);
                    objCustomer.CustomerName = Convert.ToString(item["FullName"]);
                    objCustomer.Gender = Convert.ToString(item["Gender"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["BirthDay"])))
                    {
                        objCustomer.DOB = Convert.ToDateTime(item["BirthDay"]);
                    }
                    objCustomer.CustomerThrough = Convert.ToString(item["Source"]);
                    objCustomer.EnrollingOutlet = Convert.ToString(item["OutletId"]);
                    objCustomer.MemberGroupId = "1000";
                    objCustomer.DOJ = DateTime.Now;
                    objCustomer.Status = "00";

                    objAudit.GroupId = groupId;
                    
                    objAudit.RequestedFor = "User Added";
                    objAudit.RequestedEntity = "User Added - " + objCustomer.MobileNo;
                    objAudit.RequestedBy = Convert.ToString(item["RequestedBy"]);
                    objAudit.RequestedOnForum = Convert.ToString(item["RequestedForum"]);
                    objAudit.RequestedOn = Convert.ToDateTime(item["RequestedDate"]);

                }

                result = ITOPS.AddSingleCustomerData(groupId, objCustomer, objAudit);
                if (result.ResponseCode == "00")
                {
                    var subject = "New Customer Added with Mobile No  - " + objCustomer.MobileNo;
                    var body = "New Customer Added with Mobile No - " + objCustomer.MobileNo;
                    body += "<br/><br/> Regards <br/> Blue Ocktopus Team";

                    SendEmail(GroupId, subject, body);
                }

                //if (Convert.ToBoolean(IsSMS))
                //{
                //    //Logic to send SMS to Customer whose Name is changed
                //}

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddBulkMemberData(string GroupId, HttpPostedFileBase file)
        {
            CustomerDetail objCustomer = new CustomerDetail();
            SPResponse result = new SPResponse();
            try
            {
                using (XLWorkbook workBook = new XLWorkbook(file.InputStream))
                {

                    IXLWorksheet workSheet = workBook.Worksheet(1);
                    DataTable dt = new DataTable();
                    bool firstRow = true;
                    foreach (IXLRow row in workSheet.Rows())
                    {

                        if (firstRow)
                        {
                            foreach (IXLCell cell in row.Cells())
                            {
                                if (!string.IsNullOrEmpty(cell.Value.ToString()))
                                {
                                    dt.Columns.Add(cell.Value.ToString());
                                }
                                else
                                {
                                    break;
                                }
                            }
                            firstRow = false;
                        }
                        else
                        {
                            int i = 0;
                            DataRow toInsert = dt.NewRow();
                            foreach (IXLCell cell in row.Cells(1, dt.Columns.Count))
                            {
                                try
                                {
                                    toInsert[i] = cell.Value.ToString();
                                }
                                catch (Exception ex)
                                {

                                }
                                i++;
                            }
                            dt.Rows.Add(toInsert);
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        int TotalRows = 0;
                        int index = 0;
                        int invalid = 0;

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(dr["MobileNo"])))
                            {
                                Regex regex = new Regex(@"[0-9]{10}");
                                Match match = regex.Match(Convert.ToString(dr["MobileNo"]));
                                if (match.Success)
                                {
                                    objCustomer.CustomerName = Convert.ToString(dr["CustomerName"]);
                                    objCustomer.MobileNo = Convert.ToString(dr["MobileNo"]);

                                    result = ITOPS.AddBulkCustomerData(GroupId, objCustomer);
                                    if (result.ResponseCode == "00")
                                    {
                                        TotalRows++;

                                    }

                                    if (result.ResponseCode == "01")
                                    {
                                        index++;

                                    }
                                }
                                else
                                {
                                    invalid++;

                                }
                            }
                        }
                        result.ResponseSucessCount = TotalRows.ToString();
                        result.ResponseFailCount = index.ToString();
                        result.ResponseInValidFormatCount = invalid.ToString();
                    }

                }

            }

            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
                result.ResponseCode = "-1";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    
        public void SendEmail(string GroupId, string Subject, string EmailBody)
        {
            var senderEmail = System.Configuration.ConfigurationManager.AppSettings["Email"];
            var senderEmailPassword = System.Configuration.ConfigurationManager.AppSettings["EmailPassword"];

            var email = new MailAddress(senderEmail, "Support Blue Ocktopus");
            var toemail = ITOPS.GetCustomerAdminEmail(GroupId);
            if (!string.IsNullOrEmpty(toemail))
            {
                var receiverEmail = new MailAddress(toemail);

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,

                    Credentials = new NetworkCredential(email.Address, senderEmailPassword)
                };
                using (var mess = new MailMessage(email, receiverEmail)
                {
                    Subject = Subject,
                    Body = EmailBody,
                    IsBodyHtml = true,
                    Priority = MailPriority.High,
                    BodyEncoding = System.Text.Encoding.UTF8
                })
                {
                    smtp.Send(mess);
                }
            }
        }


    }
}
