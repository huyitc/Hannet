
using KioskManagement.Data.Repositories;
using KioskManagement.Model.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KioskManagement.Service
{
    public interface IApplicationUserRoleService
    {
        //IEnumerable<App_User_Role_Mapping> GetByUserId(string userId);
         Task<IQueryable<string>> GetAllUserRole(string userId);

        Task DeleteMultipleAppUserRoleByRoleId(string roleId);

        Task<List<string>> GetListRoleCheck(string userName);
    }

    public class ApplicationUserRoleService : IApplicationUserRoleService
    {
        private readonly IApplicationUserRoleRepository _applicationUserRoleRepository;

        public ApplicationUserRoleService(IApplicationUserRoleRepository applicationUserRoleRepository)
        {
            _applicationUserRoleRepository = applicationUserRoleRepository;
        }

        public async Task<IQueryable<string>> GetAllUserRole(string userId)
        {
            return  await _applicationUserRoleRepository.GetListRole(userId);
        }

        //public IEnumerable<App_User_Role_Mapping> GetByUserId(string userId)
        //{
        //    return _applicationUserRoleRepository.GetByUserId(userId);
        //}

        public async Task DeleteMultipleAppUserRoleByRoleId(string roleId)
        {
            await _applicationUserRoleRepository.DeleteMulti(x => x.RoleId == roleId);
        }


        public async Task< List<string>> GetListRoleCheck(string userName)
        {
            return await _applicationUserRoleRepository.GetListRoleCheck(userName);
        }
    }
}