using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_Company_YouTubeDetails_Result
    {
        public long? YoutubeID { get; set; }
        public string? YoutubeLink { get; set; }
    }
}
