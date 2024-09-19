using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Controllers
{
    [Authorize]
    [Route("WebSocket/", Name = "WebSocket")]
    public class WebSocketController : ControllerBase
    {
        #region Variables
        private readonly RequestDelegate _next;
        private readonly IConfiguration _iConfiguration;
        private readonly IHiringRequest _hiringRequest;
        #endregion
        
        #region Constructor
        public WebSocketController(IConfiguration iConfiguration,IHiringRequest hiringRequest)
        {
            _iConfiguration = iConfiguration;
            _hiringRequest = hiringRequest;
        }
        #endregion

        #region Authorized APIs
        [HttpGet("SLAJobData")]
        
        public async Task<ObjectResult> GetSLAJobData(long HRID)
        {
            try
            {
                var data = await _hiringRequest.Get_HiringRequest_SLADetails(HRID).ConfigureAwait(false);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success" , Details = data });
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion
    }
}
