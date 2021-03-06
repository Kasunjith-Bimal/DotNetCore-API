﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TESTAPI.Domain;

namespace TESTAPI.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetPostsAsync(PaginationFilter paginationFilter =null);

        Task<bool> CreatePostAsync(Post postToCreate);

        Task<Post> GetPostByIdAsync(Guid postId);

        Task<bool> UpdatePostAsync(Post postToUpdate);

        Task<bool> DeletePostAsync(Guid postToDelete);
        Task<bool> UserOwnsPostAsync(Guid postId, string getUserId);
        Task<List<string>> GetAllTagsAsync();
    }
}
