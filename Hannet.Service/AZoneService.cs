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
    public interface IAZoneService
    {
        Task<AZone> Add(AZone aZone);
        Task<AZone> Update(AZone aZone);
        Task<AZone> GetById(int id);
        Task<AZone> Delete(int id);
        Task<IQueryable<AZone>> GetAll();
        Task<IQueryable<AZone>> GetAll(string keyword);
    }
    public class AZoneService : IAZoneService
    {
        private readonly IAZoneRepository _aZoneRepository;
        public AZoneService(IAZoneRepository aZoneRepository)
        {
            _aZoneRepository = aZoneRepository;
        }

        public async Task<AZone> Add(AZone aZone)
        {
            if (await _aZoneRepository.CheckContainsAsync(x => x.ZonName == aZone.ZonName))
            {
                throw new NameDuplicatedException("Tên khu vực đã tồn tại!");
            }
            return await _aZoneRepository.AddASync(aZone);
        }

        public async Task<AZone> Delete(int id)
        {
            return await _aZoneRepository.DeleteAsync(id);
        }

        public async Task<IQueryable<AZone>> GetAll()
        {
            return await _aZoneRepository.GetAllAsync(x => x.ZonStatus == true);
        }

        public async Task<IQueryable<AZone>> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return await _aZoneRepository.GetAllAsync(x => x.ZonName.Contains(keyword) || x.ZonDescription.Contains(keyword));
            return await _aZoneRepository.GetAllAsync();
        }

        public async Task<AZone> GetById(int id)
        {
            return await _aZoneRepository.GetByIdAsync(id);
        }

        public async Task<AZone> Update(AZone aZone)
        {
            if (await _aZoneRepository.CheckContainsAsync(x => x.ZonName == aZone.ZonName && x.ZonId != aZone.ZonId))
            {
                throw new NameDuplicatedException("Tên khu vực đã tồn tại!");
            }
            return await _aZoneRepository.UpdateASync(aZone);
        }
    }
}
