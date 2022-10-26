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
    public interface IAScheduleDeviceDetailService
    {
        Task<AScheduleDeviceDetail> Add(AScheduleDeviceDetail aSheduleDeviceDetail);
        Task<AScheduleDeviceDetail> Update(AScheduleDeviceDetail aSheduleDeviceDetail);
        Task<AScheduleDeviceDetail> Delete(int id);
        Task<IQueryable<AScheduleDeviceDetail>> GetAll();
        Task<IQueryable<AScheduleDeviceDetail>> GetAll(string keyword);
        Task<AScheduleDeviceDetail> GetById(int id);

    }
    public class AScheduleDeviceDetailService : IAScheduleDeviceDetailService
    {
        private readonly IAScheduleDeviceDetailRepository _iAScheduleDeviceDetailRepository;
        public AScheduleDeviceDetailService(IAScheduleDeviceDetailRepository iAScheduleDeviceDetailRepository)
        {
            _iAScheduleDeviceDetailRepository = iAScheduleDeviceDetailRepository;
        }

        public async Task<AScheduleDeviceDetail> Add(AScheduleDeviceDetail aSheduleDeviceDetail)
        {
            if (await _iAScheduleDeviceDetailRepository.CheckContainsAsync(x => x.SchDevId == aSheduleDeviceDetail.SchDevId))
            {
                throw new NameDuplicatedException("Mã truy cập đã tồn tại!");
            }
            if (await _iAScheduleDeviceDetailRepository.CheckContainsAsync(x => x.SchDevName == aSheduleDeviceDetail.SchDevName))
            {
                throw new NameDuplicatedException("Tên truy cập đã tồn tại!");
            }
            if (await _iAScheduleDeviceDetailRepository.CheckContainsAsync(x => x.Mon == aSheduleDeviceDetail.Mon && x.Tue == aSheduleDeviceDetail.Tue &&
            x.Wed == aSheduleDeviceDetail.Wed && x.Thu == aSheduleDeviceDetail.Thu && x.Fri == aSheduleDeviceDetail.Fri
            && x.Sat == aSheduleDeviceDetail.Sat && x.Sun == aSheduleDeviceDetail.Sun))
            {
                throw new NameDuplicatedException("Lịch truy cập đã tồn tại!");
            }
            return await _iAScheduleDeviceDetailRepository.AddASync(aSheduleDeviceDetail);
        }

        public async Task<AScheduleDeviceDetail> Delete(int id)
        {
            return await _iAScheduleDeviceDetailRepository.DeleteAsync(id);
        }

        public async Task<IQueryable<AScheduleDeviceDetail>> GetAll()
        {
            return await _iAScheduleDeviceDetailRepository.GetAllAsync();
        }

        public async Task<IQueryable<AScheduleDeviceDetail>> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return await _iAScheduleDeviceDetailRepository.GetAllAsync(x => x.SchDevName.Contains(keyword));
            }
            return await _iAScheduleDeviceDetailRepository.GetAllAsync();
        }

        public async Task<AScheduleDeviceDetail> GetById(int id)
        {
            return await _iAScheduleDeviceDetailRepository.GetByIdAsync(id);
        }

        public async Task<AScheduleDeviceDetail> Update(AScheduleDeviceDetail aSheduleDeviceDetail)
        {
            if (await _iAScheduleDeviceDetailRepository.CheckContainsAsync(x => x.SchDevName == aSheduleDeviceDetail.SchDevName && x.SchDevId != aSheduleDeviceDetail.SchDevId))
            {
                throw new NameDuplicatedException("Tên truy cập đã tồn tại!");
            }
            if (await _iAScheduleDeviceDetailRepository.CheckContainsAsync(x => x.Mon == aSheduleDeviceDetail.Mon && x.Tue == aSheduleDeviceDetail.Tue &&
            x.Wed == aSheduleDeviceDetail.Wed && x.Thu == aSheduleDeviceDetail.Thu && x.Fri == aSheduleDeviceDetail.Fri
            && x.Sat == aSheduleDeviceDetail.Sat && x.Sun == aSheduleDeviceDetail.Sun && x.SchDevId != aSheduleDeviceDetail.SchDevId))
            {
                throw new NameDuplicatedException("Lịch truy cập đã tồn tại!");
            }
            return await _iAScheduleDeviceDetailRepository.UpdateASync(aSheduleDeviceDetail);
        }
    }
}
