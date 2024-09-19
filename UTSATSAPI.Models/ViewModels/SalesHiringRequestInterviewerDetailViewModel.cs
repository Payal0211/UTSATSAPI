namespace UTSATSAPI.Models.ViewModel
{
    public class SalesHiringRequestInterviewerDetailViewModel
    {
        public long Id { get; set; }
        public long? HiringRequestId { get; set; }
        public long? HiringRequestDetailId { get; set; }
        public string? InterviewerName { get; set; }
        public string? InterviewLinkedin { get; set; }
        public decimal? InterviewerYearofExperience { get; set; }
        public string? TypeofInterviewerName { get; set; }
        public string? InterviewerDesignation { get; set; }
        public string? InterviewerEmailId { get; set; }
    }
}
