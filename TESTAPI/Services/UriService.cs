using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TESTAPI.Contract.V1;
using TESTAPI.Contract.V1.Requests.Queries;

namespace TESTAPI.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUrl;
        public UriService(string baseUrl)
        {
            _baseUrl = baseUrl;
        }
        public Uri GetAllPostUri(PaginationQuery pagination = null)
        {
            var uri = new Uri(_baseUrl);

            if (pagination == null)
            {
                return uri;
            }

            var modifiedUri = QueryHelpers.AddQueryString(_baseUrl, "pageNumber", pagination.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", pagination.PageSize.ToString());

            return new Uri(modifiedUri);


        }

        public Uri GetPostUri(string postId)
        {
            return new Uri(_baseUrl+ApiRoutes.Posts.Get.Replace("{postId}",postId));
        }
    }
}
