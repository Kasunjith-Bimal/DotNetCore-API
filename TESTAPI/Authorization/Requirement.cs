using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TESTAPI.Authorization
{
    public class Requirement: IAuthorizationRequirement 
    {
        public string DomainName { get; set; }

        public Requirement(string domainName)
        {
            DomainName = domainName;
        }
    }
}