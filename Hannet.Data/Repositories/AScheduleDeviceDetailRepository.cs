using Hannet.Data.Infrastructure;
using Hannet.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Data.Repositories
{
    public interface IAScheduleDeviceDetailRepository : IRepository<AScheduleDeviceDetail>
    {

    }
    public class AScheduleDeviceDetailRepository : RepositoryBase<AScheduleDeviceDetail>, IAScheduleDeviceDetailRepository
    {
        private readonly HannetDbContext _dbContext;
        public AScheduleDeviceDetailRepository(HannetDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
