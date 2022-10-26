using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using KioskManagement.Common.Exceptions;
using KioskManagement.Model.Models;
using KioskManagement.Model.ViewModels;
using KioskManagement.Service;
using KioskManagement.WebApi.Infrastructure.Core;
using KioskManagement.WebApi.Infrastructure.Extentsions;
using Microsoft.AspNetCore.Identity;

namespace KioskManagement.WebApi.Controllers
{
    /// <summary>
    /// Api phân nhóm người dùng
    /// </summary>
    [Route("api/kioskmanagement/[controller]")]
    [ApiController]
    [Authorize]
    public class AppGroupsController : ControllerBase
    {
        #region Intialize
        private readonly UserManager<AppUser> _userManager;
        private readonly IApplicationGroupService _applicationGroupService;
        private readonly IApplicationRoleService _applicationRoleService;
        private readonly IMapper _mapper;
        private ILogger<AppGroupsController> _logger;

        public AppGroupsController(ILogger<AppGroupsController> logger, UserManager<AppUser> userManager, IMapper mapper, IApplicationGroupService applicationGroupService, IApplicationRoleService applicationRoleService)
        {
            _applicationGroupService = applicationGroupService;
            _mapper = mapper;
            _applicationRoleService = applicationRoleService;
            _logger = logger;
            _userManager = userManager;
        }

        #endregion Intialize

        #region Properties

