using Hannet.Common.Ultilities;
using Hannet.Model.MappingModels;
using Hannet.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Hannet.WebApi.Controllers
{
    [Route("hannet/api/[controller]")]
    [ApiController]
    [Authorize]
    public class GetCheckinController : ControllerBase
    {
        /// <summary>
        /// Xem danh sách thiết bị
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var bod = new GetCheckin
                {
                    placeID = "9570",
                    date = "2022-12-01",
                };
                var settings = new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Populate };

                var res = await Lib.MethodPostAsyncHanet("https://partner.hanet.ai/person/getCheckinByPlaceIdInDay", bod);
                CheckInByPlaceReponse resultInfo = JsonConvert.DeserializeObject<CheckInByPlaceReponse>(res.Content);
                return Ok(resultInfo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
