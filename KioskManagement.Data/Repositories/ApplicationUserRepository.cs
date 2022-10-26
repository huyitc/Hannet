using Microsoft.EntityFrameworkCore;
using KioskManagement.Data.Infrastructure;
using KioskManagement.Model.MappingModels;
using KioskManagement.Model.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KioskManagement.Data.Repositories
{
    public interface IApplicationUserRepository : IRepository<AppUser>
    {
        //AppUser_Mapping GetUserName(string userName);

        //IEnumerable<AppUser_Employee_Account_Mapping> GetAllByMapping(string keyword);

         Task<IQueryable<AppUserMapping>> GetAllByMapping(string keyword);
    }

    public class ApplicationUserRepository : RepositoryBase<AppUser>, IApplicationUserRepository
    {
        private AIOAcessContolDbContext _dbContext;

        public ApplicationUserRepository(AIOAcessContolDbContext dbFactory) : base(dbFactory)
        {
            _dbContext = dbFactory;
        }

        public async Task< IQueryable<AppUserMapping>> GetAllByMapping(string keyword)
        {
            var query = (await (from u in _dbContext.AppUsers
                        join ug in _dbContext.AppUserGroups on u.Id equals ug.UserId into userGroup
                        from ug in userGroup.DefaultIfEmpty()
                        join g in _dbContext.AppGroups on ug.GroupId equals g.Id into appGroup
                        from g in appGroup.DefaultIfEmpty()
                        select new AppUserMapping
                        {
                            CreatedBy = u.CreatedBy,
                            CreatedDate = u.CreatedDate,
                            DeletedBy = u.DeletedBy,
                            DeletedDate = u.DeletedDate,
                            Email = u.Email,
                            FullName = u.FullName,
                            Id = u.Id,
                            Image = u.Image,
                            IsDeleted = u.IsDeleted,
                            PhoneNumber = u.PhoneNumber,
                            UpdatedDate = u.UpdatedDate,
                            UpdatedBy = u.UpdatedBy,
                            UserName = u.UserName,
                            Groups = (from g1 in _dbContext.AppGroups join ug1 in _dbContext.AppUserGroups on g1.Id equals ug1.GroupId where ug1.UserId==u.Id select g1).ToList(),
                            EmId = u.EM_ID
                        }).ToListAsync()).AsQueryable();
            if(!string.IsNullOrEmpty(keyword))
                query=query.Where(x=>x.UserName.ToLower().Contains(keyword.ToLower())||x.FullName.ToLower().Contains(keyword.ToLower()));
            return query;
            
        }

        //public IEnumerable<AppUser_Employee_Account_Mapping> GetAllByMapping(string keyword)
        //{
        //    var query = from u in DbContext.Users
        //                join e in DbContext.EMPLOYEES on u.EM_ID equals e.EM_ID
        //                select new AppUser_Employee_Account_Mapping
        //                {
        //                    Email = u.Email,
        //                    Image = e.IMAGE,
        //                    EmName = e.EM_NAME,
        //                    UserName = u.UserName,
        //                    Id = u.Id,
        //                    EmId = e.EM_ID
        //                };
        //    if (!string.IsNullOrEmpty(keyword))
        //        query = query.Where(x => x.EmName.Contains(keyword) || x.UserName.Contains(keyword));
        //    return query;
        //}

        //public AppUser_Mapping GetUserName(string userName)
        //{
        //    var query = from a in DbContext.Users
        //                where a.UserName == userName
        //                select new AppUser_Mapping
        //                {
        //                    UserName = a.UserName,
        //                    Id = a.Id
        //                };
        //    return query.FirstOrDefault();
        //}
    }
}