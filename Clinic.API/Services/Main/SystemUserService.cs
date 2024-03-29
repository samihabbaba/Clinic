using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clinic.API.DataAccess;
using Clinic.API.Models;
using Clinic.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Clinic.API.Services.Main
{
    public interface ISystemUserService
    {
        Task<bool> AddSystemUser(SystemUser systemUser,string password, string role);
        Task<bool> DeleteSystemUser(SystemUser systemUser);
        Task<bool> EditSystemUser(SystemUser systemUser);
        Task<PagedList<SystemUser>> GetAllSystemUser(ResourceParameter parameter);
        Task<SystemUser> GetSystemUserById(string id);
    }

    public class SystemUserService : ISystemUserService
    {
        private DataContext _context;
        private UserManager<SystemUser> _userManager;

        public SystemUserService(DataContext context,UserManager<SystemUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<PagedList<SystemUser>> GetAllSystemUser(ResourceParameter parameter)
        {
            var collection = _context.SystemUsers.OrderBy(x => x.Id).AsQueryable();

            if (parameter.Status != null)
                collection = collection.Where(x => x.Status == parameter.Status);

            if (!string.IsNullOrEmpty(parameter.SearchQuery))
            {
                var SearchQueryForWhere = parameter.SearchQuery.Trim().ToLowerInvariant();
                collection = collection.Where(x => x.Name.Contains(SearchQueryForWhere));

            }

            return await PagedList<SystemUser>.CreateAsync(collection, parameter.PageNumber, parameter.PageSize);
        }
        public async Task<SystemUser> GetSystemUserById(string id)
        {
            return await _context.SystemUsers.FindAsync(id);
        }

        public async Task<bool> AddSystemUser(SystemUser user,string password, string role)
        {
            try{
                    // user.Email = "Kasem@disc.com";
                    // user.DateOfBirth = DateTime.Now;
                    // user.PhoneNumber = "05338722255";
                // var user = await _context.SystemUser.AddAsync(SystemUser);
                IdentityResult userToSave = await _userManager.CreateAsync(user,password);
                if(userToSave.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user,role);
                }
            }
            catch(Exception ex)
            {

            }

            return await Save();
        }
        public async Task<bool> EditSystemUser(SystemUser systemUser)
        {
            _context.SystemUsers.Update(systemUser);
            return await Save();
        }
        public async Task<bool> DeleteSystemUser(SystemUser systemUser)
        {
            _context.SystemUsers.Remove(systemUser);
            return await Save();
        }

        private async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}