﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TESTAPI.Domain
{
    public class AuthenticationResult
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public bool Sucess { get; set; }

        public IEnumerable<string> ErrorMessage { get; set; }
    }
}
