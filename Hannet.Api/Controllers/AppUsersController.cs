using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Hannet.Model.MappingModels;
using Hannet.Model.Models;
using Hannet.Model.ViewModels;
using Hannet.Service;
using Hannet.WebApi.Infrastructure.Core;
using Hannet.WebApi.Infrastructure.Extentsions;
using Hannet.Common.Ultilities;

namespace Hannet.WebApi.Controllers
{
    /// <summary>
    /// Api tài khoản người dùng
    /// </summary>
    [Route("hannet/api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppUsersController : ControllerBase
    {
        #region Intialize

        private readonly UserManager<AppUser> _userManager;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IMapper _mapper;
        private readonly IApplicationGroupService _applicationGroupService;
        private readonly IApplicationRoleService _applicationRoleService;
        protected readonly ILogger<AppUsersController> _logger;
        //private readonly ITAccountService _iTAccountService;

        public AppUsersController(ILogger<AppUsersController> logger, UserManager<AppUser> userManager,
            IApplicationUserService applicationUserService, IMapper mapper, IApplicationGroupService applicationGroupService, IApplicationRoleService applicationRoleService)
        {
            _logger = logger;
            _userManager = userManager;
            _applicationUserService = applicationUserService;
            _mapper = mapper;
            _applicationGroupService = applicationGroupService;
            _applicationRoleService = applicationRoleService;
            //_iTAccountService = tAccountService;
        }

        #endregion Intialize

        #region Properties

        /// <summary>
        /// Get danh sách tài khoản
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        [Authorize(Roles = "ViewUser")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/appusers/getall", "GET");
                var model = await _applicationUserService.GetAll();
                var mapping = _mapper.Map<IEnumerable<AppUser>, IEnumerable<AppUserViewModel>>(model.OrderByDescending(x => x.CreatedDate));
                return Ok(mapping);
            }
            catch (Exception dex)
            {
                return BadRequest(dex);
            }
        }

        /// <summary>
        /// Get danh sách tài khoản phân trang
        /// </summary>
        /// <param name="page">trang thứ</param>
        /// <param name="pageSize">số bản ghi trong 1 trang</param>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <returns></returns>
        [HttpGet("getlistpaging")]
        [Authorize(Roles = "ViewUser")]
        public async Task<IActionResult> GetListPaging(int page = 0, int pageSize = 10, string? keyword = null)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/appusers/getlistpaging", "GET");
                var model = await _applicationUserService.GetAllByMappingAsync(keyword);
                int totalRow = 0;
                totalRow = model.Count();
                var paging = model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);
                var paginationSet = new PaginationSet<AppUserMapping>()
                {
                    Items = paging,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };

                return Ok(paginationSet);
            }
            catch (Exception dex)
            {
                return BadRequest(dex);
            }
        }

        /// <summary>
        /// Xem tài khoản theo id
        /// </summary>
        /// <param name="id">UserId</param>
        /// <returns></returns>
        [HttpGet("getbyid/{id}")]
        [Authorize(Roles = "ViewUser")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/appusers/getbyid/{id}", "GET");
                var model = await _userManager.FindByIdAsync(id);
                var mapping = _mapper.Map<AppUser, AppUserViewModel>(model);
                return Ok(mapping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Chỉnh sửa tài khoản
        /// </summary>
        /// <param name="AppUserViewModel"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [Authorize(Roles = "UpdateUser")]
        public async Task<IActionResult> Update(AppUserViewModel AppUserViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/appusers/update", "PUT");
                    AppUser appUser = await _userManager.FindByIdAsync(AppUserViewModel.Id);
                    appUser.UpdateUser(AppUserViewModel, "update");
                    appUser.UpdatedDate = DateTime.Now;
                    if (!string.IsNullOrEmpty(AppUserViewModel.PasswordHash))
                    {
                        appUser.PasswordHash = _userManager.PasswordHasher.HashPassword(appUser, AppUserViewModel.PasswordHash);
                    }
                    var result = await _userManager.UpdateAsync(appUser);
                    if (result.Succeeded)
                    {
                        var listAppUserGroup = new List<AppUserGroup>();
                        var listRole = await _applicationRoleService.GetAll();
                        foreach (var role in listRole)
                        {
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                        }
                        if (AppUserViewModel.Groups != null)
                        {
                            foreach (var group in AppUserViewModel.Groups)
                            {
                                listAppUserGroup.Add(new AppUserGroup()
                                {
                                    GroupId = group.Id,
                                    UserId = AppUserViewModel.Id
                                });

                                var listRole1 = await _applicationRoleService.GetListRoleByGroupId(group.Id);

                                foreach (var role1 in listRole1)
                                {
                                    await _userManager.AddToRoleAsync(appUser, role1.Name);
                                }
                            }
                        }
                        await _applicationGroupService.AddUserToGroups(listAppUserGroup, AppUserViewModel.Id);

                        return CreatedAtAction(nameof(Create), appUser);
                    }
                    else
                        return BadRequest(result.Errors);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        /// <summary>
        /// đổi mk
        /// </summary>
        /// <param name="AppUserViewModel"></param>
        /// <returns></returns>
        [HttpPut("UpdateFromUser")]
        [Authorize(Roles = "UpdateUser")]
        public async Task<IActionResult> UpdateFromUser(AppUserViewModel AppUserViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AppUser appUser = await _userManager.FindByIdAsync(AppUserViewModel.Id);
                    var oldUserName = appUser.UserName;
                    //appUser.UpdateUser(applicationUserViewModel, "update");
                  if (!string.IsNullOrEmpty(AppUserViewModel.PasswordHash))
                    {
                        appUser.PasswordHash = _userManager.PasswordHasher.HashPassword(appUser, AppUserViewModel.PasswordHash);
                    }
                    var result = await _userManager.UpdateAsync(appUser);
                    if (result.Succeeded)
                    {
                        //AccountMD5 mD5 = new AccountMD5();
                        //TAccount acc = await _iTAccountService.QueryByUserName(oldUserName);
                        //acc.AccPassword = mD5.GetMD5(AppUserViewModel.PasswordHash);
                        //await _iTAccountService.Update(acc);

                        return CreatedAtAction(nameof(Create), appUser);
                    }
                    else
                        return BadRequest(result.Errors);

                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Thêm mới tài khoản
        /// </summary>
        /// <param name="AppUserViewModel"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = "CreateUser")]
        public async Task<IActionResult> Create(AppUserViewModel AppUserViewModel)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/appusers/create", "POST");
            if (ModelState.IsValid)
            {
                try
                {
                    AppUser appUser = new AppUser();
                    appUser.UpdateUser(AppUserViewModel, "add");
                    appUser.CreatedDate = DateTime.Now;
                    var result = await _userManager.CreateAsync(appUser, AppUserViewModel.PasswordHash);
                    if (result.Succeeded)
                    {
                        var listAppUserGroup = new List<AppUserGroup>();
                        if (AppUserViewModel.Groups != null)
                        {
                            foreach (var group in AppUserViewModel.Groups)
                            {
                                listAppUserGroup.Add(new AppUserGroup()
                                {
                                    GroupId = group.Id,
                                    UserId = appUser.Id
                                });
                                //add role to user
                                var listRole = await _applicationRoleService.GetListRoleByGroupId(group.Id);
                                foreach (var role in listRole)
                                {
                                    await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                                    await _userManager.AddToRoleAsync(appUser, role.Name);
                                }
                            }
                            await _applicationGroupService.AddUserToGroups(listAppUserGroup, appUser.Id);
                        }

                        return CreatedAtAction(nameof(Create), appUser);
                    }
                    else
                    {
                        return BadRequest(result.Errors);
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Xóa tài khoản
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id">id tài khoản</param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "DeleteUser")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/appusers/delete/{id}", "DELETE");
                var appUser = await _userManager.FindByIdAsync(id);
                var result = await _userManager.DeleteAsync(appUser);
                if (result.Succeeded)
                    return Ok(appUser);
                else
                    return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Xóa nhiều tài khoản
        /// </summary>
        /// <param name="checkedList"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete("deletemulti")]
        [Authorize(Roles = "DeleteUser")]
        public async Task<IActionResult> DeleteMulti(string checkedList)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/appusers/deletemulti", "DELETE");
                int countSuccess = 0;
                int countError = 0;
                List<string> result = new List<string>();
                var listItem = JsonConvert.DeserializeObject<List<string>>(checkedList);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                foreach (var item in listItem)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                {
                    try
                    {
                        var appUser = await _userManager.FindByIdAsync(item);
                        var res = await _userManager.DeleteAsync(appUser);
                        countSuccess++;
                    }
                    catch (Exception)
                    {
                        countError++;
                    }
                }
                result.Add("Xóa thành công: " + countSuccess);
                result.Add("Lỗi" + countError);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #endregion Properties
    }
}