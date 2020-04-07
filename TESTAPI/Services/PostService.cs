using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TESTAPI.Data;
using TESTAPI.Domain;

namespace TESTAPI.Services
{
    public class PostService :IPostService
    {
        private readonly DataContext _dataContext;
      
        public PostService(DataContext dataContext)
        {
            _dataContext = dataContext;
           
        }
        public async Task<bool> CreatePostAsync(Post postToCreate)
        {
            await _dataContext.AddAsync(postToCreate);
            var created =  await _dataContext.SaveChangesAsync();
            return created > 0;
        }

        public async  Task<bool> DeletePostAsync(Guid postToDelete)
        {
            Post postDeleteObj = await GetPostByIdAsync(postToDelete);

            if (postDeleteObj != null)
            {
                _dataContext.Posts.Remove(postDeleteObj);
                var deleted = await _dataContext.SaveChangesAsync();
                return deleted > 0;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<string>> GetAllTagsAsync()
        {
            List<string> StringList = new List<string>() { "AA", "BB", "CC" };

            return await Task.Run(() => StringList.ToList());
        }

        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            return await _dataContext.Posts.Where(x => x.Id == postId).FirstOrDefaultAsync();
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            return await _dataContext.Posts.ToListAsync();
        }

        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {
             _dataContext.Posts.Update(postToUpdate);
              var updated = await _dataContext.SaveChangesAsync();
              return updated > 0;


        }

        public async Task<bool> UserOwnsPostAsync(Guid postId, string getUserId)
        {
            var post = await _dataContext.Posts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == postId);

            if (post == null)
            {
                return false;
            }

            if (post.UserId != getUserId)
            {
                return false;
            }

            return true;
        }
    }
}
