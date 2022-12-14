using Hannet.Common.Ultilities;
using Hannet.Model.MappingModels;
using Hannet.Model.ViewModels;
using Hannet.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Hannet.WebApi.Controllers
{
    [Route("hannet/api/[controller]")]
    [ApiController]
    [Authorize]
    public class GetCheckinController : ControllerBase
    {
        private readonly ICheckInService _checkInService;
        public GetCheckinController(ICheckInService checkInService)
        {
            _checkInService = checkInService;
        }


        /// <summary>
        /// Xem danh sách thiết bị
        /// </summary>
        /// <returns></returns>
        [HttpPost("getall")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(CheckInByPlaceInDayPram checkIn)
        {
            try
            {
               var bod = new GetCheckin
                {
                    placeID = checkIn.placeID,
                    date = checkIn.date,
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
