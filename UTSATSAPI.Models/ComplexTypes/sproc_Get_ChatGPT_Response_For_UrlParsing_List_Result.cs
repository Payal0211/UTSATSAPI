using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_Get_ChatGPT_Response_For_UrlParsing_List_Result
    {
        public string? ParsingURL { get; set; }
        public string? ParsingJDText { get; set; }
        public string? Response { get; set; }
        public string? CreatedDateTime { get; set; }
        public int? AchievedDetails { get; set; }
        public int? TotalRecords { get; set; }
        public string? GPTPrompt { get; set; }

    }
}
