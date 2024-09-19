namespace UTSATSAPI.Repositories.Interfaces
{
    using UTSATSAPI.Models.ViewModel;

    public interface IHRInterviewerDetail
    {
        List<SalesHiringRequestInterviewerDetailViewModel> GetHRInterviewerDetails(long HiringRequestID);
    }
}
