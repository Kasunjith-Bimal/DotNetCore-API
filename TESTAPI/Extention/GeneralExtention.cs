using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TESTAPI.Extention
{
    public static  class GeneralExtention
    {
        public static string GetUserId(this HttpContext httpcontext)
        {
            if (httpcontext.User == null)
            {
                return String.Empty;
            }

            return httpcontext.User.Claims.Single(x => x.Type == "id").Value;

        }
    }
}
