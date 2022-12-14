using Hannet.Data.Infrastructure;
using Hannet.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Data.Repositories
{
    public interface ICheckInRepository : IRepository<CheckIn>
    {

    }
    public class CheckInRepository : RepositoryBase<CheckIn>, ICheckInRepository
    {
        private HannetDbContext _dbContext;
        public CheckInRepository(HannetDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
