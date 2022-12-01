using Hannet.Data.Repositories;
using Hannet.Model.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hannet.Service
{
    public interface IApplicationRoleGroupService
    {
        Task<IQueryable<AppRoleGroup>> GetListByGroupId(int groupId);

        Task DeleteMultipleAppRoleGroup(int groupId);

        Task DeleteMultipleAppRoleGroupByRoleId(string roleId);
    }

    public class ApplicationRoleGroupService : IApplicationRoleGroupService
    {
        private IApplicationRoleGroupRepository _appRoleGroupRepository;

        public ApplicationRoleGroupService(IApplicationRoleGroupRepository appRoleGroupRepository)
        {
            _appRoleGroupRepository = appRoleGroupRepository;
        }

        public async Task< IQueryable<AppRoleGroup>> GetListByGroupId(int groupId)
        {
            return await _appRoleGroupRepository.GetListByGroupId(groupId);
        }

        public async Task DeleteMultipleAppRoleGroup(int groupId)
        {
            await _appRoleGroupRepository.DeleteMulti(x => x.GroupId == groupId);
        }

        public async Task DeleteMultipleAppRoleGroupByRoleId(string roleId)
        {
            await  _appRoleGroupRepository.DeleteMulti(x => x.RoleId == roleId);
        }
    }
}