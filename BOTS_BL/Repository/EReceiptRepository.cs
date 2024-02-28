using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models;
using BOTS_BL.Models.CommonDB;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.IO;
using BOTS_BL;
using System.Data.Entity.Validation;
using System.Net;
using System.Web.Script.Serialization;
using DocumentFormat.OpenXml.InkML;
using System.Runtime.Remoting.Contexts;
using System.Data.Entity.Infrastructure;
using BOTS_BL.Models.IndividualDBModels;

namespace BOTS_BL.Repository
{
    public class EReceiptRepository
    {
        CustomerRepository CR = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        public tblTempTxnJSON GetEReceiptJSON(string invoiceNo, string groupId)
        {
            tblTempTxnJSON objData = new tblTempTxnJSON();
            try
            {
                string connStr = CR.GetCustomerConnString(groupId);
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
