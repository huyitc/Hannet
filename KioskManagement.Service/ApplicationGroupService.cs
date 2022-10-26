using KioskManagement.Common.Exceptions;
using KioskManagement.Data.Repositories;
using KioskManagement.Model.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KioskManagement.Service
{
    public interface IApplicationGroupService
    {
        Task<AppGroup> GetDetail(int id);

        Task<AppGroup> GetByName(string name);

        Task<IQueryable<AppGroup>> GetAll(string keyword);

        Task<IQueryable<AppGroup>> GetAll();

        Task<AppGroup> Add(AppGroup appGroup);

        Task<AppGroup> Update(AppGroup appGroup);

        Task<AppGroup> Delete(int id);

        Task<bool> AddUserToGroups(List<AppUserGroup> groups, string userId);

        Task<IQueryable<AppGroup>> GetListGroupByUserId(string userId);

        Task<IQueryable<AppUser>> GetListUserByGroupId(int groupId);
        Task<AppGroup> GetById(int id);

        //AppGroup GetByRoleId(string roleId);
    }

    public class ApplicationGroupService : IApplicationGroupService
    {
        private IApplicationGroupRepository _appGroupRepository;
        private IApplicationUserGroupRepository _appUserGroupRepository;

        public ApplicationGroupService(IApplicationUserGroupRepository appUserGroupRepository,
            IApplicationGroupRepository appGroupRepository)
        {
            _appGroupRepository = appGroupRepository;
            _appUserGroupRepository = appUserGroupRepository;
        }

        public async Task<AppGroup> Add(AppGroup appGroup)
        {
            if (await _appGroupRepository.CheckContainsAsync(x => x.GroupCode == appGroup.GroupCode))
                throw new NameDuplicatedException("Mã không được trùng");
            return await _appGroupRepository.AddASync(appGroup);
        }

        public async Task<AppGroup> Delete(int id)
        {
            //var appGroup = await this._appGroupRepository.GetByIdAsync(id);
            return await _appGroupRepository.DeleteAsync(id);
        }

        public async Task<IQueryable<AppGroup>> GetAll()
        {
            return await _appGroupRepository.GetAllAsync();
        }

        public async Task<IQueryable<AppGroup>> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return await _appGroupRepository.GetAllAsync(x => x.GroupCode.ToUpper().Contains(keyword.ToUpper()) || x.Name.ToUpper().Contains(keyword.ToUpper()));
            else
                return await _appGroupRepository.GetAllAsync();
        }

        public async Task<AppGroup> GetDetail(int id)
        {
            return await _appGroupRepository.GetByIdAsync(id);
        }

        public async Task<AppGroup> Update(AppGroup appGroup)
        {
            if (await _appGroupRepository.CheckContainsAsync(x => x.GroupCode == appGroup.GroupCode && x.Id != appGroup.Id))
                throw new NameDuplicatedException("Mã không được trùng");
            return await _appGroupRepository.UpdateASync(appGroup);
        }

        public async Task<bool> AddUserToGroups(List<AppUserGroup> userGroups, string userId)
        {
            await _appUserGroupRepository.DeleteMulti(x => x.UserId == userId);
            foreach (var userGroup in userGroups)
            {
                await _appUserGroupRepository.AddASync(userGroup);
            }
            return true;
        }

        public async Task< IQueryable<AppGroup>> GetListGroupByUserId(string userId)
        {
            return await _appGroupRepository.GetListGroupByUserId(userId);
        }

        public async Task <IQueryable<AppUser>> GetListUserByGroupId(int groupId)
        {
            return await _appGroupRepository.GetListUserByGroupId(groupId);
        }

        public async Task<AppGroup> GetByName(string name)
        {
            return await _appGroupRepository.GetSingleByConditionAsync(x => x.GroupCode == name);
        }

        public async Task<AppGroup> GetById(int id)
        {
            return await _appGroupRepository.GetByIdAsync(id);
        }
    }
}