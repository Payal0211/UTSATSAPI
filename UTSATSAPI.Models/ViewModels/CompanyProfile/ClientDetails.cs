using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels.CompanyProfile
{
    public class ClientDetails
    {
        public long? ClientID { get; set; }
        public string? en_Id { get; set; }
        public bool? isPrimary { get; set; }
        public string? fullName { get; set; }
        public string? emailId { get; set; }
        public string? designation { get; set; }
        public string? phoneNumber { get; set; }
        public int? AccessRoleId { get; set; }
        public string? Password { get; set; }
        public string? EncryptedPassword { get; set; }
    }
}
