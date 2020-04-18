using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TESTAPI.Contract.V1.Requests
{
    public class RefreshTokenReqest
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; } 
    }
}
