using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class PostController:Controller
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }
        [HttpGet(ApiRoutes.Posts.GetAll)]
       
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _postService.GetPostsAsync());

        }

        [HttpGet(ApiRoutes.Posts.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid postId)
        {
            Post post = await _postService.GetPostByIdAsync(postId);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);

        }

        [HttpPut(ApiRoutes.Posts.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid postId,[FromBody] UpdatePostReqest updatePostReqest)
        {
            var post = new Post { Id = postId,Name = updatePostReqest.Name };

            bool isUpdated = await _postService.UpdatePostAsync(post);

            if (isUpdated)
            {
                return Ok(post);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete(ApiRoutes.Posts.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid postId)
        {
            bool isDeleted =  await _postService.DeletePostAsync(postId);

            if (isDeleted)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }

        }




        [HttpPost(ApiRoutes.Posts.Create)]
        public async Task<IActionResult> Post([FromBody] CreatePostReqest postReqest)
        {

            var post = new Post() { Name = postReqest.Name };

            bool created =  await _postService.CreatePostAsync(post);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";

            var location = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString());

            var response = new CreatePostResponse() { Id = post.Id };

            return Created(location, response);
          
          
         
        }

    }
}
