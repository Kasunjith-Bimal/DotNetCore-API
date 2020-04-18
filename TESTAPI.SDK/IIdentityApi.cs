using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TESTAPI.Contract.V1.Requests;
using TESTAPI.Contract.V1.Responses;

namespace TESTAPI.SDK
{
    public interface IIdentityApi
    {
        [Post("/api/v1/identity/register")]
        Task<ApiResponse<AuthSucessResponse>> RegisterAsync([Body] UserRegistrationReqest registrationReqest);

        [Post("/api/v1/identity/login")]
        Task<ApiResponse<AuthSucessResponse>> LoginAsync([Body] UserLoginReqest loginReqest);

        [Post("/api/v1/identity/Refresh")]
        Task<ApiResponse<AuthSucessResponse>> RefreshAsync([Body] RefreshTokenReqest refreshTokenReqest);



    }
}
