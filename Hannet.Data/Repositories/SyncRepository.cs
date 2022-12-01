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
    public interface ISyncRepository : IRepository<TEmployee>
    {
        Task<IQueryable<AdditionalSyncMapping>> GetEmployeeEditStatusToAdditionalSync(string keyword);
    }

    public class SyncRepository : RepositoryBase<TEmployee>, ISyncRepository
    {
        private readonly HannetDbContext _dbContext;
        public SyncRepository(HannetDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IQueryable<AdditionalSyncMapping>> GetEmployeeEditStatusToAdditionalSync(string keyword)
        {
            if (keyword == null)
            {
                var query = from em in _dbContext.TEmployees
                            join d in _dbContext.TDepartments on em.DepId equals d.DepId into de
                            from dep in de.DefaultIfEmpty()
                            join r in _dbContext.TRegencies on em.RegId equals r.RegId into re
                            from reg in re.DefaultIfEmpty()
                            select new AdditionalSyncMapping
                            {
                                EmId = em.EmId,
                                EmName = em.EmName,
                                EmCode = em.EmCode,
                                DepName = dep.DepName,
                                RegName = reg.RegName,
                            };
                return (await query.ToListAsync()).AsQueryable();
            }
            else
            {
                var query = from em in _dbContext.TEmployees
                            join d in _dbContext.TDepartments on em.DepId equals d.DepId into de
                            from dep in de.DefaultIfEmpty()
                            join r in _dbContext.TRegencies on em.RegId equals r.RegId into re
                            from reg in re.DefaultIfEmpty()
                            where (em.EmName.Contains(keyword) || dep.DepName.Contains(keyword) || reg.RegName.Contains(keyword))
                            select new AdditionalSyncMapping
                            {
                                EmId = em.EmId,
                                EmName = em.EmName,
                                EmCode = em.EmCode,
                                DepName = dep.DepName,
                                RegName = reg.RegName,
                            };
                return (await query.ToListAsync()).AsQueryable();
            }
        }
    }
}
