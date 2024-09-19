namespace UTSATSAPI.Repositories.Repositories
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using System.Data;
    using UTSATSAPI.Helpers;
    using UTSATSAPI.Helpers.Common;
    using UTSATSAPI.Models.ComplexTypes;
    using UTSATSAPI.Models.Generic;
    using UTSATSAPI.Models.Models;
    using UTSATSAPI.Models.ViewModels;
    using UTSATSAPI.Models.ViewModels.Request_ResponseModels;
    using UTSATSAPI.Repositories.Interfaces;


    public class ClientRepository : IClient
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        private IUnitOfWork _unitOfWork;
        private IConfiguration iConfiguration;
        private readonly IUniversalProcRunner _universalProcRunner;
        #endregion


        #region Constructor
        public ClientRepository(TalentConnectAdminDBContext _db, IUnitOfWork unitOfWork, IConfiguration _iConfiguration, IUniversalProcRunner universalProcRunner)
        {
            this.db = _db;
            this._unitOfWork = unitOfWork;
            this.iConfiguration = _iConfiguration;
            _universalProcRunner = universalProcRunner;
        }

        #endregion

        #region Public Repo
        public async Task<List<Sproc_GetPointOfContact_UserDetails_Result>> Sproc_GetPointOfContact_UserDetails(string param)
        {
            return await db.Set<Sproc_GetPointOfContact_UserDetails_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_GetPointOfContact_UserDetails, param)).ToListAsync();
        }
        public List<Sproc_GetActiveSalesUserList_Result> Sproc_GetActiveSalesUserLists()
        {
            return  db.Set<Sproc_GetActiveSalesUserList_Result>().FromSqlRaw(String.Format("{0}", Constants.ProcConstant.Sproc_GetActiveSalesUserList)).ToList();
        }
        public List<Sproc_Get_CreditWithAmount_Result> Sproc_Get_CreditWithAmountLists()
        {
            return db.Set<Sproc_Get_CreditWithAmount_Result>().FromSqlRaw(String.Format("{0}", Constants.ProcConstant.Sproc_Get_CreditWithAmount)).ToList();
        }
        public async Task<List<sproc_UTS_GetClientList_Result>> sproc_GetClient(string param)
        {
            return await db.Set<sproc_UTS_GetClientList_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_GetClientList, param)).ToListAsync();
        }
        public async Task<IEnumerable<UsrUser>> GetUserListForAMChange(long OldUserId)
        {
            return await _unitOfWork.usrUsers.GetAllByCondition(x => x.IsActive == true && (x.UserTypeId == 4 || x.UserTypeId == 9) && x.Id != OldUserId);
        }
        public void UpdateAMDetails(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_AM_Change_For_Company, param));
        }
        public async Task<List<sp_UTS_GetPOCDetails_Result>> GetPOCDetails(string param)
        {
            return await db.Set<sp_UTS_GetPOCDetails_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sp_UTS_GetPOCDetails, param)).ToListAsync();
        }
        public sp_UTS_GetCompanyClientDetails_Result GetCompanyClientDetails(string param)
        {
            return db.Set<sp_UTS_GetCompanyClientDetails_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sp_UTS_GetCompanyClientDetails, param)).AsEnumerable().FirstOrDefault();
        }
        public async Task<List<sp_UTS_GetClientWiseHRDetails_Result>> GetClientWiseHRDetails(string param)
        {
            return await db.Set<sp_UTS_GetClientWiseHRDetails_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sp_UTS_GetClientWiseHRDetails, param)).ToListAsync();
        }
        public async Task<List<Sproc_Get_Hierarchy_For_Email_Result>> GetHierarchyForEmail(string param)
        {
            return await db.Set<Sproc_Get_Hierarchy_For_Email_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_Hierarchy_For_Email, param)).ToListAsync();
        }
        public async Task<List<sproc_GetClientHappinessSurvey_Result>> sproc_GetClientHappinessSurvey(string param)
        {
            return await db.Set<sproc_GetClientHappinessSurvey_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_GetClientHappinessSurvey, param)).ToListAsync();
        }
        public async Task<GenCompany> GetCompanyByName(string companyName)
        {
            return await _unitOfWork.genCompanys.GetSingleByCondition(x => x.Company.ToLower() == companyName.ToLower());
        }
        public async Task<GenContact> GetGenContactById(long contactId)
        {
            return await _unitOfWork.genContacts.GetSingleByCondition(x => x.Id == contactId );
        }
        public async Task<GenContact> GetGenContactByCompanyId(long companyId)
        {
            return await _unitOfWork.genContacts.GetSingleByCondition(x => x.CompanyId == companyId && x.IsPrimary == true);
        }
        public async Task<GenTalent> GetGenTalentByEmail(string email)
        {
            return await _unitOfWork.genTalents.GetSingleByCondition(x => x.EmailId.ToLower() == email.ToLower());
        }
        public async Task<GenContact> GetGenContactByEmail(string email)
        {
            return await _unitOfWork.genContacts.GetSingleByCondition(x => x.EmailId.ToLower() == email.ToLower());
        }
        public async Task<IEnumerable<UsrUser>> GetUsrUserList()
        {
            return await _unitOfWork.usrUsers.GetAll();
        }
        public async Task<UsrUser> GetUsrUserListByLoggedInUserId(long loggedInUserId)
        {
            UsrUser Usersdetail = (from u in await _unitOfWork.usrUsers.GetAll()
                                   join ut in await _unitOfWork.usrUserTypes.GetAll() on u.UserTypeId equals ut.Id
                                   where u.Id == loggedInUserId && ut.IsRequiredforFrontEnd == true
                                   select u).FirstOrDefault();

            return Usersdetail;
        }
        public async Task<PrgGeo> GetPrgGeosById(int prgGeosId)
        {
            return await _unitOfWork.prgGeos.GetSingleByCondition(x => x.Id == prgGeosId);
        }
        public async Task<IEnumerable<GenCompany>> GetAllGenCompanysById(long id)
        {
            return await _unitOfWork.genCompanys.GetAllByCondition(xy => xy.Id == id);
        }
        public async Task<GenCompany> GetGenCompanysById(long id)
        {
            return await _unitOfWork.genCompanys.GetSingleByCondition(xy => xy.Id == id);
        }
        public async Task<UsrUser> GetUsrUserById(long id)
        {
            return await _unitOfWork.usrUsers.GetSingleByCondition(x => x.Id == id);
        }
        public async Task<GenCompanyContractDetail> GetGenCompanyContractDetailsById(long id)
        {
            return await _unitOfWork.genCompanyContractDetails.GetSingleByCondition(x => x.Id == id);
        }
        public async Task<GenCompany> GetGenCompaniesById(long? id)
        {
            return await _unitOfWork.genCompanys.GetSingleByCondition(x => x.Id == id);
        }
        public async Task<GenContactPointofContact> GetGenContactPointofContactsByContactIdUserId(long contactId, long userId)
        {
            return await _unitOfWork.genContactPointofContacts.GetSingleByCondition(x => x.ContactId == contactId && x.UserId == userId);
        }
        public async Task<IEnumerable<GenContactPointofContact>> GetGenContactPointofContactsListByContactId(long contactId)
        {
            return await _unitOfWork.genContactPointofContacts.GetAllByCondition(x => x.ContactId == contactId);
        }
        public async Task<IEnumerable<GenContact>> GetGenContactListByCondition(long? companyId)
        {
            return await _unitOfWork.genContacts.GetAllByCondition(x => x.CompanyId == companyId && x.IsPrimary == false && x.IsActive == true);
        }
        public async Task<bool> CreateCompanyContractDetail(GenCompanyContractDetail genCompanyContractDetail)
        {
            if (genCompanyContractDetail != null)
            {
                await _unitOfWork.genCompanyContractDetails.Add(genCompanyContractDetail);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }
        public async Task<bool> UpdateCompanyContractDetail(GenCompanyContractDetail genCompanyContractDetail)
        {
            if (genCompanyContractDetail != null)
            {

                var varObj = await _unitOfWork.genCompanyContractDetails.GetSingleByCondition(x => x.Id == genCompanyContractDetail.Id).ConfigureAwait(false);
                if (varObj != null)
                {

                    _unitOfWork.genCompanyContractDetails.Update(genCompanyContractDetail);

                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
        public async Task<IEnumerable<SelectListItem>> GetUserslist()
        {
            var varUsrUsers = await _unitOfWork.usrUsers.GetAll();
            var varUsrUserTypes = await _unitOfWork.usrUserTypes.GetAll();
            var result = (from u in varUsrUsers
                          join ut in varUsrUserTypes on u.UserTypeId equals ut.Id
                          where ut.IsRequiredforFrontEnd == true
                          orderby u.FullName ascending
                          select new SelectListItem { Text = u.FullName, Value = u.Id.ToString() }
                                  ).ToList();

            return result;
        }
        public async Task<bool> CreateGenContactPointofContacts(GenContactPointofContact genContactPointofContacts)
        {
            if (genContactPointofContacts != null)
            {
                //If Client + POC already exist in table then no need to add again
                var AlreadyExit =
                    await _unitOfWork.genContactPointofContacts.
                            GetSingleByCondition(x => x.ContactId == genContactPointofContacts.ContactId
                            && x.UserId == genContactPointofContacts.UserId).ConfigureAwait(false);

                if (AlreadyExit == null)
                {
                    await _unitOfWork.genContactPointofContacts.Add(genContactPointofContacts);

                    var result = _unitOfWork.Save();
                    if (result > 0)
                        return true;
                    else
                        return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
        public async Task<bool> UpdateGenContactPointofContacts(GenContactPointofContact genContactPointofContacts)
        {
            if (genContactPointofContacts != null)
            {
                //If Client + POC already exist in table then no need to add again
                var AlreadyExit =
                   await _unitOfWork.genContactPointofContacts.
                           GetSingleByCondition(x => x.ContactId == genContactPointofContacts.ContactId
                           && x.UserId == genContactPointofContacts.UserId).ConfigureAwait(false);

                if (AlreadyExit == null)
                {
                    var varObj = await _unitOfWork.genContactPointofContacts.GetSingleByCondition(x => x.Id == genContactPointofContacts.Id);
                    if (varObj != null)
                    {
                        _unitOfWork.genContactPointofContacts.Update(genContactPointofContacts);

                        var result = _unitOfWork.Save();

                        if (result > 0)
                            return true;
                        else
                            return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
        public async Task<bool> CreateGenCompanies(GenCompany genCompany)
        {
            if (genCompany != null)
            {
                await _unitOfWork.genCompanys.Add(genCompany);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }
        public async Task<bool> UpdateGenCompanies(GenCompany genCompany)
        {
            if (genCompany != null)
            {
                var varObj = await _unitOfWork.genCompanys.GetSingleByCondition(x => x.Id == genCompany.Id);
                if (varObj != null)
                {
                    _unitOfWork.genCompanys.Update(genCompany);

                    var result = _unitOfWork.Save();

                    if (result > 0)
                    {
                        #region UTS-6158 - Maintain history of Gen_Company
                        if (genCompany.Id > 0)
                            _universalProcRunner.Manipulation(Constants.ProcConstant.SPROC_Gen_Company_History, new object[] { genCompany.Id });
                        #endregion

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }
        public async Task<bool> CreateGenContact(GenContact genContact)
        {
            if (genContact != null)
            {
                await _unitOfWork.genContacts.Add(genContact);

                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }
        public async Task<bool> UpdateGenContact(GenContact genContact)
        {
            if (genContact != null)
            {
                var varObj = await _unitOfWork.genContacts.GetSingleByCondition(x => x.Id == genContact.Id);
                if (varObj != null)
                {
                    _unitOfWork.genContacts.Update(genContact);

                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
        public async Task<bool> DeleteGenContactsByList(IEnumerable<GenContact> genContactList)
        {
            if (genContactList != null && genContactList.Count() > 0)
            {
                var varObj = await _unitOfWork.genContacts.GetAllByCondition(x => genContactList.Any(y => y.Id == x.Id)).ConfigureAwait(false);
                if (varObj != null)
                {
                    _unitOfWork.genContacts.DeleteRange(varObj);
                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
        public async Task<bool> DeleteGenContactPointofContactByList(IEnumerable<GenContactPointofContact> genContactPointofContactList)
        {
            if (genContactPointofContactList != null && genContactPointofContactList.Count() > 0)
            {
                var genContactPointofContactIds = genContactPointofContactList.Select(x => x.Id)
                                                                              .ToList();
                var varObj = await db.GenContactPointofContacts.Where(x => genContactPointofContactIds.Contains(x.Id)).ToListAsync();
                if (varObj != null)
                {
                    db.GenContactPointofContacts.RemoveRange(varObj);
                    await db.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
        public async Task<bool> SaveClientLeadUser(long? companyId, int? leadUserId)
        {
            var companyLeadType_UserDetails = await _unitOfWork.genCompanyLeadTypeUserDetails.GetSingleByCondition(x => x.CompanyId == companyId);
            if (companyLeadType_UserDetails != null)
            {
                companyLeadType_UserDetails.LeadTypeUserId = leadUserId;
                _unitOfWork.genCompanyLeadTypeUserDetails.Update(companyLeadType_UserDetails);

                var result = _unitOfWork.Save();
                if (result > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                GenCompanyLeadTypeUserDetail gen_CompanyLeadType_UserDetails = new GenCompanyLeadTypeUserDetail
                {
                    CompanyId = companyId,
                    LeadTypeUserId = leadUserId
                };

                await _unitOfWork.genCompanyLeadTypeUserDetails.Add(gen_CompanyLeadType_UserDetails);

                var result = _unitOfWork.Save();
                if (result > 0)
                    return true;
                else
                    return false;


            }
        }
        public async Task<List<sproc_UTS_GetAutoCompleteCompanies_Result>> sproc_GetAutoCompleteCompanies(string param)
        {
            return await db.Set<sproc_UTS_GetAutoCompleteCompanies_Result>().FromSqlRaw(String.Format("{0} '{1}'", Constants.ProcConstant.sproc_UTS_GetAutoCompleteCompanies, param)).ToListAsync();

        }
        public async Task<bool> SaveHappinessSurveyFeedback(ClientHappinessSurveyViewModel ClientHappinessSurvey, long? loggedInUserId)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:MM");
            long? InsertedFeedbackId = 0;
            //long? InsertedCompanyId = 0;
            //long? InsertedContactId = 0;

            var ClientName = "";
            var CHeckTrueFalse = true;
            GenClientHappinessSurvey ObjclientHappinessSurvey = new GenClientHappinessSurvey();
            ObjclientHappinessSurvey.OtherClientEmailId = ClientHappinessSurvey.Email;
            //if (!string.IsNullOrEmpty(ClientHappinessSurvey.Other_Client_Name))
            //{
            //    GenContact ObjContact = new();
            //    ObjContact.FullName = ClientHappinessSurvey.Other_Client_Name;
            //    ClientName = ClientHappinessSurvey.Other_Client_Name;
            //    ObjContact.EmailId = ClientHappinessSurvey.Other_ClientEmail;
            //    ObjContact.Username = ClientHappinessSurvey.Other_ClientEmail;
            //    ObjContact.Password = "uplers@123";

            //    await _unitOfWork.genContacts.Add(ObjContact);

            //    var Contactresult = _unitOfWork.Save();
            //    InsertedContactId = ObjContact.Id;
            //    if (Contactresult > 0)
            //        CHeckTrueFalse = true;
            //    else
            //        CHeckTrueFalse =  false;
            //}

            //if (!string.IsNullOrEmpty(ClientHappinessSurvey.Other_Company_Name))
            //{
            //    GenCompany objCompany = new();
            //    objCompany.Company = ClientHappinessSurvey.Other_Company_Name;ClientName = ClientHappinessSurvey.Other_Client_Name;
            //    objCompany.CreatedById = (int)loggedInUserId;
            //    objCompany.CreatedByDatetime = DateTime.Now;
            //    await _unitOfWork.genCompanys.Add(objCompany);

            //    var Companyresult = _unitOfWork.Save();
            //    InsertedCompanyId = objCompany.Id;
            //}

            ObjclientHappinessSurvey.CreatedByDatetime = DateTime.Parse(date);
            if (ClientHappinessSurvey.Company_ID != null && ClientHappinessSurvey.Company_ID != 0)
            {
                ObjclientHappinessSurvey.CompanyId = ClientHappinessSurvey.Company_ID;
            }
            if (ClientHappinessSurvey.Client_ID != null && ClientHappinessSurvey.Client_ID != 0)
            {
                ObjclientHappinessSurvey.ContactId = ClientHappinessSurvey.Client_ID;

                var GenContact = await _unitOfWork.genContacts.GetSingleByCondition(x => x.Id == ClientHappinessSurvey.Client_ID);
                if (GenContact != null)
                {
                    ClientName = GenContact.FullName;
                }
            }
            if (ClientHappinessSurvey.Other_Client_Name != null)
            {
                ObjclientHappinessSurvey.OtherClientName = ClientHappinessSurvey.Other_Client_Name;
                ClientName = ClientHappinessSurvey.Other_Client_Name;
            }
            if (ClientHappinessSurvey.Other_Company_Name != null)
            {
                ObjclientHappinessSurvey.OtherCompanyName = ClientHappinessSurvey.Other_Company_Name;
            }
            if (ClientHappinessSurvey.Other_ClientEmail != null)
            {
                ObjclientHappinessSurvey.OtherClientEmailId = ClientHappinessSurvey.Other_ClientEmail;
            }
            ObjclientHappinessSurvey.CreatedById = (int)loggedInUserId;
            ObjclientHappinessSurvey.IsEmailSend = false;

            await _unitOfWork.genClientHappinessSurveys.Add(ObjclientHappinessSurvey);

            var result = _unitOfWork.Save();

            InsertedFeedbackId = ObjclientHappinessSurvey.Id;

            if (InsertedFeedbackId != 0 && InsertedFeedbackId > 0)
            {
                var ObjClientSurvey = await _unitOfWork.genClientHappinessSurveys.GetSingleByCondition(x => x.Id == InsertedFeedbackId);
                if (ObjClientSurvey != null)
                {
                    string FeedbackIdEncryted = CommonLogic.ClientHappinessSurveyEncrypt(Convert.ToString(InsertedFeedbackId));
                    string FrontEndProjectURL = iConfiguration["ClientHappinessFeedbackURL"].ToString() + "ClientHappinessSurveyFeedback/HappinessSurvey?N=" + FeedbackIdEncryted;

                    ObjClientSurvey.FeedbackUrl = FrontEndProjectURL;
                    // ObjClientSurvey.CompanyId = InsertedCompanyId;
                    // ObjClientSurvey.ContactId= InsertedContactId;
                    _unitOfWork.genClientHappinessSurveys.Update(ObjClientSurvey);

                    var Feedbackresult = _unitOfWork.Save();
                }
            }



            return CHeckTrueFalse;
        }
        public async Task<List<sproc_GetClientHappinessSurveyOptions_Result>> HappinessSurveysOption()
        {
            return await db.Set<sproc_GetClientHappinessSurveyOptions_Result>().FromSqlRaw(String.Format("{0}", Constants.ProcConstant.sproc_GetClientHappinessSurveyOptions)).ToListAsync();

        }
        public async Task<List<Sproc_Get_TrackingLead_Details_for_ClientSource_Result>> TrackingLeadDetailClientSource(long? ContactID)
        {
            return await db.Set<Sproc_Get_TrackingLead_Details_for_ClientSource_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_TrackingLead_Details_for_ClientSource, ContactID)).ToListAsync();
        }

        public List<Sproc_GET_CreditPlanDetails_ClientPortal_Result> GetCreditTransaction(string paramasString)
        {
            return db.Set<Sproc_GET_CreditPlanDetails_ClientPortal_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_GET_CreditPlanDetails_ClientPortal, paramasString)).ToList();
        }
        public async Task<bool> SendEmailForHappinessSurveyFeedback(long feedbackID, long? LoggedInUserId)
        {
            string EmailSuccessMsg = "";
            ClientHappinessSurveyViewModel clientHappinessSurveyViewModel = new ClientHappinessSurveyViewModel();
            var clientHappinessSurvey = await _unitOfWork.genClientHappinessSurveys.GetSingleByCondition(x => x.Id == feedbackID);

            var ObjContact = await _unitOfWork.genContacts.GetSingleByCondition(x => x.Id == clientHappinessSurvey.ContactId && x.IsPrimary == true);
            clientHappinessSurveyViewModel.Link = clientHappinessSurvey.FeedbackUrl;
            var ClientName = "";
            if (clientHappinessSurvey.OtherClientEmailId != null)
            {
                clientHappinessSurveyViewModel.Email = clientHappinessSurvey.OtherClientEmailId;
            }
            else
            {
                clientHappinessSurveyViewModel.Email = ObjContact.EmailId;
            }
            if (clientHappinessSurvey.OtherClientName == null)
            {
                clientHappinessSurveyViewModel.Client_Name = ObjContact.FullName;
                ClientName = ObjContact.FullName;
            }
            else
            {
                clientHappinessSurveyViewModel.Client_Name = clientHappinessSurvey.OtherClientName;
                ClientName = clientHappinessSurvey.OtherClientName;
                clientHappinessSurveyViewModel.Email = clientHappinessSurvey.OtherClientEmailId;
            }

            if (clientHappinessSurvey.IsEmailSend == true)
            {
                EmailSuccessMsg = "fail";
            }
            else
            {

                #region SendEmail
                EmailBinder emailBinder = new EmailBinder(iConfiguration, db);

                EmailSuccessMsg = emailBinder.SendEmailForAddingClientHappinessSurvey(feedbackID, clientHappinessSurveyViewModel.Email, clientHappinessSurveyViewModel.Link, ClientName, LoggedInUserId);

                #endregion
            }
            if (EmailSuccessMsg == "success")
            {
                clientHappinessSurvey.IsEmailSend = true;

                _unitOfWork.genClientHappinessSurveys.Update(clientHappinessSurvey);
                var Feedbackresult = _unitOfWork.Save();

            }
            return true;
        }
        public async Task<PrgCompanyType> GetPrgCompanyTypeById(int? Id)
        {
            return db.PrgCompanyTypes.Where(x => x.Id == Id).FirstOrDefault();
        }
        public async Task<bool> updateGenCompany(long? ContactID, CompanyInfo companyInfo)
        {
            if (companyInfo != null && ContactID > 0)
            {
                var _contact = db.GenContacts.AsNoTracking().Where(x => x.Id == ContactID).FirstOrDefault();

                if (_contact != null && _contact.CompanyId > 0)
                {
                    var genCompany = db.GenCompanies.AsNoTracking().Where(x => x.Id == _contact.CompanyId).FirstOrDefault();

                    if (genCompany != null)
                    {
                        if (!string.IsNullOrEmpty(companyInfo.aboutCompanyDesc))
                        {
                            genCompany.AboutCompanyDesc = companyInfo.aboutCompanyDesc;
                        }
                        if (!string.IsNullOrEmpty(companyInfo.companyName))
                        {
                            genCompany.Company = companyInfo.companyName;
                        }
                        if (companyInfo.companySize != null && companyInfo.companySize > 0)
                        {
                            genCompany.CompanySize = companyInfo.companySize;
                        }
                        if (!string.IsNullOrEmpty(companyInfo.website))
                        {
                            genCompany.Website = companyInfo.website;
                        }
                        if (!string.IsNullOrEmpty(companyInfo.linkedInURL))
                        {
                            genCompany.LinkedInProfile = companyInfo.linkedInURL;
                        }
                        if (!string.IsNullOrEmpty(companyInfo.industry))
                        {
                            genCompany.Industry = companyInfo.industry;
                            genCompany.IndustryType = companyInfo.industry;
                        }

                        db.GenCompanies.Attach(genCompany);
                        db.Entry(genCompany).State = EntityState.Modified;
                        db.SaveChanges();

                        #region UTS-6158 - Maintain history of Gen_Company
                        if (genCompany.Id > 0)
                            _universalProcRunner.Manipulation(Constants.ProcConstant.SPROC_Gen_Company_History, new object[] { genCompany.Id });
                        #endregion

                        return true;
                    }
                    return true;
                }
            }
            return true;
        }
        public Sp_UTS_PreviewJobPost_ClientPortal_Result Sp_UTS_PreviewJobPost_ClientPortal_Result(string param)
        {
            return db.Set<Sp_UTS_PreviewJobPost_ClientPortal_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sp_UTS_PreviewJobPost_ClientPortal, param)).AsEnumerable().FirstOrDefault();
        }

        public Sproc_UTS_GetAddClientAccess_Result Sproc_UTS_GetAddClientAccess(string param)
        {
            return db.Set<Sproc_UTS_GetAddClientAccess_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_UTS_GetAddClientAccess, param)).AsEnumerable().FirstOrDefault();
        }

        public async Task<GenContact> SignUp(ClientSignUp clientSignUp,bool IsHobspot)
        {
            GenContact genContact = new GenContact();

            string firstName = string.Empty;
            string lastName = string.Empty;

            if (!string.IsNullOrWhiteSpace(clientSignUp.FullName) && clientSignUp.FullName.Contains(" "))
            {
                var firstSpaceIndex = clientSignUp.FullName.IndexOf(" ");
                firstName = clientSignUp.FullName.Substring(0, firstSpaceIndex); // first name
                lastName = clientSignUp.FullName.Substring(firstSpaceIndex + 1); // last name             
            }

            object[] param = new object[]
            {
                clientSignUp.FullName,
                clientSignUp.WorkEmail,
                clientSignUp.CompanyName,
                clientSignUp.Password,
                clientSignUp.ClientIPAddress,
                clientSignUp.OTP,
                firstName,
                lastName,
                clientSignUp.FreeCredit,
                clientSignUp.CompanyTypeId,
                clientSignUp.InvitingUserId,
                clientSignUp.CookieId,
                clientSignUp.CompanyURL,
                clientSignUp.CompanySize_RangeorAdhoc,
                clientSignUp.CompanyIndustryType,
                clientSignUp.BriefAboutCompany,
                clientSignUp.UTMCountry,
                clientSignUp.UTMState,
                clientSignUp.UTMCity,
                clientSignUp.UTMBrowser,
                clientSignUp.UTMDevice,
                clientSignUp.ContactNumber,
                clientSignUp.IsPostaJob,
                clientSignUp.IsProfileView,
                clientSignUp.IsHybridModel.Value ? 1 : 0,
                clientSignUp.IsHybridModel,
                IsHobspot,
                clientSignUp.IsVettedProfile,
                clientSignUp.POC_ID,
                clientSignUp.CompanyLogo,
                clientSignUp.CreditAmount,
                clientSignUp.CreditCurrency,
                clientSignUp.JobPostCredit,
                clientSignUp.VettedProfileViewCredit,
                clientSignUp.NonVettedProfileViewCredit,
                clientSignUp.IsTransparentPricing,
                clientSignUp.IsPayPerHire,
                clientSignUp.IsPayPerCredit
            };

            string paramString = CommonLogic.ConvertToParamString(param);

            Sproc_RegisterClient_ClientPortal_Result result = db.Set<Sproc_RegisterClient_ClientPortal_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_InviteClient_UTS_Admin, paramString)).AsEnumerable().FirstOrDefault();

            if (result != null)
            {
                genContact = await _unitOfWork.genContacts.GetSingleByCondition(x => x.Id == result.ContactID);
            }

            return genContact;
        }
        public async Task<UsrUser> UserDetails(long id = 0)
        {
            var user = await _unitOfWork.usrUsers.GetSingleByCondition(x => x.Id == id);
            return user;
        }
        public Sproc_ValidateAddClient_Result Sproc_Validate_AddClient(string param)
        {
            return db.Set<Sproc_ValidateAddClient_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_ValidateAddPayPerCreditClient, param)).AsEnumerable().FirstOrDefault();
        }

        //public Sproc_ValidateAddClient_Result Sproc_Validate_AddClientTemp(string param)
        //{
        //    return db.Set<Sproc_ValidateAddClient_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_ValidateAddPayPerCreditClient_temp, param)).AsEnumerable().FirstOrDefault();
        //}

        public List<Sproc_Fetch_All_SalesUsers_WithHead_For_Client_Result> Sproc_Fetch_All_SalesUsers_WithHead_For_Client(string param)
        {
            return db.Set<Sproc_Fetch_All_SalesUsers_WithHead_For_Client_Result>().FromSqlRaw(String.Format("{0} '{1}'", Constants.ProcConstant.Sproc_Fetch_All_SalesUsers_WithHead_For_Client,param)).ToList();
        }

        public void Sproc_Update_SpaceID_For_Client(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_Update_SpaceID_For_Client, param));
        }
        public List<Sproc_Get_SalesUserWithHead_FOr_HiringRequest_Result> Sproc_Get_SalesUserWithHead_FOr_HiringRequest(long param)
        {
            return db.Set<Sproc_Get_SalesUserWithHead_FOr_HiringRequest_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_SalesUserWithHead_FOr_HiringRequest, param)).ToList();
        }

        public async Task<List<Sproc_Get_Credit_Transaction_CompanyWise_Result>> Sproc_Get_Credit_Transaction_CompanyWise(string param)
        {
            return await db.Set<Sproc_Get_Credit_Transaction_CompanyWise_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_Credit_Transaction_CompanyWise, param)).ToListAsync();
        }

        public void Sproc_Reset_AllHR_TalentStatus()
        {
            db.Database.ExecuteSqlRaw(String.Format("{0}", Constants.ProcConstant.Sproc_Reset_AllHR_TalentStatus));
        }

        public void sproc_Add_AWS_Email_Payload_Result(string Payload)
        {

            var PayloadData = new SqlParameter("@Payload", SqlDbType.NVarChar) { Value = Payload };
            
            db.Database.ExecuteSqlRaw("EXEC Sproc_Add_AWS_Email_Payload @Payload", parameters: new[] { PayloadData });

           // db.Database.ExecuteSqlRaw(string.Format("{0} '{1}'", Constants.ProcConstant.Sproc_Add_AWS_Email_Payload, Payload));
        }

        public void Sproc_Add_AWS_SES_EmailTracking(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_Add_AWS_SES_EmailTracking,param));
        }

       
        #endregion
    }
}
