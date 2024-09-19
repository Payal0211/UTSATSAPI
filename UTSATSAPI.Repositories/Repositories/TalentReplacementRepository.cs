using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.X509;
using UTSATSAPI.Helpers;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Repositories.Interfaces;
using static UTSATSAPI.Helpers.Enum;

namespace UTSATSAPI.Repositories.Repositories
{
    public class TalentReplacementRepository : ITalentReplacement
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        private readonly IConfiguration configuration;
        private IWebHostEnvironment webHostEnvironment;
        private IUnitOfWork _unitOfWork;

        #endregion

        #region Constructors
        public TalentReplacementRepository(TalentConnectAdminDBContext _db, IConfiguration _configuration, IWebHostEnvironment _webHostEnvironment, IUnitOfWork unitOfWork)
        {
            db = _db;
            configuration = _configuration;
            webHostEnvironment = _webHostEnvironment;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region public Methods
        public async Task<List<Sproc_Get_AMNBD_For_Replacement_Result>> GetAMNBDForReplacement(long ID)
        {
            return await db.Set<Sproc_Get_AMNBD_For_Replacement_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_AMNBD_For_Replacement, ID)).ToListAsync();
        }
        public async Task<TalentReplacement> SaveTalentReplacementData(TalentReplacement talentReplacement, IUniversalProcRunner universalProcRunner)
        {
            long OnboardId = 0;
            long ReplacementID = 0;
            long HiringRequestID = 0;
            long TalentId = 0;
            Int32 Noticeperiod = talentReplacement.Noticeperiod ?? 0;
            string ReplacementStage = talentReplacement.ReplacementStage;
            string ReasonforReplacement = talentReplacement.ReasonforReplacement;
            string ReplacementInitiatedby = talentReplacement.ReplacementInitiatedby;
            int? ReplacementHandledByID = talentReplacement.ReplacementHandledByID ?? 0;
            long? EngagementReplacementOnBoardID = talentReplacement.EngagementReplacementOnBoardID ?? 0;
            string? LastWorkingDay = null;
            var LoggedInUserID = SessionValues.LoginUserId;
            long ContactTalentPriorityId = 0;
            int? LastWorkingDateOption = talentReplacement.LastWorkingDateOption ?? 1;
            if (LastWorkingDateOption == 1 && talentReplacement.LastWorkingDay.HasValue)
            {
                LastWorkingDay = talentReplacement.LastWorkingDay.Value.ToString("MM/dd/yyyy");
            }
            var objOnBoardTalents = await _unitOfWork.genOnBoardTalents.GetSingleByCondition(x => x.Id == talentReplacement.OnboardId).ConfigureAwait(false);
            if (objOnBoardTalents != null)
            {
                OnboardId = talentReplacement.OnboardId;
                HiringRequestID = Convert.ToInt64(objOnBoardTalents.HiringRequestId);
                TalentId = Convert.ToInt64(objOnBoardTalents.TalentId);
                long contactId = Convert.ToInt64(objOnBoardTalents.ContactId);

                talentReplacement.HiringRequestID = HiringRequestID;
                talentReplacement.TalentId = TalentId;

                string executionScript = string.Format("{0} {1},{2},{3},{4},'{5}','{6}',{7},'{8}',{9},'{10}','{11}','{12}',{13},'{14}',{15},{16},'{17}'", 
                  Constants.ProcConstant.Sproc_Add_TalentReplacement_details, HiringRequestID, TalentId, contactId, OnboardId, LastWorkingDay, ReasonforReplacement, Noticeperiod, System.DateTime.Now, LoggedInUserID, System.DateTime.Now, objOnBoardTalents.ClientClosureDate, ReplacementStage, LastWorkingDateOption, ReplacementInitiatedby, ReplacementHandledByID, EngagementReplacementOnBoardID, talentReplacement.EngHRReplacement);


                var x = db.Set<SaveTalentReplacement>().FromSqlRaw(executionScript).ToList().FirstOrDefault();
                ReplacementID = x.Id;

                GenContactTalentPriority genContactTalentPriority = await _unitOfWork.genContactTalentPrioritys.GetSingleByCondition(x => x.TalentId == talentReplacement.TalentId && x.HiringRequestId == talentReplacement.HiringRequestID).ConfigureAwait(false);
                if (genContactTalentPriority != null)
                {
                    ContactTalentPriorityId = genContactTalentPriority.Id;
                }

                object[] paramater = new object[] { Action_Of_History.Talent_Status_Replacement, HiringRequestID, TalentId, false, LoggedInUserID, ContactTalentPriorityId, 0, "", OnboardId, false, false, 0, 0, (short)AppActionDoneBy.UTS };
                universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, paramater);

                if (ReplacementID != 0)
                {
                    EmailBinder binder = new EmailBinder(configuration, db);
                    //GenContact genContact = await _unitOfWork.genContacts.GetSingleByCondition(x => x.Id == contactId).ConfigureAwait(false);
                    //var IsClientNotificationSent = false;
                    //if (genContact != null)
                    //{
                    //    //IsClientNotificationSent = genContact.IsClientNotificationSend;
                    //    IsClientNotificationSent = false;
                    //}
                    //if (IsClientNotificationSent)
                    //{
                    //    string EmailNotificationToClient = binder.SendEmailForReplacementToClient(ReplacementID, TalentId, contactId, HiringRequestID);
                    //}

                    //GenTalent genTalent = await _unitOfWork.genTalents.GetSingleByCondition(x => x.Id == objOnBoardTalents.TalentId).ConfigureAwait(false);
                    //var IsTalentNotificationSent = false;
                    //if (genTalent != null)
                    //{
                    //    IsTalentNotificationSent = genTalent.IsTalentNotificationSend ?? false;
                    //}
                    //if (IsTalentNotificationSent)
                    //{
                    //    string EmailNotificationToTalentwithZoomDetails = binder.SendEmailForReplacementToTalent(ReplacementID, TalentId, contactId, HiringRequestID);
                    //}

                    //new changes added by Ashwin (email Db call placed here instead of Email function)
                    string TalentName = "", SalesPersonName = "", salesPersonEmail = "", ClientName = "", ClientEmail = "", companyName = "", EngagementID = "", HR_Number = "", Last_Working_Day = "", Reason_For_Replacement = "";
                    List<Sproc_Get_Hierarchy_For_Email_Result> sproc_Get_Hierarchy_For_Email_SalesUser = new List<Sproc_Get_Hierarchy_For_Email_Result>();

                    GenTalent _Talent = await _unitOfWork.genTalents.GetSingleByCondition(x => x.Id == TalentId).ConfigureAwait(false);
                    if (_Talent != null)
                    {
                        TalentName = _Talent.Name;
                    }

                    GenOnBoardTalentsReplacementDetail talents_ReplacementDetails = await _unitOfWork.genOnBoardTalentsReplacementDetails.GetSingleByCondition(x => x.Id == ReplacementID).ConfigureAwait(false);
                    if (talents_ReplacementDetails != null)
                    {
                        OnboardId = talents_ReplacementDetails.OnboardId;
                        Last_Working_Day = Convert.ToString(talents_ReplacementDetails.LastWorkingDay);
                        Reason_For_Replacement = talents_ReplacementDetails.ReplacementReason;
                        GenOnBoardTalent onBoardTalents = await _unitOfWork.genOnBoardTalents.GetSingleByCondition(x => x.Id == OnboardId).ConfigureAwait(false);
                        if (onBoardTalents != null)
                        {
                            EngagementID = onBoardTalents.EngagemenId;
                            if (onBoardTalents.NbdSalesPersonId != 0 && onBoardTalents.NbdSalesPersonId != null)
                            {
                                var varUserDetails = await _unitOfWork.usrUsers.GetAllByCondition(x => x.Id == onBoardTalents.AmSalesPersonId).ConfigureAwait(false);
                                var UserDetails = varUserDetails.Select(x => new
                                {
                                    FullName = x.FullName,
                                    EmailId = x.EmailId
                                }).FirstOrDefault();
                                SalesPersonName = UserDetails.FullName;
                                salesPersonEmail = UserDetails.EmailId;
                                object[] HierarchyForEmailparamater = new object[] { onBoardTalents.NbdSalesPersonId };
                                universalProcRunner.Manipulation(Constants.ProcConstant.Sproc_Get_Hierarchy_For_Email, HierarchyForEmailparamater);
                            }
                            else if (onBoardTalents.AmSalesPersonId != 0 && onBoardTalents.AmSalesPersonId != null)
                            {
                                var varUserDetails = await _unitOfWork.usrUsers.GetAllByCondition(x => x.Id == onBoardTalents.AmSalesPersonId).ConfigureAwait(false);
                                var UserDetails = varUserDetails.Select(x => new
                                {
                                    FullName = x.FullName,
                                    EmailId = x.EmailId
                                }).FirstOrDefault();
                                if (UserDetails != null)
                                {
                                    SalesPersonName = UserDetails.FullName;
                                    salesPersonEmail = UserDetails.EmailId;
                                }
                                object[] HierarchyForEmailparamater = new object[] { onBoardTalents.AmSalesPersonId };
                                universalProcRunner.Manipulation(Constants.ProcConstant.Sproc_Get_Hierarchy_For_Email, HierarchyForEmailparamater);
                            }
                        }
                    }

                    GenSalesHiringRequest _SalesHiringRequest = await _unitOfWork.genSalesHiringRequests.GetSingleByCondition(t => t.Id == HiringRequestID).ConfigureAwait(false);
                    if (_SalesHiringRequest != null)
                    {
                        var ContactId = _SalesHiringRequest.ContactId;
                        HR_Number = _SalesHiringRequest.HrNumber;
                        GenContact contact = await _unitOfWork.genContacts.GetSingleByCondition(x => x.Id == ContactId).ConfigureAwait(false);
                        if (contact != null)
                        {
                            ClientName = contact.FullName;
                            ClientEmail = contact.EmailId;
                            GenCompany company = await _unitOfWork.genCompanys.GetSingleByCondition(x => x.Id == contact.CompanyId).ConfigureAwait(false);
                            if (company != null)
                            {
                                companyName = company.Company;
                            }
                        }
                    }
                    string EmailNotificationToTeam = binder.SendEmailForReplacementToInternalTeam(webHostEnvironment.WebRootPath, HR_Number, EngagementID, TalentName, ClientName, Last_Working_Day, Reason_For_Replacement, _SalesHiringRequest.SalesUserId, SalesPersonName, salesPersonEmail, HiringRequestID, TalentId);

                    GenSalesHiringRequest hiringRequest = await _unitOfWork.genSalesHiringRequests.GetSingleByCondition(x => x.Id == HiringRequestID).ConfigureAwait(false);

                    if (hiringRequest.IsHrtypeDp == true)
                    {
                        binder.SendEmailforTalentReplacementDirectPlacement(webHostEnvironment.WebRootPath, HiringRequestID, TalentId);
                    }
                }
            }
            var Response = "Success";
            //return Task.FromResult(Response);
            //return await Task.FromResult(Response).ConfigureAwait(false);
            return talentReplacement;
        }

        public async Task<List<Sproc_Get_Engagemetns_For_Replacement_BasedOn_LWD_Option_Result>> GetEngagemetnsForReplacementBasedOnLWDOption(string param)
        {
            return await db.Set<Sproc_Get_Engagemetns_For_Replacement_BasedOn_LWD_Option_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_Engagemetns_For_Replacement_BasedOn_LWD_Option, param)).ToListAsync();
        }

        public async Task<GenTalent> GteGenTalentById(long? Id)
        {
            return await _unitOfWork.genTalents.GetSingleByCondition(x => x.Id == Id);
        }
        public async Task<GenContact> GteGenContactById(long? Id)
        {
            return await _unitOfWork.genContacts.GetSingleByCondition(x => x.Id == Id);
        }
        public async Task<GenCompany> GteGenCompanyById(long? Id)
        {
            return await _unitOfWork.genCompanys.GetSingleByCondition(x => x.Id == Id);
        }
        public async Task<UsrUser> GetUsrUserById(long Id)
        {
            return await _unitOfWork.usrUsers.GetSingleByCondition(x => x.Id == Id);
        }
        public async Task<GenTalent> GetGenTalentsById(long Id)
        {
            return await _unitOfWork.genTalents.GetSingleByCondition(x => x.Id == Id);
        }
        public async Task<GenSalesHiringRequest> GetGenSalesHiringRequestById(long Id)
        {
            return await _unitOfWork.genSalesHiringRequests.GetSingleByCondition(x => x.Id == Id);
        }
        public async Task<PrgHiringRequestStatus> GetPrgHiringRequestStatusById(long Id)
        {
            return await _unitOfWork.prgHiringRequestStatuss.GetSingleByCondition(x => x.Id == Id);
        }
        public async Task<GenOnBoardTalent> GetGenOnBoardTalentById(long Id)
        {
            return await _unitOfWork.genOnBoardTalents.GetSingleByCondition(x => x.Id == Id);
        }
        public async Task<GenContactTalentPriority> GenContactTalentPriorityByTalentIDorHiringRequestID(long TalentID, long HiringRequestID)
        {
            return await _unitOfWork.genContactTalentPrioritys.GetSingleByCondition(x => x.TalentId == TalentID && x.HiringRequestId == HiringRequestID);
        }

        public async Task<List<Sproc_Get_HRIDEngagementID_ForReplacement_Result>> Sproc_Get_HRIDEngagementID_ForReplacement(string param)
        {
            return await db.Set<Sproc_Get_HRIDEngagementID_ForReplacement_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_HRIDEngagementID_ForReplacement, param)).ToListAsync();
        }
        public async Task<Sproc_Get_OnBoardDetailFor_C2H_Result> Sproc_Get_OnBoardDetailFor_C2H(string param)
        {
            return db.Set<Sproc_Get_OnBoardDetailFor_C2H_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_OnBoardDetailFor_C2H, param)).AsEnumerable().FirstOrDefault();
        }
        #endregion
    }
}
