﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Net;
using System.Web.Http.Filters;

namespace MvcApplication1.Attributes
{
    public class RequireHttpsAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps && !IsForwardedSsl(actionContext))
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                {
                    ReasonPhrase = "HTTPS Required"
                };
            }
            else
            {
                base.OnAuthorization(actionContext);
            }
        }

        private static bool IsForwardedSsl(HttpActionContext actionContext)
        {
            var xForwardedProto = actionContext.Request.Headers.FirstOrDefault(x => x.Key == "X-Forwarded-Proto");
            var forwardedSsl = xForwardedProto.Value != null &&
                xForwardedProto.Value.Any(x => string.Equals(x, "https", StringComparison.InvariantCultureIgnoreCase));
            return forwardedSsl;
        }
    }
}