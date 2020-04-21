 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TESTAPI.Contract.V1.Requests.Queries;
using TESTAPI.Contract.V1.Responses;
using TESTAPI.Domain;
using TESTAPI.Services;

namespace TESTAPI.Helpers
{
    public class PaginationHelper
    {
        public  static PagedResponse<T> CreeatePaginatedResponse<T>(IUriService uriService, PaginationFilter pagination, List<T> postResponses)
        {

            var nextPage = pagination.PageNumber >= 1 ? uriService.GetAllPostUri(new PaginationQuery { PageNumber = pagination.PageNumber + 1, PageSize = pagination.PageSize }).ToString() : null;
            var previouspage = pagination.PageNumber - 1 >= 1 ? uriService.GetAllPostUri(new PaginationQuery { PageNumber = pagination.PageNumber - 1, PageSize = pagination.PageSize }).ToString() : null;


            return new PagedResponse<T>
            {
                Data = postResponses,
                PageNumber = pagination.PageNumber >= 1 ? pagination.PageNumber : (int?)null,
                PageSize = pagination.PageSize >= 1 ? pagination.PageSize : (int?)null,
                NextPage = postResponses.Any() ? nextPage : null,
                PreviousPage = previouspage,

            };
        }
    }
}
