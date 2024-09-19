using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_Get_Onboarding_Details_For_Second_Tab_AMAssignment_Result
    {
        public string? InVoiceRaiseTo { get; set; }
        public string? InVoiceRaiseToEmail { get; set; }
        public int? UTSContractDuration { get; set; }
        public string? BDR_MDR_Name { get; set; }
        public string? Company_Description { get; set; }
        public string? Talent_FirstWeek { get; set; }
        public string? Talent_FirstMonth { get; set; }
        public string? SoftwareToolsRequired { get; set; }
        public string? DevicesPoliciesOption { get; set; }
        public string? TalentDeviceDetails { get; set; }
        public decimal? AdditionalCostPerMonth_RDPSecurity { get; set; }
        public bool? IsRecurring { get; set; }
        public string? ProceedWithUplers_LeavePolicyOption { get; set; }
        public string? ProceedWithClient_LeavePolicyOption { get; set; }
        public string? ProceedWithClient_LeavePolicyLink { get; set; }
        public string? LeavePolicyFileName { get; set; }
        public string? Exit_Policy { get; set; }
        public string? Device_Radio_Option { get; set; }
        public int? DeviceID { get; set; }
        public decimal? TotalCost { get; set; }
        public string? Client_DeviceDescription { get; set; }
    }
}
