using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes.UpChat
{
    [Keyless]
    public class Sproc_UpChat_Get_UserList_Result
    {
        public string? channelID { get; set; }
        public string? photoURL { get; set; }
        public string? userEmpId { get; set; }
        public string? userName { get; set; }
        public string? userDesignation { get; set; }
        public string? userInitial { get; set; }
        public string? backGroudColor { get; set; }
        public string? fontColor { get; set; }
    }
}
