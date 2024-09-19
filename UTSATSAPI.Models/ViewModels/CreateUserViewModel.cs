using UTSATSAPI.Models.ViewModel;

namespace UTSATSAPI.Models.ViewModels
{
    public class CreateUserViewModel
    {
        public string en_Id { get; set; }
        public long Id { get; set; }
        public FileUploadModel? uploadModel { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsNewUser { get; set; }
        public int UserTeam { get; set; }
        public int UserType { get; set; }
        public int Manager { get; set; }
        public int PriorityCount { get; set; }
        public string SkypeID { get; set; }
        public string Email { get; set; }
        public string contact { get; set; }
        public string Desigation { get; set; }
        public string Description { get; set; }
    }
}
