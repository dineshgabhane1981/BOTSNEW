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
            EReceipt receipt = new EReceipt();
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
                            if (!string.IsNullOrEmpty(Convert.ToString(item["ISDCode"])))
                            {
                                receipt.ISDCode= Convert.ToString(item["ISDCode"]);
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(item["CounterId"])))
                            {
                                receipt.CounterId = Convert.ToString(item["CounterId"]);
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(item["Datetime"])))
                            {
                                receipt.InvoiceDate = Convert.ToDateTime(item["Datetime"]);
                            }
                            object[] objCust = new object[1];
                            objCust[0]= item["POSCustomer"];
                            POSCustomer objCustomer = new POSCustomer();
                            //var POSCustomer = item["POSCustomer"];
                            foreach (Dictionary<string, object> itemCustomer in (object[])objCust)
                            {
                                objCustomer.Address1 = Convert.ToString(itemCustomer["Address1"]);
                                objCustomer.Address2 = Convert.ToString(itemCustomer["Address2"]);
                                objCustomer.Address3 = Convert.ToString(itemCustomer["Address3"]);
                                objCustomer.Country = Convert.ToString(itemCustomer["Country"]);
                                objCustomer.ISDCode = Convert.ToString(itemCustomer["ISDCode"]);
                                objCustomer.LPCardNo = Convert.ToString(itemCustomer["LPCardNo"]);
                                objCustomer.MName = Convert.ToString(itemCustomer["MName"]);
                                objCustomer.Address1 = Convert.ToString(itemCustomer["Address1"]);
                                objCustomer.Address1 = Convert.ToString(itemCustomer["Address1"]);
                                objCustomer.Address1 = Convert.ToString(itemCustomer["Address1"]);
                                objCustomer.Address1 = Convert.ToString(itemCustomer["Address1"]);
                                objCustomer.Address1 = Convert.ToString(itemCustomer["Address1"]);
                                objCustomer.Address1 = Convert.ToString(itemCustomer["Address1"]);
                                objCustomer.Address1 = Convert.ToString(itemCustomer["Address1"]);
                                objCustomer.Address1 = Convert.ToString(itemCustomer["Address1"]);
                                objCustomer.Address1 = Convert.ToString(itemCustomer["Address1"]);
                                objCustomer.Address1 = Convert.ToString(itemCustomer["Address1"]);
                                objCustomer.Address1 = Convert.ToString(itemCustomer["Address1"]);
                                objCustomer.Address1 = Convert.ToString(itemCustomer["Address1"]);
                                objCustomer.Address1 = Convert.ToString(itemCustomer["Address1"]);
                                objCustomer.Address1 = Convert.ToString(itemCustomer["Address1"]);
                            }

                             

                            foreach (Dictionary<string, object> itemPOSBILL in (object[])item["POSBILL"])
                            {
                            }
                            foreach (Dictionary<string, object> itemBillMOP in (object[])item["POSBillMOP"])
                            {
                            }
                            foreach (Dictionary<string, object> itemPOSBillItems in (object[])item["POSBillItems"])
                            {
                            }
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
