using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TESTAPI.Contract.V1;
using TESTAPI.Contract.V1.Requests;
using TESTAPI.Contract.V1.Responses;
using TESTAPI.Domain;
using TESTAPI.Services;

namespace TESTAPI.Controllers.V1
{
    public class PostController:Controller
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }
        [HttpGet(ApiRoutes.Posts.GetAll)]
        public IActionResult GetAll()
        {
            return Ok(_postService.GetPosts());

        }

        [HttpGet(ApiRoutes.Posts.Get)]
        public IActionResult Get([FromBody] Guid postId)
        {
            Post post = _postService.GetPostById(postId);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);

        }

        [HttpPut(ApiRoutes.Posts.Update)]
        public IActionResult Update([FromRoute] Guid postId,[FromBody] UpdatePostReqest updatePostReqest)
        {
            var post = new Post { Id = postId,Name = updatePostReqest.Name };

            if (_postService.UpdatePost(post))
            {
                return Ok(post);
            }
            else
            {
                return NotFound();
            }
        }

       

        [HttpPost(ApiRoutes.Posts.Create)]
        public IActionResult Post([FromBody] CreatePostReqest postReqest)
        {

            var post = new Post() { Id = postReqest.Id };

            if (post.Id != Guid.Empty)
            {
                post.Id = Guid.NewGuid();
            }

            _postService.GetPosts().Add(post);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";

            var location = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString());

            var response = new CreatePostResponse() { Id = post.Id };

            return Created(location, response);
        }

    }
}
