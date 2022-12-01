using Hannet.Data.Infrastructure;
using Hannet.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Data.Repositories
{
    public interface IAppMenuUserRepository: IRepository<AppMenuUser>
    {

    }
    internal class AppMenuUserRepository : RepositoryBase<AppMenuUser>, IAppMenuUserRepository
    {
        public AppMenuUserRepository(HannetDbContext dbContext) : base(dbContext)
        {
        }
    }
}
