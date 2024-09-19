using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UTSATSAPI.Helpers;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Models.ViewModels.Validators;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Controllers
{
    [ApiController]
    [Route("Incentive/")]
    public class IncentiveController : ControllerBase
    {
        #region Variables
        private readonly ICommonInterface _commonInterface;
        private readonly TalentConnectAdminDBContext _db;
        private readonly IConfiguration _configuration;
        private readonly IUniversalProcRunner _universalProcRunner;
        private readonly IIncentive _iIncentive;
        #endregion

        #region Constructors
        public IncentiveController(ICommonInterface commonInterface, IConfiguration iConfiguration, TalentConnectAdminDBContext talentConnectAdminDBContext, IUniversalProcRunner universalProcRunner, IIncentive iIncentive)
        {
            _commonInterface = commonInterface;
            _configuration = iConfiguration;
            _db = talentConnectAdminDBContext;
            _universalProcRunner = universalProcRunner;
            _iIncentive = iIncentive;
        }
        #endregion

        #region Public Methods

        #region PlacementFees
        [Authorize]
        [HttpPost]
        [Route("PlacementFees/List")]
        public ObjectResult GetPlacementFees(PlacementFeesViewModel placementFeesViewModel)
        {
            try
            {
                #region PreValidation
                if (placementFeesViewModel == null || (placementFeesViewModel.pagenumber == 0 || placementFeesViewModel.totalrecord == 0))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide pagenumber and totalrecord" });
                #endregion

                string Sortdatafield = "ID";
                string Sortorder = "desc";

                object[] param = new object[] {
                placementFeesViewModel.pagenumber, placementFeesViewModel.totalrecord, Sortdatafield, Sortorder};

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<Sproc_UTS_Get_Inc_PlacementFees_Slab_Result> placementFees = _iIncentive.GetPlacementFees(paramasString);

                if (placementFees.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.ListingResponses(placementFees, placementFees[0].TotalRecords, placementFeesViewModel.totalrecord, placementFeesViewModel.pagenumber) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Placement fees available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("PlacementFees/Save")]
        public ObjectResult UpdatePlacementFees(UpdatePlacementFeesModel PlacementFees)
        {
            try
            {
                #region PreValidation
                if (PlacementFees == null)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide proper data" });
                #endregion

                #region Validation
                PlacementFeesValidator validationRules = new PlacementFeesValidator();
                ValidationResult validationResult = validationRules.Validate(PlacementFees);
                if (!validationResult.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult.Errors, "Placement Fees error") });
                }
                #endregion

                object[] param = new object[]
                {
                    PlacementFees.ID,
                    PlacementFees.SalesConsultant,
                    PlacementFees.PODManagers,
                    PlacementFees.BDR,
                    PlacementFees.BDR_Lead,
                    PlacementFees.BDRManager_Head,
                    PlacementFees.MarketingTeam,
                    PlacementFees.MarketingLead,
                    PlacementFees.MarketingHead,
                    PlacementFees.AM,
                    PlacementFees.AMHead
                };

                _universalProcRunner.Manipulation(Constants.ProcConstant.Sproc_Update_Inc_PlacementFees_Slab, param);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Successfully updated Placement fees" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region AMNRSlab

        [Authorize]
        [HttpPost]
        [Route("AMNRSlab/List")]
        public ObjectResult Get_AM_NRSlab(AM_NR_Slab aM_NR_Slab)
        {
            try
            {
                #region PreValidation
                if (aM_NR_Slab == null || (aM_NR_Slab.pagenumber == 0 || aM_NR_Slab.totalrecord == 0))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide pagenumber and totalrecord" });
                #endregion

                string Sortdatafield = "ID";
                string Sortorder = "asc";

                object[] param = new object[] {
                aM_NR_Slab.pagenumber, aM_NR_Slab.totalrecord, Sortdatafield, Sortorder};

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<Sproc_UTS_Get_Inc_AM_NRSlab_Result> nrSlabList = _iIncentive.Get_AM_NRSlab(paramasString);

                if (nrSlabList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.ListingResponses(nrSlabList, nrSlabList[0].TotalRecords, aM_NR_Slab.totalrecord, aM_NR_Slab.pagenumber) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "AM NR Slab not available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("AMNRSlab/Save")]
        public ObjectResult Update_AMNRSlab(UpdateAMSlab AMNRSlab)
        {
            try
            {
                #region PreValidation
                if (AMNRSlab == null)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide proper data" });
                #endregion

                #region Validation
                AMSlabValidator validationRules = new AMSlabValidator();
                ValidationResult validationResult = validationRules.Validate(AMNRSlab);
                if (!validationResult.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult.Errors, "AMNRSlab error") });
                }
                #endregion

                if (AMNRSlab.AM != null)
                {
                    GenIncAmNrUserRoleDetail AMUserRole = _db.GenIncAmNrUserRoleDetails.Where(x => x.ContractSlabId == AMNRSlab.ID && x.UserRoleId == 9).FirstOrDefault();
                    AMUserRole.IncPercentage = AMNRSlab.AM;
                    _db.Entry(AMUserRole).State = EntityState.Modified;
                    _db.SaveChanges();
                }

                if (AMNRSlab.AMHead != null)
                {
                    GenIncAmNrUserRoleDetail AmHeadUserRole = _db.GenIncAmNrUserRoleDetails.Where(x => x.ContractSlabId == AMNRSlab.ID && x.UserRoleId == 10).FirstOrDefault();
                    AmHeadUserRole.IncPercentage = AMNRSlab.AMHead;
                    _db.Entry(AmHeadUserRole).State = EntityState.Modified;
                    _db.SaveChanges();
                }

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Successfully updated AM NR Slab" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Contracts
        [Authorize]
        [HttpPost]
        [Route("Contracts/List")]
        public ObjectResult GetContracts(CommonFilterModel filter)
        {
            try
            {
                object[] param = new object[]
                {
                    filter.PageIndex > 0 ? filter.PageIndex : 1,
                    filter.PageSize > 0 ? filter.PageSize : 50,
                    string.IsNullOrEmpty(filter.SortExpression) ? "ID" : filter.SortExpression,
                    string.IsNullOrEmpty(filter.SortDirection) ? "asc" : filter.SortDirection
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<Sproc_Get_Inc_Contracts_Result> Contracts = _iIncentive.GetContracts(paramasString);

                if (Contracts.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = Contracts });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Contracts Slab available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("Contracts/Save")]
        public ObjectResult UpdateContracts(Update_ContractModel Contract)
        {
            try
            {
                #region PreValidation
                if (Contract == null)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide proper data" });
                #endregion

                #region Validation
                Contract_Validator validationRules = new Contract_Validator();
                ValidationResult validationResult = validationRules.Validate(Contract);
                if (!validationResult.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult.Errors, "Contract error") });
                }
                #endregion

                object[] param = new object[]
                {
                    Contract.ID,
                    Contract.SalesConsultant,
                    Contract.PODManagers,
                    Contract.BDR_USD,
                    Contract.BDRLead_USD,
                    Contract.BDRManagerHead_USD,
                    Contract.MarketingTeam,
                    Contract.MarketingLead,
                    Contract.MarketingHead,
                    Contract.AM,
                    Contract.AMHead
                };

                _universalProcRunner.Manipulation(Constants.ProcConstant.Sproc_Update_Inc_Contracts, param);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Successfully Updated Contract" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region AM Talent Deployment

        [Authorize]
        [HttpPost]
        [Route("AMTalentDeployment/List")]
        public ObjectResult GetAMTalentDeployment(CommonFilterModel filter)
        {
            try
            {
                object[] param = new object[]
                {
                    filter.PageIndex > 0 ? filter.PageIndex : 1,
                    filter.PageSize > 0 ? filter.PageSize : 50,
                    string.IsNullOrEmpty(filter.SortExpression) ? "ID" : filter.SortExpression,
                    string.IsNullOrEmpty(filter.SortDirection) ? "asc" : filter.SortDirection
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<Sproc_Get_Inc_TalentDeploySlab_Result> TalentDeploySlab = _iIncentive.GetAMTalentDeployment(paramasString);

                if (TalentDeploySlab.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = TalentDeploySlab });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Contracts Slab available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("AMTalentDeployment/Save")]
        public ObjectResult UpdateAMTalentDeployment(UpdateAMSlab AMSlab)
        {
            try
            {
                #region PreValidation
                if (AMSlab == null)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide proper data" });
                #endregion

                #region Validation
                AMSlabValidator validationRules = new AMSlabValidator();
                ValidationResult validationResult = validationRules.Validate(AMSlab);
                if (!validationResult.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult.Errors, "AM Talent Deployment error") });
                }
                #endregion

                if (AMSlab.AM != null)
                {
                    GenIncAmTalentDeployUserRoleDetail AMUserRole = _db.GenIncAmTalentDeployUserRoleDetails.Where(x => x.SlabId == AMSlab.ID && x.UserRoleId == 9).FirstOrDefault();
                    AMUserRole.IncPercentage = AMSlab.AM;
                    _db.Entry(AMUserRole).State = EntityState.Modified;
                    _db.SaveChanges();
                }

                if (AMSlab.AMHead != null)
                {
                    GenIncAmTalentDeployUserRoleDetail AmHeadUserRole = _db.GenIncAmTalentDeployUserRoleDetails.Where(x => x.SlabId == AMSlab.ID && x.UserRoleId == 10).FirstOrDefault();
                    AmHeadUserRole.IncPercentage = AMSlab.AMHead;
                    _db.Entry(AmHeadUserRole).State = EntityState.Modified;
                    _db.SaveChanges();
                }

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Successfully updated AM Talent Deployment" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Sal Goals

        [Authorize]
        [HttpPost]
        [Route("SALGoal/List")]
        public ObjectResult GetSALGoal(CommonFilterModel filter)
        {
            try
            {
                object[] param = new object[]
                {
                    filter.PageIndex > 0 ? filter.PageIndex : 1,
                    filter.PageSize > 0 ? filter.PageSize : 50,
                    string.IsNullOrEmpty(filter.SortExpression) ? "ID" : filter.SortExpression,
                    string.IsNullOrEmpty(filter.SortDirection) ? "asc" : filter.SortDirection
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<Sproc_Get_Inc_SALGoal_Result> SALGoalList = _iIncentive.GetSALGoal(paramasString);

                SalGoal salGoal = new SalGoal();

                if (SALGoalList.Any())
                {
                    salGoal.SalGoals = new List<SalGoal_INR>();
                    salGoal.FirstClosure = new List<SalGoal_USD_FirstClosure>();

                    foreach (Sproc_Get_Inc_SALGoal_Result item in SALGoalList)
                    {
                        SalGoal_INR _INR = new SalGoal_INR();
                        _INR.ID = item.ID;
                        _INR.Slab = item.SALGoal;
                        _INR.BDR_INR = item.BDR_INR;
                        _INR.BDRLead_INR = item.BDR_LeadINR;
                        salGoal.SalGoals.Add(_INR);

                        SalGoal_USD_FirstClosure _USD = new SalGoal_USD_FirstClosure();
                        _USD.ID= item.ID;
                        _USD.Slab= item.SALGoal;
                        _USD.SalesConsultant_USD = item.SalesConsultant;
                        _USD.PODManagers_USD= item.PODManagers;
                        _USD.BDR_USD = item.BDR_USD;
                        _USD.BDRLead_USD = item.BDR_LeadUSD;
                        _USD.BDRManagerHead_USD = item.BDRManagerHead_USD;
                        _USD.MarketingTeam_USD = item.MarketingTeam;
                        _USD.MarketingLead_USD = item.MarketingLead;
                        _USD.MarketingHead_USD = item.MarketingHead;
                        _USD.AM_USD = item.AM;
                        _USD.AMHead_USD = item.AMHead;
                        salGoal.FirstClosure.Add(_USD);
                    }
                }

                if (salGoal != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = salGoal });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Sal Goal available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("SALGoal/Save")]
        public ObjectResult UpdateSALGoal(SalGoal_INR salGoal_INR)
        {
            try
            {
                #region PreValidation
                if (salGoal_INR == null)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide proper data" });
                #endregion

                #region Validation
                SalGoalValidator validationRules = new SalGoalValidator();
                ValidationResult validationResult = validationRules.Validate(salGoal_INR);
                if (!validationResult.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult.Errors, "Sal Goal error") });
                }
                #endregion

                if (salGoal_INR.BDR_INR != null)
                {
                    GenIncBaseSalUserRoleDetail BDRUserRole = _db.GenIncBaseSalUserRoleDetails.Where(x => x.SalgoalSlabId == salGoal_INR.ID && x.UserRoleId == 3).FirstOrDefault();
                    BDRUserRole.IncAmountInr = salGoal_INR.BDR_INR;
                    _db.Entry(BDRUserRole).State = EntityState.Modified;
                    _db.SaveChanges();
                }

                if (salGoal_INR.BDRLead_INR != null)
                {
                    GenIncBaseSalUserRoleDetail BDRLeadUserRole = _db.GenIncBaseSalUserRoleDetails.Where(x => x.SalgoalSlabId == salGoal_INR.ID && x.UserRoleId == 4).FirstOrDefault();
                    BDRLeadUserRole.IncAmountInr = salGoal_INR.BDRLead_INR;
                    _db.Entry(BDRLeadUserRole).State = EntityState.Modified;
                    _db.SaveChanges();
                }

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Successfully Updated Sal Goal" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("SALGoal/FirstClosure/Save")]
        public ObjectResult UpdateFirstClosure(SalGoal_USD_FirstClosure salGoal_USD)
        {
            try
            {
                #region PreValidation
                if (salGoal_USD == null)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide proper data" });
                #endregion

                #region Validation
                SalGoal_FirstClosureValidator validationRules = new SalGoal_FirstClosureValidator();
                ValidationResult validationResult = validationRules.Validate(salGoal_USD);
                if (!validationResult.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult.Errors, "Sal Goal error") });
                }
                #endregion

                object[] param = new object[]
                {
                    salGoal_USD.ID,
                    salGoal_USD.SalesConsultant_USD,
                    salGoal_USD.PODManagers_USD,
                    salGoal_USD.BDR_USD,
                    salGoal_USD.BDRLead_USD,
                    salGoal_USD.BDRManagerHead_USD,
                    salGoal_USD.MarketingTeam_USD,
                    salGoal_USD.MarketingLead_USD,
                    salGoal_USD.MarketingHead_USD,
                    salGoal_USD.AM_USD,
                    salGoal_USD.AMHead_USD
                };

                _universalProcRunner.Manipulation(Constants.ProcConstant.Sproc_Update_Inc_SALGoal, param);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Successfully Updated Sal Goal" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        [Authorize]
        [HttpPost]
        [Route("CalculateIncentive")]
        public ObjectResult GetCalculateIncentive([FromBody] AchivedTargetViewModel achivedTargetViewModel)
        {
            try
            {
                #region PreValidation
                if (achivedTargetViewModel == null || (achivedTargetViewModel.pagenumber == 0 || achivedTargetViewModel.totalrecord == 0))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide pagenumber and totalrecord" });
                #endregion

                string Sortdatafield = "HRID";
                string Sortorder = "asc";

                object[] param = new object[] {
                achivedTargetViewModel.pagenumber, achivedTargetViewModel.totalrecord, Sortdatafield, Sortorder , achivedTargetViewModel.achivedTargetFilter.HRNumber , achivedTargetViewModel.achivedTargetFilter.Client , achivedTargetViewModel.achivedTargetFilter.EngagemenID, achivedTargetViewModel.achivedTargetFilter.TalentName, achivedTargetViewModel.achivedTargetFilter.User_Role, achivedTargetViewModel.achivedTargetFilter.InvoiceNumber, achivedTargetViewModel.achivedTargetFilter.CompanyCategory, achivedTargetViewModel.achivedTargetFilter.month , achivedTargetViewModel.achivedTargetFilter.year };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<Sproc_UTS_Get_Acheived_Target_Details_Result> incentiveAchivedTargetListData = _iIncentive.Get_Acheived_Target_Details(paramasString);

                if (incentiveAchivedTargetListData.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.ListingResponses(incentiveAchivedTargetListData, incentiveAchivedTargetListData[0].TotalRecords, achivedTargetViewModel.totalrecord, achivedTargetViewModel.pagenumber) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Achived target available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
