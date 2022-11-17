using KioskManagement.Data.Infrastructure;
using KioskManagement.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskManagement.Data.Repositories
{
    public interface ITEmployeeTypeRepository : IRepository<TEmployeeType>
    {

    }
    public class TEmployeeTypeRepository : RepositoryBase<TEmployeeType>, ITEmployeeTypeRepository
    {
        private AIOAcessContolDbContext _dbContext;
        public TEmployeeTypeRepository(AIOAcessContolDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
