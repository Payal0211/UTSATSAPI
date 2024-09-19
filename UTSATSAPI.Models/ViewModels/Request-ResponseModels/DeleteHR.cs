namespace UTSATSAPI.Models.ViewModels.Request_ResponseModels
{
    public class DeleteHR
    {
        public long Id { get; set; }
        public int DeleteType { get; set; }
        public long ReasonId { get; set; }
        public string Reason { get; set; }
        public string Remark { get; set; }
        public long OnBoardId { get; set; }
    }
}
