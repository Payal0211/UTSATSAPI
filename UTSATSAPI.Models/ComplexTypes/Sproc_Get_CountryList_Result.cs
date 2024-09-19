using Microsoft.EntityFrameworkCore;


namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_CountryList_Result
    {
        public int? ID { get; set; }
        public string? CountryName { get; set; }
        public string? CountryRegion { get; set; }
        public int? TotalRecords { get; set; }
    }
}
