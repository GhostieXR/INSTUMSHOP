using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace UTM.WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                try
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (authTicket != null && !authTicket.Expired)
                    {
                        var roles = authTicket.UserData.Split(',');
                        HttpContext.Current.User = new GenericPrincipal(new FormsIdentity(authTicket), roles);
                    }
                }
                catch (Exception ex)
                {
                    // Логирование ошибки (при необходимости)
                    System.Diagnostics.Debug.WriteLine("Exception in Application_PostAuthenticateRequest: " + ex.Message);

                    // Удаление аутентификационной куки
                    FormsAuthentication.SignOut();
                    HttpCookie expiredCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
                    expiredCookie.Expires = DateTime.Now.AddYears(-1);
                    Context.Response.Cookies.Add(expiredCookie);
                }
            }
        }

    }
}