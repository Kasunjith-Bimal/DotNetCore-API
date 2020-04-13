﻿using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TESTAPI.Contract.V1;
using TESTAPI.Contract.V1.Requests;
using TESTAPI.Contract.V1.Responses;
using TESTAPI.Data;

namespace TESTAPI.TESTING
{
    public class IntergrationTesting: IDisposable
    {
        protected readonly HttpClient TestClient;
        private readonly IServiceProvider _serviceProvider;

        protected IntergrationTesting()
        {
            var appFactory = new WebApplicationFactory<Startup>().
                WithWebHostBuilder(builder => {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DataContext));
                        services.AddDbContext<DataContext>(options => { options.UseInMemoryDatabase("TestDb"); });
                    });

                 });
            _serviceProvider = appFactory.Services;
            TestClient = appFactory.CreateClient();
        }

        public void Dispose()
        {
            using var serviceScope = _serviceProvider.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<DataContext>();
            context.Database.EnsureDeleted();

        }

        protected async Task AuthenticationAsync()
        {
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetJwtAsync());
        }


        protected async Task<PostResponse> CreatePostAsync(CreatePostReqest reqest)
        {
           var response =  await TestClient.PostAsJsonAsync(ApiRoutes.Posts.Create, reqest);

           return await response.Content.ReadAsAsync<PostResponse>();
        }



        private async Task<string> GetJwtAsync()
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Identity.Register, new UserRegistrationReqest
            {
                Email = "Test@intergation.com",
                Password = "SomePassword123@",
            });

            var registrationResponse = await response.Content.ReadAsAsync<AuthSucessResponse>();
            return registrationResponse.Token;

        }
    }
}
