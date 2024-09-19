namespace UTSATSAPI.Config
{
    public class HubSpot
    {
        public HubSpot()
        {
            properties = new List<HubSpotProperty>();
        }
        public List<HubSpotProperty> properties { get; set; }
    }
    public class HubSpotProperty
    {
        public string property { get; set; }
        public string value { get; set; }
    }

    public class HubSpotCompanyProperty
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class HubSpotCompany
    {
        public List<HubSpotCompanyProperty> properties { get; set; } = new List<HubSpotCompanyProperty>();
    }

    public class HubSpotDeal
    {
        public List<HubSpotDealProperty> properties { get; set; } = new List<HubSpotDealProperty>();
        public HubSpotDealAssociation associations { get; set; } = new HubSpotDealAssociation();
    }

    public class HubSpotDealProperty
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class HubSpotDealAssociation
    {
        public List<long> associatedCompanyIds { get; set; } = new List<long>();
        public List<long> associatedVids { get; set; } = new List<long>();
    }
}

