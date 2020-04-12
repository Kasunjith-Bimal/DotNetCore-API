using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TESTAPI.Contract.V1;
using TESTAPI.Services;

namespace TESTAPI.Controllers.V1
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly IPostService _postService;

        public CommentController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet(ApiRoutes.Comment.GetAll)]
        [Authorize(Policy = "WorkMyComapny")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _postService.GetAllTagsAsync());


        }
    }
}
