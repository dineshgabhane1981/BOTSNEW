using BOTS_BL;
using BOTS_BL.Models.ChitaleModel;
using BOTS_BL.Repository;
using Chitale.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ClosedXML.Excel;
using System.IO;

namespace Chitale.Controllers
{
    public class ExportController : Controller
    {
        ChitaleException newexception = new ChitaleException();
        ParticipantRepository pr = new ParticipantRepository();
        // GET: Export
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ExportParticipantList()
        {
            try
            {
                string ReportName = "Participant List";
                System.Data.DataTable table = new System.Data.DataTable();
                var UserSession = (CustomerDetail)Session["ChitaleUser"];
                ParticipantList objlist = new ParticipantList();
                List<ParticipantList> lstparticipantLists = new List<ParticipantList>();
                lstparticipantLists = pr.GetParticipantList(UserSession.CustomerId, UserSession.Type);

                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(ParticipantList));
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                if (UserSession.Type == "SuperStockiest")
                {
                    foreach (ParticipantList item in lstparticipantLists)
                    {
                        DataRow row = table.NewRow();
                        foreach (PropertyDescriptor prop in properties)
                            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                        table.Rows.Add(row);

                        List<ParticipantList> lstRetailerLists = new List<ParticipantList>();
                        lstRetailerLists = pr.GetParticipantList(item.Id, item.ParticipantType);
                        foreach(ParticipantList itemRetailer in lstRetailerLists)
                        {
                            DataRow row1 = table.NewRow();
                            foreach (PropertyDescriptor prop in properties)
                                row1[prop.Name] = prop.GetValue(itemRetailer) ?? DBNull.Value;
                            table.Rows.Add(row1);
                        }
                    }
                }
                else
                {
                    foreach (ParticipantList item in lstparticipantLists)
                    {
                        DataRow row = table.NewRow();
                        foreach (PropertyDescriptor prop in properties)
                            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                        table.Rows.Add(row);
                    }
                }

                var count = table.Rows.Count;

                string fileName = ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {                    
                    table.TableName = ReportName;
                    wb.Worksheets.Add(table);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return null;
        }
    }
}