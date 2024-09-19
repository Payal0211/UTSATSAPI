using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using UTSATSAPI.Helpers;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Models.ViewModels.Validators;
using UTSATSAPI.Repositories.Interfaces;
using static UTSATSAPI.Helpers.Enum;

namespace UTSATSAPI.Controllers
{
    [Authorize]
    [Route("AMAssignment/", Name = "AMAssignment")]
    [ApiController]
    public class AMAssignmentController : ControllerBase
    {
        #region Variables
        private readonly ICommonInterface commonInterface;
        private readonly TalentConnectAdminDBContext db;
        private readonly IUniversalProcRunner _universalProcRunner;
        private readonly IConfiguration _configuration;
        private readonly IAMAssignment _iAMAssignment;

        #endregion

        #region Constructors
        public AMAssignmentController(ICommonInterface _commonInterface, IConfiguration configuration, IUniversalProcRunner universalProcRunner, TalentConnectAdminDBContext talentConnectAdminDBContext, IAMAssignment iAMAssignment)
        {
            commonInterface = _commonInterface;
            db = talentConnectAdminDBContext;
            _universalProcRunner = universalProcRunner;
            _configuration = configuration;
            _iAMAssignment = iAMAssignment;
        }
        #endregion

        #region Public APIs

        [Authorize]
        [HttpPost]
        [Route("List")]
        public ObjectResult GetAMAssignmentList([FromBody] AMAssignmentsViewModel aMAssignmentsViewModel)
        {
            try
            {
                #region PreValidation
                if (aMAssignmentsViewModel == null || (aMAssignmentsViewModel.pagenumber == 0 || aMAssignmentsViewModel.totalrecord == 0))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide pagenumber and totalrecord" });
                #endregion

                string Sortdatafield = "ID";
                string Sortorder = "DESC";

                aMAssignmentsViewModel.FilterFields_AMAssignments ??= new();

                object[] param = new object[] {
                aMAssignmentsViewModel.pagenumber, aMAssignmentsViewModel.totalrecord, Sortdatafield, Sortorder,
                aMAssignmentsViewModel.FilterFields_AMAssignments.GEOName, aMAssignmentsViewModel.FilterFields_AMAssignments.UserName,
                aMAssignmentsViewModel.FilterFields_AMAssignments.Priority};

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<sproc_UTS_GetAMAssignments_Result> aMAssignmentListData = _iAMAssignment.GetAMAssignmentList(paramasString);

                if (aMAssignmentListData.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.ListingResponses(aMAssignmentListData, aMAssignmentListData[0].TotalRecords, aMAssignmentsViewModel.totalrecord, aMAssignmentsViewModel.pagenumber) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No AMAssignment Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("AMDataSend")]
        public ObjectResult AMDataSend(long Onboard_ID, string EngagemenID)
        {
            try
            {
                var LoggedInUserId = SessionValues.LoginUserId;
                GenSalesHiringRequest? _SalesHiringRequest = null;

                string returnMessage = string.Empty;

                long HiringRequest_Id = 0;
                long TalentID = 0;

                #region PreValidation
                if (Onboard_ID == 0)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "please provide onboard ID." });
                else
                {
                    GenOnBoardTalent _OnBoardTalents = db.GenOnBoardTalents.Where(x => x.Id == Onboard_ID && x.EngagemenId == EngagemenID).FirstOrDefault();
                    if (_OnBoardTalents == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Onboard Talent not exists." });
                    }
                    else
                    {
                        TalentID = (long)_OnBoardTalents.TalentId;
                        HiringRequest_Id = (long)_OnBoardTalents.HiringRequestId;
                    }
                }
                #endregion

                if (!string.IsNullOrEmpty(EngagemenID) && Onboard_ID > 0)
                {
                    long? userid = GetUserId(Onboard_ID);

                    if (userid.HasValue)
                    {
                        if (HiringRequest_Id > 0)
                        {
                            var AMData_Json = GetAllAMDetails(HiringRequest_Id, Onboard_ID);
                            string AMJsonData = Convert.ToString(AMData_Json);
                            if (!string.IsNullOrEmpty(AMJsonData))
                            {
                                bool isAPIResponseSuccess = true;
                                string InvoiceApp_Response = "";
                                string ResponseMessage = "";

                                GenEsalesAmApiResponse gen_Esales_AM = new GenEsalesAmApiResponse();
                                gen_Esales_AM.OnBoardId = Onboard_ID;
                                gen_Esales_AM.HiringRequestId = HiringRequest_Id;
                                gen_Esales_AM.CreatedByDateTime = DateTime.Now;
                                gen_Esales_AM.RequestPayload = AMJsonData;
                                gen_Esales_AM.EsaleSalesPersonId = "";
                                db.GenEsalesAmApiResponses.Add(gen_Esales_AM);
                                db.SaveChanges();

                                EsalesAMDataResponse esalesAMDataResponse = new EsalesAMDataResponse();
                                if (AMJsonData != "")
                                {
                                    esalesAMDataResponse = GetUTSDetailsAndSendAssignedAMDetails(AMJsonData, Onboard_ID);
                                }
                                if (esalesAMDataResponse.Status == 0)
                                {
                                    isAPIResponseSuccess = false;
                                    ResponseMessage = esalesAMDataResponse.Message;
                                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = ResponseMessage });
                                }
                                else
                                {
                                    object[] param = new object[] { esalesAMDataResponse.ResponseEsalesClientAM.HR_Number, esalesAMDataResponse.ResponseEsalesClientAM.EngagemenID, esalesAMDataResponse.ResponseEsalesClientAM.SalesPerson_EmployeeID, LoggedInUserId };
                                    string paramasString = CommonLogic.ConvertToParamString(param);

                                    _iAMAssignment.sproc_UTS_Update_EmployeeID_FromInvoiceAPIResponse(paramasString);
                                    
                                    returnMessage = $"{esalesAMDataResponse.ResponseEsalesClientAM.AMName} assigned as an AM for {esalesAMDataResponse.ResponseEsalesClientAM.HR_Number}";

                                    #region Insert HR History 
                                    param = new object[]
                                    {
                                        Action_Of_History.AM_Assignment, HiringRequest_Id, TalentID, false, LoggedInUserId, 0, 0, "", Onboard_ID, false, false, 0, 0, (short)AppActionDoneBy.UTS
                                    };
                                    _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param);
                                    #endregion

                                    #region SendEmail

                                    EmailBinder binder = new EmailBinder(_configuration, db);
                                    binder.SendEmailForAMAssignment(HiringRequest_Id, TalentID);

                                    bool SendEmailToAM = binder.SendAMAssignmentEmailToAM(HiringRequest_Id, TalentID);
                                    bool SendEmailToNBD = binder.SendAMAssignmentEmailToNBD(HiringRequest_Id, TalentID);

                                    if (SendEmailToAM == true && SendEmailToNBD == true)
                                    {
                                        GenAmnbdAssignmentEmailsendDetail AMNBD_Assignment_EmailsendDetails = db.GenAmnbdAssignmentEmailsendDetails.Where(x => x.HiringRequestId == HiringRequest_Id && x.NeedEmailSend == 1).FirstOrDefault();
                                        if (AMNBD_Assignment_EmailsendDetails != null)
                                        {
                                            AMNBD_Assignment_EmailsendDetails.NeedEmailSend = 0;
                                            db.Entry(AMNBD_Assignment_EmailsendDetails).State = EntityState.Modified;
                                            db.SaveChanges();
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = returnMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject() { statusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        #region AM Team Realted API
        [Authorize]
        [HttpGet]
        [Route("AddAMTeam")]
        public async Task<ObjectResult> AddAMTeam(string Type)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Type))
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide Type" });
                }
                AMTeam model = new AMTeam();
                model.Type = Type;
                model.AMList = await _iAMAssignment.GetAMUser();
                model.GEOList = db.PrgGeos.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Geo
                });
                if (model != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = model });
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Data not found." });
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [Authorize]
        [HttpGet("EditAMTeam")]
        public async Task<ObjectResult> EditAMTeam(long genTeamDistributionId)
        {
            if (genTeamDistributionId == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide gen Team distribution Id" });
            }
            AMTeam model = new AMTeam();
            GenTeamDistribution genTeamDistribution = new GenTeamDistribution();
            genTeamDistribution = db.GenTeamDistributions.FirstOrDefault(xy => xy.Id == genTeamDistributionId);
            if (genTeamDistribution != null)
            {
                model.Id = genTeamDistribution.Id;
                model.AMId = genTeamDistribution.UserId;
                model.GEOID = genTeamDistribution.Geoid;
                model.SortNo = genTeamDistribution.SortNo;
            }
            model.AMList = await _iAMAssignment.GetAMUser();
            model.GEOList = db.PrgGeos.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Geo
            });
            if (model != null)
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = model });
            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Data not found." });
        }

        [Authorize]
        [HttpPost("AddAMTeam")]
        public async Task<ObjectResult> AddAMTeam(AMTeam model)
        {

            try
            {
                #region PreValidation
                if (model == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please Provide the data to save operation." });
                }

                AddAMTeamValidator validations = new AddAMTeamValidator();
                ValidationResult validationUserResult = validations.Validate(model);
                if (!validationUserResult.IsValid)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationUserResult.Errors, "AMTeam") });

                #endregion

                long LoggedInUserId = SessionValues.LoginUserId;

                if (db.GenTeamDistributions.Any(g => g.Geoid == model.GEOID && g.SortNo == model.SortNo && g.Id != model.Id && g.IsDeleted == false))
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "You cannot assign same priority for Same GEO." });
                }

                if (model.Id > 0)
                {
                    if (db.GenTeamDistributions.Any(g => g.Geoid == model.GEOID && g.UserId == model.AMId && g.Id != model.Id && g.IsDeleted == false))
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "AM GEO Details Already Exists." });
                    }

                    GenTeamDistribution genTeamDistribution = new GenTeamDistribution();
                    genTeamDistribution = db.GenTeamDistributions.Where(x => x.Id == model.Id).FirstOrDefault();
                    if (genTeamDistribution != null)
                    {
                        genTeamDistribution.UserId = model.AMId;
                        genTeamDistribution.FlagType = string.IsNullOrWhiteSpace(model.Flag) ? string.Empty : model.Flag;
                        genTeamDistribution.Geoid = model.GEOID;
                        genTeamDistribution.SortNo = model.SortNo;
                        genTeamDistribution.IsActive = true;
                        genTeamDistribution.IsDeleted = false;
                        genTeamDistribution.ModifiedById = Convert.ToInt32(LoggedInUserId);
                        genTeamDistribution.ModifiedDateTime = DateTime.Now;
                        db.Entry(genTeamDistribution).State = EntityState.Modified;
                        db.SaveChanges();

                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Data Updated Successfully" });
                    }
                }

                if (model.AMId > 0 && model.GEOID > 0)
                {
                    GenTeamDistribution genTeamDistribution = new GenTeamDistribution();

                    var genTeamDistributionDataExists = db.GenTeamDistributions.Where(x => x.UserId == model.AMId && x.Geoid == model.GEOID).FirstOrDefault();
                    if (genTeamDistributionDataExists == null)
                    {
                        genTeamDistribution.UserId = model.AMId;
                        genTeamDistribution.FlagType = string.IsNullOrWhiteSpace(model.Flag) ? string.Empty : model.Flag;
                        genTeamDistribution.Geoid = model.GEOID;
                        genTeamDistribution.SortNo = model.SortNo;
                        genTeamDistribution.IsActive = true;
                        genTeamDistribution.IsDeleted = false;
                        genTeamDistribution.CreatedById = Convert.ToInt32(LoggedInUserId);
                        genTeamDistribution.CreatedDateTime = DateTime.Now;
                        db.GenTeamDistributions.Add(genTeamDistribution);
                        db.SaveChanges();
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Data save Successfully" });
                    }
                    else
                    {
                        if (genTeamDistributionDataExists.IsDeleted == false)
                        {
                            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "AM GEO Details Already Exists." });
                        }
                        else
                        {
                            genTeamDistributionDataExists.IsDeleted = false;
                            genTeamDistributionDataExists.ModifiedById = Convert.ToInt32(LoggedInUserId);
                            genTeamDistributionDataExists.ModifiedDateTime = DateTime.Now;
                            genTeamDistributionDataExists.SortNo = model.SortNo;
                            db.Entry(genTeamDistributionDataExists).State = EntityState.Modified;
                            db.SaveChanges();
                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Data save Successfully" });
                        }
                    }
                }
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Data not found." });
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        [Authorize]
        [HttpGet("ChangeAssignmentTeamDistributionPriority")]
        public async Task<ObjectResult> ChangeAssignmentTeamDistributionPriority(int id, int priority)
        {
            try
            {
                if (id <= 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide Id." });
                }
                if (priority <= 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provid priority value." });
                }
                int LoginUserId = Convert.ToInt32(SessionValues.LoginUserId);
                bool result = await _iAMAssignment.ChangeAssignmentTeamDistributionPriority(id, priority, LoginUserId);
                if (result)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Priority Updated", Details = result });
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "You cannot assign same priority for Same GEO.", Details = result });
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [Authorize]
        [HttpGet("GetGEO")]
        public ObjectResult GetGEO(int UserId)
        {
            try
            {
                if (UserId <= 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide User Id." });
                }

                var user = db.UsrUsers.Where(x => x.Id == UserId).FirstOrDefault();
                if (user != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = user.GeoId.HasValue ? user.GeoId : 0 });
                }
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "User not found" });
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [Authorize]
        [HttpGet("GetSortNo")]
        public ObjectResult GetSortNo(int geoId, string type)
        {
            try
            {
                if (geoId <= 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide geo id." });
                }
                if (string.IsNullOrEmpty(type))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide type value." });
                }
                int SortNo = db.GenTeamDistributions.Where(x => x.Geoid == geoId && x.FlagType == type && x.IsDeleted == false).Select(p => p.SortNo).AsEnumerable().DefaultIfEmpty(0).Max() + 1;

                if (SortNo > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = SortNo });
                }


                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Error" });
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [Authorize]
        [HttpGet("DeleteAMTeam")]
        public ObjectResult DeleteAMTeam(long id)
        {
            try
            {
                if (id <= 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide geo id." });
                }
                AMTeamDetails model = new AMTeamDetails();
                var Details = (from T in db.GenTeamDistributions
                               join U in db.UsrUsers on T.UserId equals U.Id
                               join G in db.PrgGeos on T.Geoid equals G.Id
                               where T.Id == id
                               select new { U.FullName, G.Geo, T.FlagType, G.Id }).FirstOrDefault();
                if (Details != null)
                {
                    model.AmName = Details.FullName;
                    model.GEO = Details.Geo;
                    model.Type = Details.FlagType;
                    model.Id = id;
                    model.GEOId = Details.Id;
                }
                if (model != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = model });
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No records found" });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("DeleteAMTeamDetails")]
        public ObjectResult DeleteAMTeamDetails(long id)
        {
            try
            {
                if (id <= 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide geo id." });
                }
                int LoggedInUserId = Convert.ToInt32(SessionValues.LoginUserId);
                GenTeamDistribution genTeamDistribution = new GenTeamDistribution();
                genTeamDistribution = db.GenTeamDistributions.FirstOrDefault(xy => xy.Id == id);
                if (genTeamDistribution == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Data not found" });
                }
                genTeamDistribution.ModifiedById = LoggedInUserId;
                genTeamDistribution.ModifiedDateTime = DateTime.Now;
                genTeamDistribution.IsDeleted = true;
                db.Entry(genTeamDistribution).State = EntityState.Modified;
                db.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "AM deleted Successfully" });
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #endregion

        #region Private methods
        [NonAction]
        private long? GetUserId(long Onboard_ID)
        {
            var onboardobject = db.GenOnBoardTalents.FirstOrDefault(x => x.Id == Onboard_ID);
            if (onboardobject != null)
            {
                long? HR_Id = onboardobject.HiringRequestId;
                var saleshiringrequest = db.GenSalesHiringRequests.Where(x => x.Id == HR_Id.Value).FirstOrDefault();
                if (saleshiringrequest != null)
                    return saleshiringrequest.SalesUserId;
            }
            return 0;
        }
        [NonAction]
        private string GetAllAMDetails(long HiringRequest_ID, long OnBoard_ID)
        {
            string AMDetails = "";

            if (HiringRequest_ID == 0)
            {
                object[] param = new object[] { null, null };
                string paramasString = CommonLogic.ConvertToParamString(param);

                var table = _iAMAssignment.SP_UTS_ESales_Get_Client_AM_Details(paramasString);

                if (table == null)
                {
                    return "";
                }

                AMDetails = JsonConvert.SerializeObject(table);

            }
            else
            {
                object[] param = new object[] { HiringRequest_ID, OnBoard_ID };
                string paramasString = CommonLogic.ConvertToParamString(param);

                var table = _iAMAssignment.SP_UTS_ESales_Get_Client_AM_Details(paramasString);

                if (table == null)
                {
                    return "";
                }

                //HRData = JsonConvert.SerializeObject(table);

                EsaleAMDetails esaleAMDetails = new EsaleAMDetails
                {
                    EsalesClientAM = table
                };

                var AllClientsForCompany = (from g in db.GenCompanies
                                            join c in db.GenContacts
                                            on g.Id equals c.CompanyId
                                            where g.Company.ToLower().Trim() == table.CompanyName.ToLower().Trim()
                                            select c).ToList();

                foreach (var cd in AllClientsForCompany)
                {
                    ClientDetail clientDetail = new ClientDetail
                    {
                        FirstName = cd.FirstName,
                        LastName = cd.LastName,
                        EmailID = cd.EmailId,
                        IsPrimary = cd.IsPrimary,
                        city = cd.City,
                        skype = "",//cd.Skype,
                        regions = cd.Regions
                    };

                    esaleAMDetails.ClientDetails.Add(clientDetail);
                }

                return JsonConvert.SerializeObject(esaleAMDetails);
            }

            return AMDetails;
        }
        [NonAction]
        private EsalesAMDataResponse GetUTSDetailsAndSendAssignedAMDetails(string strDetailsJSON,long Onboard_ID)
        {
            UTSDetailsVM utsDetailsVM = JsonConvert.DeserializeObject<UTSDetailsVM>(strDetailsJSON);

            long historyId = AddUTS_AMAssignedHistory(utsDetailsVM);

            long? CompanyID = 0;
            //bool _isAssignmentRuleApply = db.SystemSettings.Where(x => x.SettingCode == "IARA" && x.IsActive == true && x.IsDeleted == false).Select(x => x.BooleanValue).FirstOrDefault();

            bool _isAssignmentRuleApply = true;

            var jsonResponse = new EsalesAMDataResponse
            {
                Status = 0,
                Message = "",
                ResponseEsalesClientAM = new ResponseEsalesClientAM()
            };

            if (!_isAssignmentRuleApply)
            {
                jsonResponse = new EsalesAMDataResponse
                {
                    Status = 0,
                    Message = "AM Assignment Rule Feature have been tuned off, Then please turn on the feature setting to get the AM Assignment for UTS!!",
                    ResponseEsalesClientAM = new ResponseEsalesClientAM()
                };
                UpdateUTS_AMAssignedHistory(historyId, CompanyID, utsDetailsVM, jsonResponse, "", "");
                return jsonResponse;
            }
            //check if value is null , then return null
            if (utsDetailsVM == null && utsDetailsVM.esalesClientAM == null && utsDetailsVM.ClientDetails == null && utsDetailsVM.ClientDetails.Count() > 0)
            {
                jsonResponse = new EsalesAMDataResponse
                {
                    Status = 0,
                    Message = "Request json Object value can't be null",
                    ResponseEsalesClientAM = new ResponseEsalesClientAM()
                };
                UpdateUTS_AMAssignedHistory(historyId, CompanyID, utsDetailsVM, jsonResponse, "", "");
                return jsonResponse;
            }

            EsalesClientAM clientAm = utsDetailsVM.esalesClientAM;
            List<ClientDetailsVM> ClientDetails = utsDetailsVM.ClientDetails;
            //List<ClientDetailsVM> lstClientDetails = ClientDetails.Where(x => x.IsPrimary.ToLower() != "true").ToList();
            //ClientDetailsVM primaryCLient = ClientDetails.Where(x => x.IsPrimary.ToLower() == "true").FirstOrDefault();

            string ClientEmailId = "";
            //if (string.IsNullOrEmpty(ClientEmailId)) ClientEmailId = ClientDetails.Where(x => x.EmailID != null).Select(x => x.EmailID).FirstOrDefault();

            //CompanyID = db.GenContacts.Where(C => C.EmailId == ClientEmailId).Select(C => C.CompanyId).FirstOrDefault();

            var objOnBoard = db.GenOnBoardTalents.Where(x => x.Id == Onboard_ID).FirstOrDefault();
            if(objOnBoard != null)
            {
                long ContactID = objOnBoard.ContactId.Value;
                var objContact = db.GenContacts.Where(C => C.Id == ContactID).FirstOrDefault();
                if(objContact != null)
                {
                    ClientEmailId = objContact.EmailId;
                }                 
            }

            //Get GEOID
            long? GEO_ID = GetGEOIDByGEO(clientAm.GEO);
            string GEOHeadEmployeeID = GetGEOHeadEmplyeeIDByGEO(clientAm.GEO);
            //Get Next AM Assigned
            ResponseAMAssignment data = GetAMByAssignmentRule(clientAm, GEO_ID, CompanyID);
            if (data != null)
            {
                long SelectedAMUserID = data.AMUserID;
                string SalesPerson_EmployeeID = ""; long? NBD_LeadID = null;
                string AMName = "";
                if (data.Status == 1)
                {
                    UsrUser? salesPersonDetails = GetUserByUserID(SelectedAMUserID);
                    if(salesPersonDetails != null)
                    {
                        SalesPerson_EmployeeID = salesPersonDetails.EmployeeId ?? "";
                        AMName = salesPersonDetails.FullName ?? "";
                    }

                    NBD_LeadID = GetUserIDByUserIDEmployee(clientAm.NBD_Lead_EmployeeID);
                    AddClientAMAssignment(CompanyID.Value, clientAm, SelectedAMUserID, GEO_ID, NBD_LeadID, data.BAU_UTS_Tagging, ClientEmailId, clientAm.TalentName);

                    GenCompany company = db.GenCompanies.Where(x => x.Id == CompanyID).FirstOrDefault();
                    if (company != null)
                    {
                        company.AmSalesPersonId = Convert.ToInt32(SelectedAMUserID);
                        db.Entry(company).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }

                ResponseEsalesClientAM response = new ResponseEsalesClientAM();

                response.SalesPerson_EmployeeID = SalesPerson_EmployeeID;

                if (utsDetailsVM != null && utsDetailsVM.esalesClientAM != null) 
                {
                    response.HR_Number = utsDetailsVM.esalesClientAM.HR_Number;
                    response.EngagemenID = utsDetailsVM.esalesClientAM.EngagemenID; 
                    response.NBD_Lead_EmployeeID = utsDetailsVM.esalesClientAM.NBD_Lead_EmployeeID;
                }

                response.GEOLocationHeadEmployeeId = GEOHeadEmployeeID;
                response.AMName = AMName;

                jsonResponse = new EsalesAMDataResponse
                {
                    Status = data.Status,
                    Message = data.Message,
                    ResponseEsalesClientAM = response
                };

                UpdateUTS_AMAssignedHistory(historyId, CompanyID, utsDetailsVM, jsonResponse, SalesPerson_EmployeeID, ClientEmailId);
                return jsonResponse;
            }
            else
            {
                jsonResponse = new EsalesAMDataResponse
                {
                    Status = 0,
                    Message = "Some Error Ocurred While getting AM Assignment for UTS",
                    ResponseEsalesClientAM = new ResponseEsalesClientAM()
                };

                UpdateUTS_AMAssignedHistory(historyId, CompanyID, utsDetailsVM, jsonResponse.ToString(), "", ClientEmailId);
                return jsonResponse;

            }
        }
        [NonAction]
        private long AddUTS_AMAssignedHistory(UTSDetailsVM Request)
        {
            if (Request != null && Request.esalesClientAM != null)
            {
                string Requested_Json = JsonConvert.SerializeObject(Request, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
                string Response_Json = "";
                GenUtsAssignmentHistory history = new GenUtsAssignmentHistory();
                history.EmailId = "";
                history.Geoid = GetGEOIDByGEO(Request.esalesClientAM.GEO);
                history.JsonRequest = Requested_Json;
                history.JsonResponse = Response_Json;
                history.NbdLeadEmployeeId = Request.esalesClientAM.NBD_Lead_EmployeeID;
                history.SalesPersonEmployeeId = "";
                history.CreatedById = (int)SessionValues.LoginUserId;
                history.CreatedDateTime = DateTime.Now;
                history.CompanyId = 0;
                db.GenUtsAssignmentHistories.Add(history);
                db.SaveChanges();

                return history.Id;
            }
            else return 0;
        }
        [NonAction]
        private void UpdateUTS_AMAssignedHistory(long historyId, long? CompanyID, UTSDetailsVM Request, object jsonresponse, string SalesPerson_EmployeeID, string ClientEmailId)
        {
            if (Request != null && Request.esalesClientAM != null)
            {
                string Requested_Json = JsonConvert.SerializeObject(Request, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                string Response_Json = "";
                if (jsonresponse != null)
                    Response_Json = JsonConvert.SerializeObject(jsonresponse, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                GenUtsAssignmentHistory history = db.GenUtsAssignmentHistories.Where(x => x.Id == historyId).FirstOrDefault();
                history.EmailId = ClientEmailId;
                history.Geoid = GetGEOIDByGEO(Request.esalesClientAM.GEO);
                history.JsonRequest = Requested_Json;
                history.JsonResponse = Response_Json;
                history.NbdLeadEmployeeId = Request.esalesClientAM.NBD_Lead_EmployeeID;
                history.SalesPersonEmployeeId = SalesPerson_EmployeeID;
                history.CreatedById = (int)SessionValues.LoginUserId;
                history.CreatedDateTime = DateTime.Now;
                history.CompanyId = CompanyID;
                db.Entry(history).State = EntityState.Modified;
                db.SaveChanges();
            }

        }
        [NonAction]
        private long? GetGEOIDByGEO(string GEO)
        {
            return db.PrgGeos.Where(x => x.Geo.ToUpper().Trim() == GEO.ToUpper().Trim()).Select(x => x.Id).FirstOrDefault();
        }
        [NonAction]
        private string GetGEOHeadEmplyeeIDByGEO(string GEO)
        {
            string GEOHeadEmployeeID = (from u in db.UsrUsers
                                        join g in db.PrgGeos on u.GeoId equals g.Id
                                        join uh in db.UsrUserHierarchies on u.Id equals uh.UserId
                                        join up in db.UsrUsers on uh.ParentId equals up.Id
                                        where g.Geo == GEO
                                        select up.EmployeeId).FirstOrDefault();
            return GEOHeadEmployeeID;
        }
        [NonAction]
        private UsrUser GetUserByUserID(long UserId)
        {
            return db.UsrUsers.Where(x => x.Id == UserId).FirstOrDefault();
        }
        [NonAction]
        private int GetUserIDByUserIDEmployee(string UserEmployeeId)
        {
            return (int)db.UsrUsers.Where(x => x.EmployeeId == UserEmployeeId).Select(x => x.Id).FirstOrDefault();
        }
        [NonAction]
        private ResponseAMAssignment GetAMByAssignmentRule(EsalesClientAM clientAm, long? GEO_ID, long? CompanyID)
        {
            ResponseAMAssignment response = new ResponseAMAssignment()
            {
                Status = 0,
                Message = "",
                AMUserID = 0,
                BAU_UTS_Tagging = ""
            };
            //return null if clientAM object or GeoID will null
            if (clientAm == null || GEO_ID == null || GEO_ID == 0)
            {
                response = new ResponseAMAssignment()
                {
                    Status = 0,
                    Message = "Geo not found for this Company!!",
                    AMUserID = 0,
                    BAU_UTS_Tagging = ""
                };
                return response;
            }

            GenTeamDistribution selectedTeam_Distribution = new GenTeamDistribution();
            string DedicatedType = "";

            //Check that Is any AM exist for GEO ID
            bool IsAmExist = db.GenTeamDistributions.Where(x => x.Geoid == GEO_ID && x.IsDeleted == false).Any();

            if (!IsAmExist)
            {
                response = new ResponseAMAssignment()
                {
                    Status = 0,
                    Message = "AM Data not exist for same GEO, Please connect with support team!!",
                    AMUserID = 0,
                    BAU_UTS_Tagging = ""
                };
                return response;
            }
            string EngagementModelName = clientAm.EngagementModel.Trim();

            int DedicatedEngagmentModel = db.PrgClientEngagementModels.Where(x => (x.ModelName == EngagementModelName)).Select(x => x.Id).FirstOrDefault();

            DedicatedType = "UTS";

            long? nextAssignedAMID = null;
            long? selectedRuleID = null;
            string _engagementID = DedicatedEngagmentModel.ToString();
            //Get selected rule
            PrgAmAssignmentRule selectedAmAssignmentRule = db.PrgAmAssignmentRules.Where(x => x.EngagementModelId == _engagementID && x.DedicatedType == DedicatedType && !x.IsDeleted.Value && x.IsActive.Value).FirstOrDefault();

            if (selectedAmAssignmentRule != null) selectedRuleID = selectedAmAssignmentRule.Id;
            else
            {
                response = new ResponseAMAssignment()
                {
                    Status = 0,
                    Message = "AM Assignment Rule not exist for EngagmentModel!!",
                    AMUserID = 0,
                    BAU_UTS_Tagging = ""
                };
                return response;
            }

            int lastsortNo = 1;
            string flagType = "UTS";

            List<GenTeamDistribution> lstTeamDisribution = db.GenTeamDistributions.Where(x => x.FlagType == flagType && x.Geoid == GEO_ID && x.IsDeleted == false).ToList();
            int maxShortNoOfFlag = 0;

            if (lstTeamDisribution != null && lstTeamDisribution.Count > 0)
                maxShortNoOfFlag = lstTeamDisribution.Max(x => x.SortNo);
            else
            {
                //If not Team available for same Flag , then check with another one
                //if (flagType == "UTS") flagType = "BAU";
                //else if(flagType == "BAU") flagType = "UTS";
                lstTeamDisribution = db.GenTeamDistributions.Where(x => x.FlagType == flagType && x.Geoid == GEO_ID && x.IsDeleted == false).ToList();

                if (lstTeamDisribution != null && lstTeamDisribution.Count > 0)
                    maxShortNoOfFlag = lstTeamDisribution.Max(x => x.SortNo);
                else
                {
                    response = new ResponseAMAssignment()
                    {
                        Status = 0,
                        Message = "AM Data not exist for same GEO for flage type " + flagType + ", Please connect with support team!!",
                        AMUserID = 0,
                        BAU_UTS_Tagging = ""
                    };
                    return response;
                }
            }

            GenAmAssignmentRuleHistory lastHistoryByFlag = db.GenAmAssignmentRuleHistories.Where(x => x.Geoid == GEO_ID && x.BauUtcTagging == flagType && (x.InvoiceId != null || x.IsCreatedFromUts == true) && x.IsDeleted == false).OrderByDescending(x => x.Id).FirstOrDefault();
            if (lastHistoryByFlag != null)
            {
                lastsortNo = db.GenTeamDistributions.Where(x => x.UserId == lastHistoryByFlag.AmuserId && x.Id == lastHistoryByFlag.TeamDistributionId && x.IsDeleted == false).Select(x => x.SortNo).FirstOrDefault();
                if (lastsortNo < maxShortNoOfFlag)
                    lastsortNo = lastsortNo + 1;
                else lastsortNo = 1;

            }
            else lastsortNo = 1;

            if (lastsortNo > maxShortNoOfFlag)
                lastsortNo = 1;
            //Get next Am User ID
            for (int i = lastsortNo; i <= maxShortNoOfFlag; i++)
            {
                selectedTeam_Distribution = db.GenTeamDistributions.Where(x => x.SortNo == i && x.FlagType == flagType && x.Geoid == GEO_ID && x.IsActive.Value && !x.IsDeleted.Value && x.IsDeleted == false).FirstOrDefault();
                if (selectedTeam_Distribution != null)
                { break; }
            }

            if (selectedTeam_Distribution == null)
            {
                response = new ResponseAMAssignment()
                {
                    Status = 0,
                    Message = "Team data not exist for EngagmentModel and GEO!!",
                    AMUserID = 0,
                    BAU_UTS_Tagging = ""
                };
                return response;
            }

            UsrUser selectedAMUser = db.UsrUsers.Where(x => x.Id == selectedTeam_Distribution.UserId).FirstOrDefault();

            if (selectedAMUser != null)
            {
                response = new ResponseAMAssignment()
                {
                    Status = 1,
                    Message = "",
                    AMUserID = selectedAMUser.Id,
                    BAU_UTS_Tagging = selectedTeam_Distribution.FlagType
                };
                //Save selected rule details in database
                AddAssignedRuleHistory(CompanyID, clientAm.HR_Number, clientAm.EngagemenID, selectedRuleID, selectedAmAssignmentRule.EngagementModelId, selectedTeam_Distribution.UserId, selectedTeam_Distribution.FlagType, selectedTeam_Distribution.Geoid, selectedAmAssignmentRule.Priority, clientAm.CreatedByEmployeeId, selectedTeam_Distribution.Id);

                return response;
            }
            else
            {
                response = new ResponseAMAssignment()
                {
                    Status = 0,
                    Message = " No Am User exist for EngagmentModel and GEO!!",
                    AMUserID = 0,
                    BAU_UTS_Tagging = ""
                };
                return response;
            }

        }
        [NonAction]
        private void AddAssignedRuleHistory(long? CompanyID, string HR_Number, string EngagemenID, long? AMAssignmentRuleID, string AMAssignmentRuleEngagementModelID, long AMUserID, string BAU_UTC_Tagging, long GEOID, int Priority, string CreatedByEmployeeId, long TeamDistributionId)
        {
            GenAmAssignmentRuleHistory history = new GenAmAssignmentRuleHistory();
            history.AmassignmentRuleId = AMAssignmentRuleID.HasValue ? AMAssignmentRuleID.Value : 0;
            history.AmassignmentRuleEngagementModelId = AMAssignmentRuleEngagementModelID != null ? AMAssignmentRuleEngagementModelID : "";
            history.AmuserId = AMUserID;
            history.BauUtcTagging = BAU_UTC_Tagging != null ? BAU_UTC_Tagging : "";
            history.CreatedById = GetUserIDByUserIDEmployee(CreatedByEmployeeId); ;
            history.CreatedDateTime = DateTime.Now;
            history.Geoid = GEOID;
            history.Priority = Priority;
            history.IsDeleted = false;
            history.InvoiceId = null;
            history.InvoiceHistoryId = null;
            history.InvoiceGuid = "";
            history.IsCreatedFromUts = true;
            history.HrNumber = HR_Number;
            history.EngagemenId = EngagemenID;
            history.CompanyId = CompanyID;
            history.TeamDistributionId = TeamDistributionId;
            db.GenAmAssignmentRuleHistories.Add(history);
            db.SaveChanges();
        }
        [NonAction]
        private void AddClientAMAssignment(long CompanyID, EsalesClientAM clientAm, long? AM_ID, long? GEOID, long? SalesPersonID, string BAU_UTS_Tagging, string ClientEmailId, string TalentName)
        {
            GenClientAmAssignment clientAssignment = new GenClientAmAssignment();

            bool IsAlreadyExist = db.GenClientAmAssignments.Where(x => x.CompanyId == CompanyID && x.IsDeleted == false).Any();
            if (!IsAlreadyExist)
            {
                string EngagementModelName = clientAm.EngagementModel.Trim();
                //Check if any Dedicated data exist
                int DedicatedEngagmentModel = db.PrgClientEngagementModels.Where(x => (x.ModelName == EngagementModelName)).Select(x => x.Id).FirstOrDefault();

                clientAssignment.ClientEmailId = ClientEmailId != null ? ClientEmailId : "";
                clientAssignment.AssignedAmuserId = AM_ID.Value; //SalesPersonID  //AssignedAMUserID
                clientAssignment.CreatedById = GetUserIDByUserIDEmployee(clientAm.CreatedByEmployeeId);
                clientAssignment.CreatedDateTime = DateTime.Now;
                clientAssignment.EffectiveDate = DateTime.Now;
                clientAssignment.ModifiedDateTime = DateTime.Now;
                clientAssignment.IsActive = true;
                clientAssignment.IsDeleted = false;
                clientAssignment.InvoiceId = 0;
                clientAssignment.GeoId = GEOID.HasValue ? GEOID.Value : 0;
                clientAssignment.SalesPersonId = SalesPersonID.HasValue ? SalesPersonID.Value : 0;
                clientAssignment.HrNumber = clientAm.HR_Number;
                clientAssignment.EngagemenId = clientAm.EngagemenID;
                clientAssignment.BauUtsTagging = BAU_UTS_Tagging;
                clientAssignment.CompanyId = CompanyID;
                clientAssignment.EngagementModelId = DedicatedEngagmentModel;
                clientAssignment.TalentName = TalentName;
                db.GenClientAmAssignments.Add(clientAssignment);
                db.SaveChanges();

                long ClientAMAssignmentID = clientAssignment.Id;
                //insert in to gen_ClientAmAssignment_History
                GenClientAmAssignmentHistory clientAssignment_History = new GenClientAmAssignmentHistory();
                clientAssignment_History.ClientAmassignmentId = ClientAMAssignmentID;
                clientAssignment_History.ClientEmailId = ClientEmailId != null ? ClientEmailId : "";
                clientAssignment_History.AssignedAmuserId = AM_ID.Value; //SalesPersonID  //AssignedAMUserID
                clientAssignment_History.CreatedById = GetUserIDByUserIDEmployee(clientAm.CreatedByEmployeeId);
                clientAssignment_History.CreatedDateTime = DateTime.Now;
                clientAssignment_History.EffectiveDate = DateTime.Now;
                clientAssignment_History.ModifiedDateTime = DateTime.Now;
                clientAssignment_History.IsActive = true;
                clientAssignment_History.IsDeleted = false;
                clientAssignment_History.InvoiceId = 0;
                clientAssignment_History.GeoId = GEOID.HasValue ? GEOID.Value : 0;
                clientAssignment_History.SalesPersonId = SalesPersonID.HasValue ? SalesPersonID.Value : 0;
                clientAssignment_History.HrNumber = clientAm.HR_Number;
                clientAssignment_History.EngagemenId = clientAm.EngagemenID;
                clientAssignment_History.BauUtsTagging = BAU_UTS_Tagging;
                clientAssignment_History.CompanyId = CompanyID;
                clientAssignment_History.EngagementModelId = DedicatedEngagmentModel;
                clientAssignment.TalentName = TalentName;
                db.GenClientAmAssignmentHistories.Add(clientAssignment_History);
                db.SaveChanges();

                UsrUser selectedAMUser = db.UsrUsers.Where(x => x.Id == AM_ID.Value).FirstOrDefault();
            }
        }
        #endregion
    }
}
