using Microsoft.EntityFrameworkCore;
using UTSATSAPI.Helpers;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels.ResponseModels;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Repositories.Repositories
{
    /// <summary>
    /// HubSpotRepository
    /// </summary>
    /// <seealso cref="UTSATSAPI.Repositories.Interfaces.IHubSpotRepository" />
    public class HubSpotRepository : IHubSpotRepository
    {
        #region Variables
        /// <summary>
        /// The database
        /// </summary>
        private TalentConnectAdminDBContext _db;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="HubSpotRepository"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        public HubSpotRepository(TalentConnectAdminDBContext db)
        {
            _db = db;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the hub spot contact by contact identifier.
        /// </summary>
        /// <param name="ContactId">The contact identifier.</param>
        /// <returns></returns>
        public async Task<GenHubSpotContact?> GetHubSpotContactByContactID(long ContactId)
        {
            return await _db.GenHubSpotContacts.FirstOrDefaultAsync(x => x.ContactId == ContactId);
        }


        /// <summary>
        /// Saves the hub spot contact.
        /// </summary>
        /// <param name="genHubSpotContact">The gen hub spot contact.</param>
        /// <returns></returns>
        public async Task<GenHubSpotContact> SaveHubSpotContact(GenHubSpotContact genHubSpotContact)
        {
            if (genHubSpotContact.Id > 0)
            {
                _db.GenHubSpotContacts.Update(genHubSpotContact);
                await _db.SaveChangesAsync();
            }
            else
            {
                await _db.GenHubSpotContacts.AddAsync(genHubSpotContact);
                await _db.SaveChangesAsync();
            }

            return genHubSpotContact;
        }

        /// <summary>
        /// Gets the hub spot company by identifier.
        /// </summary>
        /// <param name="HubSpotCompanyId">The hub spot company identifier.</param>
        /// <returns></returns>
        public async Task<GenHubSpotCompany?> GetHubSpotCompanyById(long HubSpotCompanyId)
        {
            return await _db.GenHubSpotCompanies.FirstOrDefaultAsync(x => x.CompanyId == HubSpotCompanyId);
        }

        /// <summary>
        /// Gets the hub spot company by company name and owner identifier.
        /// </summary>
        /// <param name="CompanyName">Name of the company.</param>
        /// <param name="HubSpotOwnerId">The hub spot owner identifier.</param>
        /// <returns></returns>
        public async Task<GenHubSpotCompany?> GetHubSpotCompanyByCompanyNameAndOwnerId(string CompanyName, long HubSpotOwnerId)
        {
            return await _db.GenHubSpotCompanies.FirstOrDefaultAsync(x => x.Company == CompanyName && x.HubspotOwnerId == HubSpotOwnerId);
        }

        /// <summary>
        /// Saves the hub spot company.
        /// </summary>
        /// <param name="genHubSpotCompany">The gen hub spot company.</param>
        /// <returns></returns>
        public async Task<GenHubSpotCompany> SaveHubSpotCompany(GenHubSpotCompany genHubSpotCompany)
        {
            if (genHubSpotCompany.Id > 0)
            {
                _db.GenHubSpotCompanies.Update(genHubSpotCompany);
                await _db.SaveChangesAsync();
            }
            else
            {
                await _db.GenHubSpotCompanies.AddAsync(genHubSpotCompany);
                await _db.SaveChangesAsync();
            }

            return genHubSpotCompany;
        }

        /// <summary>
        /// Saves the hubspot webhook notification.
        /// </summary>
        /// <param name="hubspotWebhookNotification">The hubspot webhook notification.</param>
        /// <returns></returns>
        public async Task<HubspotWebhookNotification> SaveHubspotWebhookNotification(HubspotWebhookNotification hubspotWebhookNotification)
        {
            if (hubspotWebhookNotification.Id > 0)
            {
                _db.HubspotWebhookNotifications.Update(hubspotWebhookNotification);
                await _db.SaveChangesAsync();
            }
            else
            {
                await _db.HubspotWebhookNotifications.AddAsync(hubspotWebhookNotification);
                await _db.SaveChangesAsync();
            }

            return hubspotWebhookNotification;
        }

        public async Task<List<sproc_UTS_GetAutoCompleteHubSpotCompanies_Result>> GetAutoCompleteHubSpotAllCompanies(string Company)
        {
            return await _db.Set<sproc_UTS_GetAutoCompleteHubSpotCompanies_Result>().FromSqlRaw(String.Format("{0} '{1}'", Constants.ProcConstant.sproc_UTS_GetAutoCompleteHubSpotCompanies, Company)).ToListAsync();
        }

        public void Sproc_AddUpdate_Hubspot_Company(string paramString)
        {
            _db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_AddUpdate_Hubspot_Company, paramString));            
        }
        public void sproc_UpdateCompanyLogo_forHubpsoCompany(string paramString)
        {
            _db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_UpdateCompanyLogo_forHubpsoCompany, paramString));
        }

        public void Sproc_AddUpdate_HubSpot_Contact(string paramString)
        {
            _db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_AddUpdate_HubSpot_Contact, paramString));
        }
        //public async Task<GenCompany> GetGenCompanysById(long? CompanyId)
        //{
        //    return await _db.GenCompanies.Where(x => x.Id == CompanyId).AsQueryable().FirstOrDefaultAsync();
        //}
        //public async Task<bool> SaveClientLeadUser(long? companyId, int? leadUserId)
        //{
        //    var companyLeadType_UserDetails = await _db.GenCompanyLeadTypeUserDetails.Where(x => x.CompanyId == companyId).FirstOrDefaultAsync();

        //    if (companyLeadType_UserDetails != null)
        //    {
        //        companyLeadType_UserDetails.LeadTypeUserId = leadUserId;
        //        _db.Entry(companyLeadType_UserDetails).State = EntityState.Modified;
        //        _db.SaveChanges();

        //        //_unitOfWork.genCompanyLeadTypeUserDetails.Update(companyLeadType_UserDetails);

        //        //var result = _unitOfWork.Save();
        //        //if (result > 0)
        //        //    return true;
        //        //else
        //        //    return false;
        //    }
        //    else
        //    {
        //        GenCompanyLeadTypeUserDetail gen_CompanyLeadType_UserDetails = new GenCompanyLeadTypeUserDetail
        //        {
        //            CompanyId = companyId,
        //            LeadTypeUserId = leadUserId
        //        };

        //        _db.GenCompanyLeadTypeUserDetails.Add(gen_CompanyLeadType_UserDetails);
        //        _db.SaveChanges();
        //        //var result = _unitOfWork.Save();
        //        //if (result > 0)
        //        //    return true;
        //        //else
        //        //    return false;


        //    }
        //    return true;
        //}

        //public async Task<GenContact> GetGenContactByIdwithIsActive(long contactId)
        //{
        //    return await _db.GenContacts.Where(x => x.Id == contactId && x.IsPrimary == true).FirstOrDefaultAsync();
        //}

        //public async Task<bool> UpdateGenContact(GenContact genContact)
        //{
        //    if (genContact != null)
        //    {
        //        var varObj = await _db.GenContacts.Where(x => x.Id == genContact.Id).FirstOrDefaultAsync();
        //        if (varObj != null)
        //        {
        //            _db.GenContacts.Update(genContact);
        //            await _db.SaveChangesAsync();
        //        }
        //    }
        //    return true;
        //}

        //public async Task<bool> CreateandupdateGenContact(GenContact genContact)
        //{
        //    if (genContact != null)
        //    {
        //        var varObj = await _db.GenContacts.AsNoTracking().Where(x => x.Id == genContact.Id).FirstOrDefaultAsync().ConfigureAwait(false);
        //        if (varObj != null)
        //        {
        //            _db.GenContacts.Update(genContact);
        //            await _db.SaveChangesAsync();
        //        }
        //        else
        //        {
        //            await _db.GenContacts.AddAsync(genContact);
        //            await _db.SaveChangesAsync();
        //        }
        //    }
        //    return true;
        //}

        //public async Task<GenCompanyContractDetail> GetGenCompanyContractDetailsById(long id)
        //{
        //    return await _db.GenCompanyContractDetails.Where(x => x.Id == id).FirstOrDefaultAsync();
        //}


        //public async Task<IEnumerable<GenContactPointofContact>> GetGenContactPointofContactsListByContactId(long contactId)
        //{
        //    return await _db.GenContactPointofContacts.Where(x => x.ContactId == contactId).ToListAsync();
        //}

        //public async Task<GenContactPointofContact> GetGenContactPointofContactsByContactIdUserId(long contactId, long userId)
        //{
        //    return await _db.GenContactPointofContacts.Where(x => x.ContactId == contactId && x.UserId == userId).FirstOrDefaultAsync();
        //}

        //public async Task<UsrUser> GetUsrUserById(long id)
        //{
        //    return await _db.UsrUsers.Where(x => x.Id == id).FirstOrDefaultAsync();
        //}

        //public async Task<bool> CreateGenContactPointofContacts(GenContactPointofContact genContactPointofContacts)
        //{
        //    if (genContactPointofContacts != null)
        //    {
        //        await _db.GenContactPointofContacts.AddAsync(genContactPointofContacts);
        //        await _db.SaveChangesAsync();
        //        return true;

        //    }
        //    return false;
        //}

        //public async Task<bool> UpdateGenContactPointofContacts(GenContactPointofContact genContactPointofContacts)
        //{
        //    if (genContactPointofContacts != null)
        //    {
        //        var varObj = await _db.GenContactPointofContacts.Where(x => x.Id == genContactPointofContacts.Id).FirstOrDefaultAsync();
        //        if (varObj != null)
        //        {
        //            _db.GenContactPointofContacts.Update(genContactPointofContacts);
        //            await _db.SaveChangesAsync();
        //            return true;

        //        }
        //    }
        //    return false;
        //}

        //public async Task<bool> DeleteGenContactPointofContactByList(IEnumerable<GenContactPointofContact> genContactPointofContactList)
        //{
        //    if (genContactPointofContactList != null && genContactPointofContactList.Count() > 0)
        //    {
        //        var genContactPointofContactIds = genContactPointofContactList.Select(x => x.Id)
        //                                                                      .ToList();
        //        var varObj = await _db.GenContactPointofContacts.Where(x => genContactPointofContactIds.Contains(x.Id)).ToListAsync();
        //        if (varObj != null)
        //        {
        //            _db.GenContactPointofContacts.RemoveRange(varObj);
        //            await _db.SaveChangesAsync();
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        //public async Task<bool> UpdateGenCompanies(GenCompany genCompany)
        //{
        //    if (genCompany != null)
        //    {
        //        var varObj = await _db.GenCompanies.Where(x => x.Id == genCompany.Id).FirstOrDefaultAsync();
        //        if (varObj != null)
        //        {
        //            _db.GenCompanies.Update(genCompany);
        //           await  _db.SaveChangesAsync();
        //            return true;

        //        }
        //    }
        //    return false;
        //}

        //public async Task<GenContact> GetContactDetailsByID(long? ContactId)
        //{
        //   return await _db.GenContacts.Where(xy => xy.Id == ContactId).FirstOrDefaultAsync();
        //}

        #endregion
    }
}
