using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoolchandRedemption.App_Start
{
    public static class MenuHelper
    {
        public static string IsSelected(this HtmlHelper html, string controller = null, string action = null)
        {
            var currentController = string.Empty;
            var currentAction = string.Empty;
            const string cssClass = "mm-active";
            var controllers = controller.Split(',');
            var actions = action.Split(',');
            if (controllers.Length > 1)
            {
                foreach (var c in controllers)
                {
                    if (c == (string)html.ViewContext.RouteData.Values["controller"])
                    {
                        currentController = (string)html.ViewContext.RouteData.Values["controller"];
                        currentAction = (string)html.ViewContext.RouteData.Values["action"];
                        controller = currentController;
                    }
                }
            }
            else if (actions.Length > 1)
            {
                foreach (var a in actions)
                {
                    if (a == (string)html.ViewContext.RouteData.Values["action"])
                    {
                        currentController = (string)html.ViewContext.RouteData.Values["controller"];
                        currentAction = (string)html.ViewContext.RouteData.Values["action"];
                        action = currentAction;
                    }
                }
            }
            else
            {
                currentAction = (string)html.ViewContext.RouteData.Values["action"];
                currentController = (string)html.ViewContext.RouteData.Values["controller"];
            }


            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            return controller == currentController && action == currentAction ?
                cssClass : String.Empty;
        }
    }
}