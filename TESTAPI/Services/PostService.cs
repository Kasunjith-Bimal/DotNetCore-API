using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TESTAPI.Domain;

namespace TESTAPI.Services
{
    public class PostService :IPostService
    {
        private readonly List<Post> _PostList;

        public PostService()
        {
            _PostList = new List<Post>();
            for (int i = 0; i < 10; i++)
            {
                _PostList.Add(new Post() { Id = Guid.NewGuid(), Name = $"Post Name {i}" });
            }
        }

        public Post GetPostById(Guid postId)
        {
            return _PostList.Where(x => x.Id == postId).FirstOrDefault();
        }

        public List<Post> GetPosts()
        {
            return _PostList;
        }

        public bool UpdatePost(Post postToUpdate)
        {
            var exists = GetPostById(postToUpdate.Id);
            if (exists != null)
            {
                var index = _PostList.FindIndex(x => x.Id == postToUpdate.Id);
                _PostList[index] = postToUpdate;
                return true;
            }
            else
            {
                return false;
            }
          
        }
    }
}
