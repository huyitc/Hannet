using KioskManagement.Data.Infrastructure;
using KioskManagement.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskManagement.Data.Repositories
{
    public interface ITDepartmentRepository : IRepository<TDepartment>
    {

    }
    public class TDepartmentRepository : RepositoryBase<TDepartment>, ITDepartmentRepository
    {
        private readonly AIOAcessContolDbContext DbContext;

        public TDepartmentRepository(AIOAcessContolDbContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }
    }
}
