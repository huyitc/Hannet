using Hannet.Data.Infrastructure;
using Hannet.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Data.Repositories
{
    public interface ITDeviceTypeRepository : IRepository<TDeviceType>
    {
        Task<IQueryable<TDeviceType>> GetAllbyId(int Id);
    }
    public class TDeviceTypeRepository : RepositoryBase<TDeviceType>, ITDeviceTypeRepository
    {
        private HannetDbContext AIOAcessContolDbContext;

        public TDeviceTypeRepository(HannetDbContext aIOAcessContolDbContext) : base(aIOAcessContolDbContext)
        {
            AIOAcessContolDbContext = aIOAcessContolDbContext;
        }

        public async Task<IQueryable<TDeviceType>> GetAllbyId(int Id)
        {
            var query = await (from DT in AIOAcessContolDbContext.TDeviceTypes
                               join TD in AIOAcessContolDbContext.TDeviceTypes
                               on DT.DevTypeId equals TD.DevTypeId
                               where TD.DevTypeId == Id
                               select DT).ToListAsync();
            return query.AsQueryable();
        }
    }
}
