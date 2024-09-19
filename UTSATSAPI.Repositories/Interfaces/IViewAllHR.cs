using Microsoft.AspNetCore.Mvc.Rendering;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModel;
using UTSATSAPI.Models.ViewModels;

namespace UTSATSAPI.Repositories.Interfaces
{
    public interface IViewAllHR
    {
        List<sproc_ViewAllHRs_Result> GetAllHRs(ViewAllHRViewModel viewAllHRViewModel, int loggedInUserTypeId, long LoggedInUserID);
        HRDetailViewModel ShowHRDetail(long id, long? WhatToaddClick = 0);
        NotesViewModel SaveHRNotes(NotesViewModel notesViewModel, string userEmployeeID);
        NotesViewModel UpChatSaveHRNotes(NotesViewModel notesViewModel, string userEmployeeID);
        Task<HRFilterListViewModel> GetFiltersLists();
        GenSalesHiringRequestPriority GetHRPriorityData(long hRId);
        void sproc_UTS_gen_ContactTalentPriorityupdate(string param);
        GenSalesHiringRequest GetHRData(long hRId);
        long HiringRequestPriority(string param);
        sp_get_HRClientDetails_Result GetClientCompanyDetail(long HiringRequestID);
        GenSalesHiringRequest SaveSalesHiringRequest(GenSalesHiringRequest genSalesHiringRequest);
        GenSalesHrTracceptedDetail SaveSalesHRTracceptedDetail(GenSalesHrTracceptedDetail genSalesHrTracceptedDetail);
        string SaveHiringRequestHistory(string Action, long HiringRequest_ID, int Talent_ID, bool Created_From, long CreatedById, int ContactTalentPriority_ID, int InterviewMaster_ID, string HR_AcceptedDateTime, int OnBoard_ID, bool IsManagedByClient, bool IsManagedByTalent);
        List<Sproc_UTS_Get_TalentDetails_For_ContractualtoDP_Conversion_Result> Sproc_UTS_Get_TalentDetails_For_ContractualtoDP_Conversion(string param);
        List<Sproc_UTS_Get_TalentDetails_For_DPToContractual_Conversion_Result> sproc_UTS_Get_TalentDetails_For_DPToContractual_Conversion(string param);
        void sproc_UTS_UpdateDPAmountDetails(string param);
        void sproc_UTS_UpdateIsHrtypedpandDPAmount(string param);
        CheckValidationMessage Sproc_Get_Validation_Message_For_Priority(long loggedInUser);
        void InsertHiringRequestPriorityReviseFlow(string param);
        Task<List<Sproc_Get_Priority_Set_Remaining_Count_Details_Result>> GetPrioritySetRemainingCountDetailsResult(long logingUserId);
        Task<List<Sproc_Check_Validation_Message_For_User_Edit_TR_Result>> Get_Sproc_Check_Validation_Message_For_User_Edit_TR(string param);
        Task<List<Sproc_UpdateTR_Result>> Get_Sproc_UpdateTR(string param);
        Sproc_UTS_FetchUsersWithSpecialEditAccess_Result CheckSpecialEdits(string param);

        // -----SLA details, Update, and history----

        Task<List<sproc_Get_HiringRequest_SLADetails_Result>> Get_HiringRequest_SLADetails(string param);
        
        Task<List<SelectListItem>?> GetSLAEditReasons();

        void Sproc_Update_SLADate(string param);

        Task<List<Sproc_Get_SLAUpdate_History_Result>> Get_SLAUpdate_History(string param);
        Task<Sproc_Get_SLAUpdateDetails_ForEmail_Result> Sproc_Get_SLAUpdateDetails_ForEmail(string param);

        List<sproc_ViewAllUnAssignedHRs_Result> GetAllUnAssignedHRs(ViewAllUnAssignedHRViewModel viewAllUnAssignedHRViewModel, int loggedInUserTypeId, long LoggedInUserID);
        void sproc_AssignedPOCToUnAssignedHRs(string param);

        sp_UTS_TalentCalculation_PayPerHire_Result GetTalentLevelCalculation(string param);
        Task<object> GetAllHRDataForAdmin(long HRID);
        List<sp_UTS_get_HRHistory_UsingPagination_Result> sp_UTS_get_HRHistory_UsingPagination_Result(string param);
        List<sp_UTS_get_HRTalentDetails_UsingPagination_Result> sp_UTS_get_HRTalentDetails_UsingPagination_Result(string param);
        Sproc_EmailHRTypeChanged_Result Sproc_EmailHRTypeChanged(string param);
        // -----SLA details, Update, and history----
    }
}
