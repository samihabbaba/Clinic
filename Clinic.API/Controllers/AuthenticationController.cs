using API.Clinic.API.Contracts;
using Clinic.API.Dtos.SystemUsersDto;
using Clinic.Services.Main;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")] 
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticationService _context;
        private ILoggerManager _logger;

        public AuthenticationController(IAuthenticationService context
        , ILoggerManager logger)
        {
            _context = context;
            _logger = logger;
        }


    

        [HttpPost("login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> Authenticate([FromBody]LoginDto ar)
        {
            var token = await _context.AuthenticateUser(ar);
            if (token == "error")
            {
                _logger.LogError($"Failed Login attempt for {ar.UserName}");
            }
            return Ok(token);
        }

        // [HttpPost(ApiRoutes.Authentication.Refresh)]
        // [ProducesResponseType(typeof(AuthenticationResult), StatusCodes.Status200OK)]
        // public async Task<IActionResult> RefreshToken([FromBody]RefreshTokenRequest ar)
        // {
        //     var staff = await _context.RefreshTokenAsync(ar.Token, ar.RefreshToken);
        //     return Ok(staff);
        // }
    }
}
