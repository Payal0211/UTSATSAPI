namespace UTSATSAPI.Models.ViewModels
{
    public class ATSHiringReqeustModel1
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

        public ATS_PayPerHire1? ATS_PayPerHire { get; set; }
        public ATS_PayPerCredit1? ATS_PayPerCredit { get; set; }
        public VitalInformation1? VitalInformation { get; set; }
    }
    public class ATS_PayPerHire1
    {
        public bool? IsHRTypeDP { get; set; } // True or false
        public decimal? DpPercentage { get; set; } // percentage values if Hr type Dp : true, else 0
        public decimal? NRMargin { get; set; }// percentage values if Hr type Dp : false, else 0 (contractual)
        public bool? IsTransparentPricing { get; set; } // True or false
        public int? HrTypePricingId { get; set; } //prg_HiringType_Pricing master ID
        public int? PayrollTypeId { get; set; }//prg_PayrollType master ID
        public string? PayrollPartnerName { get; set; }
    }
    public class ATS_PayPerCredit1
    {
        public bool? IsVettedProfile { get; set; } // True or false
        public bool? IsHiringLimited { get; set; } // True or false
        public int? JobTypeId { get; set; } // True or false
    }
    public class VitalInformation1
    {
        public string[]? CompensationOption { get; set; }
        public string[]? CandidateIndustry { get; set; }
        public string? Prerequisites { get; set; }
        public bool? HasPeopleManagementExp { get; set; }
    }

    //-----------------------------------------------------------------------------

    public class ATSHiringReqeustModel
    {
        //HR 1st tab
        public long? hiring_request_id { get; set; }// 65756585
        public string? hr_number { get; set; }//HR9789799879
        public long? contact_id { get; set; } //87679678
        public string? availability { get; set; } //Full Time/Part Time
        public int? month_duration { get; set; } //3,6,9,12 if indefinate -1
        public string? currency { get; set; }//USD / INR
        public decimal? cost { get; set; } // 1200.00
        public decimal? cost_start { get; set; } // 1500.50
        public decimal? cost_end { get; set; } // 2000.00
        public int? is_confidential_budget { get; set; } //1 or 0
        public int? mode_of_working { get; set; }  //1, 2, or 3 as per master id
        public string? jd_filename { get; set; }//HR_b2aa6dd3-edc0-44af-9bba-0832e0ee684c.pdf
        public string? jd_file_aws_url { get; set; } //ATS Uploded URL_with file name
        public string? jd_url { get; set; } //JD url
        public decimal? years_of_exp { get; set; } //6.00 or 2.00 etc
        public int? is_fresher_allowed { get; set; }//1 or 0
        public int? no_of_talents { get; set; } //1 or 2 or 3 etc
        public int? timezone_id { get; set; } // id as propvide from master
        public string? shift_start_time { get; set; }//2:00 AM
        public string? shift_end_time { get; set; }//10:00 AM
        public string? notice_period { get; set; }//30 Days or 45 Days etc
        public int? partial_engagement_type { get; set; } // 1 or 2
        public int? no_of_hours_working { get; set; } // 45
        public string? durationType { get; set; } //Short Term

        //Debrief Tab
        public string? job_title { get; set; } // php or dotnet etc. any string 
        public string? RoleAndResponsibilites { get; set; } // any string
        public string? Requirements { get; set; } // any string
        public string? job_desciption { get; set; } // any string
        public string? must_have_skills { get; set; }//"Communication, Computer proficiency, Employee relations, Organizational ability,Bootstrap,Laravel"
        public string? good_to_have_skills { get; set; }//"Communication, Computer proficiency, Employee relations, Organizational ability,Bootstrap,Laravel"
        public bool? is_hr_focused { get; set; } //true or false
        public int? LastModifiedById { get; set; } //ATS login User id

        //Location Changes
        public string? location { get; set; }//JobLocation
        public long? location_id { get; set; }//ATS_JobLocationID
        public int? is_open_to_work_near_by { get; set; }//IsOpenToWorkNearByCities   1 or 0
        public int? frequency_office_visit { get; set; }//FrequencyOfficeVisitID
        public string? near_by_cities { get; set; }//NearByCities
        public string? ats_near_by_cities { get; set; }//ATS_NearByCities


        public ATS_PayPerHire? pay_per_hire { get; set; }
        public ATS_PayPerCredit? pay_per_credit { get; set; }
        public VitalInformation? vital_information { get; set; }
        public List<POC_Info>? job_poc { get; set; }

        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
    public class ATS_PayPerHire
    {
        public int? is_dp { get; set; } // True or false -- int
        public decimal? dp_margin { get; set; } // percentage values if Hr type Dp : true, else 0
        public decimal? nr_margin { get; set; }// percentage values if Hr type Dp : false, else 0 (contractual)
        public int? is_transparent { get; set; } // True or false   -- int
        public int? pricing_id { get; set; } //prg_HiringType_Pricing master ID
        public int? payroll_type_id { get; set; }//prg_PayrollType master ID
        public string? payroll_partner_name { get; set; }
    }
    public class ATS_PayPerCredit
    {
        public int? is_vetted_profile { get; set; } // True or false   -- int
        public int? is_hiring_limited { get; set; } // True or false  --- int
        public int? job_type_id { get; set; }
    }
    public class VitalInformation
    {
        public string[]? compensation_option { get; set; }
        public string[]? industry { get; set; }
        public string? prerequisites { get; set; }
        public int? has_people_management_exp { get; set; }
    }
    public class POC_Info
    {
        public long? contact_id { get; set; }
        public string? contact_number { get; set; }
        public int? show_email { get; set; }  // int
        public int? show_contact_number { get; set; } // int
    }

    public class HRUpdateReponse
    {
        public long? hiring_request_id { get; set; }
        public string? response_messages { get; set; }
    }

    public class FilePath
    {
        public string FileName { get; set; }
    }
}
