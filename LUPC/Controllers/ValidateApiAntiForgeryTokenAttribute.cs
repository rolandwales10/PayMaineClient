using System;
using System.Web.Helpers;
using System.Web.Mvc;

/*
 *  Synopsis: Supports anti forgery token validation for API posts.  This is built into the
 *      framework for generating and reloading pages, but API posts require a custom approach.
 *  
 */

namespace LUPC.Controllers
{
    /*
     * This custom class is needed because the standard .NET validation doesn't work with Ajax calls and Json interface
     */
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ValidateApiAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            var httpContext = filterContext.HttpContext;
            var cookie = httpContext.Request.Cookies[AntiForgeryConfig.CookieName];
            AntiForgery.Validate(cookie != null ? cookie.Value : null, httpContext.Request.Headers["__RequestVerificationToken"]);
        }
    }
}