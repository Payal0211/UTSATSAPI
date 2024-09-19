namespace UTSATSAPI.Models.ViewModels
{
    public class LoginUserHRInfo_Add
    {
        public bool isDirectHR { get; set; }
        public DisabledFields disabledFields { get; set; }
        public RemoveFields removeFields { get; set; }
        public DefaultProperties defaultProperties { get; set; }
    }

    public class LoginUserHRInfo_Edit
    {
        public bool isDirectHR { get; set; }
        public bool isBDR_MDRUser { get; set; }
        public DisabledFields disabledFields { get; set; }
        public RemoveFields removeFields { get; set; }
    }

    public class DisabledFields
    {
        public DisabledFields()
        {
            transparentPricing = true;
            talentRequired = false;
            salesPerson = false;
            Role = true;
            InterviewerSection = true;
        }
        public bool? transparentPricing { get; set; }
        public bool? talentRequired { get; set; }
        public bool? salesPerson { get; set; }
        public bool? Role { get; set; }
        public bool? InterviewerSection { get; set; }
    }

    public class RemoveFields
    {
        public RemoveFields()
        {
            postalCode = true;
            state = true;
            address = true;
            dealID = true;
            hrFormLink = true;
            discoveryCallLink = true;
        }
        public bool? postalCode { get; set; }
        public bool? state { get; set; }
        public bool? address { get; set; }
        public bool? dealID { get; set; }
        public bool? hrFormLink { get; set; }
        public bool? discoveryCallLink { get; set; }
    }

    public class DefaultProperties
    {
        public DefaultProperties()
        {
            isTransparentPricing = true;
            talentsNumber = 1;
            salesPerson = 0;
            currency = "USD";
        }
        public bool? isTransparentPricing { get; set; }//true
        public int? talentsNumber { get; set; }//TR = 1
        public long? salesPerson { get; set; }//Darshan
        public string? currency { get; set; }//USD
    }
}
