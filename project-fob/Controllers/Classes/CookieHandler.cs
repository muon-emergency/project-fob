using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace project_fob.Controllers
{
    public static class CookieHandler
    {
        public static Guid ManageAndReturnCookie(ControllerBase cbase)
        {
            string id = GetUserIDFromCookie(cbase);
            id = SetCookie(id, cbase);
            return Guid.Parse(id);
        }

        public static ActionResult CreateOrUpdateCookies(ControllerBase cbase)
        {
            string id = GetUserIDFromCookie(cbase);
            SetCookie(id,cbase);
            return cbase.Ok();
        }

        private static string GetUserID(ControllerBase cbase)
        {
            string id = GetUserIDFromCookie(cbase);
            SetCookie(id, cbase);
            return id;
        }


        private static string GetUserIDFromCookie(ControllerBase cbase)
        {
            return cbase.Request.Cookies["ID"];
        }

        private static string SetCookie(string id, ControllerBase cbase)
        {
            CookieOptions cookie = new CookieOptions();
            cookie.Expires = DateTime.Now.AddYears(5);
            if (id == null || id.Length == 0)
            {
                string newId = IDGenerators.GenerateId();
                cbase.Response.Cookies.Append("ID", newId);
                return newId;
            }
            else
            {
                cbase.Response.Cookies.Append("ID", id);
                return id;
            }

        }

        private static void SetCookie(string id, string value, ControllerBase cbase)
        {
            CookieOptions cookie = new CookieOptions();
            cookie.Expires = DateTime.Now.AddYears(5);
            if (id != null && id.Length == 0)
            {
                cbase.Response.Cookies.Append(id, value);
            }
        }

        //There are methods which can do similar but I saved this one as this is a special one :)
        public static bool HaveCookieId(out Guid id, ControllerBase cbase)
        {
            string strignId = cbase.Request.Cookies["ID"];
            if (strignId == null || strignId.Length == 0)
            {
                return false;
            }

            return Guid.TryParse(strignId, out id);
        }
    }
}
