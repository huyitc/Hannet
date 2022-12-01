using Hannet.Common.Exceptions;
using Hannet.Data.Repositories;
using Hannet.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Service
{
    public interface ITGroupAccessService
    {
        Task<TGroupAccess> GetById(int GAId);

        Task<IQueryable<TGroupAccess>> GetAll();

        Task<TGroupAccess> Add(TGroupAccess groupAccess);

        Task<TGroupAccess> Update(TGroupAccess groupAccess);

        Task<TGroupAccess> Delete(int GAId);

        Task<IQueryable<TGroupAccess>> GetAll(string keyword);
    }
    public class TGroupAccessService : ITGroupAccessService
    {
        private ITGroupAccessRepository _tGroupAccessRepository;

        public TGroupAccessService(ITGroupAccessRepository tGroupAccessRepository)
        {
            _tGroupAccessRepository = tGroupAccessRepository;
        }

        public async Task<TGroupAccess> Add(TGroupAccess groupAccess)
        {
            if (await _tGroupAccessRepository.CheckContainsAsync(r => r.GaName == groupAccess.GaName))
                throw new NameDuplicatedException("Tên không đc trùng!!!");
            return await _tGroupAccessRepository.AddASync(groupAccess);
        }

        public async Task<TGroupAccess> Delete(int GAId)
        {
            return await _tGroupAccessRepository.DeleteAsync(GAId);
        }

        public async Task<IQueryable<TGroupAccess>> GetAll()
        {
            return await _tGroupAccessRepository.GetAllAsync(x => x.GaStatus == true);
        }

        public async Task<IQueryable<TGroupAccess>> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return await _tGroupAccessRepository.GetAllAsync(x => x.GaName.Contains(keyword));
            return await _tGroupAccessRepository.GetAllAsync();
        }

        public async Task<TGroupAccess> GetById(int GAId)
        {
            return await _tGroupAccessRepository.GetByIdAsync(GAId);
        }

        public async Task<TGroupAccess> Update(TGroupAccess groupAccess)
        {
            if (await _tGroupAccessRepository.CheckContainsAsync(r => r.GaName == groupAccess.GaName && r.GaId != groupAccess.GaId))
                throw new NameDuplicatedException("Tên không đc trùng!!!");
            return await _tGroupAccessRepository.UpdateASync(groupAccess);
        }
    }
}
