namespace UTSATSAPI.Controllers
{
    using AutoMapper;
    using ClosedXML.Excel;
    using DevExpress.Data.Filtering.Helpers;
    using DocumentFormat.OpenXml.Drawing;
    using FluentValidation.Results;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using RestSharp;
    using System;
    using System.Dynamic;
    using System.Net;
    using System.Text.RegularExpressions;
    using UTSATSAPI.ATSCalls;
    using UTSATSAPI.ChatGPTCalls;
    using UTSATSAPI.Config;
    using UTSATSAPI.Helpers;
    using UTSATSAPI.Helpers.Common;
    using UTSATSAPI.Models.ComplexTypes;
    using UTSATSAPI.Models.Models;
    using UTSATSAPI.Models.ViewModel;
    using UTSATSAPI.Models.ViewModels;
    using UTSATSAPI.Models.ViewModels.Reports;
    using UTSATSAPI.Models.ViewModels.ResponseModels;
    using UTSATSAPI.Models.ViewModels.Validators;
    using UTSATSAPI.Repositories.Interfaces;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
    using static UTSATSAPI.Config.HubSpotResponseUTSAdmin;
    using static UTSATSAPI.Helpers.Enum;

    [Authorize]
    [ApiController]
    [Route("Client/")]
    public class ClientController : ControllerBase
    {
        #region Variables
        private readonly TalentConnectAdminDBContext _talentConnectAdminDBContext;
        private readonly IMapper _mapper;
        private readonly IUniversalProcRunner _universalProcRunner;
        private readonly IConfiguration _configuration;
        private readonly IClient _iClient;
        private readonly ICompany _iCompany;
        private readonly IReport _iReport;
        #endregion


        #region Constructors
        public ClientController(TalentConnectAdminDBContext talentConnectAdminDBContext, IUniversalProcRunner universalProcRunner, IMapper mapper, IConfiguration configuration, IClient iClient, IReport iReport,ICompany icompany)
        {
            _talentConnectAdminDBContext = talentConnectAdminDBContext;
            _mapper = mapper;
            _universalProcRunner = universalProcRunner;
            _configuration = configuration;
            _iClient = iClient;
            _iReport = iReport;
            _iCompany = icompany;
        }
        #endregion


        #region Authorized API
        [HttpGet("CheckDuplicateCompanyName")]
        public async Task<ObjectResult> CheckDuplicateCompanyName(string companyName)
        {
            try
            {
                if (!string.IsNullOrEmpty(companyName))
                {
                    var companyFound = await _iClient.GetCompanyByName(companyName).ConfigureAwait(false);

                    if (companyFound != null)
                    {
                        createClientRequest returnCompany = new createClientRequest();


                        //returnCompany.company = ModelBinder.BindResponseModelCompany(returnCompany.company, companyFound);
                        returnCompany.company = _mapper.Map<Company>(companyFound);


                        GenContact genContact = new GenContact();
                        genContact = await _iClient.GetGenContactByCompanyId(companyFound.Id).ConfigureAwait(false);
                        if (genContact != null)
                            returnCompany.primaryClient = ModelBinder.BindResponseModelPrimaryClient(returnCompany.primaryClient, genContact);

                        return StatusCode(StatusCodes.Status409Conflict, new ResponseObject() { statusCode = StatusCodes.Status409Conflict, Message = "Company name is Exists", Details = returnCompany });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "No duplicate company name found" });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide company name" });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("CheckDuplicateEmail")]
        public async Task<ObjectResult> CheckDuplicateEmail(string email)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    var talentFound = await _iClient.GetGenTalentByEmail(email).ConfigureAwait(false);
                    var contactFound = await _iClient.GetGenContactByEmail(email).ConfigureAwait(false);
                    if (talentFound != null || contactFound != null)
                    {
                        return StatusCode(StatusCodes.Status409Conflict, new ResponseObject() { statusCode = StatusCodes.Status409Conflict, Message = "Email is Exists" });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "No duplicate email name found" });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide email" });

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("Create")]
        public async Task<ObjectResult> CreateClient(createClientRequest createClientRequest, long LoggedInUserId = 0)
        {
            try
            {
                LoggedInUserId = SessionValues.LoginUserId;

                decimal? decJpcreditBalance = createClientRequest.company.JPCreditBalance;

                long InsertedClientId = 0;
                long? InsertedCompanyId = 0;
                GenContact genContact = new GenContact();


                #region Pre Validation
                if (createClientRequest == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty" });
                }
                #endregion

                #region Validation
                if (!createClientRequest.isSaveasDraft)
                {
                    #region Company
                    CompanyModelValidator companyValidator = new CompanyModelValidator();
                    ValidationResult validationResult_companyValidator = companyValidator.Validate(createClientRequest.company);
                    if (!validationResult_companyValidator.IsValid)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult_companyValidator.Errors, "company") });
                    }

