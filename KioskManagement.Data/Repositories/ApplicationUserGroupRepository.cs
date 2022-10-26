using KioskManagement.Data.Infrastructure;
using KioskManagement.Model.Models;

namespace KioskManagement.Data.Repositories
{
    public interface IApplicationUserGroupRepository : IRepository<AppUserGroup>
    {
    }

    public class ApplicationUserGroupRepository : RepositoryBase<AppUserGroup>, IApplicationUserGroupRepository
    {
        public ApplicationUserGroupRepository(AIOAcessContolDbContext dbFactory) : base(dbFactory)
        {
        }
    }
}