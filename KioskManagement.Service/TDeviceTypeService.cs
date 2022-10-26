using KioskManagement.Common.Exceptions;
using KioskManagement.Data.Repositories;
using KioskManagement.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskManagement.Service
{
    public interface ITDeviceTypeService
    {
        Task<IEnumerable<TDeviceType>> GetAll();

        Task<TDeviceType> GetDetail(int Id);

        Task<TDeviceType> Add(TDeviceType deviceType);

        Task<TDeviceType> Update(TDeviceType deviceType);

        Task<TDeviceType> Delete(int Id);

        Task<IQueryable<TDeviceType>> GetAll(string keyword);
    }
    public class TDeviceTypeService : ITDeviceTypeService
    {
        private readonly ITDeviceTypeRepository _iTDeviceTypeRepository;

        public TDeviceTypeService(ITDeviceTypeRepository iTDeviceTypeRepository)
        {
            _iTDeviceTypeRepository = iTDeviceTypeRepository;
        }

        public async Task<TDeviceType> Add(TDeviceType deviceType)
        {
            if (await _iTDeviceTypeRepository.CheckContainsAsync(r => r.DevTypeName == deviceType.DevTypeName))
                throw new NameDuplicatedException("Tên Loại thiết bị đã tồn tại!");
            if (await _iTDeviceTypeRepository.CheckContainsAsync(r => r.DevTypeCode == deviceType.DevTypeCode))
                throw new NameDuplicatedException("Mã loại thiết bị đã tồn tại!");
            return await _iTDeviceTypeRepository.AddASync(deviceType);
        }

        public async Task<TDeviceType> Delete(int Id)
        {
            return await _iTDeviceTypeRepository.DeleteAsync(Id);
        }

        public async Task<IEnumerable<TDeviceType>> GetAll()
        {
            return await _iTDeviceTypeRepository.GetAllAsync();
        }

        public async Task<IQueryable<TDeviceType>> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return await _iTDeviceTypeRepository.GetAllAsync(x => x.DevTypeName.ToLower().Contains(keyword.ToLower()) || x.DevTypeCode.ToLower().Contains(keyword.ToLower()));
            return await _iTDeviceTypeRepository.GetAllAsync();
        }

        public async Task<TDeviceType> GetDetail(int Id)
        {
            return await _iTDeviceTypeRepository.GetByIdAsync(Id);
        }

        public async Task<TDeviceType> Update(TDeviceType deviceType)
        {
            if (await _iTDeviceTypeRepository.CheckContainsAsync(r => r.DevTypeName == deviceType.DevTypeName && r.DevTypeId != deviceType.DevTypeId))
                throw new NameDuplicatedException("Mã loại thiết bị đã tồn tại!");
            if (await _iTDeviceTypeRepository.CheckContainsAsync(r => r.DevTypeCode == deviceType.DevTypeCode && r.DevTypeId != deviceType.DevTypeId))
                throw new NameDuplicatedException("Mã loại thiết bị đã tồn tại!");
            return await _iTDeviceTypeRepository.UpdateASync(deviceType);
        }
    }
}
