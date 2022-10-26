using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KioskManagement.Model.MappingModels;
using KioskManagement.Model.Models;
using KioskManagement.Service;

namespace KioskManagement.WebApi.Controllers
{
    [Route("api/kioskmanagement/[controller]")]
    [ApiController]
    [Authorize]
    public class AppMenuUsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ILogger<AppMenuUsersController> _logger;
        private readonly IAppMenuUserService _appMenuUserService;

        public AppMenuUsersController(IMapper mapper, ILogger<AppMenuUsersController> logger, IAppMenuUserService appMenuUserService)
        {
            _mapper = mapper;
            _logger = logger;
            _appMenuUserService= appMenuUserService;
        }

        /// <summary>
        /// Cấp quyền truy cập menu cho tài khoản
        /// </summary>
        /// <param name="appMenuUserMapping"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = "UpdateUserMenu")]
        public async Task<IActionResult> Create(AppMenuUserMapping appMenuUserMapping)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/appmenuusers/create", "POST");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (appMenuUserMapping.AppMenus.Count >0)
                {
                    await _appMenuUserService.Delete(appMenuUserMapping.UserId);
                    foreach (var item in appMenuUserMapping.AppMenus)
                    {
                        AppMenuUser appMenuUser= new AppMenuUser();
                        appMenuUser.UserId = appMenuUserMapping.UserId;
                        appMenuUser.MenuId = item.Id;
                        await _appMenuUserService.Add(appMenuUser);
                    }
                }

                return CreatedAtAction(nameof(Create), appMenuUserMapping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
