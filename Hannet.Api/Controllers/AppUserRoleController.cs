using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Hannet.Service;

namespace Hannet.WebApi.Controllers
{
    [Route("hannet/api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppUserRoleController : ControllerBase
    {
        #region Intialize

        private readonly IApplicationUserRoleService _applicationUserRoleService;
        private readonly IMapper _mapper;
        private readonly ILogger<AppUserRoleController> _logger;

        public AppUserRoleController(ILogger<AppUserRoleController> logger, IApplicationUserRoleService applicationUserRoleService, IMapper mapper)
        {
            _applicationUserRoleService = applicationUserRoleService;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion Intialize

        #region Properties

        /// <summary>
        /// Get danh sách roles
        /// </summary>
        /// <returns></returns>
        [HttpGet("getuserroleid")]
        public async Task<IActionResult> GetUserRoleId(string userId)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/approles/getuserroleid/userId?={userId}", "GET");
                var model = await _applicationUserRoleService.GetAllUserRole(userId);

                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #endregion Properties
    }
}