                    // As we should not allow to add company, only update is allowed.
                    if (string.IsNullOrEmpty(createClientRequest.company.en_Id))
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please choose any company or fetch details from hubspot." });
                    }
                    #endregion

                    #region Primary Client
                    PrimaryClientModelValidator primaryClientvalidator = new PrimaryClientModelValidator();
                    ValidationResult validationResult_primaryClientvalidator = primaryClientvalidator.Validate(createClientRequest.primaryClient);
                    if (!validationResult_primaryClientvalidator.IsValid)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult_primaryClientvalidator.Errors, "primaryClient") });
                    }
                    #endregion

                    #region Secondary Client
                    if (createClientRequest.secondaryClients != null && createClientRequest.secondaryClients.Count() > 0)
                    {
                        foreach (var secondaryclient in createClientRequest.secondaryClients)
                        {
                            SecondaryValidator validations_sec = new SecondaryValidator();
                            ValidationResult validationResult_validations_sec = validations_sec.Validate(secondaryclient);
                            if (!validationResult_validations_sec.IsValid)
                            {
                                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult_validations_sec.Errors, "secondaryclient") });
                            }
                        }
                    }
                    #endregion

                    #region Leagal Info
                    LegalInfoValidator validations = new LegalInfoValidator();
                    ValidationResult validationResult = validations.Validate(createClientRequest.legalInfo);
                    if (!validationResult.IsValid)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult.Errors, "legalInfo") });
                    }
                    #endregion
                }
                else
                {
                    if (string.IsNullOrEmpty(createClientRequest.company.company) || string.IsNullOrEmpty(createClientRequest.primaryClient.fullName))
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please fill company name and Client name" });
                    }
                }
                #endregion

                #region Insert/Update Company 
                if (!string.IsNullOrEmpty(createClientRequest.company.en_Id) && !string.IsNullOrEmpty(CommonLogic.Decrypt(createClientRequest.company.en_Id)) && Convert.ToInt64(CommonLogic.Decrypt(createClientRequest.company.en_Id)) > 0)
                {
                    createClientRequest.company.id = Convert.ToInt64(CommonLogic.Decrypt(createClientRequest.company.en_Id));
                    InsertedCompanyId = createClientRequest.company.id;
                    GenCompany company = new();
                    company = await _iClient.GetGenCompanysById(createClientRequest.company.id).ConfigureAwait(false);

                    if (InsertedCompanyId != 0 && createClientRequest.LeadUserId != 0)
                    {
                        await _iClient.SaveClientLeadUser(InsertedCompanyId, createClientRequest.LeadUserId);
                    }

                    #region Update Company Details

                    CommonLogic.DBOperator(_talentConnectAdminDBContext, ModelBinder.BindCompanyModel(company, createClientRequest, 1, _talentConnectAdminDBContext, LoggedInUserId, true), EntityState.Modified);

                    #region UTS-6158 - Maintain history of Gen_Company
                    if (company.Id > 0)
                        _universalProcRunner.Manipulation(Constants.ProcConstant.SPROC_Gen_Company_History, new object[] { company.Id });
                    #endregion


                    #region FreeCreditUpdate
                    if (company.Id > 0 && decJpcreditBalance != null)
                        _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_FreeCreditUpdate, 
                            new object[] { 
                                company.Id, 
                                decJpcreditBalance, 
                                createClientRequest.contactId, 
                                Convert.ToInt64(LoggedInUserId),
                                createClientRequest?.company?.CreditAmount,
                                createClientRequest?.company?.CreditCurrency
                            }
                            );
                    #endregion

                    try
                    {
                        #region Update CreditInfo UTS-6930
                        _universalProcRunner.Manipulation(Constants.ProcConstant.Sproc_UTS_UpdateCompanyCreditInfo,
                            new object[] {
                            company.Id,
                            createClientRequest?.company?.CreditAmount,
                            createClientRequest?.company?.CreditCurrency,
                            createClientRequest?.company?.JobPostCredit,
                            createClientRequest?.company?.VettedProfileViewCredit,
                            createClientRequest?.company?.NonVettedProfileViewCredit
                            }
                          );
                        #endregion
                    }
                    catch( Exception ex )
                    {

                    }

                    createClientRequest.company.en_Id = CommonLogic.Encrypt(Convert.ToString(createClientRequest.company.id));

                    if (createClientRequest.company.fileUpload != null)
                    {
                        byte[] img = CommonLogic.UploadImageFromBase64(createClientRequest.company.fileUpload.Base64ProfilePic);
                        if (img != null && img.Length > 0)
                        {
                            string fileName = string.Format("{0}.{1}", createClientRequest.companyname, createClientRequest.company.fileUpload.Extenstion);
                            var pathToSave = CommonLogic.GetFileUploadFolderFor(Constants.MediaConstant.CompanyLogo);
                            string modifiedFilename = string.Format("{0}/{1}", pathToSave, fileName);
                            System.IO.File.WriteAllBytes(modifiedFilename, img);


                            var Companylogopathsave = _configuration["CompanyLogo"].ToString();
                            string companylogomodifiedFilename = string.Format("{0}/{1}", Companylogopathsave, fileName);
                            System.IO.File.WriteAllBytes(companylogomodifiedFilename, img);

                            var updateCompany = await _iClient.GetGenCompanysById(createClientRequest.company.id).ConfigureAwait(false);
                            if (updateCompany != null)
                            {
                                updateCompany.CompanyLogo = fileName;

                                await _iClient.UpdateGenCompanies(updateCompany).ConfigureAwait(false);
                            }
                        }
                    }

                    #endregion
                }
                else
                {
                    #region Insert Company
                    GenCompany genCompany = new GenCompany();
                    GenCompany companyResult = ModelBinder.BindCompanyModel(genCompany, createClientRequest, 1, _talentConnectAdminDBContext, LoggedInUserId);
                    if (companyResult != null)
                    {
                        await _iClient.CreateGenCompanies(companyResult).ConfigureAwait(false);

                        InsertedCompanyId = companyResult.Id;

                        #region UTS-6158 - Maintain history of Gen_Company
                        if (InsertedCompanyId > 0)
                            _universalProcRunner.Manipulation(Constants.ProcConstant.SPROC_Gen_Company_History, new object[] { InsertedCompanyId });
                        #endregion

                        createClientRequest.company.en_Id = CommonLogic.Encrypt(Convert.ToString(companyResult.Id));
                    }
                    if (InsertedCompanyId != 0 && createClientRequest.LeadUserId != 0)
                    {
                        await _iClient.SaveClientLeadUser(InsertedCompanyId, createClientRequest.LeadUserId);
                    }
                    if (createClientRequest.company.fileUpload != null)
                    {
                        byte[] img = CommonLogic.UploadImageFromBase64(createClientRequest.company.fileUpload.Base64ProfilePic);
                        if (img != null && img.Length > 0)
                        {
                            string fileName = string.Format("{0}.{1}", DateTime.Now.ToFileTime(), createClientRequest.company.fileUpload.Extenstion);
                            var pathToSave = CommonLogic.GetFileUploadFolderFor(Constants.MediaConstant.CompanyLogo);
                            string modifiedFilename = string.Format("{0}/{1}", pathToSave, fileName);
                            System.IO.File.WriteAllBytes(modifiedFilename, img);


                            var Companylogopathsave = _configuration["CompanyLogo"].ToString();
                            string companylogomodifiedFilename = string.Format("{0}/{1}", Companylogopathsave, fileName);
                            System.IO.File.WriteAllBytes(companylogomodifiedFilename, img);

                            var updateCompany = await _iClient.GetGenCompanysById(companyResult.Id).ConfigureAwait(false);
                            if (updateCompany != null)
                            {
                                updateCompany.CompanyLogo = fileName;
                                await _iClient.UpdateGenCompanies(updateCompany).ConfigureAwait(false);
                            }
                        }
                    }
                    #endregion
                }
                #endregion

                #region Insert/Update Primary Client

                if (!string.IsNullOrEmpty(createClientRequest.primaryClient.en_Id) && !string.IsNullOrEmpty(CommonLogic.Decrypt(createClientRequest.primaryClient.en_Id)) && Convert.ToInt64(CommonLogic.Decrypt(createClientRequest.primaryClient.en_Id)) > 0)
                {
                    createClientRequest.primaryClient.id = Convert.ToInt64(CommonLogic.Decrypt(createClientRequest.primaryClient.en_Id));

                    #region Update Contact
                    GenContact contact = new GenContact();
                    contact = await _iClient.GetGenContactById(createClientRequest.primaryClient.id).ConfigureAwait(false);
                    if (createClientRequest.primaryClient.fileUpload != null)
                    {
                        byte[] img = CommonLogic.UploadImageFromBase64(createClientRequest.primaryClient.fileUpload.Base64ProfilePic);
                        if (img != null && img.Length > 0)
                        {
                            string fileName = string.Format("{0}.{1}", DateTime.Now.ToFileTime(), createClientRequest.primaryClient.fileUpload.Extenstion);
                            var pathToSave = CommonLogic.GetFileUploadFolderFor(Constants.MediaConstant.ClientProfilePic);
                            string modifiedFilename = string.Format("{0}/{1}", pathToSave, fileName);
                            System.IO.File.WriteAllBytes(modifiedFilename, img);
                            //var updateCompany = await _iClient.GetGenCompanysById(createClientRequest.company.id).ConfigureAwait(false);
                            if (contact != null)
                            {
                                createClientRequest.primaryClient.PhotoImage = fileName;
                            }
                        }
                    }
                    genContact = ModelBinder.BindContact(contact, createClientRequest, null, LoggedInUserId, InsertedCompanyId.Value, true, true, true);

                    await _iClient.UpdateGenContact(genContact).ConfigureAwait(false);

                    InsertedClientId = genContact.Id;
                    createClientRequest.contactId = genContact.Id;
                    createClientRequest.clientemail = genContact.EmailId;
                    createClientRequest.companyname = createClientRequest.company.company;
                    createClientRequest.primaryClient.en_Id = CommonLogic.Encrypt(Convert.ToString(genContact.Id));
                    #endregion
                }
                else
                {
                    #region Insert Contact
                    GenContact contact = new GenContact();
                    genContact = ModelBinder.BindContact(contact, createClientRequest, null, LoggedInUserId, InsertedCompanyId.Value, true, true, false);

                    var isGenContactCreate = await _iClient.CreateGenContact(genContact).ConfigureAwait(false);
                    if (isGenContactCreate)
                    {
                        InsertedClientId = genContact.Id;
                        createClientRequest.contactId = genContact.Id;
                        createClientRequest.clientemail = genContact.EmailId;
                        createClientRequest.companyname = createClientRequest.company.company;
                        createClientRequest.primaryClient.en_Id = CommonLogic.Encrypt(Convert.ToString(genContact.Id));
                    }
                    #endregion
                }
                #endregion

                #region Insert Secondary Clients
                if (createClientRequest.secondaryClients != null && createClientRequest.secondaryClients.Count() > 0)
                {
                    foreach (var secondaryclient in createClientRequest.secondaryClients)
                    {
                        #region  Update/Insert Secondary Clients
                        if (!string.IsNullOrEmpty(secondaryclient.en_Id) && !string.IsNullOrEmpty(CommonLogic.Decrypt(secondaryclient.en_Id)) && Convert.ToInt64(CommonLogic.Decrypt(secondaryclient.en_Id)) > 0)
                        {
                            secondaryclient.id = Convert.ToInt64(CommonLogic.Decrypt(secondaryclient.en_Id));
                            GenContact secondaryContact = new GenContact();
                            secondaryContact = _talentConnectAdminDBContext.GenContacts.FirstOrDefault(xy => xy.Id == Convert.ToInt64(secondaryclient.id));
                            secondaryContact = ModelBinder.BindContact(secondaryContact, createClientRequest, secondaryclient, LoggedInUserId, InsertedCompanyId.Value, false, true, true);
                            _talentConnectAdminDBContext.GenContacts.Update(secondaryContact);
                            _talentConnectAdminDBContext.SaveChanges();
                            secondaryclient.en_Id = CommonLogic.Encrypt(Convert.ToString(secondaryContact.Id));
                        }
                        else
                        {
                            GenContact secondaryContact = new GenContact();
                            secondaryContact = ModelBinder.BindContact(secondaryContact, createClientRequest, secondaryclient, LoggedInUserId, InsertedCompanyId.Value, false, true, false);
                            _talentConnectAdminDBContext.GenContacts.Add(secondaryContact);
                            _talentConnectAdminDBContext.SaveChanges();
                            secondaryclient.en_Id = CommonLogic.Encrypt(Convert.ToString(secondaryContact.Id));
                        }
                        #endregion
                    }
                }
                #endregion

                #region Insert/Update Company Contract
                CompanyContract(InsertedCompanyId, createClientRequest, LoggedInUserId);
                #endregion

                #region pointOfcontact
                if (createClientRequest.pocList != null)
                {
                    var InsertedPointofContact = await _iClient.GetGenContactPointofContactsListByContactId(InsertedClientId).ConfigureAwait(false);

                    await _iClient.DeleteGenContactPointofContactByList(InsertedPointofContact).ConfigureAwait(false);

                    for (int i = 0; i < createClientRequest.pocList.Count(); i++)
                    {
                        GenContactPointofContact InsertcontactPointofContact = new GenContactPointofContact();

                        long User_Id = Convert.ToInt64(createClientRequest.pocList[i].contactName);

                        GenContactPointofContact contactPointofContact = await _iClient.GetGenContactPointofContactsByContactIdUserId(InsertedClientId, User_Id).ConfigureAwait(false);
                        if (User_Id != 0)
                        {
                            UsrUser usr_User = await _iClient.GetUsrUserById(User_Id).ConfigureAwait(false);

                            if (contactPointofContact == null)
                            {
                                InsertcontactPointofContact.UserId = Convert.ToInt32(User_Id);
                                InsertcontactPointofContact.ContactId = InsertedClientId;
                                if (usr_User != null)
                                {
                                    InsertcontactPointofContact.Description = usr_User.Description;
                                }
                                InsertcontactPointofContact.CompanyId = InsertedCompanyId;

                                await _iClient.CreateGenContactPointofContacts(InsertcontactPointofContact).ConfigureAwait(false);
                            }
                            else
                            {
                                contactPointofContact.Id = contactPointofContact.Id;
                                contactPointofContact.UserId = Convert.ToInt32(User_Id);
                                contactPointofContact.ContactId = InsertedClientId;
                                contactPointofContact.Description = contactPointofContact.Description;
                                contactPointofContact.CompanyId = contactPointofContact.CompanyId;
                                await _iClient.UpdateGenContactPointofContacts(contactPointofContact).ConfigureAwait(false);
                            }
                        }
                    }
                }
                #endregion

                #region ATS Call
                AddUpdateCompanyViewModel addUpdateCompanyViewModel = new AddUpdateCompanyViewModel();
                if (!_configuration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                {
                    _talentConnectAdminDBContext.Entry(genContact).Reload();

                    var ContactDetails = await _iClient.GetGenContactById(genContact.Id).ConfigureAwait(false);
                    if (ContactDetails != null)
                    {
                        var CompanyData = await _iClient.GetGenCompaniesById(InsertedCompanyId).ConfigureAwait(false);
                        
                        if (CompanyData != null)
                        {
                            _talentConnectAdminDBContext.Entry(CompanyData).Reload();
                            addUpdateCompanyViewModel = await BindAddUpdateCompany(addUpdateCompanyViewModel, CompanyData).ConfigureAwait(false);
                            if (addUpdateCompanyViewModel != null)
                            {
                                try
                                {
                                    var json = JsonConvert.SerializeObject(addUpdateCompanyViewModel);
                                    ATSCall atsCall = new ATSCall(_configuration, _talentConnectAdminDBContext);
                                    atsCall.SendAddEditCompanyData(json, LoggedInUserId);
                                }
                                catch (Exception)
                                {
                                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Client has been saved successfully", Details = createClientRequest });
                                }
                            }
                        }
                    }
                }
                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Client has been saved successfully", Details = createClientRequest });

            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Upload CompanyLogo
        [Authorize]
        [HttpPost]
        [Route("UploadCompanyLogo")]
        public async Task<ObjectResult> UploadCompanyLogo([FromForm] IFormFile file, [FromQuery] long CompanyID, [FromQuery] bool isDelete)
        {
            try
            {
                #region Validation

                if (CompanyID == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide Company ID." });
                }

                if (!isDelete)
                {
                    if (file == null)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "File Required", Details = null });
                    }
                    else if (file.Length == 0)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "You are uploading corrupt file", Details = null });
                    }

                    var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
                    string[] allowedFileExtension = { ".pdf", ".doc", ".docx", ".txt", ".rtf", ".jpg", ".jpeg", ".png" };

                    if (!allowedFileExtension.Contains(fileExtension))
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Your file format is incorrect.", Details = null });
                    }

                    var fileSize = (file.Length / 1024) / 1024;
                    if (fileSize >= 0.5)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "File size must be less than 500 KB", Details = null });
                    }
                }

                #endregion

                string DocfileName = file.FileName;
                string folderPath = System.IO.Path.Combine("Media/Client/CompanyLogo", CompanyID.ToString());
                string filePath = System.IO.Path.Combine(folderPath, DocfileName);

                if (isDelete)
                {
                    //If file path exists then delete the path.
                    if (Directory.Exists(folderPath))
                    {
                        System.IO.DirectoryInfo di = new DirectoryInfo(folderPath);

                        foreach (FileInfo doc in di.GetFiles())
                        {
                            doc.Delete();
                        }

                        Directory.Delete(folderPath);
                    }

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject()
                    {
                        statusCode = StatusCodes.Status200OK,
                        Message = "File removed successfully"
                    });
                }

                //If file path does not exists then create the path.
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new ResponseObject()
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Company Logo Uploaded Successfully",
                    Details = DocfileName
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Upload Client Profile Pic
        [Authorize]
        [HttpPost]
        [Route("UploadClientProfilePic")]
        public async Task<ObjectResult> UploadClientProfilePic([FromForm] IFormFile file, [FromQuery] long ContactID, [FromQuery] bool isDelete)
        {
            try
            {
                #region Validation

                if (ContactID == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide Contact ID." });
                }

                if (!isDelete)
                {
                    if (file == null)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "File Required", Details = null });
                    }
                    else if (file.Length == 0)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "You are uploading corrupt file", Details = null });
                    }

                    var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
                    string[] allowedFileExtension = { ".pdf", ".doc", ".docx", ".txt", ".rtf", ".jpg", ".jpeg", ".png" };

                    if (!allowedFileExtension.Contains(fileExtension))
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Your file format is incorrect.", Details = null });
                    }

                    var fileSize = (file.Length / 1024) / 1024;
                    if (fileSize >= 0.5)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "File size must be less than 500 KB", Details = null });
                    }
                }

                #endregion

                string DocfileName = file.FileName;
                string folderPath = System.IO.Path.Combine("Media/Client/ClientProfilePic", ContactID.ToString());
                string filePath = System.IO.Path.Combine(folderPath, DocfileName);

                if (isDelete)
                {
                    //If file path exists then delete the path.
                    if (Directory.Exists(folderPath))
                    {
                        System.IO.DirectoryInfo di = new DirectoryInfo(folderPath);

                        foreach (FileInfo doc in di.GetFiles())
                        {
                            doc.Delete();
                        }

                        Directory.Delete(folderPath);
                    }

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject()
                    {
                        statusCode = StatusCodes.Status200OK,
                        Message = "File removed successfully"
                    });
                }

                //If file path does not exists then create the path.
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new ResponseObject()
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Client Profile Pic Uploaded Successfully",
                    Details = DocfileName
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Client list, View Client

        [HttpPost("List")]
        public async Task<ObjectResult> GetClientList([FromBody] ListClientFilter listClientFilter)
        {
            try
            {
                var varLoggedInUserId = SessionValues.LoginUserId;
                #region PreValidation
                if (listClientFilter == null || (listClientFilter.pagenumber == 0 || listClientFilter.totalrecord == 0))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide pagenumber and totalrecord" });
                #endregion

                string Sortdatafield = "AddedDate";
                string Sortorder = "DESC";

                object[] param = new object[]
                {
                    listClientFilter.pagenumber,
                    listClientFilter.totalrecord,
                    Sortdatafield, Sortorder,
                    listClientFilter.filterFields_Client.CompanyStatus ?? "",
                    listClientFilter.filterFields_Client.GEO ?? "",
                    listClientFilter.filterFields_Client.AddingSource ?? "",
                    listClientFilter.filterFields_Client.Category ?? "",
                    listClientFilter.filterFields_Client.POC ?? "",
                    listClientFilter.filterFields_Client.fromDate ?? "",
                    listClientFilter.filterFields_Client.toDate ?? "",
                    listClientFilter.filterFields_Client.searchText ?? "",
                    listClientFilter.filterFields_Client.SearchSourceCategory ?? "",
                    listClientFilter.filterFields_Client.LeadUserID  ?? 0,
                    listClientFilter.filterFields_Client.SearchCompanyModel ?? "",

                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<sproc_UTS_GetClientList_Result> clientListData = await _iClient.sproc_GetClient(paramasString).ConfigureAwait(false);

                if (clientListData.Any())
                {
                    var reactClientPortalURL = _configuration["ReactClientPortalURL"];

                    foreach (var item in clientListData)
                    {
                        
                        item.SSO_Login = reactClientPortalURL + "login?AD=" + MyExtensions.Encrypt("1") + "&CT=" + MyExtensions.Encrypt(""+ item.ClientID+ "") + "&UID="+ MyExtensions.Encrypt("" + varLoggedInUserId + "")  + "";
                    }
                    dynamic result = new ExpandoObject();
                    result.Data = CustomRendering.ListingResponses(clientListData, clientListData[0].TotalRecords, listClientFilter.totalrecord, listClientFilter.pagenumber);

                    // Check if the logged in user has the access to add the client from backend.
                    Sproc_UTS_GetAddClientAccess_Result sproc_UTS_GetAddClientAccess_Result = _iClient.Sproc_UTS_GetAddClientAccess(SessionValues.LoginUserId.ToString());
                    if (sproc_UTS_GetAddClientAccess_Result != null)
                    {
                        result.ShowAddClient = sproc_UTS_GetAddClientAccess_Result.AllowClientAdd;
                    }

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = result });
                }
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Client Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("FilterList")]
        public async Task<ObjectResult> GetClientFilterList()
        {
            try
            {
                dynamic dObject = new ExpandoObject();

                var Company_Categories = new Dictionary<string, string>
                {
                    { "A", "A" },
                    { "B", "B" },
                    { "C", "C" }
                };

                dObject.CompanyCategory = Company_Categories.ToList().Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Key.ToString()
                });

                var Adding_Source = new Dictionary<string, string>
                {
                    { "Hubspot", "Hubspot" },
                    { "Self Signup", "Self Signup" },
                    { "Post a job - Credit Based", "Post a job - Credit Based" },
                    { "Added By Sales", "Added By Sales" },
                };

                dObject.AddingSource = Adding_Source.ToList().Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Key.ToString()
                });

                var ContactStatusList = await _talentConnectAdminDBContext.PrgContactStatuses.Select(x => x.ContactStatus).ToListAsync();

                dObject.ContactStatus = ContactStatusList.Select(x => new SelectListItem
                {
                    Value = x,
                    Text = x
                });

                var Geo = await _talentConnectAdminDBContext.PrgGeos.ToListAsync().ConfigureAwait(false);
                dObject.Geo = Geo.Select(x => new SelectListItem { Value = x.Geo, Text = x.Id.ToString() });

                var POCList = await _talentConnectAdminDBContext.UsrUsers.Where(x => x.IsActive == true).ToListAsync();

                dObject.POCList = POCList.Select(x => new SelectListItem
                {
                    Value = x.FullName,
                    Text = x.Id.ToString()
                });

                var LeadTypeList = await _iReport.GetALLUsrUserByCondition(x => x.DeptId == 1 && x.LevelId == 1 && (x.UserTypeId == 11 || x.UserTypeId == 12) && x.IsActive == true).ConfigureAwait(false);

                dObject.LeadTypeList = LeadTypeList.Select(x => new SelectListItem
                {
                    Text = x.Id.ToString(),
                    Value = (x.UserTypeId == 12 ? "InBound" : "OutBound") + " - " + Convert.ToString(x.FullName)
                }).ToList();

                dObject.LeadTypeList.Insert(0, new SelectListItem() { Value = "0", Text = "--Select--" });


                dObject.CompanyModel = await _talentConnectAdminDBContext.PrgCompanyTypes.Select(y => new SelectListItem
                {
                    Text = y.Id.ToString(),
                    Value = y.CompanyType
                }).OrderBy(y => y.Value).ToListAsync();


                if (dObject != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.FilterListsResponse(dObject) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetPOCDetails")]
        public async Task<ObjectResult> GetPOCDetails(long CompanyID, long ClientID)
        {
            try
            {
                #region Prevalidation
                if (CompanyID <= 0 && ClientID <= 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide CompanyID & ClientID" });
                }
                #endregion

                object[] param = new object[] { CompanyID, ClientID };
                string paramasString = CommonLogic.ConvertToParamString(param);

                List<sp_UTS_GetPOCDetails_Result> POCList = await _iClient.GetPOCDetails(paramasString).ConfigureAwait(false);

                if (POCList != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = POCList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Poc Not Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("ViewClient")]
        public async Task<ObjectResult> ViewClientDetails([FromBody] ViewClientDetails clientDetails)
        {
            try
            {
                #region Prevalidation
                if (clientDetails != null)
                {
                    if (clientDetails.CompanyID <= 0 && clientDetails.ClientID <= 0)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide CompanyID & ClientID" });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide CompanyID & ClientID" });
                }
                #endregion

                object[] param = new object[] { clientDetails.CompanyID, clientDetails.ClientID };
                string paramasString = CommonLogic.ConvertToParamString(param);

                ViewClient viewClient = new ViewClient();
                viewClient.ClientDetails = _iClient.GetCompanyClientDetails(paramasString);
                viewClient.HRList = await _iClient.GetClientWiseHRDetails(clientDetails.ClientID.ToString()).ConfigureAwait(false);

                if (viewClient.ClientDetails != null && viewClient.HRList.Any())
                {
                    double TR = 0;
                    foreach (var item in viewClient.HRList)
                    {
                        if (item.TotalTR != null && item.TotalTR > 0)
                        {
                            TR = TR + (double)item.TotalTR;
                        }
                    }
                    viewClient.ClientDetails.TR = TR;
                }

                if (viewClient != null && viewClient.ClientDetails != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = viewClient });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Client Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Edit AM
        [HttpGet("GetAMDetails")]
        public async Task<ObjectResult> GetAMDetails(long CompanyID, long ClientID)
        {
            try
            {
                #region Prevalidation
                if (CompanyID <= 0 && ClientID <= 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide CompanyID & ClientID" });
                }
                #endregion

                object[] param = new object[] { CompanyID, ClientID };
                string paramasString = CommonLogic.ConvertToParamString(param);

                sp_UTS_GetCompanyClientDetails_Result Detail = _iClient.GetCompanyClientDetails(paramasString);

                ViewAMDetails viewAMDetails = null;

                if (Detail != null)
                {
                    viewAMDetails = new ViewAMDetails();
                    viewAMDetails.CompanyID = CompanyID;
                    viewAMDetails.CompanyName = Detail.CompanyName;
                    viewAMDetails.GEO = Detail.GEO;
                    viewAMDetails.ClientID = ClientID;
                    viewAMDetails.ClientName = Detail.ClientName;
                    viewAMDetails.OldAM_SalesPersonID = Detail.AM_SalesPersonID;
                    viewAMDetails.OldAM_UserName = Detail.AM_UserName;

                    var UserList = await _iClient.GetUserListForAMChange((long)Detail.AM_SalesPersonID);

                    viewAMDetails.AMList = UserList.ToList().Select(x => new SelectListItem
                    {
                        Value = x.FullName,
                        Text = x.Id.ToString()
                    });
                }

                if (viewAMDetails != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = viewAMDetails });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "AM Not Available" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = ex.Message });
            }
        }

        [HttpPost("UpdateAMForCompany")]
        public async Task<ObjectResult> AMChangeForCompany(UpdateAMDetails updateAM)
        {
            try
            {
                #region Prevalidation
                if (updateAM != null)
                {
                    if (updateAM.CompanyID == null || updateAM.CompanyID <= 0)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide CompanyID" });
                    }
                    if (updateAM.OldAM_SalesPersonID == null || updateAM.OldAM_SalesPersonID <= 0)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide OldAM_SalesPersonID" });
                    }
                    if (updateAM.NewAM_SalesPersonID == null || updateAM.NewAM_SalesPersonID <= 0)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide NewAM_SalesPersonID" });
                    }
                    if (string.IsNullOrEmpty(updateAM.Comment))
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide Comment" });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide proper details, object must not be null" });
                }
                #endregion

                #region update AM details

                long LoggedInUserId = SessionValues.LoginUserId;

                object[] param = new object[]
                {
                    updateAM.OldAM_SalesPersonID, updateAM.NewAM_SalesPersonID, updateAM.CompanyID, LoggedInUserId, updateAM.Comment
                };
                string paramasString = CommonLogic.ConvertToParamString(param);

                _iClient.UpdateAMDetails(paramasString);

                #endregion

                #region SendEmail
                //var oldAMUserHierarchy = await _iClient.GetHierarchyForEmail(updateAM.OldAM_SalesPersonID.ToString());
                //var newAMUserHierarchy = await _iClient.GetHierarchyForEmail(updateAM.NewAM_SalesPersonID.ToString());

                EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                emailBinder.SendEmailForChangeAMForCompany(updateAM);
                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "AM change Successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = ex.Message });
            }
        }
        #endregion

        [HttpGet("GetPointOfContact")]
        public async Task<ObjectResult> GetPointOfContact(long contactid, string ActionMode, long LoggedInUserIdTC)
        {
            try
            {
                VMClient createClientRequest = new VMClient();

                createClientRequest.primaryClient.Id = contactid;

                createClientRequest.ActionName = ActionMode;


                var pointOfContact = new List<SelectListItem>();


                var Userslist = await _iClient.GetUserslist().ConfigureAwait(false);

                List<TalentPointofContact> UserNameList = new List<TalentPointofContact>();
                object[] objectsParam = new object[]
              {
                 createClientRequest.primaryClient.Id
              };
                var varUserNameListTemp = await _iClient.Sproc_GetPointOfContact_UserDetails(CommonLogic.ConvertToParamString(objectsParam)).ConfigureAwait(false);
                var UserNameListTemp = varUserNameListTemp.Select(x => new TalentPointofContact
                {
                    UserName = x.UserName,
                    UserId = x.UserID,
                    listUsers = (List<SelectListItem>)Userslist
                });

                if (UserNameList != null && UserNameList.Count > 0)
                {
                    createClientRequest.talentPointofContact = UserNameList;
                }
                else
                {

                    long LoggedInUserId = SessionValues.LoginUserId;

                    var Usersdetail = await _iClient.GetUsrUserListByLoggedInUserId(LoggedInUserId).ConfigureAwait(false);

                    if (Usersdetail != null)
                    {
                        UserNameList.Add(new TalentPointofContact
                        {
                            UserName = Usersdetail.Username,
                            UserId = Usersdetail.Id,
                            listUsers = (List<SelectListItem>)Userslist
                        });
                        createClientRequest.talentPointofContact = UserNameList;
                    }
                    else
                    {
                        UserNameList.Add(new TalentPointofContact
                        {
                            UserName = string.Empty,
                            UserId = 0,
                            listUsers = (List<SelectListItem>)Userslist
                        });
                        createClientRequest.talentPointofContact = UserNameList;
                    }

                }

                var varUsrUsers = await _iClient.GetUsrUserList().ConfigureAwait(false);

                createClientRequest.pointOfContactName = varUsrUsers.OrderBy(x => x.Username).Select(x => new MastersResponseModel
                {
                    Id = x.Id,
                    Value = x.Username
                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Point of Contact", Details = createClientRequest.pointOfContactName });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("HappinessSurvey/List")]
        public async Task<IActionResult> GetHappinessSurvey([FromBody] ListingHappinessSurveyViewModel listingHappinessSurveyViewModel, [FromQuery] bool IsExportToExcel = false)
        {
            try
            {
                #region PreValidation
                if (listingHappinessSurveyViewModel == null || (listingHappinessSurveyViewModel.pagenumber == 0 || listingHappinessSurveyViewModel.totalrecord == 0))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide pagenumber and totalrecord" });
                #endregion


                string Sortdatafield = "ID";
                string Sortorder = "DESC";
                string search = "";

                if (!string.IsNullOrEmpty(listingHappinessSurveyViewModel.filterFields_HappinessSurvey.search))
                {
                    search = listingHappinessSurveyViewModel.filterFields_HappinessSurvey.search.Replace("'", "''");
                }

                object[] param = new object[] {
                listingHappinessSurveyViewModel.pagenumber, listingHappinessSurveyViewModel.totalrecord, Sortdatafield, Sortorder,
                listingHappinessSurveyViewModel.filterFields_HappinessSurvey.StartDate,
                listingHappinessSurveyViewModel.filterFields_HappinessSurvey.EndDate,
                listingHappinessSurveyViewModel.filterFields_HappinessSurvey.RatingFrom,
                listingHappinessSurveyViewModel.filterFields_HappinessSurvey.RatingTo,
                listingHappinessSurveyViewModel.filterFields_HappinessSurvey.Company,
                listingHappinessSurveyViewModel.filterFields_HappinessSurvey.Client,
                listingHappinessSurveyViewModel.filterFields_HappinessSurvey.Email,
                listingHappinessSurveyViewModel.filterFields_HappinessSurvey.Rating,
                listingHappinessSurveyViewModel.filterFields_HappinessSurvey.Question,
                listingHappinessSurveyViewModel.filterFields_HappinessSurvey.FeedbackStatus,
                listingHappinessSurveyViewModel.filterFields_HappinessSurvey.selectedFormat,
                listingHappinessSurveyViewModel.filterFields_HappinessSurvey.Options,
                search
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<sproc_GetClientHappinessSurvey_Result> clientListData = await _iClient.sproc_GetClientHappinessSurvey(paramasString).ConfigureAwait(false);
                #region Export to excel
                if (clientListData.Any() && IsExportToExcel)
                {
                    try
                    {
                        return Export_ClientHappinessSurveyList(clientListData);
                    }
                    catch
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Some data issue in Export to excel" });
                    }
                }
                #endregion

                if (clientListData.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.ListingResponses(clientListData, clientListData[0].TotalRecords, listingHappinessSurveyViewModel.totalrecord, listingHappinessSurveyViewModel.pagenumber) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Client Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("AutoComplete/Company")]
        public async Task<ObjectResult> GetAutoCompleteCompany([FromQuery] string Search)
        {
            try
            {
                #region PreValidation
                if (!string.IsNullOrEmpty(Search))
                {

                    List<sproc_UTS_GetAutoCompleteCompanies_Result> searchData = await _iClient.sproc_GetAutoCompleteCompanies(Search);
                    if (searchData.Any())
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = searchData });
                    else
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No contact Available" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide text for search" });
                }

                #endregion
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("SaveClientHappinessSurveys")]
        public async Task<ObjectResult> SaveClientHappinessSurveys([FromBody] ClientHappinessSurveyViewModel ClientHappinessSurvey, long LoggedInUserId = 0)
        {
            try
            {
                LoggedInUserId = SessionValues.LoginUserId;

                var HappinessSurvey = await _iClient.SaveHappinessSurveyFeedback(ClientHappinessSurvey, LoggedInUserId);
                if (HappinessSurvey)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Feedback has been saved successfully" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Error" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }



        [HttpGet]
        [Route("ClientHappinessSurveysOption")]
        public async Task<ObjectResult> ClientHappinessSurveysOption()
        {
            try
            {
                List<sproc_GetClientHappinessSurveyOptions_Result> SurveyOptions = await _iClient.HappinessSurveysOption();
                if (SurveyOptions.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = SurveyOptions });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No SurveyOptions Available" });

            }
            catch (Exception)
            {

                throw;
            }
        }



        [HttpPost]
        [Route("SendEmailForFeedback")]
        public async Task<ObjectResult> SendEmailForFeedback(long feedbackID, long LoggedInUserId = 0)
        {
            try
            {

                LoggedInUserId = SessionValues.LoginUserId;
                var EmailSendCheck = await _iClient.SendEmailForHappinessSurveyFeedback(feedbackID, LoggedInUserId);

                if (EmailSendCheck)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Send Email For Feedback" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Error" });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }


        [Authorize]
        [HttpPost("TrackingLeadDetailClientSource")]
        public async Task<ObjectResult> TrackingLeadDetailClientSource(long ContactID)
        {
            try
            {
                List<Sproc_Get_TrackingLead_Details_for_ClientSource_Result> result = null;

                result = await (_iClient.TrackingLeadDetailClientSource(ContactID).ConfigureAwait(false));

                if (result.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = result });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region GetPendingHR's Details

        [HttpGet("GetDraftJobDetails")]
        public ObjectResult GetDraftJobDetails(long contactId, string guid)
        {
            #region Pre-Validation
            if (string.IsNullOrEmpty(guid) || contactId == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please Enter Contact ID OR Guid" });
            }

            #endregion

            object[] param = new object[] { contactId, guid };
            string paramString = CommonLogic.ConvertToParamString(param);
            Sp_UTS_PreviewJobPost_ClientPortal_Result jobPreview = _iClient.Sp_UTS_PreviewJobPost_ClientPortal_Result(paramString);
            dynamic result = new ExpandoObject();

            if (jobPreview != null)
            {
                result.JobDetails = jobPreview;
                result.JDLink = $"{_configuration["ClientProjectAPIURL"]}Media/JDParsing/JDFiles/{jobPreview.JDFileName}";
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "success.", Details = result });
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data found." });
            }
        }
        #endregion


        #region AddClient
        [HttpPost("AddClientwithCredits")]
        public async Task<ObjectResult> AddClientwithCredits([FromBody] ClientSignUp clientSignUp)
        {
            #region new code
            try
            {
                long LoggedInUserId = SessionValues.LoginUserId;
                #region PreValidation
                if (clientSignUp == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty, please check the datatype or naming convention", Details = clientSignUp });
                }
                #endregion

                #region Validation

                Sproc_ValidateAddClient_Result obj = new();

                object[] param = new object[]
                                    {
                                      clientSignUp.WorkEmail,clientSignUp.CompanyName
                                     };
                string paramasString = CommonLogic.ConvertToParamString(param);

                obj = _iClient.Sproc_Validate_AddClient(paramasString);

                // Set the logged in userId 
                clientSignUp.InvitingUserId = LoggedInUserId;    
                

              

                //Create Company AND Contact
                #region
                if (obj.status == 201)
                {
                    AllCompanyDetails companyDetailsViewModel = await GetCompanyDetailsFromPython(clientSignUp.CompanyURL);
                    if (companyDetailsViewModel != null)
                    {
                        //clientSignUp.CompanyURL = clientSignUp.CompanyURL;
                        //clientSignUp.CompanySize = companyDetailsViewModel.Company_Size;
                        clientSignUp.CompanySize_RangeorAdhoc = companyDetailsViewModel.Company_Size;
                        clientSignUp.CompanyIndustryType = companyDetailsViewModel.Company_Industry_Type;
                        clientSignUp.BriefAboutCompany = companyDetailsViewModel.Brief_About_Company;
                        clientSignUp.Culture = companyDetailsViewModel.Brief_About_Culture;
                        clientSignUp.FoundedYear = companyDetailsViewModel.Founded_In;
                        clientSignUp.CompanyType = companyDetailsViewModel.Company_Type;
                        clientSignUp.Headquaters = companyDetailsViewModel.Headquarters;

                        
                    }
                    #region Fetch Logo
                    //try
                    //{
                    //    string CompanyLogoName = await CompanyLogo(clientSignUp.CompanyURL);
                    //    if (!string.IsNullOrEmpty(CompanyLogoName))
                    //    {
                    //        clientSignUp.CompanyLogo = CompanyLogoName;
                    //    }
                    //    if (CompanyLogoName != "")
                    //    {
                    //        EmailBinder email = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                    //        var emailres = email.SendEmailToInternalTeamWhenCompanyLogoScrap(clientSignUp.FullName, clientSignUp.CompanyName, clientSignUp.WorkEmail, true,CompanyLogoName,clientSignUp.CompanyURL);
                    //    }
                    //}
                    //catch (Exception)
                    //{

                    //    throw;
                    //}
                    #endregion

                    GenContact genContact = new();                    

                    genContact = await _iClient.SignUp(clientSignUp, false).ConfigureAwait(false);

                    short? Portal = (short)AppActionDoneBy.UTS;

                    if(genContact != null)
                    {
                        object[] param4 = new object[] { genContact.CompanyId, clientSignUp.CompanySize_RangeorAdhoc, clientSignUp.CompanyIndustryType, clientSignUp.BriefAboutCompany, clientSignUp.Culture, clientSignUp.FoundedYear, clientSignUp.CompanyType, clientSignUp.Headquaters, LoggedInUserId };
                        string paramString = CommonLogic.ConvertToParamString(param4);
                        _iCompany.sproc_Update_Company_Details_From_Scrapping_Result(paramString);

                        #region Save Funding details Perks Details and Culture details and Other Company Details
                        if (companyDetailsViewModel.Funding_Details != null)
                        {
                            foreach (var item in companyDetailsViewModel.Funding_Details)
                            {
                                object[] param1 = new object[] { genContact.CompanyId, item.Funding_Amount, item.Funding_Round, item.Type, item.Month, item.Year, item.Investors, LoggedInUserId, Portal, 0 };
                                string paramString1 = CommonLogic.ConvertToParamString(param1);
                                _iCompany.Sproc_Add_Company_Funding_Details_Result(paramString1);
                            }
                        }

                        if (companyDetailsViewModel.CultureDetails != null)
                        {
                            foreach (var item in companyDetailsViewModel.CultureDetails)
                            {
                                object[] param2 = new object[] { genContact.CompanyId, item.Culture_Images, LoggedInUserId, Portal, 0 };
                                string paramString2 = CommonLogic.ConvertToParamString(param2);
                                _iCompany.Sproc_Add_Company_CultureandPerksDetails_Result(paramString2);
                            }
                        }

                        if (companyDetailsViewModel.Perks != null)
                        {
                            string PerksString = string.Join(",", companyDetailsViewModel.Perks);
                            if (!string.IsNullOrEmpty(PerksString))
                            {
                                object[] param3 = new object[]
                                {
                                genContact.CompanyId,
                                PerksString,
                                LoggedInUserId,
                                Portal
                                };
                                string paramString1 = CommonLogic.ConvertToParamStringWithNull(param3);
                                _iCompany.Sproc_Add_Company_PerksDetails_Result(paramString1);
                            }
                        }

                        #endregion
                    }



                    if (genContact != null)
                    {
                        EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);

                        // Company registered from backend so send password set link
                        var loggedInUserDetail = await _iClient.UserDetails(LoggedInUserId).ConfigureAwait(false);
                        if (loggedInUserDetail != null)
                        {
                            emailBinder.SetPasswordSendEmail(genContact, loggedInUserDetail.FullName,loggedInUserDetail.EmailId, loggedInUserDetail.Designation);
                        }

                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Client registered successfully", Details = genContact });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status500InternalServerError, Message = "Registration failed" });
                    }
                }
                #endregion
                //Create Contact
                #region
                if (obj.status == 202)
                {

                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = obj.result });
                }
                #endregion
                //Update Contact
                #region
                if (obj.status == 203)
                {
                    GenContact genContact = new();

                    genContact = await _iClient.SignUp(clientSignUp, false);

                    if (genContact != null)
                    {
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Client update successfully", Details = genContact });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status500InternalServerError, Message = "Registration failed" });
                    }
                }
                #endregion
                //Update Contact
                #region
                if (obj.status == 204)
                {

                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = obj.result });
                }
                #endregion
                //add new Contact only
                #region
                if (obj.status == 205)
                {

                    GenContact genContact = new();

                    genContact = await _iClient.SignUp(clientSignUp, true).ConfigureAwait(false);


                    if (genContact != null)
                    {
                        EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);

                        // Company registered from backend so send password set link
                        var loggedInUserDetail = await _iClient.UserDetails(LoggedInUserId).ConfigureAwait(false);
                        if (loggedInUserDetail != null)
                        {
                            emailBinder.SetPasswordSendEmail(genContact, loggedInUserDetail.FullName,loggedInUserDetail.EmailId, loggedInUserDetail.Designation);
                        }

                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status500InternalServerError, Message = "Registration failed" });
                    }


                    if (_configuration["Environment"].ToLower().ToString() == _configuration["HubSpotEnviorment"].ToLower().ToString())
                    {
                        var hubSpot = new HubSpot();

                        hubSpot.properties.Add(new HubSpotProperty
                        {
                            property = "email",
                            value = clientSignUp.WorkEmail ?? ""
                        });

                        hubSpot.properties.Add(new HubSpotProperty
                        {
                            property = "firstname",
                            value = clientSignUp?.FullName?.Split(' ')[0] ?? ""
                        });

                        hubSpot.properties.Add(new HubSpotProperty
                        {
                            property = "lastname",
                            value = clientSignUp?.FullName?.Split(' ')[1] ?? ""
                        });

                        hubSpot.properties.Add(new HubSpotProperty
                        {
                            property = "company",
                            value = clientSignUp?.CompanyName ?? ""
                        });

                        hubSpot.properties.Add(new HubSpotProperty
                        {
                            property = "inbound_outbound",
                            value = "MQL (Signup)"
                        });

                        hubSpot.properties.Add(new HubSpotProperty
                        {
                            property = "lead_source",
                            value = "Inbound"
                        });
                        var varContactData = SendHubSpotData(hubSpot);

                        var companyFound = await _iClient.GetCompanyByName(clientSignUp.CompanyName).ConfigureAwait(false);


                        var HubSpotContactId = Convert.ToInt64(0);
                        var HubSpotCompanyId = Convert.ToInt64(0);
                        var PortalId = Convert.ToInt32(0);
                        var canonicalvid = Convert.ToInt64(0);
                        if (varContactData != null)
                        {
                            HubSpotContactId = varContactData.vid;
                            HubSpotCompanyId = companyFound.HubSpotCompany ?? 0;
                            SendCompanyContactAssociationDataHubSpotData(HubSpotContactId, HubSpotCompanyId);
                            genContact.HubSpotContactId = HubSpotContactId;

                            var updateGenContact = _iClient.UpdateGenContact(genContact).ConfigureAwait(false);
                        }
                    }

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Client registered successfully", Details = genContact });

                }
                #endregion
                #endregion



            }
            catch (Exception)
            {
                throw;
            }
            #endregion
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status500InternalServerError, Message = "Bad Request" });

        }

        [HttpPost("ResendInviteEmail")]
        public async Task<ObjectResult> ResendInviteEmail(long ContactId, long InvitingUserId)
        {
            try
            {
                long LoggedInUserId = SessionValues.LoginUserId;
                #region PreValidation
                if (ContactId < 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "invalid request" });
                }
                #endregion

                GenContact genContact = new();
                
                genContact = await _iClient.GetGenContactById(ContactId);

                if (genContact != null)
                {
                    EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);

                    // Company registered from backend so send password set link
                    var loggedInUserDetail = await _iClient.UserDetails(LoggedInUserId);
                    if (loggedInUserDetail != null)
                    {
                        emailBinder.SetPasswordSendEmail(genContact, loggedInUserDetail.FullName,loggedInUserDetail.EmailId, loggedInUserDetail.Designation);
                    }

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Email Send Successfully." });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status500InternalServerError, Message = "Contact not found" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #endregion

        [HttpGet("GetCreditTransactionHistory")]
        public ObjectResult GetCreditTransactionHistory(long CompanyID, long ClientID)
        {
            try
            {
                List<Sproc_GET_CreditPlanDetails_ClientPortal_Result> CreditTransactionList = new();

                string paramString = $"{CompanyID},{ClientID}";

                CreditTransactionList = _iClient.GetCreditTransaction(paramString);

                if (CreditTransactionList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CreditTransactionList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetActiveSalesUserList")]
        public ObjectResult GetActiveSalesUserList()
        {
            try
            {
                var varActiveSalesUserList = _iClient.Sproc_GetActiveSalesUserLists();

                if (varActiveSalesUserList.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Active Sales User List", Details = varActiveSalesUserList });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "No data" });

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetCreditWithAmountList")]
        public ObjectResult GetCreditWithAmountList()
        {
            try
            {
                var varActiveSalesUserList = _iClient.Sproc_Get_CreditWithAmountLists();

                if (varActiveSalesUserList.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Active Sales User List", Details = varActiveSalesUserList });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "No data" });

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region CommonLines
        private async void CompanyContract(long? InsertedCompanyId, createClientRequest createClientRequest, long LoggedInUserId = 0)
        {

            LoggedInUserId = SessionValues.LoginUserId;

            GenCompanyContractDetail companyContractDetail = new GenCompanyContractDetail();

            if (!string.IsNullOrEmpty(createClientRequest.legalInfo.en_Id) && !string.IsNullOrEmpty(CommonLogic.Decrypt(createClientRequest.legalInfo.en_Id)) && Convert.ToInt64(CommonLogic.Decrypt(createClientRequest.legalInfo.en_Id)) > 0)
            {
                GenCompanyContractDetail genCompanyContractDetail = await _iClient.GetGenCompanyContractDetailsById(Convert.ToInt64(CommonLogic.Decrypt(createClientRequest.legalInfo.en_Id))).ConfigureAwait(false);
                companyContractDetail = ModelBinder.BindCompanyContractDetailForClient(companyContractDetail, genCompanyContractDetail, createClientRequest, LoggedInUserId, InsertedCompanyId.Value, true);

                var isCompanyContractDetailUpdate = await _iClient.UpdateCompanyContractDetail(companyContractDetail).ConfigureAwait(false);
                if (isCompanyContractDetailUpdate)
                {
                    createClientRequest.legalInfo.en_Id = CommonLogic.Encrypt(companyContractDetail.Id.ToString());
                }
            }
            else
            {
                companyContractDetail = ModelBinder.BindCompanyContractDetailForClient(companyContractDetail, null, createClientRequest, LoggedInUserId, InsertedCompanyId.Value, false);

                var isCompanyContractDetailCreated = await _iClient.CreateCompanyContractDetail(companyContractDetail).ConfigureAwait(false);
                if (isCompanyContractDetailCreated)
                {
                    createClientRequest.legalInfo.en_Id = CommonLogic.Encrypt(companyContractDetail.Id.ToString());
                }
            }
        }

        private async Task<AddUpdateCompanyViewModel> BindAddUpdateCompany(AddUpdateCompanyViewModel addUpdateCompanyViewModel, GenCompany CompanyData)
        {
            if (addUpdateCompanyViewModel == null)
                addUpdateCompanyViewModel = new AddUpdateCompanyViewModel();

            addUpdateCompanyViewModel.CompanyData.ID = CompanyData.Id;
            addUpdateCompanyViewModel.CompanyData.Company = CompanyData.Company;
            addUpdateCompanyViewModel.CompanyData.Website = CompanyData.Website;
            addUpdateCompanyViewModel.CompanyData.domain_id = 0; // CompanyData.DomainId;
            addUpdateCompanyViewModel.CompanyData.LinkedInProfile = CompanyData.LinkedInProfile;
            addUpdateCompanyViewModel.CompanyData.CompanySize = CompanyData.CompanySize;
            addUpdateCompanyViewModel.CompanyData.TimeZone_ID = 0; // CompanyData.TimeZoneId;
            addUpdateCompanyViewModel.CompanyData.CurrencyID = CompanyData.CurrencyId;
            addUpdateCompanyViewModel.CompanyData.Address = CompanyData.Address;
            addUpdateCompanyViewModel.CompanyData.phone = CompanyData.Phone;
            addUpdateCompanyViewModel.CompanyData.industry = CompanyData.Industry;
            addUpdateCompanyViewModel.CompanyData.city = CompanyData.City;
            addUpdateCompanyViewModel.CompanyData.state = CompanyData.State;
            addUpdateCompanyViewModel.CompanyData.country = CompanyData.Country;
            addUpdateCompanyViewModel.CompanyData.zip = CompanyData.Zip;
            addUpdateCompanyViewModel.CompanyData.IsNDASigned = false; // CompanyData.IsNdasigned;
            addUpdateCompanyViewModel.CompanyData.CreatedByID = CompanyData.CreatedById;
            addUpdateCompanyViewModel.CompanyData.CreatedByDatetime = CompanyData.CreatedByDatetime;
            addUpdateCompanyViewModel.CompanyData.ModifiedByID = CompanyData.ModifiedById;
            addUpdateCompanyViewModel.CompanyData.ModifiedByDatetime = CompanyData.ModifiedByDatetime;
            addUpdateCompanyViewModel.CompanyData.IsActive = CompanyData.IsActive;
            addUpdateCompanyViewModel.CompanyData.Location = CompanyData.Location;
            addUpdateCompanyViewModel.CompanyData.Client_StatusID = CompanyData.ClientStatusId;
            addUpdateCompanyViewModel.CompanyData.TeamManagement = 0; // CompanyData.TeamManagement;
            addUpdateCompanyViewModel.CompanyData.Score = CompanyData.Score;
            addUpdateCompanyViewModel.CompanyData.Category = CompanyData.Category;
            addUpdateCompanyViewModel.CompanyData.GEO_ID = CompanyData.GeoId;
            addUpdateCompanyViewModel.CompanyData.AM_SalesPersonID = CompanyData.AmSalesPersonId;
            addUpdateCompanyViewModel.CompanyData.NBD_SalesPersonID = CompanyData.NbdSalesPersonId;
            addUpdateCompanyViewModel.CompanyData.Discovery_Call = CompanyData.DiscoveryCall;
            addUpdateCompanyViewModel.CompanyData.Lead_Type = CompanyData.LeadType;

            if (!string.IsNullOrEmpty(CompanyData.CompanyLogo))
            {
                addUpdateCompanyViewModel.CompanyData.CompanyLogo = _configuration["ProjectURL_API"].ToString() + "Media/CompanyLogo/" + CompanyData.CompanyLogo;
            }
            else
            {
                addUpdateCompanyViewModel.CompanyData.CompanyLogo = string.Empty;
            }
            addUpdateCompanyViewModel.CompanyData.CompanyIndustry_Type = CompanyData.IndustryType;
            if (CompanyData.GeoId.HasValue)
            {
                var CompanyGeoDetails = await _iClient.GetPrgGeosById(CompanyData.GeoId.Value).ConfigureAwait(false);
                if (CompanyGeoDetails != null)
                    addUpdateCompanyViewModel.CompanyData.CompanyGEO = CompanyGeoDetails.Geo;
                else
                    addUpdateCompanyViewModel.CompanyData.CompanyGEO = "";
            }
            else
            {
                addUpdateCompanyViewModel.CompanyData.CompanyGEO = "";
            }
            addUpdateCompanyViewModel.CompanyData.AboutCompany = CompanyData.AboutCompanyDesc;
            addUpdateCompanyViewModel.CompanyData.HRTypeID = null;
            addUpdateCompanyViewModel.CompanyData.HRTypeText = "";

            if (CompanyData.CompanyTypeId != null && CompanyData.CompanyTypeId > 0)
            {
                var varGetPrgCompanyType = await _iClient.GetPrgCompanyTypeById(CompanyData.CompanyTypeId).ConfigureAwait(false);
                if (varGetPrgCompanyType != null)
                {
                    addUpdateCompanyViewModel.CompanyData.HRTypeID = varGetPrgCompanyType.Id;
                    addUpdateCompanyViewModel.CompanyData.HRTypeText = varGetPrgCompanyType.CompanyType;
                }
            }

            addUpdateCompanyViewModel.CompanyData.VettedProfileViewCredit = CompanyData.VettedProfileViewCredit;
            addUpdateCompanyViewModel.CompanyData.NonVettedProfileViewCredit = CompanyData.NonVettedProfileViewCredit;

            return addUpdateCompanyViewModel;
        }

        private IActionResult Export_ClientHappinessSurveyList(List<sproc_GetClientHappinessSurvey_Result> HappinessSurvey)
        {
            var ExportFileName = "ClientHappinessSurveyExport_" + DateTime.Now.ToString("yyyyMMddHHmm") + @".xlsx";
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("ClientHappinessSurveytList");

            var currentRow = 1;

            worksheet.Cell(currentRow, 1).Value = "AddedDate";
            worksheet.Cell(currentRow, 2).Value = "FeedbackDate";
            worksheet.Cell(currentRow, 3).Value = "Company";
            worksheet.Cell(currentRow, 4).Value = "Client";
            worksheet.Cell(currentRow, 5).Value = "Email";
            worksheet.Cell(currentRow, 6).Value = "Rating";
            worksheet.Cell(currentRow, 7).Value = "Question";
            worksheet.Cell(currentRow, 8).Value = "Options";
            worksheet.Cell(currentRow, 9).Value = "Comments";
            worksheet.Cell(currentRow, 10).Value = "Link";

            foreach (var HS in HappinessSurvey)
            {
                currentRow++;
                var currentColumn = 1;

                worksheet.Cell(currentRow, currentColumn++).Value = HS.AddedDate;
                worksheet.Cell(currentRow, currentColumn++).Value = HS.FeedbackDate;
                worksheet.Cell(currentRow, currentColumn++).Value = HS.Company;
                worksheet.Cell(currentRow, currentColumn++).Value = HS.Client;
                worksheet.Cell(currentRow, currentColumn++).Value = HS.Email;
                worksheet.Cell(currentRow, currentColumn++).Value = HS.Rating;
                worksheet.Cell(currentRow, currentColumn++).Value = HS.Question;
                worksheet.Cell(currentRow, currentColumn++).Value = HS.Options;
                worksheet.Cell(currentRow, currentColumn++).Value = HS.Comments;
                worksheet.Cell(currentRow, currentColumn++).Value = HS.Link;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ExportFileName);
        }
        #endregion

        #region  Function to send Data to HubSpot
        private ContactResponseUtsAdmin SendHubSpotData(HubSpot hubSpot)
        {
            var client = new RestClient("https://api.hubapi.com/contacts/v1/"); // Replace with your API endpoint
            var request = new RestRequest("/contact", RestSharp.Method.Post); // Replace with your specific endpoint and HTTP method

            // Convert the Person object to JSON and add it as the request body
            request.AddJsonBody(hubSpot);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer pat-na1-0b521ca1-6639-47db-a45f-3dae7c6ce7b3"); // Replace with your actual access token

            // Execute the request
            var response = client.Execute(request);
            ContactResponseUtsAdmin? hubspotContactData = new();
            // Check the response
            if (response.IsSuccessful)
            {
                hubspotContactData = JsonConvert.DeserializeObject<ContactResponseUtsAdmin>(response.Content);
                Console.WriteLine("Data sent successfully.");
            }
            else
            {
                Console.WriteLine("Error sending data. Response: " + response.Content);
            }

            return hubspotContactData;
        }

        private void SendCompanyContactAssociationDataHubSpotData(long vid, long cid)
        {
            var client = new RestClient("https://api.hubapi.com/companies/v2/"); // Replace with your API endpoint
            var request = new RestRequest($"/companies/{cid}/contacts/{vid}", RestSharp.Method.Put); // Replace with your specific endpoint and HTTP method

            // Convert the Person object to JSON and add it as the request body
            //request.AddJsonBody(hubSpot);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer pat-na1-0b521ca1-6639-47db-a45f-3dae7c6ce7b3"); // Replace with your actual access token

            // Execute the request
            var response = client.Execute(request);
            CompanyDetailsUtsadmin? hubspotCompanyData = new();
            // Check the response
            if (response.IsSuccessful)
            {
                hubspotCompanyData = JsonConvert.DeserializeObject<CompanyDetailsUtsadmin>(response.Content);
                Console.WriteLine("Data sent successfully.");
            }
            else
            {
                Console.WriteLine("Error sending data. Response: " + response.Content);
            }
            //return hubspotCompanyData;
        }

        #endregion

        #region  Company Logo Scrapping
        [NonAction]
        public async Task<string> CompanyLogo(string Company)
        {
            string pythonURL = _configuration["PythonScrapeCompanyLogo"] + "?CompanyURL=" + Company;
            string content;
            string logoName = "";

            if (!string.IsNullOrWhiteSpace(pythonURL))
            {
                using (WebResponse wr = await WebRequest.Create(pythonURL).GetResponseAsync())
                {
                    using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                    {
                        content = await sr.ReadToEndAsync();
                        if (content.Trim() != "Logo not found")
                        {
                            logoName = content.Trim();
                        }
                    }
                };
            }

            return logoName;
        }
        #endregion

        #region  Fetch Company Details
        private async Task<AllCompanyDetails> GetCompanyDetailsFromPython(string companyName)
        {
            string pythonURL = _configuration["PythonParsingCompanyDetailsFromName"] + "?CompanyURL=" + companyName;
            string content;
            AllCompanyDetails? companyDetailsViewModel = new AllCompanyDetails();

            try
            {
                if (!string.IsNullOrWhiteSpace(pythonURL))
                {
                    using (WebResponse wr = await WebRequest.Create(pythonURL).GetResponseAsync())
                    {
                        using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                        {
                            content = await sr.ReadToEndAsync();
                            if (!string.IsNullOrEmpty(content))
                            {
                                string pattern = @"\{(?:[^{}]|(?<Open>\{)|(?<-Open>\}))+(?(Open)(?!))\}";
                                string JsonString = "";
                                Regex regex = new Regex(pattern);
                                MatchCollection matches = regex.Matches(content);
                                foreach (Match match in matches)
                                {
                                    JsonString = match.Value;
                                }
                                if (!string.IsNullOrEmpty(JsonString))
                                {
                                    companyDetailsViewModel = JsonConvert.DeserializeObject<AllCompanyDetails>(JsonString);
                                }
                            }
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                companyDetailsViewModel = new AllCompanyDetails();
            }

            return companyDetailsViewModel;
        }

        #endregion

        #region Fetch SalesUSers with Heads EmailID while Crating GSpace
        [HttpGet("GetSalesUserWithHead")]
        public async Task<ObjectResult> GetSalesUserWithHead(string EmailID)
        {
            try
            {
                #region Validation

                if (string.IsNullOrEmpty(EmailID))
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide Client Email." });
                }
                #endregion


                var varActiveSalesUserList = _iClient.Sproc_Fetch_All_SalesUsers_WithHead_For_Client(EmailID);

                if (varActiveSalesUserList.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Sales User List", Details = varActiveSalesUserList });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "No data" });

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update_SpaceID_For_Client
        [HttpPost("UpdateSpaceIDForClient")]
        public async Task<ObjectResult> UpdateSpaceIDForClient([FromBody] UpdateSpaceIDForClient updateSpaceIDForClient)
        {
            try
            {
                #region Validation

                if (updateSpaceIDForClient == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide data." });
                }
                #endregion

                object[] param = new object[]
                {
                    updateSpaceIDForClient.clientEmail,updateSpaceIDForClient.SpaceID,updateSpaceIDForClient.TokenObject
                };
                string paramasString = CommonLogic.ConvertToParamString(param);

                _iClient.Sproc_Update_SpaceID_For_Client(paramasString);
                
                 return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success" });
               
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region List Credit Utilization
        [HttpPost("ListCreditUtilization")]
        public async Task<ObjectResult> GetCreditUtilizationList([FromBody] CreditUtilizationListFilter creditUtilizationListFilter)
        {
            try
            {
                var varLoggedInUserId = SessionValues.LoginUserId;
                
                #region PreValidation
                if (creditUtilizationListFilter == null || (creditUtilizationListFilter.pagenumber == 0 || creditUtilizationListFilter.totalrecord == 0))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide pagenumber and totalrecord" });
                #endregion

                string Sortdatafield = "CreatedByDate";
                string Sortorder = "DESC";

                object[] param = new object[]
                {
                    creditUtilizationListFilter.CompanyId,
                    creditUtilizationListFilter.pagenumber,
                    creditUtilizationListFilter.totalrecord,
                    Sortdatafield,
                    Sortorder
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<Sproc_Get_Credit_Transaction_CompanyWise_Result> CreditTransactionData = await _iClient.Sproc_Get_Credit_Transaction_CompanyWise(paramasString).ConfigureAwait(false);

                if (CreditTransactionData.Any())
                {      
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CreditTransactionData });
                }
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Reset All HR Talent status for Demo client

        [Authorize]
        [HttpGet("ResetAllDemoHRTalentStatus")]
        public ObjectResult ResetAllDemoHRTalentStatus()
        {
            try
            {
                _iClient.Sproc_Reset_AllHR_TalentStatus();

                ATSCall aTSCall = new(_configuration, _talentConnectAdminDBContext);
                aTSCall.SendResetAllDemoHRRequest("", SessionValues.LoginUserId, 0);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Sucess" });

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
