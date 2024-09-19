namespace UTSATSAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using UTSATSAPI.Helpers.Common;
    using UTSATSAPI.Models.ComplexTypes;
    using UTSATSAPI.Models.Models;
    using UTSATSAPI.Models.ViewModels;
    using UTSATSAPI.Repositories.Interfaces;

    [Route("CompanyLegalInfo/", Name = "CompanyLegalInfo")]
    public class CompanyLegalInfoController : ControllerBase
    {
        #region Variables
        private readonly ICompany _iCompany;
        #endregion

        #region Constructors
        public CompanyLegalInfoController(ICompany iCompany)
        {
            _iCompany = iCompany;
        }
        #endregion

        #region Public APIs
        [Authorize]
        [HttpPost]
        [Route("List")]
        public async Task<ObjectResult> GetCompanyLegalInfo([FromBody] ListLegalInfoFilter listLegalInfoFilter)
        {
            try
            {
                #region PreValidation
                if(listLegalInfoFilter == null || (listLegalInfoFilter.pagenumber == 0 || listLegalInfoFilter.totalrecord == 0))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide pagenumber and totalrecord" });
                #endregion


                string Sortdatafield = "ID";
                string Sortorder = "DESC";


                object[] param = new object[] {
                listLegalInfoFilter.pagenumber, listLegalInfoFilter.totalrecord, Sortdatafield, Sortorder,
                0, listLegalInfoFilter.FilterFields_LegalInfo.CompanyId,
                listLegalInfoFilter.FilterFields_LegalInfo.DocumentType, listLegalInfoFilter.FilterFields_LegalInfo.DocumentName,
                listLegalInfoFilter.FilterFields_LegalInfo.AgreementStatus, listLegalInfoFilter.FilterFields_LegalInfo.Company
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<sproc_GetCompanyLegalInfo_Result> legalInfoListData = await _iCompany.GetLegalInfoList(paramasString).ConfigureAwait(false);


                if (legalInfoListData.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.ListingResponses(legalInfoListData, legalInfoListData[0].TotalRecords, listLegalInfoFilter.totalrecord, listLegalInfoFilter.pagenumber) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Legal Info Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
