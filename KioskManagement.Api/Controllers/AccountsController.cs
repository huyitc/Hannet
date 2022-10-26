using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using KioskManagement.Model.Models;
using KioskManagement.Model.ViewModels;
using KioskManagement.Service;

namespace KioskManagement.WebApi.Controllers
{
    [Route("api/kioskmanagement/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        protected readonly ILogger<AccountsController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly IIdentityService _identityService;
        private readonly AppSettings _appSettings;

        public AccountsController(ILogger<AccountsController> logger, IIdentityService identityService, UserManager<AppUser> userManager, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _userManager = userManager;
            _identityService = identityService;
            _appSettings = appSettings.Value;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel model)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/accounts/login", "POST");
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user == null)
                {
                    return Unauthorized();
                }

                var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
                List<string> roles = (List<string>)await _userManager.GetRolesAsync(user);
                if (!passwordValid)
                {
                    return Unauthorized();
                }

                var token = this._identityService.GenerateJwtToken(
                    user.Id,
                    user.UserName,
                    roles,
                    this._appSettings.Secret);

                return new LoginResponseModel
                {
                    Id = user.Id,
                    EmId = user.EM_ID,
                    Access_token = "Bearer " + token,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    Image = user.Image,
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("getprofileasync")]
        public async Task<ActionResult<LoginResponseModel>> GetProfileAsync(string userName, string password)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/accounts/getprofileasync", "GET");
                var user = await _userManager.FindByNameAsync(userName);
                if (user == null)
                {
                    return Unauthorized();
                }

                var passwordValid = await _userManager.CheckPasswordAsync(user, password);

                if (!passwordValid)
                {
                    return Unauthorized();
                }
                return new LoginResponseModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    Image = user.Image,
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("Checkpassword")]
        [AllowAnonymous]
        public async Task<bool> CheckPasswd(string userName, string pass)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return false;
            }
            else
            {
                return await _userManager.CheckPasswordAsync(user, pass);
            }

        }
    }
}