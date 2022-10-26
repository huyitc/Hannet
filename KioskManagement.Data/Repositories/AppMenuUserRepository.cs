using KioskManagement.Data.Infrastructure;
using KioskManagement.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskManagement.Data.Repositories
{
    public interface IAppMenuUserRepository: IRepository<AppMenuUser>
    {

    }
    internal class AppMenuUserRepository : RepositoryBase<AppMenuUser>, IAppMenuUserRepository
    {
        public AppMenuUserRepository(AIOAcessContolDbContext dbContext) : base(dbContext)
        {
        }
    }
}
