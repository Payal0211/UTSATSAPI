using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgCrmApiTestsResponse
    {
        public int Id { get; set; }
        public int ApiDetailId { get; set; }
        public int TestsId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public bool IsArchived { get; set; }
        public string PublicTestLink { get; set; } = null!;
        public string InviteUrl { get; set; } = null!;
        public string SessionsUrl { get; set; } = null!;
        public int TotalNumberOfTasks { get; set; }
        public int PossibleTotalScore { get; set; }
        public int TimeLimit { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreateByDateTime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedByDateTime { get; set; }

        public virtual PrgCrmApiDetail ApiDetail { get; set; } = null!;
    }
}
