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
    public interface ITGroupAccessRepository : IRepository<TGroupAccess>
    {
        Task<IQueryable<TGroupAccess>> GetAllbyId(int GAId);
    }
    public class TGroupAccessRepository : RepositoryBase<TGroupAccess>, ITGroupAccessRepository
    {
        private HannetDbContext AIOAcessContolDbContext;

        public TGroupAccessRepository(HannetDbContext aIOAcessContolDbContext) : base(aIOAcessContolDbContext)
        {
            AIOAcessContolDbContext = aIOAcessContolDbContext;
        }

        public async Task<IQueryable<TGroupAccess>> GetAllbyId(int GAId)
        {
            var query = await (from g in AIOAcessContolDbContext.TGroupAccesses
                               join gr in AIOAcessContolDbContext.TGroupAccesses
                               on g.GaId equals gr.GaId
                               where gr.GaId == GAId
                               select g).ToListAsync();
            return query.AsQueryable();
        }
    }
}
