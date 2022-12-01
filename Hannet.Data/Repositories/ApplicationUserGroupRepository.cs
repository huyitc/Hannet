using Hannet.Data.Infrastructure;
using Hannet.Model.Models;

namespace Hannet.Data.Repositories
{
    public interface IApplicationUserGroupRepository : IRepository<AppUserGroup>
    {
    }

    public class ApplicationUserGroupRepository : RepositoryBase<AppUserGroup>, IApplicationUserGroupRepository
    {
        public ApplicationUserGroupRepository(HannetDbContext dbFactory) : base(dbFactory)
        {
        }
    }
}