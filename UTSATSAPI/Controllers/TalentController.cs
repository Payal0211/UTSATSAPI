namespace UTSATSAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using UTSATSAPI.Helpers.Common;
    using UTSATSAPI.Models.ComplexTypes;
    using UTSATSAPI.Models.Models;
    using UTSATSAPI.Models.ViewModels;
    using UTSATSAPI.Repositories.Interfaces;

    [Authorize]
    [Route("Talent/", Name = "Talent")]
    [ApiController]
    public class TalentController : ControllerBase
    {
        #region Variables
        private readonly ITalent _iTalent;
        #endregion

        #region Constructors
        public TalentController(ITalent iTalent)
        {
            _iTalent = iTalent;
        }
        #endregion

        #region Public APIs

        [HttpPost]
        [Route("List")]
        public ObjectResult GetTalentList([FromBody] TalentViewModel talentViewModel)
        {
            try
            {
                #region PreValidation
                if (talentViewModel == null || (talentViewModel.pagenumber == 0 || talentViewModel.totalrecord == 0))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide pagenumber and totalrecord" });
                #endregion

                string Sortdatafield = "CreatedbyDatetime";
                string Sortorder = "DESC";

                talentViewModel.FilterFields_Talent ??= new();

                object[] param = new object[] {
                talentViewModel.pagenumber, talentViewModel.totalrecord, Sortdatafield, Sortorder,
                talentViewModel.FilterFields_Talent.UserName, talentViewModel.FilterFields_Talent.Name,
                talentViewModel.FilterFields_Talent.EmailID, talentViewModel.FilterFields_Talent.ContactNumber,
                talentViewModel.FilterFields_Talent.Status, talentViewModel.FilterFields_Talent.UserType,
                talentViewModel.FilterFields_Talent.TalentRole, talentViewModel.FilterFields_Talent.AccountStatus,
                talentViewModel.FilterFields_Talent.AfterClientSelectionStatus, talentViewModel.FilterFields_Talent.TalentCategory,
                talentViewModel.FilterFields_Talent.Talent_Type, talentViewModel.FilterFields_Talent.FinalCost};

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<sproc_UTS_GetTalentList_Result> talentListData = _iTalent.GetTalentList(paramasString);

                if (talentListData.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.ListingResponses(talentListData, talentListData[0].TotalRecords, talentViewModel.totalrecord, talentViewModel.pagenumber) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Talent Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("ManagedTalent/List")]
        public ObjectResult GetManagedTalentList([FromBody] ManagedTalentViewModel managedTalentViewModel)
        {
            try
            {
                #region PreValidation
                if (managedTalentViewModel == null || (managedTalentViewModel.pagenumber == 0 || managedTalentViewModel.totalrecord == 0))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide pagenumber and totalrecord" });
                #endregion

                string Sortdatafield = "ID";
                string Sortorder = "ASC";

                managedTalentViewModel.FilterFields_ManagedTalent ??= new();

                object[] param = new object[] {
                managedTalentViewModel.pagenumber, managedTalentViewModel.totalrecord, Sortdatafield, Sortorder,
                managedTalentViewModel.FilterFields_ManagedTalent.ManagedTalentFirstName, managedTalentViewModel.FilterFields_ManagedTalent.ManagedTalentLastName,
                managedTalentViewModel.FilterFields_ManagedTalent.ManagedTalentEmailID, managedTalentViewModel.FilterFields_ManagedTalent.TalentRole,
                managedTalentViewModel.FilterFields_ManagedTalent.ManagedTalent_Level, managedTalentViewModel.FilterFields_ManagedTalent.Talent_Type,
                managedTalentViewModel.FilterFields_ManagedTalent.NRPercentage, managedTalentViewModel.FilterFields_ManagedTalent.HR_Number,
                managedTalentViewModel.FilterFields_ManagedTalent.ManagedTalentCost, managedTalentViewModel.FilterFields_ManagedTalent.HRCost,
                managedTalentViewModel.FilterFields_ManagedTalent.POC_FullName};

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<Sproc_UTS_ManagedTalent_Result> managedTalentListData = _iTalent.GetManagedTalentList(paramasString);

                if (managedTalentListData.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.ListingResponses(managedTalentListData, managedTalentListData[0].TotalRecords, managedTalentViewModel.totalrecord, managedTalentViewModel.pagenumber) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Managed Talent Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("LegalInfo/List")]
        public ObjectResult GetTalentLegalInfo([FromBody] TalentLegalInfo talentLegalInfo)
        {
            try
            {
                #region PreValidation
                if (talentLegalInfo == null || (talentLegalInfo.pagenumber == 0 || talentLegalInfo.totalrecord == 0))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide pagenumber and totalrecord" });
                #endregion

                string Sortdatafield = "ID";
                string Sortorder = "DESC";

                talentLegalInfo.FilterFields_TalentLegalInfo ??= new();

                object[] param = new object[] {
                talentLegalInfo.pagenumber, talentLegalInfo.totalrecord, Sortdatafield, Sortorder,
                talentLegalInfo.FilterFields_TalentLegalInfo.TalentId, talentLegalInfo.FilterFields_TalentLegalInfo.DocumentType,
                talentLegalInfo.FilterFields_TalentLegalInfo.DocumentName, talentLegalInfo.FilterFields_TalentLegalInfo.AgreementStatus,
                talentLegalInfo.FilterFields_TalentLegalInfo.Name, talentLegalInfo.FilterFields_TalentLegalInfo.EmailId};

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<Sproc_UTS_GetTalentLegalInfo_Result> talentListData = _iTalent.GetTalentLegalInfo(paramasString);

                if (talentListData.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.ListingResponses(talentListData, talentListData[0].TotalRecords, talentLegalInfo.totalrecord, talentLegalInfo.pagenumber) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Talent Legal Info Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
