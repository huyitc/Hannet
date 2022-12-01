using AutoMapper;
using Hannet.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hannet.WebApi.Controllers
{
    [Route("hannet/api/[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ILogger<SyncController> _logger;
        private readonly ISyncService _syncService;
        private readonly ITEmployeeService _tEmployeeService;
        public SyncController(IMapper mapper, ILogger<SyncController> logger, ISyncService syncService, ITEmployeeService tEmployeeService)
        {
            _mapper = mapper;
            _logger = logger;
            _syncService = syncService;
            _tEmployeeService = tEmployeeService;
        }

        /// <summary>
        /// Xem danh sách employee editstatus = 1
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetEmployeeEditStatus")]
        [AllowAnonymous]
        public async Task<IActionResult> GetEmployeeEditStatus(string? keyword = null)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/aioaccesscontrol/Sync/GetEmployeeEditStatus", "GET");
                var responseData = await _syncService.GetEmployeeEditStatusToAdditionalSync(keyword);
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


    }
}
