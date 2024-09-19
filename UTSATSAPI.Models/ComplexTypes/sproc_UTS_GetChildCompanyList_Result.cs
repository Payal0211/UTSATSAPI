using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_UTS_GetChildCompanyList_Result
    {
        public string ChildCompanyName { get; set; }
        public long ChildCompanyID { get; set; }
    }
}
