using Microsoft.EntityFrameworkCore;
using Hannet.Data.Infrastructure;
using Hannet.Model.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hannet.Data.Repositories
{
    public interface IApplicationGroupRepository : IRepository<AppGroup>
    {
        Task<IQueryable<AppGroup>> GetListGroupByUserId(string userId);

        Task<IQueryable<AppUser>> GetListUserByGroupId(int groupId);
    }

    public class ApplicationGroupRepository : RepositoryBase<AppGroup>, IApplicationGroupRepository
    {
        private readonly HannetDbContext DbContext;

        public ApplicationGroupRepository(HannetDbContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<IQueryable<AppGroup>> GetListGroupByUserId(string userId)
        {
            var query = await (from g in DbContext.AppGroups
                               join ug in DbContext.AppUserGroups
                               on g.Id equals ug.GroupId
                               where ug.UserId == userId
                               select g).ToListAsync();
            return query.AsQueryable();
        }

        public async Task<IQueryable<AppUser>> GetListUserByGroupId(int groupId)
        {
            var query = await (from g in DbContext.AppGroups
                               join ug in DbContext.AppUserGroups
                               on g.Id equals ug.GroupId
                               join u in DbContext.Users
                               on ug.UserId equals u.Id
                               where ug.GroupId == groupId
                               select u).ToListAsync();
            return (IQueryable<AppUser>)query;
        }
    }
}