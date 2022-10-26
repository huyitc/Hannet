using KioskManagement.Data.Infrastructure;
using KioskManagement.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskManagement.Data.Repositories
{
    public interface IAScheduleDeviceDetailRepository : IRepository<AScheduleDeviceDetail>
    {

    }
    public class AScheduleDeviceDetailRepository : RepositoryBase<AScheduleDeviceDetail>, IAScheduleDeviceDetailRepository
    {
        private readonly AIOAcessContolDbContext _dbContext;
        public AScheduleDeviceDetailRepository(AIOAcessContolDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
