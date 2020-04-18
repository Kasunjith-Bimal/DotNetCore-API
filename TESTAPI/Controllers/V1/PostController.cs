using AutoMapper;
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
using TESTAPI.Extention;
using TESTAPI.Services;

namespace TESTAPI.Controllers.V1
{
    [Authorize (Roles ="Poster,Admin")]
    [Produces("application/json")]
    public class PostController:Controller
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public PostController(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        /// <summary>
        /// Return All Psot In System
        /// </summary>
        /// <response code="200">Return all the tags in the system</response>
        [HttpGet(ApiRoutes.Posts.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var posts = await _postService.GetPostsAsync();
            var postResponses = _mapper.Map<List<PostResponse>>(posts);
            return Ok(postResponses);

        }

        [HttpGet(ApiRoutes.Posts.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PostResponse>(post));

        }

        [HttpPut(ApiRoutes.Posts.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid postId,[FromBody] UpdatePostReqest updatePostReqest)
        {

            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());

            if (!userOwnsPost)
            {
                return BadRequest(new { error = "You do not on this post" });
            }

            var post = await _postService.GetPostByIdAsync(postId);

            post.Name = updatePostReqest.Name;

            bool isUpdated = await _postService.UpdatePostAsync(post);

            if (isUpdated)
            {
                return Ok(_mapper.Map<PostResponse>(post));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete(ApiRoutes.Posts.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid postId)
        {

            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());

            if (!userOwnsPost)
            {
                return BadRequest(new { error = "You do not on this post" });
            }

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


        /// <summary>
        /// Create a Post in the System 
        /// </summary>
        /// <response code="201">Create a tag in the system </response>
        /// <response code="400">Unable to Create the post due to validation error </response>
        /// <response code="401">Unauthorize </response>
        [HttpPost(ApiRoutes.Posts.Create)]
        [Authorize(Roles = "Poster")]
        [ProducesResponseType(typeof(PostResponse),201)]
        [ProducesResponseType(typeof(ErrorResponse),400)]
        public async Task<IActionResult> Post([FromBody] CreatePostReqest postReqest)
        {

            var post = new Post() {
                Name = postReqest.Name,
                UserId = HttpContext.GetUserId() 
            };

            bool created =  await _postService.CreatePostAsync(post);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";

            var location = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString());

            return Created(location, _mapper.Map<PostResponse>(post));
          
          
         
        }

    }
}
