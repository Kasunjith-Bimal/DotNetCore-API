using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TESTAPI.Contract.V1.Requests.Queries;

namespace TESTAPI.Services
{
    public interface IUriService
    {
        Uri GetPostUri(string postId);

        Uri GetAllPostUri(PaginationQuery pagination = null);

    }
}
