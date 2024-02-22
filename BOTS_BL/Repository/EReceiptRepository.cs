using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using BOTS_BL.Models.CommonDB;
using System.Web.Mvc;
using System.Configuration;
using System.Threading;
using System.Net.Mail;
using System.Net.Mime;
using System.Globalization;
using System.Xml;
using System.Web.Script.Serialization;

namespace BOTS_BL.Repository
{
    public class EReceiptRepository
    {
        Exceptions newexception = new Exceptions();
        public tblTempTxnJSON GetEReceiptJSON(string invoiceNo, string connStr)
        {
            tblTempTxnJSON objData = new tblTempTxnJSON();
            try
            {
                using (var context = new BOTSDBContext(connStr))
                {
                    invoiceNo = "TMB-Dec23-022909";
                    objData = context.tblTempTxnJSONs.Where(x => x.InvoiceNo == invoiceNo).FirstOrDefault();
                    if (objData != null)
                    {
                        JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                        json_serializer.MaxJsonLength = int.MaxValue;
                        object[] receiptObj = (object[])json_serializer.DeserializeObject(objData.JSON);
                        foreach (Dictionary<string, object> item in receiptObj)
                        {
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetEReceiptJSON");
            }
            return objData;
        }
    }
}
