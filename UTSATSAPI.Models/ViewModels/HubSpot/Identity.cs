using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels.HubSpot
{
    public class Identity
    {
        public string type { get; set; }
        public string value { get; set; }
        public object timestamp { get; set; }

        [JsonProperty("is-primary")]
        public bool isprimary { get; set; }
    }

    public class IdentityProfile
    {
        public long vid { get; set; }

        [JsonProperty("saved-at-timestamp")]
        public long savedattimestamp { get; set; }

        [JsonProperty("deleted-changed-timestamp")]
        public long deletedchangedtimestamp { get; set; }
        public List<Identity> identities { get; set; }
    }

    public class Root1
    {
        [JsonProperty("identity-profiles")]
        public List<IdentityProfile> identityprofiles { get; set; }
    }
}
