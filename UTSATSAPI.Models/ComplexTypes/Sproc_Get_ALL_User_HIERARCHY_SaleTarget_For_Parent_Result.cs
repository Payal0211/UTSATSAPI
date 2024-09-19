using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_ALL_User_HIERARCHY_SaleTarget_For_Parent_Result
    {
        public long? UserID { get; set; }
        public int? UNDER_PARENT { get; set; }
        public string? child { get; set; }
        public string? parent { get; set; }
        public decimal? UserTarget { get; set; }
        public decimal? SelfTarget { get; set; }
    }
}
