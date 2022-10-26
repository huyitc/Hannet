using KioskManagement.Common.Exceptions;
using KioskManagement.Data.Repositories;
using KioskManagement.Model.MappingModels;
using KioskManagement.Model.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KioskManagement.Service
{
    public interface IApplicationRoleService
    {
        Task<AppRole> GetDetail(string id);

        Task<IQueryable<AppRole>> GetAll(string keyword);

        Task<IQueryable<AppRole>> GetAll();

        Task<AppRole> Add(AppRole appRole);

        Task<AppRole> Update(AppRole appRole);

        Task<AppRole> Delete(string id);

        Task<bool> AddRolesToGroup(List<AppRoleGroup> roleGroups, int groupId);

        Task< IQueryable<AppRole>> GetListRoleByGroupId(int groupId);

        Task<IQueryable<AppRoleMapping>> GetTreeRoles();
        Task<IQueryable<AppMenuMappingNew>> GetTreeMenuByUserId(string userId);

        Task<IQueryable<AppUser>> GetListUserByGroupId(int groupId);
    }

    public class ApplicationRoleService : IApplicationRoleService
    {
        private IApplicationRoleRepository _appRoleRepository;
        private IApplicationRoleGroupRepository _appRoleGroupRepository;

        public ApplicationRoleService(IApplicationRoleRepository appRoleRepository, IApplicationRoleGroupRepository appRoleGroupRepository)
        {
            _appRoleRepository = appRoleRepository;
            _appRoleGroupRepository = appRoleGroupRepository;
        }

        public async Task<AppRole> Add(AppRole appRole)
        {
            if (await _appRoleRepository.CheckContainsAsync(r => r.Description == appRole.Description || r.Name == appRole.Name))
                throw new NameDuplicatedException("Tên không được trùng.");
            return await _appRoleRepository.AddASync(appRole);
        }

        public async Task<bool> AddRolesToGroup(List<AppRoleGroup> roleGroups, int groupId)
        {
            await _appRoleGroupRepository.DeleteMulti(x => x.GroupId == groupId);
            foreach (var roleGroup in roleGroups)
            {
                await _appRoleGroupRepository.AddASync(roleGroup);
            }
            return true;
        }

        public async Task<AppRole> Delete(string id)
        {
            return await _appRoleRepository.DeleteAsync(id);
        }

        public async Task<IQueryable<AppRole>> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return await _appRoleRepository.GetAllAsync(x => x.Name.ToUpper().Contains(keyword.ToUpper()) || x.Description.Contains(keyword.ToUpper()));
            else
                return await _appRoleRepository.GetAllAsync();
        }

        public async Task<IQueryable<AppRole>> GetAll()
        {
            return await _appRoleRepository.GetAllAsync();
        }

        public async Task<AppRole> GetDetail(string id)
        {
            return await _appRoleRepository.GetSingleByConditionAsync(s => s.Id == id);
        }

        public async Task<IQueryable<AppRole>> GetListRoleByGroupId(int groupId)
        {
            return await _appRoleRepository.GetListRoleByGroupId(groupId);
        }

        public async Task< IQueryable<AppRoleMapping>> GetTreeRoles()
        {
            return await _appRoleRepository.GetTreeRoles();
        }

        public async Task<AppRole> Update(AppRole appRole)
        {
            if (await _appRoleRepository.CheckContainsAsync(x => x.Description == appRole.Description && x.Name == appRole.Name && x.Id != appRole.Id))
                throw new NameDuplicatedException("Tên không được trùng.");
            return await _appRoleRepository.UpdateASync(appRole);
        }
        public async Task<IQueryable<AppMenuMappingNew>> GetTreeMenuByUserId(string userId)
        {
            return await _appRoleRepository.GetTreeMenuByUserId(userId);
        }

        public async Task<IQueryable<AppUser>> GetListUserByGroupId(int groupId)
        {
            return await _appRoleRepository.GetListUserByGroupId(groupId);
        }

       
    }
}