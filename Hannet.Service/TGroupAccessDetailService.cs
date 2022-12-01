using Hannet.Data.Repositories;
using Hannet.Model.MappingModels;
using Hannet.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Service
{
    public interface ITGroupAccessDetailService
    {
        Task<TGroupAccessDetail> Add(TGroupAccessDetail tGroupAccessDetail);
        Task<TGroupAccessDetail> Update(TGroupAccessDetail tGroupAccessDetail);
        Task<IQueryable<int>> GetDevIdsByGaIdAndHaveStatus(int gaId);
        Task<IQueryable<AZoneDeviceMapping>> GetTreeAZoneTDevice();
        Task<IQueryable<TEmployee>> GetTEmployeeByGaIdAndHaveStatus(int gaId);
        Task<TGroupAccessDetail> GetByGaIdAndDevId(int gaId, int devId);
    }
    public class TGroupAccessDetailService : ITGroupAccessDetailService
    {
        private readonly ITGroupAccessDetailRepository _tGroupAccessDetailRepository;
        public TGroupAccessDetailService(ITGroupAccessDetailRepository tGroupAccessDetailRepository)
        {
            _tGroupAccessDetailRepository = tGroupAccessDetailRepository;
        }

        public async Task<TGroupAccessDetail> Add(TGroupAccessDetail tGroupAccessDetail)
        {
            return await _tGroupAccessDetailRepository.AddASync(tGroupAccessDetail);
        }

        public async Task<TGroupAccessDetail> GetByGaIdAndDevId(int gaId, int devId)
        {
            return await _tGroupAccessDetailRepository.GetSingleByConditionAsync(x => x.GaId == gaId && x.DevId == devId);
        }

        public async Task<IQueryable<int>> GetDevIdsByGaIdAndHaveStatus(int gaId)
        {
            return await _tGroupAccessDetailRepository.GetDevIdsByGaIdAndHaveStatus(gaId);
        }

        public async Task<IQueryable<TEmployee>> GetTEmployeeByGaIdAndHaveStatus(int gaId)
        {
            return await _tGroupAccessDetailRepository.GetTEmployeeByGaIdAndHaveStautus(gaId);
        }

        public async Task<IQueryable<AZoneDeviceMapping>> GetTreeAZoneTDevice()
        {
            return await _tGroupAccessDetailRepository.GetTreeAZoneTDevice();
        }

        public async Task<TGroupAccessDetail> Update(TGroupAccessDetail tGroupAccessDetail)
        {
            return await _tGroupAccessDetailRepository.UpdateASync(tGroupAccessDetail);
        }
    }
}
