using Hannet.Data.Infrastructure;
using Hannet.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Data.Repositories
{
    public interface ITEmployeeTypeRepository : IRepository<TEmployeeType>
    {

    }
    public class TEmployeeTypeRepository : RepositoryBase<TEmployeeType>, ITEmployeeTypeRepository
    {
        private HannetDbContext _dbContext;
        public TEmployeeTypeRepository(HannetDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
