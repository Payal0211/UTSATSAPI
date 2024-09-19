namespace UTSATSAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Dynamic;
    using UTSATSAPI.Helpers.Common;
    using UTSATSAPI.Models.ComplexTypes;
    using UTSATSAPI.Models.Models;
    using UTSATSAPI.Models.ViewModels;
    using UTSATSAPI.Repositories.Interfaces;
    using System.Linq;

    [Authorize]
    [Route("Deal/", Name = "Deal List")]
    [ApiController]
    public class DealListController : ControllerBase
    {
        #region Variables
        private readonly IDeals _iDeals;

        private readonly ILogger<DealListController> _logger;

        private readonly ICommonInterface _commonInterface;
        #endregion

        #region Constructors
        public DealListController(IDeals iDeals, ILogger<DealListController> logger, ICommonInterface commonInterface)
        {
            _logger = logger;
            _iDeals = iDeals;
            _commonInterface = commonInterface;
        }
        #endregion

        #region Public APIs

        [HttpPost]
        [Route("List")]
        public async Task<ObjectResult> GetDealList([FromBody] DealListViewModel dealListViewModel)
        {
            try
            {
                #region PreValidation
                if (dealListViewModel == null || (dealListViewModel.pagenumber == 0 || dealListViewModel.totalrecord == 0))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide pagenumber and totalrecord" });
                #endregion

                string Sortdatafield = "DealDate";
                string Sortorder = "Desc";

                dealListViewModel.FilterFields_DealList ??= new();

                if (!string.IsNullOrEmpty(dealListViewModel.FilterFields_DealList.fromDate) && !string.IsNullOrEmpty(dealListViewModel.FilterFields_DealList.toDate))
                {
                    dealListViewModel.FilterFields_DealList.fromDate = CommonLogic.ConvertString2DateTime(dealListViewModel.FilterFields_DealList.fromDate).ToString("yyyy-MM-dd");
                    dealListViewModel.FilterFields_DealList.toDate = CommonLogic.ConvertString2DateTime(dealListViewModel.FilterFields_DealList.toDate).ToString("yyyy-MM-dd");
                }

                //Trim the blank spaces from the end of the search text.
                string searchText = string.Empty;
                if (!string.IsNullOrEmpty(dealListViewModel.searchText))
                {
                    searchText = dealListViewModel.searchText.TrimStart().TrimEnd();
                }

                string LeadType = string.Empty;

                if (!string.IsNullOrEmpty(dealListViewModel.FilterFields_DealList.Lead_Type))
                {
                    if (dealListViewModel.FilterFields_DealList.Lead_Type.Contains("1") && dealListViewModel.FilterFields_DealList.Lead_Type.Contains("2"))
                    {
                        LeadType = "InBound,OutBound";
                    }
                    else
                    {
                        if (dealListViewModel.FilterFields_DealList.Lead_Type.Contains("1"))
                        {
                            LeadType = "InBound";
                        }
                        if (dealListViewModel.FilterFields_DealList.Lead_Type.Contains("2"))
                        {
                            LeadType = "OutBound";
                        }
                    }
                }

                object[] param = new object[] 
                {
                    dealListViewModel.pagenumber, 
                    dealListViewModel.totalrecord, 
                    Sortdatafield, 
                    Sortorder,
                    dealListViewModel.FilterFields_DealList.Deal_Id ?? "", 
                    LeadType,
                    dealListViewModel.FilterFields_DealList.Pipeline ?? "", 
                    dealListViewModel.FilterFields_DealList.Company ?? "",
                    dealListViewModel.FilterFields_DealList.GEO ?? "", 
                    dealListViewModel.FilterFields_DealList.BDR ?? "",
                    dealListViewModel.FilterFields_DealList.Sales_Consultant ?? "", 
                    dealListViewModel.FilterFields_DealList.DealStage ?? "", 
                    dealListViewModel.FilterFields_DealList.fromDate, 
                    dealListViewModel.FilterFields_DealList.toDate,
                    searchText
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<sproc_UTS_GetDealList_Result> dealListData = await _iDeals.GetDealList(paramasString);

                if (dealListData.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.ListingResponses(dealListData, dealListData[0].TotalRecords.Value, dealListViewModel.totalrecord, dealListViewModel.pagenumber) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Deals Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("Detail")]
        public async Task<ObjectResult> GetDealDetails([FromQuery] long DealId)
        {
            try
            {
                #region PreValidation
                if (DealId == null || DealId == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide Deal ID" });
                #endregion

                DealDetailViewModel dealDetail = null;

                GenDeal _Deals = await _iDeals.GetGenDealByDealNumber(DealId).ConfigureAwait(false);

                if (_Deals != null)
                {
                    dealDetail = new DealDetailViewModel();
                    PrgDealStage stages = await _iDeals.GetPrgDealStageById(_Deals.DealStageId).ConfigureAwait(false);
                    PrgDealType type = await _iDeals.GetPrgDealTypeById(_Deals.DealTypeId).ConfigureAwait(false);

                    dealDetail.DealName = _Deals.DealName;

                    if (stages != null)
                        dealDetail.DealStage = stages.Stage;

                    if (type != null)
                        dealDetail.DealType = type.DealType;

                    string companyid = string.Empty;

                    dealDetail.GetDealCompanydetails = await _iDeals.GetDealCompanydetails(_Deals.DealId).ConfigureAwait(false);
                    if (dealDetail.GetDealCompanydetails.Any())
                    {
                        dealDetail.DealOwner = string.IsNullOrEmpty(dealDetail.GetDealCompanydetails[0].DealOwner) ? "" : dealDetail.GetDealCompanydetails[0].DealOwner;
                        dealDetail.POC = string.IsNullOrEmpty(dealDetail.GetDealCompanydetails[0].POC) ? "" : dealDetail.GetDealCompanydetails[0].POC;
                        companyid = dealDetail.GetDealCompanydetails.FirstOrDefault().CompanyId.ToString();
                    }

                    dealDetail.GetDealLeadDetails = await _iDeals.GetDealLeadDetails(_Deals.DealId).ConfigureAwait(false);
                    dealDetail.GetDealPrimaryClient = await _iDeals.GetDealPrimaryClient(_Deals.DealId).ConfigureAwait(false);
                    dealDetail.GetDealSecondaryClient = await _iDeals.GetDealSecondaryClient(_Deals.DealId).ConfigureAwait(false);
                    dealDetail.GetDealActivity = await _iDeals.GetDealActivity(_Deals.DealId).ConfigureAwait(false);
                    var LoggedInUserTypeID = SessionValues.LoginUserTypeId;
                    var LoggedInUserID = SessionValues.LoginUserId;
                    if (!string.IsNullOrWhiteSpace(companyid))
                    {
                        ViewAllHRViewModel viewAllHRViewModel = new ViewAllHRViewModel();
                        viewAllHRViewModel.Pagenum = 1;
                        viewAllHRViewModel.Pagesize = 0;
                        viewAllHRViewModel.FilterFields_ViewAllHRs = new FilterFields_ViewAllHR();
                        viewAllHRViewModel.FilterFields_ViewAllHRs.Company = companyid;
                        dealDetail.GetAllHRs = _commonInterface.ViewAllHR.GetAllHRs(viewAllHRViewModel, LoggedInUserTypeID, LoggedInUserID);
                    }
                }

                if (dealDetail != null)

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = dealDetail });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Deal Not found" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("FilterCriterias")]
        public async Task<ObjectResult> FilterCriterias()
        {
            try
            {

                dynamic dObject = new ExpandoObject();

                var varCompany = await _iDeals.GetGenCompanyList().ConfigureAwait(false);

                dObject.Company = varCompany.Select(x => new SelectListItem() { Text = x.Id.ToString(), Value = x.Company });

                var varGeo = await _iDeals.GetPrgGeosList().ConfigureAwait(false);
                dObject.Geo = varGeo.Select(x => new SelectListItem { Value = x.Geo, Text = x.Id.ToString() });

                var varBDR = await _iDeals.sp_get_UTS_GetFilterTypeForDeals_Result("'3,4,5'").ConfigureAwait(false);
                var varSalesConsultant = await _iDeals.sp_get_UTS_GetFilterTypeForDeals_Result("'1'").ConfigureAwait(false);
                var varManager = await _iDeals.sp_get_UTS_GetFilterTypeForDeals_Result("'9,10,'").ConfigureAwait(false);
                dObject.BDR = varBDR.Select(x => new SelectListItem { Value = x.FullName, Text = x.ID.Value.ToString() });
                dObject.SalesConsultant = varSalesConsultant.Select(x => new SelectListItem { Value = x.FullName, Text = x.ID.Value.ToString() });
                dObject.Manager = varManager.Select(x => new SelectListItem { Value = x.FullName, Text = x.ID.Value.ToString() });

                var varPipeline = await _iDeals.GetPrgPipelineList().ConfigureAwait(false);

                dObject.Pipeline = varPipeline.Select(x => new SelectListItem { Value = x.PipelineLabel, Text = x.Id.ToString() });
                var varDealId = await _iDeals.GetGenDealsList().ConfigureAwait(false);
                dObject.DealId = varDealId.Select(x => new SelectListItem { Value = x.DealNumber, Text = x.Id.ToString() });

                List<SelectListItem> bindLeadTypes = new List<SelectListItem>();

                SelectListItem bindLeadType1 = new SelectListItem();
                bindLeadType1.Text = "1";
                bindLeadType1.Value = "InBound";
                bindLeadTypes.Add(bindLeadType1);

                SelectListItem bindLeadType2 = new SelectListItem();
                bindLeadType2.Text = "2";
                bindLeadType2.Value = "OutBound";
                bindLeadTypes.Add(bindLeadType2);

                dObject.LeadSource = bindLeadTypes;
                var varDealStage = await _iDeals.GetPrgDealStageList().ConfigureAwait(false);
                dObject.DealStage = varDealStage.Select(x => new SelectListItem { Value = x.Stage, Text = x.Id.ToString() });


                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.FilterListsResponse(dObject) });

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
