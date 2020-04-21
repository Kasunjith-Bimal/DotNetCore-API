using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TESTAPI.Contract.V1;
using TESTAPI.Contract.V1.Requests;
using TESTAPI.Contract.V1.Requests.Queries;
using TESTAPI.Contract.V1.Responses;
using TESTAPI.Domain;
using TESTAPI.Extention;
using TESTAPI.Helpers;
using TESTAPI.Services;

namespace TESTAPI.Controllers.V1
{
    [Authorize (Roles ="Poster,Admin")]
    [Produces("application/json")]
    public class PostController:Controller
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        public PostController(IPostService postService, IMapper mapper, IUriService uriService)
        {
            _postService = postService;
            _mapper = mapper;
            _uriService = uriService;
        }

        /// <summary>
        /// Return All Psot In System
        /// </summary>
        /// <response code="200">Return all the tags in the system</response>
        [HttpGet(ApiRoutes.Posts.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery]PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
           
            var posts = await _postService.GetPostsAsync(pagination);
         
            var postResponses = _mapper.Map<List<PostResponse>>(posts);

            if (pagination == null || pagination.PageSize < 1 || pagination.PageNumber <1)
            {
                return Ok(new PagedResponse<PostResponse>(postResponses));
            }

            var paginationResponse = PaginationHelper.CreeatePaginatedResponse(_uriService, pagination, postResponses);

           

            return Ok(paginationResponse);

        }

        [HttpGet(ApiRoutes.Posts.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(new Response<PostResponse>(_mapper.Map<PostResponse>(post)));

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
                return Ok(new Response<PostResponse>( _mapper.Map<PostResponse>(post)));
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

            // var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";

            //var location = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString());

            var location = _uriService.GetPostUri(post.Id.ToString());
            return Created(location, new Response<PostResponse>( _mapper.Map<PostResponse>(post)));
          
          
         
        }

    }
}
