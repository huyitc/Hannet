using Hannet.Data.Infrastructure;
using Hannet.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Data.Repositories
{
    public interface IAZoneRepository : IRepository<AZone>
    {

    }
    public class AZoneRepository : RepositoryBase<AZone>, IAZoneRepository
    {

        private HannetDbContext _dbContext;
        public AZoneRepository(HannetDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
