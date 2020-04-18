using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TESTAPI.Contract.V1.Requests;

namespace TESTAPI.SwaggerExample.Reqest
{
    public class CreatePostReqestExample : IExamplesProvider<CreatePostReqest>
    {
        public CreatePostReqest GetExamples()
        {
            return new CreatePostReqest
            {
                Name = "Sample Name"
            };
        }
    }
}
