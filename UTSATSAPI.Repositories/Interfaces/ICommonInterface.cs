namespace UTSATSAPI.Repositories.Interfaces
{
    public interface ICommonInterface
    {
        IViewAllHR ViewAllHR { get; }
        ISendEmailNotes SendEmailNotes { get; }
        IInterview interview { get; }
        IHiringRequest hiringRequest { get; }
        ITalentStatus TalentStatus { get; }
        
    }
}
