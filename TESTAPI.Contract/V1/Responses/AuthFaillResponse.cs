using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TESTAPI.Contract.V1.Responses
{
    public class AuthFaillResponse
    {
        public IEnumerable<string> Error { get; set; }
    }
}
