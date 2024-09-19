using Microsoft.AspNetCore.Mvc;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Repositories.Interfaces;
namespace UTSATSAPI.Controllers
{
    [Authorize]
    [Route("ChatGPTResonse/", Name = "ChatGPTResonse")]
    [ApiController]
    public class ChatGPTResonseController: ControllerBase
    {
        #region Variables
        private readonly IChatGPTResponse _iChatGPTResponse;
        #endregion

        #region Constructors
        public ChatGPTResonseController(IChatGPTResponse chatGPTResponse)
        {
            _iChatGPTResponse = chatGPTResponse;
        }
        #endregion
        #region ChatGPTResponse

        [Authorize]
        [HttpPost("ChatGPTResponseForUrlParsing")]
        public async Task<ObjectResult> GetChatGPTResponseForUrlParsing(ChatGPTResponseViewModel filter)
        {
            try
            {
                object[] param = new object[]
                {
                    filter.PageIndex > 0 ? filter.PageIndex : 1,
                    filter.PageSize > 0 ? filter.PageSize : 50,
                    string.IsNullOrEmpty(filter.SortExpression) ? "CreatedDateTime" : filter.SortExpression,
                    string.IsNullOrEmpty(filter.SortDirection) ? "desc" : filter.SortDirection,
                    filter.IsParsingURL
                };

                string paramasString = CommonLogic.ConvertToParamString(param);
                filter.PageIndex = filter.PageIndex > 0 ? filter.PageIndex : 1;
                filter.PageSize = filter.PageSize > 0 ? filter.PageSize : 50;
                List<sproc_Get_ChatGPT_Response_For_UrlParsing_List_Result> ChatGPTResponseList = new();

                string val = paramasString.Replace("''", "null");
                ChatGPTResponseList = await (_iChatGPTResponse.GetChatGPTResponse(val).ConfigureAwait(false));

                if (ChatGPTResponseList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.ListingResponses(ChatGPTResponseList, Convert.ToInt64(ChatGPTResponseList[0].TotalRecords), Convert.ToInt64(filter.PageSize), Convert.ToInt64(filter.PageIndex)) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
