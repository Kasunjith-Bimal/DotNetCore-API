using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TESTAPI.Domain;

namespace TESTAPI.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegistrationAsync(string email, string password);

        Task<AuthenticationResult> LoginAsync(string email, string password);
    }
}
