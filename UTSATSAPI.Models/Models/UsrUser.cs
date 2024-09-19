using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class UsrUser
    {
        public long Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public int? UserTypeId { get; set; }
        public string? EmployeeId { get; set; }
        public string? EmailId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? ContactNumber { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedbyDatetime { get; set; }
        public int? ModifyById { get; set; }
        public DateTime? ModifyByDatetime { get; set; }
        public string? SkypeId { get; set; }
        public string? Description { get; set; }
        public string? ProfilePic { get; set; }
        public string? Designation { get; set; }
        public int? RoleId { get; set; }
        public bool? IsPasswordChanged { get; set; }
        public int? PriorityCount { get; set; }
        public int? RemainingPriorityCount { get; set; }
        public bool? IsNewUser { get; set; }
        public bool? IsOdr { get; set; }
        public string? Utsguid { get; set; }
        public long? HubspotOwnerId { get; set; }
        public int? GeoId { get; set; }
        public string? BdrMdrName { get; set; }
        public bool? IsPartnerUser { get; set; }
        public long? DeptId { get; set; }
        public string? TeamId { get; set; }
        public long? LevelId { get; set; }
        public int? AnotherUserTypeId { get; set; }
    }
}
