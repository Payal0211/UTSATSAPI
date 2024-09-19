using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgCrmApiDetail
    {
        public PrgCrmApiDetail()
        {
            PrgCrmApiTestsResponses = new HashSet<PrgCrmApiTestsResponse>();
        }

        public int Id { get; set; }
        public int PartnerId { get; set; }
        public string Url { get; set; } = null!;
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public string AuthorizationType { get; set; } = null!;
        public string AuthorizationValue { get; set; } = null!;
        public int? CreatedById { get; set; }
        public DateTime? CreateByDateTime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedByDateTime { get; set; }
        public bool? IsActive { get; set; }

        public virtual PrgCrmApiPartner Partner { get; set; } = null!;
        public virtual ICollection<PrgCrmApiTestsResponse> PrgCrmApiTestsResponses { get; set; }
    }
}
