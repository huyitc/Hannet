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
    public interface ICheckInService
    {
        Task<CheckIn> Add(CheckIn checkIn);
    }
    public class CheckInService : ICheckInService
    {
        private readonly ICheckInRepository _checkInRepository;
        public CheckInService(ICheckInRepository checkInRepository)
        {
            _checkInRepository = checkInRepository;
        }

        public async Task<CheckIn> Add(CheckIn checkIn)
        {
            return await _checkInRepository.AddASync(checkIn);
        }
    }
}
