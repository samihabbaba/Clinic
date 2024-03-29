using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Clinic.API.DataTransferObject;
using AutoMapper;
using Clinic.API.Dtos;
using Clinic.API.Dtos.UserCreationDto;
using Clinic.API.Dtos.UserListDto;
using Clinic.API.Dtos.UserUpdateDto;
using Clinic.API.Models;
using Clinic.API.Services.Main;
using Clinic.Extensions;
using Clinic.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace Clinic.API.Controllers
{
    [Produces("application/json")]
    [Route("api/suers")]
    public class SystemUserController : ControllerBase
    {
        private readonly ISystemUserService _context;
        private readonly LinkGenerator _link;
        private readonly IMapper _mapper;
        private readonly UserManager<SystemUser> _userManager;
        public SystemUserController(ISystemUserService context,
        LinkGenerator link, IMapper mapper, UserManager<SystemUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            _link = link;
            _mapper = mapper;
        }

    [HttpGet("all")]
    [Authorize(Roles = "Doctor, Admin")]
    public async Task<IActionResult> GetAllSystemUser(ResourceParameter parameter)
    {
        var model = await _context.GetAllSystemUser(parameter);
        var prevLink = model.HasPrevious ? CreateSystemUserListResourceUri(parameter, ResourceUriType.PreviousPage) : null;

        var nextLink = model.HasNext ? CreateSystemUserListResourceUri(parameter, ResourceUriType.NextPage) : null;

        var pageInfo = new PagingDto
        {
            totalCount = model.Count,
            pageSize = model.PageSize,
            totalPages = model.TotalPages,
            currentPage = model.CurrentPage,
            prevLink = prevLink,
            nextLink = nextLink,
        };


        var SystemUserMap = new SystemUserPaging
        {
            SystemUsers = _mapper.Map<IEnumerable<SystemUserViewDto>>(model),
            PagingInfo = pageInfo
        };

        return Ok(SystemUserMap);
    }

    [HttpGet("SystemUser/{id}")]
    [Authorize(Roles = "Doctor, Admin")]
    [ProducesResponseType(typeof(SystemUser), StatusCodes.Status200OK)]

    public async Task<IActionResult> GetSystemUserById(string id)
    {
        var model = await _context.GetSystemUserById(id);
        return Ok(model);
    }

    [HttpPost("SystemUser")]
    [Authorize(Roles = "Admin, Doctor")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddSystemUser([FromBody] SystemUserCreationDto systemUser)
    {
        var role = HttpContext.GetUserRole();

        if((systemUser == null) || (role != "Admin" && systemUser.Role == "Admin"))
            return BadRequest();


        var systemUserEntity = _mapper.Map<SystemUser>(systemUser);

        var result = await _context.AddSystemUser(systemUserEntity, systemUser.Password, systemUser.Role);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Doctor")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]

    public async Task<IActionResult> UpdateSystemUser(string id, [FromBody] UpdateDto systemUser)
    {
        if (systemUser == null)
            return BadRequest();

        var systemUserFromDb = await _context.GetSystemUserById(id);

        if (systemUserFromDb == null)
            return NotFound();

        systemUserFromDb.Status = systemUser.Status;
        systemUserFromDb.Description = systemUser.Description;

        var result = await _context.EditSystemUser(systemUserFromDb);

        return Ok(result);
    }


    [HttpPut("editProfile")]
    [Authorize(Roles = "Patient, Doctor")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]

    public async Task<IActionResult> EditProfile(string id,[FromBody]EditProfileDto systemUser)
    {

        if (systemUser == null)
            return BadRequest();

        // var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);


        var systemUserFromDb = await _context.GetSystemUserById(id);

        if (systemUserFromDb == null)
            return NotFound();

        systemUserFromDb.Email = systemUser.Email;
        systemUserFromDb.Country = systemUser.Country;
        systemUserFromDb.Description = systemUser.Description;
        systemUserFromDb.Address = systemUser.Address;
        systemUserFromDb.AboutMe = systemUser.AboutMe;

        var result = await _context.EditSystemUser(systemUserFromDb);


        return Ok(result);
    }


    private string CreateSystemUserListResourceUri(ResourceParameter parameter, ResourceUriType type)
    {
        switch (type)
        {
            case ResourceUriType.PreviousPage:
                return _link.GetPathByAction(HttpContext, "GetAllSystemUser",
                    values: new
                    {
                        searchQuery = parameter.SearchQuery,
                        pageNumber = parameter.PageNumber - 1,
                        pageSize = parameter.PageSize,
                    });
            case ResourceUriType.NextPage:
                return _link.GetPathByAction(HttpContext, "GetAllSystemUser",
                    values: new
                    {
                        searchQuery = parameter.SearchQuery,
                        pageNumber = parameter.PageNumber + 1,
                        pageSize = parameter.PageSize,

                    });
            case ResourceUriType.Current:
                return _link.GetPathByAction(HttpContext, "GetAllSystemUser",
                    values: new
                    {
                        searchQuery = parameter.SearchQuery,
                        pageNumber = parameter.PageNumber,
                        pageSize = parameter.PageSize,

                    });
            default:
                return _link.GetPathByAction(HttpContext, "GetAllSystemUser",
                    values: new
                    {
                        searchQuery = parameter.SearchQuery,
                        pageNumber = parameter.PageNumber,
                        pageSize = parameter.PageSize,

                    });
        }
    }
}
}