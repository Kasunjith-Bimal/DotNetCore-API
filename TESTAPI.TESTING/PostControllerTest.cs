using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TESTAPI.Contract.V1;
using TESTAPI.Domain;
using Xunit;

namespace TESTAPI.TESTING
{
    public class PostControllerTest : IntergrationTesting
    {
        [Fact]
        public async Task GetAll_WithoutAnyPosts_ReturnEmptyResponse()
        {
            //Arrange
            await AuthenticationAsync();
            //Act
            var response = await TestClient.GetAsync(ApiRoutes.Posts.GetAll);
            //Assert 
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<List<Post>>()).Should().BeEmpty();
        }

        [Fact]
        public async Task Get_ReturnsPost_WhenPostExistInTheDatabase()
        {
            //Arrange
            await AuthenticationAsync();
            var createdPost =    await CreatePostAsync(new Contract.V1.Requests.CreatePostReqest { Name = "TestPost" });


            //Act
            var response = await TestClient.GetAsync(ApiRoutes.Posts.Get.Replace ("{postId}", createdPost.Id.ToString()));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnPost = await response.Content.ReadAsAsync<Post>();
            returnPost.Id.Should().Be(createdPost.Id);
            returnPost.Name.Should().Be("TestPost");
        }

    }
}
