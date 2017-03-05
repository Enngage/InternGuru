using System;
using System.Collections.Specialized;
using System.Web.Routing;

namespace UI.Extensions
{
    public static class NameValueExtensions
    {
        /// <summary>
        /// Example:
        /// Url.Action("action", "controller", Request.QueryString.ToRouteValues(new{ id=0 }));
        /// </summary>
        /// <param name="col"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static RouteValueDictionary ToRouteValues(this NameValueCollection col, object obj)
        {
            var values = new RouteValueDictionary(obj);
            if (col != null)
            {
                foreach (string key in col)
                {
                    //values passed in object override those already in collection
                    if (!values.ContainsKey(key)) values[key] = col[key];
                }
            }
            return values;
        }
    }
}
