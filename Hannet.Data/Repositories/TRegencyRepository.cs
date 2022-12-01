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
    public interface ITRegencyRepository : IRepository<TRegency>
    {
        Task<IQueryable<TRegency>> GetAllbyId(int Id);
    }
    public class TRegencyRepository : RepositoryBase<TRegency>, ITRegencyRepository
    {
        private HannetDbContext AIOAcessContolDbContext;

        public TRegencyRepository(HannetDbContext aIOAcessContolDbContext) : base(aIOAcessContolDbContext)
        {
            AIOAcessContolDbContext = aIOAcessContolDbContext;
        }

        public async Task<IQueryable<TRegency>> GetAllbyId(int Id)
        {
            var query = await (from tr in AIOAcessContolDbContext.TRegencies
                               join trr in AIOAcessContolDbContext.TRegencies
                               on tr.RegId equals trr.RegId
                               where trr.RegId == Id
                               select trr).ToListAsync();
            return query.AsQueryable();
        }
    }
}
