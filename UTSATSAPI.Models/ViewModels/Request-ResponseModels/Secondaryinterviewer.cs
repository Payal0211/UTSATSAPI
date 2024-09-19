namespace UTSATSAPI.Models.ViewModels.Request_ResponseModels
{
    public class Interviewer
    {
        public InterviewerClass primaryInterviewer { get; set; }
        public List<InterviewerClass> secondaryinterviewerList { get; set; }
    }
    public class InterviewerClass
    {
        public long? interviewerId { get; set; }
        public string? fullName { get; set; }
        public string? emailID { get; set; }
        public string? linkedin { get; set; }
        public string? designation { get; set; }
        public bool? isUserAddMore { get; set; }
    }
}
