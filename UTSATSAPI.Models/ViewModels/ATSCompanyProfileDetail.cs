using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class ATSCompanyProfileDetail
    {
        public CompanyBasicDetails? basic_details { get; set; }
        public List<CompanyFundingDetails>? funding_details { get; set; }
        public List<string> culture_details { get; set; }
        public List<string> perk_details { get; set; }
        public List<string> youtube_links { get; set; }
        public List<ClientDetails>? client_details { get; set; }
        public CompanyEngagementDetails? engagement_details { get; set; }
        public long? poc_id { get; set; }
    }

    public class CompanyBasicDetails
    {
        public long? company_id { get; set; }
        public string? company_number { get; set; }
        public string? company_name { get; set; }
        public string? company_logo { get; set; } // we will use as company_logo_aws_url
        //public string? company_logo_aws_url { get; set; }
        public string? website_url { get; set; }
        public string? founded_year { get; set; }
        //public int? company_size { get; set; }
        //public string? CompanyType { get; set; }
        public string? industry { get; set; }
        public string? headquaters { get; set; }
        public string? about_company_desc { get; set; }
        public string? culture { get; set; }
        public int? is_self_funded { get; set; }
        public string? linkedin_profile { get; set; }
        public string? teamsize { get; set; }
        //public bool? is_delete_company_logo { get; set; }
    }

    public class CompanyFundingDetails
    {
        public string? funding_amount { get; set; }
        public string? funding_round { get; set; }
        public string? series { get; set; }
        public string? month { get; set; }
        public string? year { get; set; }
        public string? investors { get; set; }
        public string? additional_information { get; set; }
    }

    public class ClientDetails
    {
        public long? client_id { get; set; }
        public string? full_Name { get; set; }
        public string? email_Id { get; set; }
        public string? designation { get; set; }
        public string? phone_number { get; set; }
        public int? access_role_id { get; set; }
        //public string? en_Id { get; set; }
        //public bool? is_primary { get; set; }
        //public string? password { get; set; }
        //public string? encrypted_password { get; set; }
    }

    public class CompanyEngagementDetails
    {
        //public int? CompanyTypeID { get; set; }
        //public int? AnotherCompanyTypeID { get; set; }
        public int? is_pay_per_hire { get; set; }
        public int? is_pay_per_credit { get; set; }
        public int? is_post_job { get; set; }
        public int? is_profile_view { get; set; }
        public decimal? free_credit { get; set; } //jp_credit_balance
        public int? is_transparent_pricing { get; set; }
        public int? is_vetted_profile { get; set; }
        public decimal? credit_amount { get; set; }
        public string? credit_currency { get; set; }
        public int? job_post_credit { get; set; }
        //public int? VettedProfileViewCredit { get; set; }
        //public int? NonVettedProfileViewCredit { get; set; }
        public int? hiring_type_pricingid { get; set; }
    }

    public class SummaryDetails
    {
        public long? company_id { get; set; }
        public string? company_number { get; set; }
        public string? company_name { get; set; }
        public List<SummaryClientDetails> summary_clients { get; set; }
    }
    public class SummaryClientDetails
    {
        public long? client_id { get; set; }
        public string? client_email { get; set; }
        public bool? is_newly_added { get; set; }
    }
}
