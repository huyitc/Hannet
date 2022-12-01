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
    public interface ITGroupAccessDetailRepository : IRepository<TGroupAccessDetail>
    {
        Task<IQueryable<AZoneDeviceMapping>> GetTreeAZoneTDevice();
        Task<IQueryable<TEmployee>> GetTEmployeeByGaIdAndHaveStautus(int gaId);
        Task<IQueryable<int>> GetDevIdsByGaIdAndHaveStatus(int gaId);
    }

    public class TGroupAccessDetailRepository : RepositoryBase<TGroupAccessDetail>, ITGroupAccessDetailRepository
    {
        private readonly HannetDbContext _dbContext;
        public TGroupAccessDetailRepository(HannetDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IQueryable<int>> GetDevIdsByGaIdAndHaveStatus(int gaId)
        {
            var query = await (from gad in _dbContext.TGroupAccessDetails
                               join dev in _dbContext.TDevices on gad.DevId equals dev.DevId
                               where gad.GaId == gaId && gad.GadStatus == true && dev.DevStatus == true
                               select dev.DevId).ToListAsync();
            return query.AsQueryable();
        }

        public async Task<IQueryable<TEmployee>> GetTEmployeeByGaIdAndHaveStautus(int gaId)
        {
            var query = await (from em in _dbContext.TEmployees
                               where em.EmStatus == true
                               select em).ToListAsync();
            return query.AsQueryable();
        }

        public async Task<IQueryable<AZoneDeviceMapping>> GetTreeAZoneTDevice()
        {
            var query = await (from z in _dbContext.AZones
                               select new AZoneDeviceMapping
                               {
                                   Id = z.ZonId,
                                   Name = z.ZonName,
                                   Childrens = (from d in _dbContext.TDevices
                                                where d.ZonId == z.ZonId && d.DevStatus == true
                                                select new AZoneDeviceMapping
                                                {
                                                    Id = d.DevId,
                                                    Name = "( " + d.DevName + " - [ " + d.DevIp + " ] )"
                                                }).ToList()
                               }).ToListAsync();
            return query.AsQueryable();
        }
    }
}
