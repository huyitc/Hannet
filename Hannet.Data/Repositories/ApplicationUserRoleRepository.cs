using Microsoft.EntityFrameworkCore;
using Hannet.Data.Infrastructure;
using Hannet.Model.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hannet.Data.Repositories
{
    public interface IApplicationUserRoleRepository : IRepository<AppUserRole>
    {
        //List<App_User_Role_Mapping> GetByUserId(string userId);
        Task<List<string>> GetListRoleCheck(string userName);

        Task<IQueryable<string>> GetListRole(string userId);
    }

    public class ApplicationUserRoleRepository : RepositoryBase<AppUserRole>, IApplicationUserRoleRepository
    {
        private HannetDbContext DbContext;

        public ApplicationUserRoleRepository(HannetDbContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<List<string>> GetListRoleCheck(string userName)
        {
            var query = await (from r in DbContext.Roles
                               join rg in DbContext.AppRoleGroups on r.Id equals rg.RoleId
                               join ug in DbContext.AppUserGroups on rg.GroupId equals ug.GroupId
                               join u in DbContext.Users on ug.UserId equals u.Id
                               where u.UserName == userName
                               select r.Name).ToListAsync();
            return query;
        }

        public async Task<IQueryable<string>> GetListRole(string userId)
        {
            //return (await (from r in DbContext.UserRoles
            //               join role in DbContext.Roles on r.RoleId equals role.Id
            //               where r.UserId == userId
            //               select role.Name).ToListAsync()).AsQueryable();

            return (await (from ar in DbContext.AppRoles
                                       join arg in DbContext.AppRoleGroups on ar.Id equals arg.RoleId into approleGroup
                                       from arg in approleGroup.DefaultIfEmpty()

                                       join ag in DbContext.AppGroups on arg.GroupId equals ag.Id into appGroup
                                       from ag in appGroup.DefaultIfEmpty()

                                       join aug in DbContext.AppUserGroups on ag.Id equals aug.GroupId into appUserGroup
                                       from aug in appUserGroup.DefaultIfEmpty()

                                       join au in DbContext.AppUsers on aug.UserId equals au.Id into appUser
                                       from au in appUser.DefaultIfEmpty()
                                       where au.Id == userId 
                                       select ar.Name).ToListAsync()).AsQueryable();
        }
    }
}