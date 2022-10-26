using KioskManagement.Data.Repositories;
using KioskManagement.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskManagement.Service
{
    public interface IAppMenuUserService
    {
        Task<AppMenuUser> Add(AppMenuUser user);
        Task Delete(string userId);
    }
    internal class AppMenuUserService: IAppMenuUserService
    {
        private readonly IAppMenuUserRepository _appMenuRepository;
        public AppMenuUserService(IAppMenuUserRepository appMenuRepository)
        {
            _appMenuRepository = appMenuRepository;
        }

        public async Task<AppMenuUser> Add(AppMenuUser user)
        {
            return await _appMenuRepository.AddASync(user);
        }

        public async Task Delete(string userId)
        {
            await _appMenuRepository.DeleteMulti(x=>x.UserId == userId);
            
        }
    }
}
