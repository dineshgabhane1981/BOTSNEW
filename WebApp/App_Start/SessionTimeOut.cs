using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp
{

    public class MyActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var sessionUser = filterContext.HttpContext.Session["UserSession"];
            var routeValues = filterContext.RequestContext.RouteData.Values["controller"].ToString();
            if (sessionUser == null && !routeValues.Equals("Login"))
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.ClearContent();
                    filterContext.HttpContext.Items["AjaxPermissionDenied"] = true;
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/Login/Index");
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }



}