using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Models.ViewModels
{
    public class DirectPlacementViewModel
    {
        public long? HiringRequestId { get; set; }
        public string? ModeOfWork { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public decimal? DpPercentage { get; set; }
    }
}
