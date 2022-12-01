using Hannet.Data.Infrastructure;
using Hannet.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Data.Repositories
{
    public interface ITDepartmentRepository : IRepository<TDepartment>
    {

    }
    public class TDepartmentRepository : RepositoryBase<TDepartment>, ITDepartmentRepository
    {
        private readonly HannetDbContext DbContext;

        public TDepartmentRepository(HannetDbContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }
    }
}
