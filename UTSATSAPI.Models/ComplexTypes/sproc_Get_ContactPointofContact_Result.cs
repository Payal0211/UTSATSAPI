namespace UTSATSAPI.Models.ComplexTypes
{
    using Microsoft.EntityFrameworkCore;

    [Keyless]
    public class sproc_Get_ContactPointofContact_Result
    {
        public long ID { get; set; }
        public Nullable<long> ContactID { get; set; }
        public Nullable<long> User_ID { get; set; }
        public string FullName { get; set; }
        public string EmailID { get; set; }
    }
}
