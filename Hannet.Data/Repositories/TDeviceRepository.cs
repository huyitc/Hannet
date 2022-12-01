using Hannet.Data.Infrastructure;
using Hannet.Model.MappingModels;
using Hannet.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Data.Repositories
{
    public interface ITDeviceRepository : IRepository<TDevice>
    {
        Task<IQueryable<TDeviceMapping>> GetListPaging(string keyword);
        Task<IQueryable<TDevice>> GetDeviceMorpho();
        Task<IQueryable<TDevice>> GetDeviceUNV();
        Task<IQueryable<TDevice>> GetDeviceHAN();
    }
    public class TDeviceRepository : RepositoryBase<TDevice>, ITDeviceRepository
    {
        private readonly HannetDbContext _dbContext;
        public TDeviceRepository(HannetDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IQueryable<TDevice>> GetDeviceHAN()
        {
            var query = (from d in _dbContext.TDevices
                         join dt in _dbContext.TDeviceTypes on d.DevTypeId equals dt.DevTypeId
                         where d.DevStatus == true && dt.DevTypeCode == "HAN"
                         select d).ToListAsync();
            return (await query).AsQueryable();
        }

        public async Task<IQueryable<TDevice>> GetDeviceMorpho()
        {
            var query = (from d in _dbContext.TDevices
                         join dt in _dbContext.TDeviceTypes on d.DevTypeId equals dt.DevTypeId
                         where d.DevStatus == true && dt.DevTypeCode == "Morpho"
                         select d).ToListAsync();
            return (await query).AsQueryable();
        }

        public async Task<IQueryable<TDevice>> GetDeviceUNV()
        {
            var query = (from d in _dbContext.TDevices
                         join dt in _dbContext.TDeviceTypes on d.DevTypeId equals dt.DevTypeId
                         where d.DevStatus == true && dt.DevTypeCode == "UNV"
                         select d).ToListAsync();
            return (await query).AsQueryable();
        }

        public async Task<IQueryable<TDeviceMapping>> GetListPaging(string keyword)
        {
            var query = from d in _dbContext.TDevices
                        join dt in _dbContext.TDeviceTypes on d.DevTypeId equals dt.DevTypeId
                        join z in _dbContext.AZones on d.ZonId equals z.ZonId
                        select new TDeviceMapping
                        {
                            DevId = d.DevId,
                            DevTypeId = dt.DevTypeId,
                            ZonId = z.ZonId,
                            DevName = d.DevName,
                            DevIp = d.DevIp,
                            DevPort = d.DevPort,
                            DevCode = d.DevCode,
                            DevSerialnumber = d.DevSerialnumber,
                            DevPartnumber = d.DevPartnumber,
                            DevMacaddress = d.DevMacaddress,
                            DevTimeCheck = d.DevTimeCheck,
                            DevStatus = d.DevStatus,
                            DevLaneCheck = d.DevLaneCheck,
                            ZonName = z.ZonName,
                            DevTypeName = dt.DevTypeName
                        };
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(
                    x => x.DevName.Contains(keyword)
                || x.DevIp.Contains(keyword)
                || x.DevTypeName.Contains(keyword)
                || x.ZonName.Contains(keyword)
                || x.DevSerialnumber.Contains(keyword)
                || x.DevPartnumber.Contains(keyword));
            return (await query.ToListAsync()).AsQueryable();
        }
    }
}
