using Microsoft.AspNetCore.Mvc;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Controllers
{
    [Authorize]
    [Route("InterviewToSuccess")]
    [ApiController]
    public class InterviewToSuccessController : ControllerBase
    {
        #region Variables
        private readonly IInterviewToSuccess _interviewToSuccess;
        private readonly ILogger<InterviewToSuccessController> _logger;
        #endregion

        #region Constructors
        public InterviewToSuccessController(IInterviewToSuccess interviewToSuccess, ILogger<InterviewToSuccessController> logger)
        {
            _logger = logger;
            _interviewToSuccess = interviewToSuccess;
        }
        #endregion

        #region Public APIs

        [Authorize]
        [HttpGet("InterviewToSuccessList")]
        public ObjectResult GetInterviewToSuccess(string startDate, string endDate, bool? IsHRFocused = false)
        {
            #region PreValidation

            if (string.IsNullOrEmpty(startDate))
            {
                startDate = DateTime.Now.AddMonths(-1).Date.ToString("yyyy-MM-dd");
            }

            if (string.IsNullOrEmpty(endDate))
            {
                endDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
            }

            #endregion

            try
            {
                object[] param = new object[] { startDate, endDate, IsHRFocused ?? null };
                string paramasString = CommonLogic.ConvertToParamString(param);
                var interviwToSuccessDetails = _interviewToSuccess.GetInterviewtoSuccessReport(paramasString);

                return StatusCode(StatusCodes.Status200OK, CommonLogic.ReturnObject(StatusCodes.Status200OK, "InterviewToSuccess Details", interviwToSuccessDetails));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CommonLogic.ReturnObject(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }


        [Authorize]
        [HttpGet("GetInterviewToSuccessPopupDetails")]
        public ObjectResult GetInterviewToSuccessPopupDetails(string teamID, string I2SLabel, string fromDate, string toDate, bool? IsHRFocused = false)
        {
            #region PreValidation

            if (teamID == null)
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide teamID" });

            if (I2SLabel == null)
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide I2SLabel" });

            if (fromDate == null)
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide From Date" });

            if (toDate == null)
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide To Date" });

            #endregion

            try
            {
                object[] param = new object[] { teamID, I2SLabel, fromDate, toDate, IsHRFocused ?? null };
                string paramasString = CommonLogic.ConvertToParamString(param);
                var interviwToSuccessDetails = _interviewToSuccess.GetInterviewToSuccessPopUpReport(paramasString);

                return StatusCode(StatusCodes.Status200OK, CommonLogic.ReturnObject(StatusCodes.Status200OK, "InterviewToSuccess Popup Details", interviwToSuccessDetails));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CommonLogic.ReturnObject(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        #endregion
    }
}