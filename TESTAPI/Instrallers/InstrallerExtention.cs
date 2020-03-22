using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TESTAPI.Instrallers
{
    public static class InstrallerExtention
    {
        public static void InstrallServiceAssembly(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddCors(options => options.AddPolicy("Cors", builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));

            var instrallers = typeof(Startup).Assembly.ExportedTypes.Where(x => typeof(IInstraller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).Select(Activator.CreateInstance).Cast<IInstraller>().ToList();


            instrallers.ForEach(instraller => instraller.InstallServices(services, Configuration));

        }
    }
}
