namespace UTSATSAPI.Models.Models
{
    public class ResponseObject
    {
        public int statusCode { get; set; }
        public string Message { get; set; }
        public object Details { get; set; }
    }
}
