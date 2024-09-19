using Microsoft.AspNetCore.Mvc;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Controllers
{
    [Route("AMAssignmentRules/", Name = "AMAssignmentRules")]
    [ApiController]
    public class AMAssignmentRulesController : ControllerBase
    {
        #region Variables
        private readonly IAMAssignmentRules _iAMAssignmentRules;
        private readonly ILogger<AMAssignmentRulesController> _logger;
        #endregion

        #region Constructors
        public AMAssignmentRulesController(IAMAssignmentRules iAMAssignmentRules, ILogger<AMAssignmentRulesController> logger)
        {
            _iAMAssignmentRules = iAMAssignmentRules;
            _logger = logger;
        }
        #endregion

        #region Public APIs

        [Authorize]
        [HttpPost]
        [Route("List")]
        public async  Task<ObjectResult> GetAMAssignmentRules([FromBody] AMAssignmentRulesViewModel aMAssignmentRulesViewModel)
        {
            try
            {
                #region PreValidation
                if (aMAssignmentRulesViewModel == null || (aMAssignmentRulesViewModel.pagenumber == 0 || aMAssignmentRulesViewModel.totalrecord == 0))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide pagenumber and totalrecord" });
                #endregion

                string Sortdatafield = "Id";
                string Sortorder = "ASC";

                aMAssignmentRulesViewModel.FilterFields_AMAssignmentRules ??= new();

                object[] param = new object[] {
                aMAssignmentRulesViewModel.pagenumber, aMAssignmentRulesViewModel.totalrecord, Sortdatafield, Sortorder,
                aMAssignmentRulesViewModel.FilterFields_AMAssignmentRules.Description,
                aMAssignmentRulesViewModel.FilterFields_AMAssignmentRules.Priority};

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<sproc_UTS_GetAMAssignmentRules_Result> aMAssignmentRules = await _iAMAssignmentRules.GetAMAssignmentRules(paramasString);

                if (aMAssignmentRules.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.ListingResponses(aMAssignmentRules, aMAssignmentRules[0].TotalRecords, aMAssignmentRulesViewModel.totalrecord, aMAssignmentRulesViewModel.pagenumber) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No AMAssignment Rules Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

    }
}
