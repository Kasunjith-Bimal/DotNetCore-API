using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TESTAPI.Contract.V1;
using TESTAPI.Contract.V1.Requests;
using TESTAPI.Contract.V1.Responses;
using TESTAPI.Services;

namespace TESTAPI.Controllers.V1
{
    public class IdentityController:Controller
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationReqest reqest)
        {
            var authResponse = await _identityService.RegistrationAsync(reqest.Email, reqest.Password);

            if (!authResponse.Sucess)
            {
                return BadRequest(new AuthFaillResponse {
                    Error = authResponse.ErrorMessage
                }); 
            }

            return Ok(new AuthSucessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken

            });
        }


        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginReqest reqest)
        {
            var authResponse = await _identityService.LoginAsync(reqest.Email, reqest.Password);

            if (!authResponse.Sucess)
            {
                return BadRequest(new AuthFaillResponse
                {
                    Error = authResponse.ErrorMessage
                });
            }

            return Ok(new AuthSucessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken

            });
        }

        [HttpPost(ApiRoutes.Identity.Refresh)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenReqest reqest)

        {
            var authResponse = await _identityService.RefreshTokenAsync(reqest.Token, reqest.RefreshToken);

            if (!authResponse.Sucess)
            {
                return BadRequest(new AuthFaillResponse
                {
                    Error = authResponse.ErrorMessage
                });
            }

            return Ok(new AuthSucessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken

            });
        }
    }
} 
