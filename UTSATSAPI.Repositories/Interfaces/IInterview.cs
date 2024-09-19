namespace UTSATSAPI.Repositories.Interfaces
{
    using System.Collections.Generic;
    using UTSATSAPI.Models.ComplexTypes;

    public interface IInterview
    {
        List<sproc_GetHiringInterview_Result> sproc_UTS_GetHiringInterview(string param);
        Sproc_InsertBookSlot_Result Sproc_InsertBookSlot(string param);
        Sproc_InsertBookSlot_CHECK_Result Sproc_InsertBookSlot_CHECK(string param);
        sproc_Get_ContactPointofContact_Result sproc_Get_ContactPointofContact(string param);
        Sproc_InterviewRoundDetails_Result Sproc_Add_TalentSelected_ClientFeedback(string param);
        List<sproc_UTS_InterviewerDetails_Result> sproc_GetCurrentInterviewerDetails(string param);
        long sproc_Insert_NextInterviewRoundDetails_WithFeedbackOption(string param);

    }
}
