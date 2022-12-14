using Hannet.Data.Infrastructure;
using Hannet.Model.MappingModels;
using Hannet.Model.Models;
using Hannet.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Data.Repositories
{
    public interface ICheckInRepository : IRepository<CheckIn>
    {
        Task<bool> SyncLogCheckIn(IEnumerable<CheckIn> checkInByPlace, GetCheckin bod);
    }
    public class CheckInRepository : RepositoryBase<CheckIn>, ICheckInRepository
    {
        private HannetDbContext _dbContext;
        public CheckInRepository(HannetDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> SyncLogCheckIn(IEnumerable<CheckIn> checkInByPlace, GetCheckin bod)
        {
            _dbContext.CheckIns.RemoveRange(_dbContext.CheckIns.Where(x => x.PlaceID == bod.placeID && x.Date == bod.date));
            _dbContext.SaveChanges();
            var query = _dbContext.AddRangeAsync(checkInByPlace);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
