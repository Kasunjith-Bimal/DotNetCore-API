using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TESTAPI.Contract.V1.Responses
{
    public class AuthSucessResponse
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }


    }
}
