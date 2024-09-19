namespace UTSATSAPI.Models.Models
{
    public class ATSHiringReqeustModel
    {
        //HR 1st tab
        public long? HrId { get; set; }// 65756585
        public long? ContactId { get; set; } //87679678
        public string? Availability { get; set; } //Full Time/Part Time
        public int? ContractDuration { get; set; } //3,6,9,12 if indefinate -1
        public string? Currency { get; set; }//USD / INR
        public decimal? AdhocBudgetCost { get; set; } // 1200.00
        public decimal? MinimumBudget { get; set; } // 1500.50
        public decimal? MaximumBudget { get; set; } // 2000.00
        public bool? IsConfidentialBudget { get; set; } // true/false : boolean value
        public int? ModeOfWorkingId { get; set; }  //1 or 2 or 3 as masterID
        public string? City { get; set; } //Ahmedabad
        public string? Country { get; set; } //India
        public string? JDFilename { get; set; }//HR_b2aa6dd3-edc0-44af-9bba-0832e0ee684c.pdf
        public string? JDFile_ATSURL { get; set; } //ATS Uploded URL_with file name
        public string? JDURL { get; set; } //JD url
        public decimal? YearOfExp { get; set; } //6.00 or 2.00 etc
        public int? NoofTalents { get; set; } //1 or 2 or 3 etc
        public int? TimezoneId { get; set; } // id as propvide from master
        public string? TimeZoneFromTime { get; set; }//2:00 AM
        public string? TimeZoneEndTime { get; set; }//10:00 AM
        public string? HowSoon { get; set; }//30 Days or 45 Days etc
        public int? PartialEngagementTypeID { get; set; } // 1 or 2
        public int? NoofHoursworking { get; set; } // 45
        public string? DurationType { get; set; } //Short Term
        
        //Debrief Tab
        public string? HrTitle { get; set; } // php or dotnet etc. any string 
        public string? RoleAndResponsibilites { get; set; } // any string
        public string? Requirements { get; set; } // any string
        public string? JobDescription { get; set; } // any string
        public string? MustHaveSkills { get; set; }//"Communication, Computer proficiency, Employee relations, Organizational ability,Bootstrap,Laravel"
        public string? GoodToHaveSkills { get; set; }//"Communication, Computer proficiency, Employee relations, Organizational ability,Bootstrap,Laravel"
        public bool? IsHrfocused { get; set; } //true or false
        public int? LastModifiedById { get; set; } //ATS login User id
        
        //Location Changes
        public string? JobLocation { get; set; }//JobLocation
        public int? FrequencyOfficeVisitID { get; set; }//FrequencyOfficeVisitID
        public bool? IsOpenToWorkNearByCities { get; set; }//IsOpenToWorkNearByCities
        public string? NearByCities { get; set; }//NearByCities
        public long? ATS_JobLocationID { get; set; }//ATS_JobLocationID
        public string? ATS_NearByCities { get; set; }//ATS_NearByCities

        public ATS_PayPerHire? ATS_PayPerHire { get; set; }
        public ATS_PayPerCredit? ATS_PayPerCredit { get; set; }
        public VitalInformation? VitalInformation { get; set; }
    }
    public class ATS_PayPerHire
    {
        public bool? IsHRTypeDP { get; set; } // True or false
        public decimal? DpPercentage { get; set; } // percentage values if Hr type Dp : true, else 0
        public decimal? NRMargin { get; set; }// percentage values if Hr type Dp : false, else 0 (contractual)
        public bool? IsTransparentPricing { get; set; } // True or false
        public int? HrTypePricingId { get; set; } //prg_HiringType_Pricing master ID
        public int? PayrollTypeId { get; set; }//prg_PayrollType master ID
        public string? PayrollPartnerName { get; set; }
    }
    public class ATS_PayPerCredit
    {
        public bool? IsVettedProfile { get; set; } // True or false
        public bool? IsHiringLimited { get; set; } // True or false
        public int? JobTypeId { get; set; } // True or false
    }
    public class VitalInformation
    {
        public string[]? CompensationOption { get; set; }
        public string[]? CandidateIndustry { get; set; }
        public string? Prerequisites { get; set; }
        public bool? HasPeopleManagementExp { get; set; }
    }
}
