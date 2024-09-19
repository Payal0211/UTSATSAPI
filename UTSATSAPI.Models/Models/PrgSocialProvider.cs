using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgSocialProvider
    {
        public int Id { get; set; }
        public string SocialProviderName { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
