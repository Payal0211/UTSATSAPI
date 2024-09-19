namespace UTSATSAPI.Models.ViewModels
{
    public class ArchiveResponseObject
    {
        public ArchiveResponseObject()
        {
            Result = false;
        }
        public bool Result { get; set; }
        public List<Datas> Data { get; set; }
        public string Message { get; set; }
    }
    public class Datas
    {
        public int InvoiceID { get; set; }
        public int StatusID { get; set; }
    }
}
