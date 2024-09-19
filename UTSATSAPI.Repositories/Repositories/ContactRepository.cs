using Microsoft.EntityFrameworkCore;
using UTSATSAPI.Helpers;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Repositories.Repositories
{
    /// <summary>
    /// ContactRepository
    /// </summary>
    /// <seealso cref="UTSATSAPI.Repositories.Interfaces.IContactRepository" />
    public class ContactRepository : IContactRepository
    {
        #region Variables

        /// <summary>
        /// The database
        /// </summary>
        private TalentConnectAdminDBContext _db;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactRepository"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        public ContactRepository(TalentConnectAdminDBContext db)
        {
            _db = db;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the contact by hub spot identifier and emil.
        /// </summary>
        /// <param name="HubSpotContactId">The hub spot contact identifier.</param>
        /// <param name="Email">The email.</param>
        /// <returns></returns>
        public async Task<GenContact?> GetContactByHubSpotIdAndEmil(long HubSpotContactId, string Email)
        {
            return await _db.GenContacts.FirstOrDefaultAsync(x => x.HubSpotContactId == HubSpotContactId && x.EmailId == Email);
        }

        /// <summary>
        /// Saves the contact.
        /// </summary>
        /// <param name="genContact">The gen contact.</param>
        /// <returns></returns>
        public async Task<GenContact> SaveContact(GenContact genContact)
        {
            if (genContact.Id > 0)
            {
                _db.GenContacts.Update(genContact);
                await _db.SaveChangesAsync();
            }
            else
            {
                await _db.GenContacts.AddAsync(genContact);
                await _db.SaveChangesAsync();
            }

            return genContact;
        }


        public async Task<List<sproc_UTS_GetContactDetails_Result>> GetContactDetails(long? CompanyId)
        {
            return await _db.Set<sproc_UTS_GetContactDetails_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_GetContactDetails, CompanyId)).ToListAsync();

        }
        public async Task<List<GenContactPointofContact>> GetPointofContact(long? CompanyId)
        {
            List<GenContactPointofContact> genContactPointofContacts = new();
            GenContact gencontact = _db.GenContacts.Where(xy => xy.CompanyId == CompanyId && xy.IsPrimary == true).FirstOrDefault();
            if (gencontact != null)
            {
                genContactPointofContacts = await _db.GenContactPointofContacts.Where(x => x.ContactId == gencontact.Id).ToListAsync();
            }
            return genContactPointofContacts;

        }
        public async Task<List<GenContactPointofContact>> GetPointOfContactForCompany(long? CompanyId)
        {
            List<GenContactPointofContact> genContactPointofContacts = new();
            //GenContact gencontact = _db.GenContacts.Where(xy => xy.CompanyId == CompanyId && xy.IsPrimary == true).FirstOrDefault();
            //if (gencontact != null)
            //{
                genContactPointofContacts = await _db.GenContactPointofContacts.Where(x => x.CompanyId == CompanyId).ToListAsync();
            //}
            return genContactPointofContacts;

        }
        public async Task<GenCompanyContractDetail> GetLegalContact(long? CompanyId)
        {
            return await _db.GenCompanyContractDetails.Where(x => x.CompanyId == CompanyId).FirstOrDefaultAsync();
        }

        public async Task<GenContact> GetContactDetailsByID(long? ContactId)
        {
            return await _db.GenContacts.Where(xy => xy.Id == ContactId).FirstOrDefaultAsync();
        }


        #endregion
    }
}
