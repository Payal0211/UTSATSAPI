using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;

namespace UTSATSAPI.Repositories.Interfaces
{
    /// <summary>
    /// IContactRepository
    /// </summary>
    public interface IContactRepository
    {
        /// <summary>
        /// Gets the contact by hub spot identifier and emil.
        /// </summary>
        /// <param name="HubSpotContactId">The hub spot contact identifier.</param>
        /// <param name="Email">The email.</param>
        /// <returns></returns>
        Task<GenContact?> GetContactByHubSpotIdAndEmil(long HubSpotContactId, string Email);

        /// <summary>
        /// Saves the contact.
        /// </summary>
        /// <param name="genContact">The gen contact.</param>
        /// <returns></returns>
        Task<GenContact> SaveContact(GenContact genContact);

        Task<List<sproc_UTS_GetContactDetails_Result>> GetContactDetails(long? CompanyId);

        Task<List<GenContactPointofContact>> GetPointofContact(long? CompanyId);
        Task<List<GenContactPointofContact>> GetPointOfContactForCompany(long? CompanyId);
        Task<GenCompanyContractDetail> GetLegalContact(long? CompanyId);
        Task<GenContact> GetContactDetailsByID(long? ContactId);
    }
}
