namespace UTSATSAPI.Models.ViewModels
{

    public class SalesTarget
    {
        public string targetMonthYear { get; set; }
        public List<UserHierarchySalesTarget> UserHierarchySalesTarget { get; set; }
    }
    public class UserHierarchySalesTarget
    {
        public long? UserID { get; set; }
        public int? UNDER_PARENT { get; set; }
        public string? child { get; set; }
        public string? parent { get; set; }
        public decimal UserTarget { get; set; }
        public decimal SelfTarget { get; set; }
    }


}
