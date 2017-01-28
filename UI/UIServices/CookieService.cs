
using System;
using System.Web;

namespace UI.UIServices
{
    public class CookieService : ICookieService
    {
        public void SetCookie(string cookieName, string value, DateTime expires)
        {
            if (string.IsNullOrEmpty(cookieName))
            {
                throw new ArgumentNullException("cookieName");
            }

            if (expires < DateTime.Now)
            {
                throw new ArgumentException("expires has lower value then current time");
            }

            var cookie = HttpContext.Current.Request.Cookies.Get(cookieName);

            if (cookie == null)
            {
                // create new cookie
                var newCookie = new HttpCookie(cookieName);
                newCookie.Value = value;
                newCookie.Expires = expires;
                newCookie.Path = "/";

                HttpContext.Current.Response.Cookies.Add(newCookie);
            }
            else
            {
                // edit existing cookie
                cookie.Value = value;
                cookie.Expires = expires;
                cookie.Path = "/";

                HttpContext.Current.Response.Cookies.Set(cookie);
            }
 
        }

        public void RemoveCookie(string cookieName)
        {
            var cookie = HttpContext.Current.Request.Cookies.Get(cookieName);

            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddYears(-1);
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
        }

        public string GetCookieValue(string cookieName)
        {
            var cookie = HttpContext.Current.Request.Cookies.Get(cookieName);

            return cookie?.Value;
        }
    }
}
