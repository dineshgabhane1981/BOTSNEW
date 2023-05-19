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
            string actionName = filterContext.RequestContext.RouteData.Values["action"].ToString();
            if (sessionUser == null && !routeValues.Equals("Login") && !actionName.Equals("ResetPassword") && !routeValues.Equals("botsapi") 
                && !routeValues.Equals("CustomerOnBoarding") && !routeValues.Equals("Home") && !actionName.Equals("CheckerView") && !routeValues.Equals("Feedback") && !routeValues.Equals("GenerateReports")
                && !routeValues.Equals("Events") && !routeValues.Equals("EventForm") && !routeValues.Equals("GenerateEventReports"))
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