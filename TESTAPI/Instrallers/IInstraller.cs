using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TESTAPI.Instrallers
{
    public interface IInstraller
    {
        void InstallServices(IServiceCollection services, IConfiguration Configuration);
    }
}
 