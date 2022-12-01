
using Hannet.Data;
using Hannet.Data.Repositories;
using Hannet.Model.MappingModels;
using Hannet.Model.Models;
using Hannet.Model.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hannet.Service
{
    public interface IApplicationUserService
    {
        //AppUser_Mapping GetUserName(string userName);

        //IEnumerable<AppUser_Employee_Account_Mapping> GetAllByMapping(string keyword);
        Task<IQueryable<AppUser>> GetAll();
        Task<IQueryable<AppUser>> GetAll(string keyword);
        Task<IQueryable<AppUserMapping>> GetAllByMappingAsync(string keyword);
    }

    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private HannetDbContext _dbContext;

        public ApplicationUserService(IApplicationUserRepository applicationUserRepository, HannetDbContext dbContext)
        {
            _applicationUserRepository = applicationUserRepository;
            _dbContext = dbContext;
        }

        public async Task<IQueryable<AppUser>> GetAll()
        {
            return await _applicationUserRepository.GetAllAsync();
        }

        public async Task<IQueryable<AppUser>> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return await _applicationUserRepository.GetAllAsync(x => x.UserName.ToUpper().Contains(keyword));
            }
            else
                return await _applicationUserRepository.GetAllAsync();
        }

        public async Task<IQueryable<AppUserMapping>> GetAllByMappingAsync(string keyword)
        {
            return await  _applicationUserRepository.GetAllByMapping(keyword);
        }
                

        //public IEnumerable<AppUser_Employee_Account_Mapping> GetAllByMapping(string keyword)
        //{
        //    return _applicationUserRepository.GetAllByMapping(keyword);
        //}

        //public AppUser_Mapping GetUserName(string userName)
        //{
        //    return _applicationUserRepository.GetUserName(userName);
        //}
    }
}