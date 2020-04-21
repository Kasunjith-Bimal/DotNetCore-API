using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TESTAPI.Contract.V1.Requests.Queries;
using TESTAPI.Domain;

namespace TESTAPI.Mapping
{
    public class ReqestToDomainProfile: Profile
    {
        public ReqestToDomainProfile()
        {
            CreateMap<PaginationQuery, PaginationFilter>();


        }
    }
}