        /// <summary>
        /// Get danh sách phân nhóm không truyền params
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        [Authorize(Roles = "ViewGroup")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/appgroups/getall", "GET");
                var model = await _applicationGroupService.GetAll();
                var mapping = _mapper.Map<IEnumerable<AppGroup>, IEnumerable<AppGroupViewModel>>(model.OrderByDescending(x => x.Id));
                return Ok(mapping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get phân nhóm phân trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet("getallbypaging")]
        [Authorize(Roles = "ViewGroup")]
        public async Task<IActionResult> GetAllByPaging(int page = 0, int pageSize = 100, string? keyword = null)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/appgroups/getallbypaging", "GET");
                var model = await _applicationGroupService.GetAll(keyword);
                int totalRow = 0;
                var data = model.OrderByDescending(x => x.Id).Skip(page * pageSize).Take(pageSize);
                var mapping = _mapper.Map<IEnumerable<AppGroup>, IEnumerable<AppGroupViewModel>>(data);

                totalRow = model.Count();

                var paging = new PaginationSet<AppGroupViewModel>()
                {
                    Items = mapping,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                return Ok(paging);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get thông tin phân nhóm theo id
        /// </summary>
        /// <param name="id">Id nhóm</param>
        /// <returns></returns>
        [HttpGet("getbyid/{id}")]
        [Authorize(Roles = "ViewGroup")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/appgroups/getbyid/{id}", "GET");
                var model = await _applicationGroupService.GetDetail(id);
                var mapping = _mapper.Map<AppGroup, AppGroupViewModel>(model);
                return Ok(mapping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Thêm mới phân nhóm
        /// </summary>
        /// <param name="appGroupViewModel"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = "CreateGroup")]
        public async Task<IActionResult> Create(AppGroupViewModel appGroupViewModel)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/appgroups/create", "POST");
            if (ModelState.IsValid)
            {
                var newAppGroup = new AppGroup();
                newAppGroup.UpdateGroup(appGroupViewModel);
                newAppGroup.CreatedDate = DateTime.Now;
                try
                {
                    var appGroup = await _applicationGroupService.Add(newAppGroup);

                    //save group
                    var listRoleGroup = new List<AppRoleGroup>();
                    if (appGroupViewModel.Roles != null)
                    {
                        foreach (var role in appGroupViewModel.Roles)
                        {
                            listRoleGroup.Add(new AppRoleGroup()
                            {
                                GroupId = appGroup.Id,
                                RoleId = role.Id
                            });
                        }
                        await _applicationRoleService.AddRolesToGroup(listRoleGroup, appGroup.Id);
                    }

                    return CreatedAtAction(nameof(Create), new { id = newAppGroup.Id }, newAppGroup);
                }
                catch (NameDuplicatedException dex)
                {
                    return BadRequest(dex.Message);
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
        /// Chỉnh sửa phân nhóm
        /// </summary>
        /// <param name="appGroupViewModel"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [Authorize(Roles = "UpdateGroup")]
        public async Task<IActionResult> Update(AppGroupViewModel appGroupViewModel)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/appgroups/update", "PUT");
            if (ModelState.IsValid)
            {
                //var newAppGroup = new AppGroup();
                //newAppGroup.UpdateGroup(appGroupViewModel);
                var newAppGroup = await _applicationGroupService.GetById(appGroupViewModel.Id);
                newAppGroup.UpdateGroup(appGroupViewModel);
                newAppGroup.UpdatedDate = DateTime.Now;

                try
                {
                    var appGroup = await _applicationGroupService.Update(newAppGroup);

                    var listRole = await _applicationRoleService.GetListRoleByGroupId(appGroup.Id);
                    var listUsers = await _applicationRoleService.GetListUserByGroupId(appGroup.Id);
                    foreach (var user in listUsers)
                    {
                        foreach (var role in listRole)
                        {
                            var appUser = await _userManager.FindByIdAsync(user.Id);
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                            //await _userManager.AddToRolesAsync(appUser, listRole.Select(x => x.Name));
                        }
                    }

                    //save group
                    var listRoleGroup = new List<AppRoleGroup>();
                    if (appGroupViewModel.Roles != null)
                    {
                        foreach (var role in appGroupViewModel.Roles)
                        {
                            listRoleGroup.Add(new AppRoleGroup()
                            {
                                GroupId = appGroup.Id,
                                RoleId = role.Id
                            });
                        }
                    }

                    await _applicationRoleService.AddRolesToGroup(listRoleGroup, appGroup.Id);
                    listRole = await _applicationRoleService.GetListRoleByGroupId(appGroup.Id);
                    foreach (var user in listUsers)
                    {
                            var appUser = await _userManager.FindByIdAsync(user.Id);
                            await _userManager.AddToRolesAsync(appUser, listRole.Select(x => x.Name));
                        
                    }

                    return CreatedAtAction(nameof(Update), new { id = appGroupViewModel.Id }, appGroupViewModel);
                }
                catch (NameDuplicatedException dex)
                {
                    return BadRequest(dex.Message);
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
        /// Xóa 1 bản ghi
        /// </summary>
        /// <param name="id">id nhóm</param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "DeleteGroup")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/appgroups/delete/{id}", "DELETE");
            try
            {
                var appGroup = await _applicationGroupService.Delete(id);
                return Ok(appGroup);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="checkedList">list id nhóm cần xóa</param>
        /// <returns></returns>
        [HttpDelete("deletemulti")]
        [Authorize(Roles = "DeleteGroup")]
        public async Task<IActionResult> DeleteMulti(string checkedList)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/appgroups/deletemulti", "DELETE");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                try
                {
                    int countSuccess = 0;
                    int countError = 0;
                    List<string> result = new List<string>();
                    var listItem = JsonConvert.DeserializeObject<List<int>>(checkedList);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    foreach (var item in listItem)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    {
                        try
                        {
                            await _applicationGroupService.Delete(item);
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
                    return BadRequest(ex.Message);
                }
            }
        }

        /// <summary>
        /// Lấy danh sách nhóm theo id User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("getlistgroupbyuser")]
        [Authorize(Roles = "ViewGroup")]
        public async Task<IActionResult> GetListGroupByUser(string userId)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/appgroups/getlistgroupbyuser?userId={userId}", "GET");
                var result = await _applicationGroupService.GetListGroupByUserId(userId);
                var mapping = _mapper.Map<IEnumerable<AppGroup>, IEnumerable<AppGroupViewModel>>(result);
                return Ok(mapping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #endregion Properties
    }
}