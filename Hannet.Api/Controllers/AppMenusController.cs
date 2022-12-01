using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Hannet.Model.Models;
using Hannet.Model.ViewModels;
using Hannet.Service;
using Hannet.WebApi.Infrastructure.Core;
using Hannet.WebApi.Infrastructure.Extentsions;

namespace Hannet.WebApi.Controllers
{
    [Route("hannet/api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppMenusController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ILogger<AppMenusController> _logger;
        private readonly IAppMenuService _appMenuService;

        public AppMenusController(IMapper mapper, ILogger<AppMenusController> logger, IAppMenuService appMenuService)
        {
            _mapper = mapper;
            _logger = logger;
            _appMenuService = appMenuService;
        }

        /// <summary>
        /// Xem danh sách menu
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        [Authorize(Roles = "ViewMenu")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/appmenus/getall", "GET");
                var model = await _appMenuService.GetAll();
                var query = _mapper.Map<IEnumerable<AppMenu>, IEnumerable<AppMenuViewModel>>(model);
                return Ok(query);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Xem danh sách menu phân trang
        /// </summary>
        /// <param name="page">Trang thứ</param>
        /// <param name="pageSize">Số bản ghi hiển thị trong 1 trang</param>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <returns></returns>
        [HttpGet("getlistpaging")]
        [Authorize(Roles = "ViewMenu")]
        public async Task<IActionResult> GetListPaging(int page = 0, int pageSize = 10, string? keyword = null)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/appmenus/getlistpaging", "GET");
                var model = await _appMenuService.GetAll(keyword);
                int totalRow = 0;
                totalRow = model.Count();
                var paging = model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);
                var mapping = _mapper.Map<IEnumerable<AppMenu>, IEnumerable<AppMenuViewModel>>(paging);
                var paginationSet = new PaginationSet<AppMenuViewModel>()
                {
                    Items = mapping,
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
        /// Danh sách menu được cấp cho tài khoản phân theo treeview
        /// </summary>
        /// <param name="userId">Id tài khoản</param>
        /// <returns></returns>
        [HttpGet("gettreeviewbyuser")]
        //[Authorize(Roles = "ViewMenu")]
        public async  Task<IActionResult> GetTreeView(string userId)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/appmenus/gettreeviewbyuser?userid={userId}", "GET");
                var query = await _appMenuService.GetTreeMenuByUserIdNew(userId);
                return Ok(query);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Danh sách menu treeview
        /// </summary>
        /// <returns></returns>
        [HttpGet("getmenutree")]
        [Authorize(Roles = "ViewMenu")]
        public async Task< IActionResult> GetMenuTree()
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/appmenus/getmenutree", "GET");
                var query = await _appMenuService.GetTreeMenu();
                return Ok(query);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get danh sách menu theo tài khoản
        /// </summary>
        /// <param name="id">id tài khoản</param>
        /// <returns></returns>
        [HttpGet("getmenuuser")]
        //[Authorize(Roles = "ViewMenu")]
        public async Task< IActionResult> GetMenuUser(string id)
        {
            try
            {
                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/appmenus/getmenuuser", "GET");
                var query = await _appMenuService.GetMenuUser(id);
                return Ok(query);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Thêm mới menu
        /// </summary>
        /// <param name="appMenuViewModel"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = "CreateMenu")]
        public async Task<IActionResult> Create(AppMenuViewModel appMenuViewModel)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/appmenus/create", "POST");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                AppMenu menu = new AppMenu();
                menu.UpDateAppMenu(appMenuViewModel);
                menu.CreatedDate = DateTime.Now;
                menu.IsDeleted = false;
                var result = await _appMenuService.Add(menu);
                return CreatedAtAction(nameof(Create), result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Chỉnh sửa menu
        /// </summary>
        /// <param name="appMenuViewModel"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [Authorize(Roles = "ViewMenu")]
        public async Task<IActionResult> Update(AppMenuViewModel appMenuViewModel)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/appmenus/update", "PUT");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                AppMenu menu = await _appMenuService.GetById(appMenuViewModel.Id);
                menu.UpDateAppMenu(appMenuViewModel);
                menu.UpdatedDate = DateTime.Now;
                var result = await _appMenuService.Update(menu);
                return CreatedAtAction(nameof(Update), result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Xóa 1 bản ghi
        /// </summary>
        /// <param name="id">id nhóm</param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "DeleteMenu")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/kioskmanagement/appgroups/delete/{id}", "DELETE");
            try
            {
                var appGroup = await _appMenuService.Delete(id);
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
        [Authorize(Roles = "DeleteMenu")]
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
                            await _appMenuService.Delete(item);
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
    }
}