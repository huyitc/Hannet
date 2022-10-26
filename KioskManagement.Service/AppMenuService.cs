using KioskManagement.Data.Repositories;
using KioskManagement.Model.MappingModels;
using KioskManagement.Model.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KioskManagement.Service
{
    public interface IAppMenuService
    {
        Task<IQueryable<AppMenu>> GetAll();

        Task<IQueryable<AppMenu>> GetAll(string keyword);

        Task<IQueryable<AppMenuMapping>> GetTreeMenuByUserId(string userId);
        Task<IQueryable<AppMenuMappingNew>> GetTreeMenuByUserIdNew(string userId);

        Task<AppMenu> Add(AppMenu menu);

        Task<AppMenu> Update(AppMenu menu);

        Task<AppMenu> Delete(int id);

        Task<AppMenu> GetById(int id);

        Task<IQueryable<AppMenuMapping>> GetTreeMenu();

        Task<IQueryable<AppMenuMapping>> GetMenuUser(string userId);
        Task<IQueryable<AppMenuMappingNew>> GetMenuUserNew(string userId);
    }

    internal class AppMenuService : IAppMenuService
    {
        private readonly IAppMenuRepository _appMenuRepository;

        public AppMenuService(IAppMenuRepository appMenuRepository)
        {
            _appMenuRepository = appMenuRepository;
        }

        public async Task<AppMenu> Add(AppMenu menu)
        {
            return await _appMenuRepository.AddASync(menu);
        }

        public async Task<AppMenu> Delete(int id)
        {
            return await _appMenuRepository.DeleteAsync(id);
        }

        public async Task<IQueryable<AppMenu>> GetAll()
        {
            return await _appMenuRepository.GetAllAsync();
        }

        public async Task<IQueryable<AppMenu>> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return await _appMenuRepository.GetAllAsync(x => x.MenuName.Contains(keyword));
            return await _appMenuRepository.GetAllAsync();
        }

        public async Task<AppMenu> GetById(int id)
        {
            return await _appMenuRepository.GetByIdAsync(id);
        }

        public async Task< IQueryable<AppMenuMapping>> GetMenuUser(string userId)
        {
            return await _appMenuRepository.GetMenuUser(userId);
        }
        public async Task<IQueryable<AppMenuMappingNew>> GetMenuUserNew(string userId)
        {
            return await _appMenuRepository.GetMenuUserNew(userId);
        }

        public async Task< IQueryable<AppMenuMapping>> GetTreeMenu()
        {
           return await _appMenuRepository.GetTreeMenu();
        }

        public async Task< IQueryable<AppMenuMapping>> GetTreeMenuByUserId(string userId)
        {
            return await _appMenuRepository.GetTreeMenuByUserId(userId);
        }

        public async Task<IQueryable<AppMenuMappingNew>> GetTreeMenuByUserIdNew(string userId)
        {
            return await _appMenuRepository.GetTreeMenuByUserIdNew(userId);
        }

        public async Task<AppMenu> Update(AppMenu menu)
        {
            return await _appMenuRepository.UpdateASync(menu);
        }
    }
}