namespace UTSATSAPI.Models.ViewModels.Request_ResponseModels
{
    public class SelectTalentModel
    {
        public long HRId { get; set; }
        public List<ListOfTalents> listOfTalents { get; set; }
    }

    public class ListOfTalents
    {
        public long talentId { get; set; }
        public decimal? amount { get; set; } //This represents the talent cost        
        public decimal? CurrentCTC { get; set; }
        public decimal? DpPercentage { get; set; }
        public int IsReplacement { get; set; } = 0;
    }
}
