using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Hannet.Common.Exceptions;
using Hannet.Model.Models;
using Hannet.Model.ViewModels;
using Hannet.Service;
using Hannet.WebApi.Infrastructure.Core;
using Hannet.WebApi.Infrastructure.Extentsions;

namespace Hannet.WebApi.Controllers
{
    /// <summary>
    /// Api quyền người dùng
    /// </summary>
    [Route("hannet/api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppRolesController : ControllerBase
    {
        #region Intialize

        private readonly IApplicationRoleService _applicationRoleService;
        private readonly IMapper _mapper;
        private readonly ILogger<AppRolesController> _logger;

        public AppRolesController(ILogger<AppRolesController> logger, IApplicationRoleService applicationRoleService, IMapper mapper)
        {
            _applicationRoleService = applicationRoleService;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion Intialize

        #region Properties

        /// <summary>
        /// Get danh sách quyền phân trang
        /// </summary>
        /// <param name="request"></param>
        /// <param name="page">Trang thứ</param>
        /// <param name="pageSize">Số bản ghi hiển thị trong 1 trang</param>
        /// <param name="filter">Từ khóa tìm kiếm</param>
        /// <returns></returns>
        [HttpGet("getlistpaging")]
        [Authorize(Roles = "ViewRole")]
        public async Task<IActionResult> GetListPaging(int page = 0, int pageSize = 100, string? keyword = null)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/approles/getlistpaging", "GET");
                int totalRow = 0;
                var model = await _applicationRoleService.GetAll(keyword);
                totalRow = model.Count();
                var paging = model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);
                IEnumerable<AppRoleViewModel> modelVm = _mapper.Map<IEnumerable<AppRole>, IEnumerable<AppRoleViewModel>>(paging);

                PaginationSet<AppRoleViewModel> pagedSet = new PaginationSet<AppRoleViewModel>()
                {
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize),
                    Items = modelVm
                };

                return Ok(pagedSet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get id role theo group id
        /// </summary>
        /// <param name="groupId">id group</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("getlistbygroupid")]
        [Authorize(Roles = "ViewRole")]
        public async Task<IActionResult> GetListByGroupId(int groupId)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/approles/getlistpaging?groupId={groupId}", "GET");
                var model = await _applicationRoleService.GetListRoleByGroupId(groupId);
                IEnumerable<AppRoleViewModel> modelVm = _mapper.Map<IEnumerable<AppRole>, IEnumerable<AppRoleViewModel>>(model);
                return Ok(modelVm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get danh sách quyền
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("getall")]
        [Authorize(Roles = "ViewRole")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/approles/getall", "GET");
                var model = await _applicationRoleService.GetAll();
                IEnumerable<AppRoleViewModel> modelVm = _mapper.Map<IEnumerable<AppRole>, IEnumerable<AppRoleViewModel>>(model.OrderByDescending(x => x.CreatedDate));

                return Ok(modelVm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Danh sách phân quyền treeview
        /// </summary>
        /// <returns></returns>
        [HttpGet("gettreeroles")]
        [Authorize(Roles = "ViewRole")]
        public async Task<IActionResult> GetTreeRoles()
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/approles/gettreeroles", "GET");
                var model = await _applicationRoleService.GetTreeRoles();
                // IEnumerable<AppRoleViewModel> modelVm = _mapper.Map<IEnumerable<AppRole>, IEnumerable<AppRoleViewModel>>(model.OrderByDescending(x => x.CreatedDate));

                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Xem thông tin chi tiết quyền
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id">id quyền cần xem</param>
        /// <returns></returns>
        [HttpGet("getbyid/{id}")]
        [Authorize(Roles = "ViewRole")]
        public async Task<IActionResult> GetById(string id)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/approles/getbyid/{id}", "GET");
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(nameof(id) + " không có giá trị.");
            }
            try
            {
                AppRole appRole = await _applicationRoleService.GetDetail(id);
                var modelVm = _mapper.Map<AppRole, AppRoleViewModel>(appRole);
                return Ok(appRole);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Thêm mới quyền
        /// </summary>
        /// <param name="request"></param>
        /// <param name="AppRoleViewModel"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = "CreateRole")]
        public async Task<IActionResult> Create(AppRoleViewModel AppRoleViewModel)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/approles/create", "POST");
            if (ModelState.IsValid)
            {
                var newAppRole = new AppRole();
                newAppRole.UpdateApplicationRole(AppRoleViewModel, "add");
                newAppRole.CreatedDate = DateTime.Now;
                //newAppRole.Id = Guid.NewGuid().ToString();
                try
                {
                    var role = await _applicationRoleService.Add(newAppRole);
                    var result = _mapper.Map<AppRole, AppRoleViewModel>(role);
                    return CreatedAtAction(nameof(Create), new { id = result.Id }, result);
                }
                catch (NameDuplicatedException dex)
                {
                    return BadRequest(dex);
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
        /// Chỉnh sửa quyền
        /// </summary>
        /// <param name="AppRoleViewModel"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [Authorize(Roles = "UpdateRole")]
        public async Task<IActionResult> Update(AppRoleViewModel AppRoleViewModel)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/approles/update", "PUT");
            if (ModelState.IsValid)
            {
                var appRole = await _applicationRoleService.GetDetail(AppRoleViewModel.Id);
                try
                {
                    appRole.UpdateApplicationRole(AppRoleViewModel, "update");
                    appRole.UpdatedDate = DateTime.Now;
                    var result = await _applicationRoleService.Update(appRole);
                    return CreatedAtAction(nameof(Update), result);
                }
                catch (NameDuplicatedException dex)
                {
                    return BadRequest(dex);
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
        /// Xóa quyền
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "DeleteRole")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/approles/delete/{id}", "DELETE");
                var result = await _applicationRoleService.Delete(id);
                return CreatedAtAction(nameof(Delete), result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="request"></param>
        /// <param name="checkedList">List id cần xóa</param>
        /// <returns></returns>
        [HttpDelete("deletemulti")]
        [Authorize(Roles = "DeleteRole")]
        public async Task<IActionResult> DeleteMulti(string checkedList)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/approles/deletemulti", "DELETE");
                int countSuccess = 0;
                int countError = 0;
                List<string> result = new List<string>();
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                List<string> listItem = JsonConvert.DeserializeObject<List<string>>(checkedList);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                foreach (var item in listItem)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                {
                    try
                    {
                        await _applicationRoleService.Delete(item);
                        countSuccess++;
                    }
                    catch (Exception)
                    {
                        countError++;
                    }
                }
                result.Add("Xoá thành công: " + countSuccess);
                result.Add("Lỗi: " + countError);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("gettreeviewbyuser")]
        //[Authorize(Roles = "ViewMenu")]
        public async Task<IActionResult> GetTreeView(string userId)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/appmenus/gettreeviewbyuser?userid={userId}", "GET");
                var query = await _applicationRoleService.GetTreeMenuByUserId(userId);
                return Ok(query);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #endregion Properties
    }
}