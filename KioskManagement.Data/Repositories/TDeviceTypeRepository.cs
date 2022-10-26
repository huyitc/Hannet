using KioskManagement.Data.Infrastructure;
using KioskManagement.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskManagement.Data.Repositories
{
    public interface ITDeviceTypeRepository : IRepository<TDeviceType>
    {
        Task<IQueryable<TDeviceType>> GetAllbyId(int Id);
    }
    public class TDeviceTypeRepository : RepositoryBase<TDeviceType>, ITDeviceTypeRepository
    {
        private AIOAcessContolDbContext AIOAcessContolDbContext;

        public TDeviceTypeRepository(AIOAcessContolDbContext aIOAcessContolDbContext) : base(aIOAcessContolDbContext)
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
