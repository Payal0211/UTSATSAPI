using Newtonsoft.Json;

namespace UTSATSAPI.Config
{
    public class HubSpotResponseUTSAdmin
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class CompanyUtsAdmin
        {
            public string value { get; set; }
            public List<Version> versions { get; set; }
        }

        public class Createdate
        {
            public string value { get; set; }
            public List<Version> versions { get; set; }
        }

        public class Email
        {
            public string value { get; set; }
            public List<Version> versions { get; set; }
        }

        public class Firstname
        {
            public string value { get; set; }
            public List<Version> versions { get; set; }
        }

        public class HsAllContactVids
        {
            public string value { get; set; }
            public List<Version> versions { get; set; }
        }

        public class HsEmailDomain
        {
            public string value { get; set; }
            public List<Version> versions { get; set; }
        }

        public class HsIsContact
        {
            public string value { get; set; }
            public List<Version> versions { get; set; }
        }

        public class HsIsUnworked
        {
            public string value { get; set; }
            public List<Version> versions { get; set; }
        }

        public class HsLifecyclestageLeadDate
        {
            public string value { get; set; }
            public List<Version> versions { get; set; }
        }

        public class HsMarketableStatus
        {
            public string value { get; set; }
            public List<Version> versions { get; set; }
        }

        public class HsMarketableUntilRenewal
        {
            public string value { get; set; }
            public List<Version> versions { get; set; }
        }

        public class HsObjectId
        {
            public string value { get; set; }
            public List<Version> versions { get; set; }
        }

        public class HsObjectSource
        {
            public string value { get; set; }
            public List<Version> versions { get; set; }
        }

        public class HsObjectSourceId
        {
            public string value { get; set; }
            public List<Version> versions { get; set; }
        }

        public class HsObjectSourceLabel
        {
            public string value { get; set; }
            public List<Version> versions { get; set; }
        }

        public class HsPipeline
        {
            public string value { get; set; }
            public List<Version> versions { get; set; }
        }

        public class Identity
        {
            public string type { get; set; }
            public string value { get; set; }
            public object timestamp { get; set; }

            [JsonProperty("is-primary")]
            public bool isprimary { get; set; }
            public string source { get; set; }
        }

        public class IdentityProfile
        {
            public int vid { get; set; }

            [JsonProperty("is-deleted")]
            public bool isdeleted { get; set; }

            [JsonProperty("is-contact")]
            public bool iscontact { get; set; }

            [JsonProperty("pointer-vid")]
            public int pointervid { get; set; }

            [JsonProperty("previous-vid")]
            public int previousvid { get; set; }

            [JsonProperty("linked-vids")]
            public List<object> linkedvids { get; set; }

            [JsonProperty("saved-at-timestamp")]
            public long savedattimestamp { get; set; }

            [JsonProperty("deleted-changed-timestamp")]
            public int deletedchangedtimestamp { get; set; }
            public List<Identity> identities { get; set; }
        }

        public class Lastmodifieddate
        {
            public string value { get; set; }
            public List<Version> versions { get; set; }
        }

        public class Lastname
        {
            public string value { get; set; }
            public List<Version> versions { get; set; }
        }

        public class Lifecyclestage
        {
            public string value { get; set; }
            public List<Version> versions { get; set; }
        }

        public class Properties
        {
            public HsIsUnworked hs_is_unworked { get; set; }
            public Firstname firstname { get; set; }
            public Lastmodifieddate lastmodifieddate { get; set; }
            public HsPipeline hs_pipeline { get; set; }
            public Createdate createdate { get; set; }
            public Lastname lastname { get; set; }
            public HsAllContactVids hs_all_contact_vids { get; set; }
            public HsObjectSourceLabel hs_object_source_label { get; set; }
            public HsMarketableUntilRenewal hs_marketable_until_renewal { get; set; }
            public HsMarketableStatus hs_marketable_status { get; set; }
            public HsObjectSourceId hs_object_source_id { get; set; }
            public HsIsContact hs_is_contact { get; set; }
            public HsObjectId hs_object_id { get; set; }
            public HsEmailDomain hs_email_domain { get; set; }
            public CompanyUtsAdmin company { get; set; }
            public HsObjectSource hs_object_source { get; set; }
            public HsLifecyclestageLeadDate hs_lifecyclestage_lead_date { get; set; }
            public Lifecyclestage lifecyclestage { get; set; }
            public Email email { get; set; }
        }

        public class ContactResponseUtsAdmin
        {
            public long vid { get; set; }

            [JsonProperty("canonical-vid")]
            public long canonicalvid { get; set; }

            [JsonProperty("merged-vids")]
            public List<object> mergedvids { get; set; }

            [JsonProperty("portal-id")]
            public int portalid { get; set; }

            [JsonProperty("is-contact")]
            public bool iscontact { get; set; }
            public Properties properties { get; set; }

            [JsonProperty("form-submissions")]
            public List<object> formsubmissions { get; set; }

            [JsonProperty("list-memberships")]
            public List<object> listmemberships { get; set; }

            [JsonProperty("identity-profiles")]
            public List<IdentityProfile> identityprofiles { get; set; }

            [JsonProperty("merge-audits")]
            public List<object> mergeaudits { get; set; }
        }

        public class Version
        {
            public string value { get; set; }

            [JsonProperty("source-type")]
            public string sourcetype { get; set; }

            [JsonProperty("source-id")]
            public string sourceid { get; set; }

            [JsonProperty("source-label")]
            public object sourcelabel { get; set; }

            [JsonProperty("updated-by-user-id")]
            public object updatedbyuserid { get; set; }
            public long timestamp { get; set; }
            public bool selected { get; set; }
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class CompanyCreatedate
        {
            public string sourceId { get; set; }
            public long timestamp { get; set; }
            public List<CompanyVersion> versions { get; set; }
            public string value { get; set; }
            public string source { get; set; }
        }

        public class Description
        {
            public object sourceId { get; set; }
            public long timestamp { get; set; }
            public List<CompanyVersion> versions { get; set; }
            public string value { get; set; }
            public string source { get; set; }
        }

        public class Name
        {
            public object sourceId { get; set; }
            public long timestamp { get; set; }
            public List<CompanyVersion> versions { get; set; }
            public string value { get; set; }
            public string source { get; set; }
        }

        public class CompanyProperties
        {
            public CompanyCreatedate createdate { get; set; }
            public Name name { get; set; }
            public Description description { get; set; }
        }

        public class CompanyDetailsUtsadmin
        {
            public long portalId { get; set; }
            public List<object> additionalDomains { get; set; }
            public CompanyProperties properties { get; set; }
            public bool isDeleted { get; set; }
            public long companyId { get; set; }
        }

        public class CompanyVersion
        {
            public string name { get; set; }
            public string sourceId { get; set; }
            public long timestamp { get; set; }
            public string value { get; set; }
            public string source { get; set; }
            public List<object> sourceVid { get; set; }
        }


    }
}
