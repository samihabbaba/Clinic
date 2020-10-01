using API.Clinic.API.Contracts;
using AutoMapper;
using Clinic.API.Dtos.SystemUsersDto;
using Clinic.API.Helpers;
using Clinic.API.Models;
using Clinic.API.Services.Main;
using Clinic.Services.Main;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Clinic.Helpers;

namespace Clinic.API.Controllers
{
    [Produces("application/json")]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticationService _context;
        private ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly ISystemUserService _systemUserService;
        private readonly UserManager<SystemUser> _userManager;
        private readonly IConfiguration _config;

        public AuthenticationController(IAuthenticationService context
        , ILoggerManager logger, IMapper mapper, ISystemUserService systemUserService,
        UserManager<SystemUser> userManager, IConfiguration config)
        {
            _config = config;
            _userManager = userManager;
            _systemUserService = systemUserService;
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }



        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {

            var userToCreate = _mapper.Map<SystemUser>(registerDto);
            
            try
            {
                await _systemUserService.AddSystemUser(userToCreate, registerDto.Password, registerDto.Role);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
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

    }
}