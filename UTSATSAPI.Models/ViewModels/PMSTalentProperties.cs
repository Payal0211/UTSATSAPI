using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class PMSTalentProperties
    {
        public PMSTalentProperties()
        {
            Root root = new Root();
            Data data = new Data();
            ProfileData profileData = new ProfileData();
            List<Legal> legals = new List<Legal>();
            List<Assesment> assesments = new List<Assesment>();
        }
        public long status { get; set; }
        public string message { get; set; }
        public Data data { get; set; }

        public class Achievement
        {
            public int id { get; set; }
            public int talent_id { get; set; }
            public string title { get; set; }
            public int status { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public string enc_id { get; set; }
            public int uuid { get; set; }
        }

        public class Assesment
        {
            public int id { get; set; }
            public DateTime created_at { get; set; }
            public string short_created_at { get; set; }
            public DateTime updated_at { get; set; }
            public int created_by { get; set; }
            public int talent_id { get; set; }
            public string assessment_tool { get; set; }
            public int assessment_id { get; set; }
            public string assessment_name { get; set; }
            public string assessment_date { get; set; }
            public int status { get; set; }
            public string status_text { get; set; }
            public int assessment_uuid_id { get; set; }
            public string assessment_url { get; set; }
            public string result { get; set; }
            public decimal score { get; set; }
            public int? benchmark { get; set; }
            public int attempt { get; set; }
            public int sequence { get; set; }
            public string interview_template { get; set; }
            public string duration { get; set; }
            public string duration_formatted { get; set; }
            public string assesment_report { get; set; }
            public string client_assesment_report { get; set; }
            public string enc_id { get; set; }
            public List<Skill> skills { get; set; }
        }

        public class BasicDetails
        {
            public int id { get; set; }
            public DateTime created_at { get; set; }
            public string short_created_at { get; set; }
            public DateTime updated_at { get; set; }
            public int user_id { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            //public PointOfContact point_of_contact { get; set; }
            public string contact_number { get; set; }
            public string objective { get; set; }
            public string current_employment_status { get; set; }
            public double? total_experience { get; set; }
            public string worked_with_international_client { get; set; }
            public string availability { get; set; }
            public string joining_period { get; set; }
            public string current_ctc { get; set; }
            public string expected_ctc { get; set; }
            public string repository_url { get; set; }
            public string linkedin_id { get; set; }
            public string role { get; set; }
            public string resume { get; set; }
            public string resume_url { get; set; }
            public string enc_id { get; set; }
            public string profile_pic { get; set; }
            public string profile_pic_url { get; set; }
            public int status { get; set; }
            public string status_text { get; set; }
            public string talent_monthly_cost_in_usd { get; set; }
            public string talent_monthly_cost_in_inr { get; set; }
            public long ATS_TalentID { get; set; }
            public bool is_odr_active { get; set; }
            public string ATS_Talent_LiveURL { get; set; }
            public string ATS_Non_NDAURL { get; set; }
            public string job_title { get; set; }
            public string talent_source { get; set; }
            public string talent_password { get; set; }
            public string designation { get; set; }
        }

        public class Certification
        {
            public int id { get; set; }
            public int talent_id { get; set; }
            public int certifications_id { get; set; }
            public string name { get; set; }
            public string image { get; set; }
            public int status { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public string enc_id { get; set; }
        }

        public class Clientele
        {
            public int id { get; set; }
            public int talent_id { get; set; }
            public string title { get; set; }
            public int status { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public string enc_id { get; set; }
            public int uuid { get; set; }
        }

        public class Data
        {
            public ProfileData profileData { get; set; }
            public List<Legal> legals { get; set; }
            public List<Assesment> assesments { get; set; }
            public Nullable<long> HRID { get; set; }
        }

        public class Education
        {
            public int id { get; set; }
            public int talent_id { get; set; }
            public string university { get; set; }
            public string degree { get; set; }
            public string degree_type { get; set; }
            public string start_date { get; set; }
            public string end_date { get; set; }
            public int? is_current { get; set; }
            public int status { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public string enc_id { get; set; }
            public int uuid { get; set; }
        }

        public class Experience
        {
            public int id { get; set; }
            public int talent_id { get; set; }
            public string title { get; set; }
            public string company_name { get; set; }
            public string job_description { get; set; }
            public string start_date { get; set; }
            public string end_date { get; set; }
            public int? is_current { get; set; }
            public int status { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public string enc_id { get; set; }
            public int uuid { get; set; }
            public List<Skill> skills { get; set; }
        }

        public class Interest
        {
            public int id { get; set; }
            public int talent_id { get; set; }
            public int interest_id { get; set; }
            public string name { get; set; }
            public string image { get; set; }
            public int status { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public string enc_id { get; set; }
        }

        public class Legal
        {
            public int id { get; set; }
            public int talent_id { get; set; }
            public string document_type { get; set; }
            public string description { get; set; }
            public string document_name { get; set; }
            public string file { get; set; }
            public string file_type { get; set; }
            public string valid_start_date { get; set; }
            public string valid_end_date { get; set; }
            public string document_status { get; set; }
            public string signed_date { get; set; }
            public int status { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
        }

        public class PointOfContact
        {
            public string name { get; set; }
            public string email { get; set; }
            public string employee_id { get; set; }
            public string contact_number { get; set; }
            public string skype_id { get; set; }
            public string description { get; set; }
            public string designation { get; set; }
            public string profile_pic { get; set; }
            public string profile_pic_url { get; set; }
        }

        public class Primaryskill
        {
            public int id { get; set; }
            public string name { get; set; }
            public string years_of_experience { get; set; }
        }

        public class ProfileData
        {
            public BasicDetails basicDetails { get; set; }
            public List<Experience> experiences { get; set; }
            public List<Primaryskill> primaryskills { get; set; }
            public List<Secondaryskill> secondaryskills { get; set; }
            public List<Project> projects { get; set; }
            public List<Certification> certifications { get; set; }
            public List<Shift> shifts { get; set; }
            public List<Education> educations { get; set; }
            public List<Testimonial> testimonials { get; set; }
            public List<Clientele> clienteles { get; set; }
            public List<Interest> interests { get; set; }
            public List<Achievement> achievements { get; set; }
            public List<Tool> tools { get; set; }
        }

        public class Project
        {
            public int id { get; set; }
            public int talent_id { get; set; }
            public string title { get; set; }
            public string company_name { get; set; }
            public string start_date { get; set; }
            public string end_date { get; set; }
            public string description { get; set; }
            public int status { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public string enc_id { get; set; }
            public int uuid { get; set; }
            public List<Skill> skills { get; set; }
        }

        public class Root
        {
            public int status { get; set; }
            public string message { get; set; }
            public Data data { get; set; }
        }

        public class Secondaryskill
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public class Shift
        {
            public int id { get; set; }
            public int talent_id { get; set; }
            public int shift_id { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public string enc_id { get; set; }
            public string shift { get; set; }
        }

        public class Skill
        {
            public int id { get; set; }
            public string name { get; set; }
            public string Proficiency { get; internal set; }
        }

        public class Testimonial
        {
            public int id { get; set; }
            public int talent_id { get; set; }
            public string title { get; set; }
            public string name { get; set; }
            public string role { get; set; }
            public int status { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public string enc_id { get; set; }
            public int uuid { get; set; }
            public List<Skill> skills { get; set; }
        }

        public class Tool
        {
            public int id { get; set; }
            public int talent_id { get; set; }
            public int tool_id { get; set; }
            public string name { get; set; }
            public string image { get; set; }
            public int status { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public string enc_id { get; set; }
        }
    }

    public class PMSTalentOutput
    {
        public long? TalentId { get; set; }
        public string? Message { get; set; }
    }
}
