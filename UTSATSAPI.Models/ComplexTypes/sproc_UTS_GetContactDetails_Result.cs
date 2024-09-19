using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_UTS_GetContactDetails_Result
    {
        public long? ID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? EmailID { get; set; }
        public string? Designation { get; set; }
        public string? LinkedIn { get; set; }
        public string? ContactNo { get; set; }
        public long? CompanyID { get; set; }
        public bool? IsPrimary { get; set; }
        public string? ClientProfilePic { get; set; }
        public bool? ResendInviteEmail { get; set; }
        public int? RoleID { get; set; }
        public string? en_Id { get; set; }
    }
}
