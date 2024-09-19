using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenDirectPlacement
    {
        public long Id { get; set; }
        public long? HiringRequestId { get; set; }
        public string? ModeOfWork { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedByDateTime { get; set; }
        public string? JobLocation { get; set; }
        public int? FrequencyOfficeVisitId { get; set; }
        public bool? IsOpenToWorkNearByCities { get; set; }
        public string? NearByCities { get; set; }
        public long? AtsJobLocationId { get; set; }
        public string? AtsNearByCities { get; set; }
    }
}
