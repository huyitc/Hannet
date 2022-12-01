using Hannet.Common.Exceptions;
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
    public interface ITDeviceService
    {
        Task<TDevice> Add(TDevice tDevice);
        Task<TDevice> Update(TDevice tDevice);
        Task<TDevice> GetById(int id);
        Task<TDevice> Delete(int id);
        Task<IQueryable<TDevice>> GetAll();
        Task<IQueryable<TDevice>> GetAll(string keyword);
        Task<IQueryable<TDeviceMapping>> GetListPaging(string keyword);
        Task<IQueryable<TDevice>> GetDeviceMorpho();
        Task<IQueryable<TDevice>> GetDeviceUNV();
        Task<IQueryable<TDevice>> GetDeviceHAN();
    }
    public class TDeviceService : ITDeviceService
    {
        private readonly ITDeviceRepository _iTDeviceRepository;
        public TDeviceService(ITDeviceRepository iTDeviceRepository)
        {
            _iTDeviceRepository = iTDeviceRepository;
        }

        public async Task<TDevice> Add(TDevice tDevice)
        {
            if (await _iTDeviceRepository.CheckContainsAsync(x => x.DevIp == tDevice.DevIp))
            {
                throw new NameDuplicatedException("Ip đã tồn tại!");
            }
            return await _iTDeviceRepository.AddASync(tDevice);
        }

        public async Task<TDevice> Delete(int id)
        {
            return await _iTDeviceRepository.DeleteAsync(id);
        }

        public async Task<IQueryable<TDevice>> GetAll()
        {
            return await _iTDeviceRepository.GetAllAsync(x => x.DevStatus == true);
        }

        public async Task<IQueryable<TDevice>> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return await _iTDeviceRepository.GetAllAsync(x => x.DevName.Contains(keyword) || x.DevIp.Contains(keyword));
            }
            return await _iTDeviceRepository.GetAllAsync();
        }

        public async Task<TDevice> GetById(int id)
        {
            return await _iTDeviceRepository.GetByIdAsync(id);
        }

        public async Task<IQueryable<TDevice>> GetDeviceHAN()
        {
            return await _iTDeviceRepository.GetDeviceHAN();
        }

        public async Task<IQueryable<TDevice>> GetDeviceMorpho()
        {
            return await _iTDeviceRepository.GetDeviceMorpho();
        }

        public async Task<IQueryable<TDevice>> GetDeviceUNV()
        {
            return await _iTDeviceRepository.GetDeviceUNV();
        }

        public async Task<IQueryable<TDeviceMapping>> GetListPaging(string keyword)
        {
            return await _iTDeviceRepository.GetListPaging(keyword);
        }

        public async Task<TDevice> Update(TDevice tDevice)
        {
            if (await _iTDeviceRepository.CheckContainsAsync(x => x.DevIp == tDevice.DevIp && x.DevId != tDevice.DevId))
            {
                throw new NameDuplicatedException("Ip đã tồn tại!");
            }
            return await _iTDeviceRepository.UpdateASync(tDevice);
        }
    }
}
