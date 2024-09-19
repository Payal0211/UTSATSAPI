using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenSocialMediaLoginDetail
    {
        public long Id { get; set; }
        public string? SocialMediaLogin { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? ResponseEmailId { get; set; }
        public string? ResponseName { get; set; }
        public DateTime? LoginDate { get; set; }
    }
}
