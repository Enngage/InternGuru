using System.Web;
using System.Web.Routing;
using Core.Extensions;

namespace Web.Lib.RouteConstraints
{

    public class AlphanumericRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var value = values[parameterName].ToString();

            return value.IsAlphaNumeric();
        }
    }
}