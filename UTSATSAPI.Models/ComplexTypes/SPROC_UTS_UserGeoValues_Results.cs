using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class SPROC_UTS_UserGeoValues_Result 
    {
        public string? GeoID { get; set; }
    }
}
