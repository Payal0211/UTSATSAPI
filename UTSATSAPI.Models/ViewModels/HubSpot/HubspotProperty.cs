namespace UTSATSAPI.Models.ViewModels.HubSpot
{
    public class HubspotProperty
    {
        public PropertyField createdate { get; set; }
        public PropertyField hubspot_owner_id { get; set; }
        public PropertyField first_deal_created_date { get; set; }
        public PropertyField num_associated_deals { get; set; }
        public PropertyField exclude { get; set; }
        public PropertyField hubspotscore { get; set; }
        public PropertyField recent_deal_close_date { get; set; }
        public PropertyField venture { get; set; }
        public PropertyField prg_ZohoTemplate_Item_ID { get; set; }
        public PropertyField currentlyinworkflow { get; set; }
        public PropertyField firstname { get; set; }
        public PropertyField lastname { get; set; }
        public PropertyField associatedcompanyid { get; set; }
        public PropertyField hubspot_team_id { get; set; }
        public PropertyField recent_deal_amount { get; set; }
        public PropertyField associated_company { get; set; }
        public PropertyField email { get; set; }
        public PropertyField lastmodifieddate { get; set; }
        public PropertyField contact_deal_id { get; set; }
        public PropertyField hubspot_owner_assigneddate { get; set; }
        public PropertyField total_revenue { get; set; }
        public PropertyField new_company_name { get; set; }
        public PropertyField lifecyclestage { get; set; }
        public PropertyField phone { get; set; }
        public PropertyField website { get; set; }
        public PropertyField regions { get; set; }
        public PropertyField city { get; set; }
        //public PropertyField skype { get; set; }
        public PropertyField jobtitle { get; set; }
        public PropertyField company { get; set; }
    }
    public class PropertyField
    {
        public string value { get; set; }
    }
}
