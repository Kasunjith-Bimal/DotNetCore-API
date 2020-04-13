using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TESTAPI.Contract.V1.Responses;
using TESTAPI.Domain;

namespace TESTAPI.Mapping
{
    public class DomainToResponse:Profile
    {
        public DomainToResponse()
        {
            CreateMap<Post, PostResponse>();

            
        }
    }
}
