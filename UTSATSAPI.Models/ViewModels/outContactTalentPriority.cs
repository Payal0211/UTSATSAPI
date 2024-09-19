namespace UTSATSAPI.Models.ViewModels
{
    public class outContactTalentPriority
    {
        public long HRID { get; set; }
        public int HRStatusID { get; set; }
        public string HRStatus { get; set; }
        public List<outTalentDetail> outTalentDetails { get; set; }


    }
}
