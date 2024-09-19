namespace UTSATSAPI.Models.ViewModels
{
    public class EditHRPocViewModel
    {
        public long HRID { get; set; }
        public long? ContactID { get; set; }
        public string? ContactNo { get; set; }
        public bool? ShowEmailToTalent { get; set; }
        public bool? ShowContactNumberToTalent { get; set; }
    }
}
