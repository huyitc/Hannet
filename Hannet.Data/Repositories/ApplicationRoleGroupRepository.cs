using Microsoft.EntityFrameworkCore;
using Hannet.Data.Infrastructure;
using Hannet.Model.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hannet.Data.Repositories
{
    public interface IApplicationRoleGroupRepository : IRepository<AppRoleGroup>
    {
        Task<IQueryable<AppRoleGroup>> GetListByGroupId(int groupId);
    }

    public class ApplicationRoleGroupRepository : RepositoryBase<AppRoleGroup>, IApplicationRoleGroupRepository
    {
        private HannetDbContext DbContext;

        public ApplicationRoleGroupRepository(HannetDbContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<IQueryable<AppRoleGroup>> GetListByGroupId(int groupId)
        {
            var query = await (from ag in DbContext.AppGroups
                               join agr in DbContext.AppRoleGroups on ag.Id equals agr.GroupId
                               join ar in DbContext.Roles on agr.RoleId equals ar.Id
                               where agr.GroupId == groupId
                               select new AppRoleGroup
                               {
                                   GroupId = ag.Id,
                                   RoleId = ar.Id
                               }).ToListAsync();
            return query.AsQueryable();
        }
    }
}