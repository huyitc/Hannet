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
    public interface ITRegencyService
    {
        Task<TRegency> GetById(int Id);

        Task<IQueryable<TRegency>> GetAll();

        Task<TRegency> Add(TRegency regency);

        Task<TRegency> Update(TRegency regency);

        Task<TRegency> Delete(int Id);

        Task<IQueryable<TRegency>> GetAll(string keyword);
    }
    public class TRegencyService : ITRegencyService
    {
        private readonly ITRegencyRepository _tregencyRepository;

        public TRegencyService(ITRegencyRepository tregencyRepository)
        {
            _tregencyRepository = tregencyRepository;
        }

        public async Task<TRegency> Add(TRegency regency)
        {
            if (await _tregencyRepository.CheckContainsAsync(r => r.RegName == regency.RegName))
                throw new NameDuplicatedException("Tên đã tồn tại!!!");
            return await _tregencyRepository.AddASync(regency);
        }

        public async Task<TRegency> Delete(int Id)
        {
            return await _tregencyRepository.DeleteAsync(Id);
        }

        public async Task<IQueryable<TRegency>> GetAll()
        {
            return await _tregencyRepository.GetAllAsync(x => x.RegStatus == true);
        }

        public async Task<IQueryable<TRegency>> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return await _tregencyRepository.GetAllAsync(x => x.RegName.ToLower().Contains(keyword.ToLower()) || x.RegDescription.ToLower().Contains(keyword.ToLower()));
            return await _tregencyRepository.GetAllAsync();
        }

        public async Task<TRegency> GetById(int Id)
        {
            return await _tregencyRepository.GetByIdAsync(Id);
        }

        public async Task<TRegency> Update(TRegency regency)
        {
            if (await _tregencyRepository.CheckContainsAsync(r => r.RegName == regency.RegName && r.RegId != regency.RegId))
                throw new NameDuplicatedException("Tên đã tồn tại!!!");
            return await _tregencyRepository.UpdateASync(regency);
        }
    }
}
