using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenRoleAndHiringTypeClientPortal
    {
        public long Id { get; set; }
        public long? Gptjdid { get; set; }
        public string? Guid { get; set; }
        public string? RoleName { get; set; }
        public long? ContactId { get; set; }
        public int? ExperienceYears { get; set; }
        public int? NoOfTalents { get; set; }
        public string? IsHiringLimited { get; set; }
        public string? Availability { get; set; }
        public int? ContractDuration { get; set; }
        public string? Ipaddress { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedByDateTime { get; set; }
        public string? ProcessType { get; set; }
        public bool? IsActive { get; set; }
        public int? WorkingModeId { get; set; }
        public string? CompanyLocation { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public int? AnotherCompanyId { get; set; }
        public bool? IsFresherAllowed { get; set; }
        public bool? ShowHrpocdetailsToTalents { get; set; }
        public string? JobLocation { get; set; }
        public int? FrequencyOfficeVisitId { get; set; }
        public bool? IsOpenToWorkNearByCities { get; set; }
        public string? NearByCities { get; set; }
        public long? AtsJobLocationId { get; set; }
        public string? AtsNearByCities { get; set; }
    }
}
