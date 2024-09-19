namespace UTSATSAPI.Models.ViewModels
{
    public class Meeting
    {
        public string id { get; set; }
        public string assistant_id { get; set; }
        public string host_email { get; set; }
        public string registration_url { get; set; }
        public string topic { get; set; }
        public int type { get; set; }
        public string start_time { get; set; }
        public int duration { get; set; }
        public string timezone { get; set; }
        public string created_at { get; set; }
        public string agenda { get; set; }
        public string start_url { get; set; }
        public string join_url { get; set; }
        public string password { get; set; }
        public string h323_password { get; set; }
        public int pmi { get; set; }
        public List<object> tracking_fields { get; set; }

        public Settings settings { get; set; }
    }
    public class Settings
    {
        public bool host_video { get; set; }
        public bool participant_video { get; set; }

        public int jbh_time { get; set; }

        public bool cn_meeting { get; set; }
        public bool in_meeting { get; set; }
        public bool join_before_host { get; set; }
        public bool mute_upon_entry { get; set; }
        public bool watermark { get; set; }
        public bool use_pmi { get; set; }
        public int approval_type { get; set; }
        public int registration_type { get; set; }
        public string audio { get; set; }
        public string auto_recording { get; set; }
        public bool enforce_login { get; set; }
        public string enforce_login_domains { get; set; }
        public string alternative_hosts { get; set; }
        public bool close_registration { get; set; }
        public bool waiting_room { get; set; }
        public List<object> global_dial_in_countries { get; set; }
        public List<object> global_dial_in_numbers { get; set; }
        public string contact_name { get; set; }
        public string contact_email { get; set; }
        public bool registrants_confirmation_email { get; set; }
        public bool registrants_email_notification { get; set; }
        public bool meeting_authentication { get; set; }
        public string authentication_option { get; set; }
        public string authentication_domains { get; set; }
        public string authentication_name { get; set; }
        public List<object> interpreters { get; set; }
        public bool show_share_button { get; set; }
        public bool allow_multiple_devices { get; set; }
        public string encryption_type { get; set; }
        public ZoomMeeting approved_or_denied_countries_or_regions { get; set; }
    }

    public class ZoomMeeting
    {
        public bool enable { get; set; }
        public string method { get; set; }
        public List<object> approved_list { get; set; }
        public List<object> denied_list { get; set; }
    }
}
