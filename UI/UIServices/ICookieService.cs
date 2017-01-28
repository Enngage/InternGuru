using System;

namespace UI.UIServices
{
    public interface ICookieService
    {
        string GetCookieValue(string cookieName);
        void SetCookie(string cookieName, string value, DateTime expires);
        void RemoveCookie(string cookieName);
    }
}
