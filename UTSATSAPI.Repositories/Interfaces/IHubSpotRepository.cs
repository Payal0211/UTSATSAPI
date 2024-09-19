using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;

namespace UTSATSAPI.Repositories.Interfaces
{
    /// <summary>
    /// IHubSpotRepository
    /// </summary>
    public interface IHubSpotRepository
    {
        /// <summary>
        /// Gets the hub spot contact by contact identifier.
        /// </summary>
        /// <param name="ContactId">The contact identifier.</param>
        /// <returns></returns>
        Task<GenHubSpotContact?> GetHubSpotContactByContactID(long ContactId);
        /// <summary>
        /// Saves the hub spot contact.
        /// </summary>
        /// <param name="genHubSpotContact">The gen hub spot contact.</param>
        /// <returns></returns>
        Task<GenHubSpotContact> SaveHubSpotContact(GenHubSpotContact genHubSpotContact);
        /// <summary>
        /// Gets the hub spot company by identifier.
        /// </summary>
        /// <param name="HubSpotCompanyId">The hub spot company identifier.</param>
        /// <returns></returns>
        Task<GenHubSpotCompany?> GetHubSpotCompanyById(long HubSpotCompanyId);
        /// <summary>
        /// Gets the hub spot company by company name and owner identifier.
        /// </summary>
        /// <param name="CompanyName">Name of the company.</param>
        /// <param name="HubSpotOwnerId">The hub spot owner identifier.</param>
        /// <returns></returns>
        Task<GenHubSpotCompany?> GetHubSpotCompanyByCompanyNameAndOwnerId(string CompanyName, long HubSpotOwnerId);
        /// <summary>
        /// Saves the hub spot company.
        /// </summary>
        /// <param name="genHubSpotCompany">The gen hub spot company.</param>
        /// <returns></returns>
        Task<GenHubSpotCompany> SaveHubSpotCompany(GenHubSpotCompany genHubSpotCompany);
        /// <summary>
        /// Saves the hubspot webhook notification.
        /// </summary>
        /// <param name="hubspotWebhookNotification">The hubspot webhook notification.</param>
        /// <returns></returns>
        Task<HubspotWebhookNotification> SaveHubspotWebhookNotification(HubspotWebhookNotification hubspotWebhookNotification);

        Task<List<sproc_UTS_GetAutoCompleteHubSpotCompanies_Result>> GetAutoCompleteHubSpotAllCompanies(string Company);
        void Sproc_AddUpdate_Hubspot_Company(string paramString);
       void sproc_UpdateCompanyLogo_forHubpsoCompany(string paramString);
        void Sproc_AddUpdate_HubSpot_Contact(string paramString);
        //Task<GenContact> GetContactDetailsByID(long? ContactId);

        //Task<GenCompany> GetGenCompanysById(long? CompanyId);
        //Task<bool> SaveClientLeadUser(long? companyId, int? leadUserId);
        //Task<GenContact> GetGenContactByIdwithIsActive(long ContactID);
        //Task<bool> UpdateGenContact(GenContact genContact);
        //Task<bool> CreateandupdateGenContact(GenContact genContact);
        //Task<GenCompanyContractDetail> GetGenCompanyContractDetailsById(long id);
        //Task<IEnumerable<GenContactPointofContact>> GetGenContactPointofContactsListByContactId(long contactId);
        //Task<GenContactPointofContact> GetGenContactPointofContactsByContactIdUserId(long contactId, long userId);
        //Task<UsrUser> GetUsrUserById(long id);
        //Task<bool> CreateGenContactPointofContacts(GenContactPointofContact genContactPointofContacts);
        //Task<bool> UpdateGenContactPointofContacts(GenContactPointofContact genContactPointofContacts);
        //Task<bool> DeleteGenContactPointofContactByList(IEnumerable<GenContactPointofContact> genContactPointofContactList);
        //Task<bool> UpdateGenCompanies(GenCompany genCompany);
    }
}
