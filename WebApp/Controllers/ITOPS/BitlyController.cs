using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            var groupId = (string)Session["GroupId"];
            ViewBag.GroupId = groupId;
            return View();
        }
        public ActionResult Upload()
        {
            return View("Index");
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
                        return View("Index");
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
                return View("Index");
            }
            catch (WebException ex)
            {
                responseString = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
                return View("Index");
            }
            catch (Exception ex)
            {
                responseString = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
                return View("Index");
            }
            return View("Index");
        }


        public ActionResult CreateBitly()
        {
            string Path3 = ConfigurationManager.AppSettings["Path3"].ToString();
            string _FileName2 = (string)Session["FileName"];           
            string data1 = Path3 + _FileName2;
            string responseString, result_00003;
            string _Url = "https://api-ssl.bitly.com/v4/shorten";
            try
            {
                var httpWebRequest_00003 = (HttpWebRequest)WebRequest.Create(_Url);
                httpWebRequest_00003.ContentType = "application/json";
                httpWebRequest_00003.Headers.Add("Authorization", "f22c9274b5565860b85d1e4af701d4d6a4c795fa");
           
                httpWebRequest_00003.Method = "POST";

                using (var streamWriter_00003 = new StreamWriter(httpWebRequest_00003.GetRequestStream()))
                {

                    string json_00003 = 
                                    "{\"long_url\":\"" + data1 + "\"," +
                                    "\"domain\":\"bit.ly\"," +
                                    "\"group_guid\":\"\"}";
                    streamWriter_00003.Write(json_00003);
                }

                var httpResponse_00003 = (HttpWebResponse)httpWebRequest_00003.GetResponse();
                using (var streamReader_00003 = new StreamReader(httpResponse_00003.GetResponseStream()))
                {
                     result_00003 = streamReader_00003.ReadToEnd();
                }

                JObject jsonObj = JObject.Parse(result_00003);
                var Balance = JObject.Parse(result_00003)["id"];
                string itemshortUrl = (string)Balance;
                ViewData["Bitly"] = itemshortUrl;
                //foreach (var item in (dynamic)result_00003)
                //{
                //    if (item.Key == "id")
                //    {
                //        var itemResult = item.Value;
                //        foreach (var item1 in (dynamic)itemResult)
                //        {
                //            var itemLink = item1.Value;
                //            foreach (var item2 in (dynamic)itemLink)
                //            {
                //                if (item2.Key == "shortUrl")
                //                {
                //                    string itemshortUrl = item2.Value;
                //                    ViewData["Bitly"] = itemshortUrl;
                //                }

                //            }
                //        }
                //    }
                //}

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
            return View("Index");
        }
    }
}