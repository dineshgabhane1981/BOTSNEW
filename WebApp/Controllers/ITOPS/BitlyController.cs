using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace WebApp.Controllers.ITOPS
{
    public class BitlyController : Controller
    {
        // GET: Bitly
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Upload()
        {
            return View("Upload");
        }
        public ActionResult UploadData(HttpPostedFileBase Data)
        {
            string Path = ConfigurationManager.AppSettings["Path"].ToString();
            string responseString;
            try
            {
                string _FileName = Data.FileName;
                Session["FileName"] = _FileName;
                string Path2 = Path + _FileName;
                System.IO.FileInfo file = new System.IO.FileInfo(Path2);
                if (!file.Exists)
                {
                    if (Data.ContentLength > 0)
                    {
                        Data.SaveAs(Path2);
                        Session["Path2"] = Path2;
                        ViewBag.Message = "File Uploaded Successfully!!";
                        return View("Upload");
                    }
                }
                else
                {
                    ViewBag.Message = "File Already Exists";
                }

            }
            catch (ArgumentException ex)
            {
                responseString = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
                return View("Upload");
            }
            catch (WebException ex)
            {
                responseString = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
                return View("Upload");
            }
            catch (Exception ex)
            {
                responseString = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
                return View("Upload");
            }
            return View("Upload");
        }


        public ActionResult CreateBitly()
        {
            string Path3 = ConfigurationManager.AppSettings["Path3"].ToString();
            string _FileName2 = (string)Session["FileName"];
            string data1 = Path3 + _FileName2;
            string responseString;
            string _Url = "http://api.bit.ly/shorten?";
            try
            {
                StringBuilder sbposdata = new StringBuilder();
                sbposdata.AppendFormat(_Url);
                sbposdata.AppendFormat("format=json");
                sbposdata.AppendFormat("&version={0}", "2.0.1");
                sbposdata.AppendFormat("&longUrl={0}", data1);
                sbposdata.AppendFormat("&login={0}", "o_1bla0j44vl");
                sbposdata.AppendFormat("&apiKey={0}", "R_934b9e704bc14d50be7c22c58d5ed588");
                string Url = sbposdata.ToString();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(Url);
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(sbposdata.ToString());
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;
                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                responseString = reader.ReadToEnd();
                JavaScriptSerializer jconvert = new JavaScriptSerializer();
                var responseData = jconvert.DeserializeObject(responseString);


                foreach (var item in (dynamic)responseData)

                {
                    if (item.Key == "results")
                    {
                        var itemResult = item.Value;
                        foreach (var item1 in (dynamic)itemResult)
                        {
                            var itemLink = item1.Value;
                            foreach (var item2 in (dynamic)itemLink)
                            {
                                if (item2.Key == "shortUrl")
                                {
                                    string itemshortUrl = item2.Value;
                                    Session["Bitly"] = itemshortUrl;
                                }

                            }
                        }
                    }
                }
                reader.Close();
                response.Close();

            }
            catch (ArgumentException ex)
            {
                responseString = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
            }
            catch (WebException ex)
            {
                responseString = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                responseString = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }
            return View("Upload");
        }
    }
}