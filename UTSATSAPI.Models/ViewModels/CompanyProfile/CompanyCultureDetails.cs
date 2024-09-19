namespace UTSATSAPI.Models.ViewModels.CompanyProfile
{
    public class CompanyCultureDetails
    {
        public long? CultureID { get; set; }
        public string? Culture_Image { get; set; }
    }

    public class DeleteCultureImage
    {
        public long? CultureID { get; set; }
        public string? Culture_Image { get; set; }
        public long? CompanyID { get; set; }
    }
}
