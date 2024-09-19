namespace UTSATSAPI.Models.ViewModels
{
    public class SearchHiringRequestViewModel
    {
        public int rows { get; set; }
        public int page { get; set; }
        public long HiringRequestId { get; set; }
        public string? EmailId { get; set; }
    }
}
