using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace project_fob.Controllers
{
    public static class CookieHandler
    {
        public static Guid ManageAndReturnCookie(ICookies cookies)
        {
            string id = GetUserIDFromCookie(cookies);
            id = SetCookie(id, cookies);
            return Guid.Parse(id);
        }

        public static void CreateOrUpdateCookies(ICookies cookies)
        {
            string id = GetUserIDFromCookie(cookies);
            SetCookie(id,cookies);
        }

        private static string GetUserID(ICookies cookies)
        {
            string id = GetUserIDFromCookie(cookies);
            SetCookie(id, cookies);
            return id;
        }


        private static string GetUserIDFromCookie(ICookies cookies)
        {
            return cookies.Request["ID"];
        }

        private static string SetCookie(string id, ICookies cookies)
        {
            CookieOptions cookie = new CookieOptions();
            cookie.Expires = DateTime.Now.AddYears(5);
            if (id == null || id.Length == 0)
            {
                string newId = IDGenerators.GenerateId();
                cookies.Response.Append("ID", newId);
                return newId;
            }
            else
            {
                cookies.Response.Append("ID", id);
                return id;
            }

        }

        private static void SetCookie(string id, string value, ICookies cookies)
        {
            CookieOptions cookie = new CookieOptions();
            cookie.Expires = DateTime.Now.AddYears(5);
            if (id != null && id.Length == 0)
            {
                cookies.Response.Append(id, value);
            }
        }

        private static void SetCookieDateExpired(string id, string value, ICookies cookies)
        {
            CookieOptions cookie = new CookieOptions();
            cookie.Expires = DateTime.Now.AddYears(-1);
            if (id != null && id.Length == 0)
            {
                cookies.Response.Append(id, value);
            }
        }

        //There are methods which can do similar but I saved this one as this is a special one :)
        public static bool HaveCookieId(out Guid id, ICookies cookies)
        {
            string strignId = cookies.Request["ID"];
            if (strignId == null || strignId.Length == 0)
            {
                return false;
            }

            return Guid.TryParse(strignId, out id);
        }
    }
}
