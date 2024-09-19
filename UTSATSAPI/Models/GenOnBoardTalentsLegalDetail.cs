using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenOnBoardTalentsLegalDetail
    {
        public long Id { get; set; }
        public long? OnBoardId { get; set; }
        public long? CompanyLegalId { get; set; }
        public long? HiringRequestId { get; set; }
    }
}
