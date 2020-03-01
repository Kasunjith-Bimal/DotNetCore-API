using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TESTAPI.Contract.V1;
using TESTAPI.Domain;

namespace TESTAPI.Controllers.V1
{
    public class PostController:Controller
    {
        private List<Post> _PostList;

        public PostController()
        {
            _PostList = new List<Post>();
            for (int i = 0; i < 10; i++)
            {
                _PostList.Add(new Post() { Id = Guid.NewGuid().ToString() });
            }

        }
        [HttpGet(ApiRoutes.Posts.GetAll)]
        public IActionResult Get()
        {
            return Ok(_PostList);

        }

    }
}
