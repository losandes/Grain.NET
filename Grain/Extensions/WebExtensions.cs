using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace Grain.Extensions
{
    public static class WebExtensions
    {
        public static NameValueCollection GetQueryStringCollection(HttpContext context)
        {
            return HttpUtility.ParseQueryString(context.Request.Url.Query.Substring(1));
        }
    }
}
