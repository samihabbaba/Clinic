using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Clinic.API.Contracts;
using AutoMapper;
using Clinic.API.Dtos.SystemUsersDto;
using Clinic.API.Dtos.UserListDto;
using Clinic.API.Helpers;
using Clinic.API.Models;
using Clinic.API.Services.Main;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Clinic.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        // private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly SignInManager<SystemUser> _signInManager;
        private readonly UserManager<SystemUser> _userManager;
        private IAuthenticationService _context;
        private ILoggerManager _logger;
        private readonly ISystemUserService _systemUserService;
        public AuthController(
        IMapper mapper, UserManager<SystemUser> userManager, SignInManager<SystemUser> signInManager,
        IAuthenticationService context, ILoggerManager logger,ISystemUserService systemUserService)
        {
            _systemUserService = systemUserService;
            _userManager = userManager;
            _signInManager = signInManager;
             _mapper = mapper;
            _context = context;
            _logger = logger;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] RegisterDto registerDto)
        {

            var userToCreate = _mapper.Map<SystemUser>(registerDto);

            try
            {
                _systemUserService.AddSystemUser(userToCreate, registerDto.Password, registerDto.Role);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        // [HttpPost("register")]
        // public async Task<IActionResult> Register(RegisterDto registerDto)
        // {
        // var userToCreate = _mapper.Map<SystemUser>(registerDto);

        // var result = await _userManager.CreateAsync(userToCreate,
        // registerDto.Password);

        // var userToReturn = _mapper.Map<SystemUserViewDto>(userToCreate);

        // if (result.Succeeded)
        // {
        //     return CreatedAtRoute("GetSystemUser", new { controller = "SystemUser",
        //     id = userToCreate.Id }, userToReturn);
        // }
        // return BadRequest(result.Errors); 
        // }

        // var result = await _userManager.CreateAsync(userToCreate,
        // registerDto.Password);

        // var userToReturn = _mapper.Map<SystemUserViewDto>(userToCreate);

        // if (result.Succeeded)
        // {
        //     return CreatedAtRoute("GetSystemUser", new
        //     {
        //         controller = "SystemUser",
        //         id = userToCreate.Id
        //     }, userToReturn);
        // }

        // return BadRequest(result.Errors);
    
    // [HttpPost("login")]
    // [ProducesResponseType(typeof(AuthenticationResult), StatusCodes.Status200OK)]
    // public async Task<IActionResult> Authenticate([FromBody]LoginDto ar)
    // {
    //     var staff = await _context.AuthenticateUser(ar);
    //     if (!staff.Success)
    //     {
    //         _logger.LogError($"Failed Login attempt for {ar.Username}");
    //     }
    //     return Ok(staff);
    // }

    // [HttpPost("login")]
    // public async Task<IActionResult> Login(LoginDto loginDto)
    // {
    //     var user = await _userManager.FindByNameAsync(loginDto.Username);

    //     var result = await _signInManager.CheckPasswordSignInAsync(user,
    //     loginDto.Password, false);

    //     if (result.Succeeded)
    //     {
    //         var appUser = _mapper.Map<SystemUserViewDto>(user);

    //         return Ok(new
    //         {
    //             token = GenerateJwtToken(user).Result,
    //             user = appUser
    //         });
    //     }

    //     return Unauthorized();

    // }

    // private async Task<string> GenerateJwtToken(SystemUser systemUser)
    // {
    //     var claims = new List<Claim>
    // {
    //         new Claim(ClaimTypes.NameIdentifier, systemUser.Id.ToString()),
    //         new Claim(ClaimTypes.Name, systemUser.UserName)
    //     };

    //     var roles = await _userManager.GetRolesAsync(systemUser);

    //     foreach (var role in roles)
    //     {
    //         claims.Add(new Claim(ClaimTypes.Role, role));
    //     }

    //     var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Secret").Value));

    //     var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

    //     var tokenDescriptor = new SecurityTokenDescriptor
    //     {
    //         Subject = new ClaimsIdentity(claims),
    //         Expires = DateTime.Now.AddDays(1),
    //         SigningCredentials = creds
    //     };

    //     var tokenHandler = new JwtSecurityTokenHandler();

    //     var token = tokenHandler.CreateToken(tokenDescriptor);

    //     return tokenHandler.WriteToken(token);
    // }

    }
}