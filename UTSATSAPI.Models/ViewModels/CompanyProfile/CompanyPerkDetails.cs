namespace UTSATSAPI.Models.ViewModels.CompanyProfile
{
    public class CompanyPerkDetails
    {
        public string? Perks { get; set; }
    }

    public class CompanyYouTubeDetails
    {
        public long? YoutubeID { get; set; }
        public string? YoutubeLink { get; set; }
    }

    public class DeleteYouTubeDetails
    {
        public long? YoutubeID { get; set; }
        public long? CompanyID { get; set; }
    }
}
