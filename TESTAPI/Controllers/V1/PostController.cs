using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TESTAPI.Contract.V1;
using TESTAPI.Contract.V1.Requests;
using TESTAPI.Contract.V1.Responses;
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

        [HttpPost(ApiRoutes.Posts.Create)]
        public IActionResult Post([FromBody] CreatePostReqest postReqest)
        {

            var post = new Post() { Id = postReqest.Id };

            if (string.IsNullOrEmpty(post.Id))
            {
                post.Id = Guid.NewGuid().ToString();
            }

            _PostList.Add(post);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";

            var location = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id);

            var response = new CreatePostResponse() { Id = post.Id };

            return Created(location, response);
        }

    }
}
