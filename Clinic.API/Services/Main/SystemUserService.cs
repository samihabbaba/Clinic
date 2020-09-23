using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clinic.API.DataAccess;
using Clinic.API.Models;
using Clinic.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Clinic.API.Services.Main
{
    public interface ISystemUserService
    {
        Task<bool> AddSystemUser(SystemUser SystemUser);
        Task<bool> DeleteSystemUser(SystemUser SystemUser);
        Task<bool> EditSystemUser(SystemUser SystemUser);
        Task<PagedList<SystemUser>> GetAllSystemUser(ResourceParameter parameter);
        Task<SystemUser> GetSystemUserById(int id);
    }

    public class SystemUserService : ISystemUserService
    {
        private DataContext _context;

        public SystemUserService(DataContext context)
        {
            _context = context;
        }

        public async Task<PagedList<SystemUser>> GetAllSystemUser(ResourceParameter parameter)
        {
            var collection = _context.SystemUser.OrderBy(x => x.Id).AsQueryable();

            if (parameter.Status != null)
                collection = collection.Where(x => x.Status == parameter.Status);

            if (!string.IsNullOrEmpty(parameter.SearchQuery))
            {
                var SearchQueryForWhere = parameter.SearchQuery.Trim().ToLowerInvariant();
                collection = collection.Where(x => x.Name.Contains(SearchQueryForWhere));

            }

            return await PagedList<SystemUser>.CreateAsync(collection, parameter.PageNumber, parameter.PageSize);
        }
        public async Task<SystemUser> GetSystemUserById(int id)
        {
            return await _context.SystemUser.FindAsync(id);
        }

        public async Task<bool> AddSystemUser(SystemUser SystemUser)
        {
            await _context.SystemUser.AddAsync(SystemUser);
            return await Save();
        }
        public async Task<bool> EditSystemUser(SystemUser SystemUser)
        {
            _context.SystemUser.Update(SystemUser);
            return await Save();
        }
        public async Task<bool> DeleteSystemUser(SystemUser SystemUser)
        {
            _context.SystemUser.Remove(SystemUser);
            return await Save();
        }


        private async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}