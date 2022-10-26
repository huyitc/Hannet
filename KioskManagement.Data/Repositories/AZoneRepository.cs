using KioskManagement.Data.Infrastructure;
using KioskManagement.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskManagement.Data.Repositories
{
    public interface IAZoneRepository : IRepository<AZone>
    {

    }
    public class AZoneRepository : RepositoryBase<AZone>, IAZoneRepository
    {

        private AIOAcessContolDbContext _dbContext;
        public AZoneRepository(AIOAcessContolDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
