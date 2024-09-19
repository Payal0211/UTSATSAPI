using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenEsalesAmApiResponse
    {
        public long Id { get; set; }
        public long OnBoardId { get; set; }
        public long HiringRequestId { get; set; }
        public long CompanyId { get; set; }
        public string EsaleSalesPersonId { get; set; } = null!;
        public string? SalesPersonName { get; set; }
        public string? EsalesParentId { get; set; }
        public string? NbdsalesPersonName { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
        public string? ResponseData { get; set; }
        public string? RequestPayload { get; set; }
    }
}
