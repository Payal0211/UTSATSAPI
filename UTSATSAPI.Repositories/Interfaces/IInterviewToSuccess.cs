namespace UTSATSAPI.Repositories.Interfaces
{
    using UTSATSAPI.ComplexTypes;
    using UTSATSAPI.Models.ComplexTypes;

    public interface IInterviewToSuccess
    {
        IEnumerable<Sproc_Get_InterviewToSuccess_PopUp_Result> GetInterviewToSuccessPopUpReport(string paramasString);
        IEnumerable<Sproc_GetInterviewToSuccessReport_Result> GetInterviewtoSuccessReport(string paramasString);
    }
}