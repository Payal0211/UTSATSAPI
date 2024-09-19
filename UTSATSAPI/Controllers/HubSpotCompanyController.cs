using Microsoft.AspNetCore.Mvc;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Controllers
{
    [Route("HubSpotCompany/", Name = "HubSpotCompany")]
    public class HubSpotCompanyController : ControllerBase
    {
        #region Variables
        private readonly ICompany _iCompany;
        #endregion

        #region Constructors
        public HubSpotCompanyController(ICompany iCompany)
        {
            _iCompany = iCompany;
        }
        #endregion

        #region Public APIs
        [Authorize]
        [HttpPost]
        [Route("List")]
        public async Task<ObjectResult> GetHubSpotCompanyList([FromBody] HubSpotCompanyViewModel hubSpotCompanyViewModel)
        {
            try
            {
                #region PreValidation
                if (hubSpotCompanyViewModel == null || (hubSpotCompanyViewModel.pagenumber == 0 || hubSpotCompanyViewModel.totalrecord == 0))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide pagenumber and totalrecord" });
                #endregion

                hubSpotCompanyViewModel.FilterFields_HubSpotCompanyList ??= new();

                hubSpotCompanyViewModel.Sortdatafield = string.IsNullOrEmpty(hubSpotCompanyViewModel.Sortdatafield) ? "CreatedbyDatetime" : hubSpotCompanyViewModel.Sortdatafield;
                hubSpotCompanyViewModel.Sortorder = string.IsNullOrEmpty(hubSpotCompanyViewModel.Sortorder) ? "desc" : hubSpotCompanyViewModel.Sortorder;

                object[] param = new object[] {
                hubSpotCompanyViewModel.pagenumber, hubSpotCompanyViewModel.totalrecord,
                hubSpotCompanyViewModel.Sortdatafield, hubSpotCompanyViewModel.Sortorder,
                hubSpotCompanyViewModel.FilterFields_HubSpotCompanyList.Company,
                    hubSpotCompanyViewModel.FilterFields_HubSpotCompanyList.Website,
                hubSpotCompanyViewModel.FilterFields_HubSpotCompanyList.Domain};

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<Sproc_UTS_GetHubSpotCompanyList_Result> hubspotCompanyListData = await _iCompany.GetHubSpotCompanyList(paramasString).ConfigureAwait(false);

                if (hubspotCompanyListData.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.ListingResponses(hubspotCompanyListData, hubspotCompanyListData[0].TotalRecords, hubSpotCompanyViewModel.totalrecord, hubSpotCompanyViewModel.pagenumber) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No HubSpotCompany Available", Details = CustomRendering.EmptyRows() });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
