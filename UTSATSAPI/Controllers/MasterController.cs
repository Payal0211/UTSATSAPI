namespace UTSATSAPI.Controllers
{
    using ClosedXML.Excel;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using System;
    using System.Dynamic;
    using System.Net;
    using System.Net.Http.Headers;
    using UTSATSAPI.ATSCalls;
    using UTSATSAPI.Helpers;
    using UTSATSAPI.Helpers.Common;
    using UTSATSAPI.Models.ComplexTypes;
    using UTSATSAPI.Models.Models;
    using UTSATSAPI.Models.ViewModels;
    using UTSATSAPI.Repositories.Interfaces;

    [Route("MastersAPI/", Name = "User Related Services")]
    public class MasterController : ControllerBase
    {
        #region Variables
        private readonly TalentConnectAdminDBContext _talentConnectAdminDBContext;
        private readonly ICommonInterface _commonInterface;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUniversalProcRunner _universalProcRunner;
        private readonly IUsers _iUsers;
        private readonly IMasters _iMasters;
        private readonly IConfiguration _iConfiguration;
        #endregion

        #region Constructor
        public MasterController(ICommonInterface commonInterface, IConfiguration iConfiguration, TalentConnectAdminDBContext talentConnectAdminDBContext, IHttpContextAccessor httpContextAccessor, IUniversalProcRunner universalProcRunner, IUsers iUsers, IMasters iMasters)
        {
            _talentConnectAdminDBContext = talentConnectAdminDBContext;
            _commonInterface = commonInterface;
            _httpContextAccessor = httpContextAccessor;
            _universalProcRunner = universalProcRunner;
            _iUsers = iUsers;
            _iMasters = iMasters;
            _iConfiguration = iConfiguration;
        }
        #endregion

        #region Authorized API
        [Authorize]
        [HttpGet("GetFixedValues")]
        public ObjectResult GetFixedDropDownValues()
        {
            try
            {
                List<MastersResponseModel> bindLeadTypes = new List<MastersResponseModel>();

                MastersResponseModel bindLeadType1 = new MastersResponseModel();
                bindLeadType1.Id = 1;
                bindLeadType1.Value = "InBound";
                bindLeadTypes.Add(bindLeadType1);

                MastersResponseModel bindLeadType2 = new MastersResponseModel();
                bindLeadType2.Id = 2;
                bindLeadType2.Value = "OutBound";
                bindLeadTypes.Add(bindLeadType2);

                //Added a new option called partnership for the Lead Type dropdown.
                MastersResponseModel bindLeadType3 = new MastersResponseModel();
                bindLeadType3.Id = 3;
                bindLeadType3.Value = "Partnership";
                bindLeadTypes.Add(bindLeadType3);

                Dictionary<int, string> BindTeamManagement = new Dictionary<int, string>
            {
                { 0, "No" },
                { 1, "Yes" }
            };
                Dictionary<string, string> BindHiringSkillLevel = new Dictionary<string, string>
            {
                { "Strong", "Strong" },
                { "Moderate", "Moderate" },
                { "Basic" , "Basic" }
            };

                Dictionary<string, string> BindHiringCommunicationType = new Dictionary<string, string>
            {
                { "Good", "Good" },
                { "Very Good", "Very Good" },
                { "Excellent" , "Excellent" }
            };

                List<MastersResponseModel> BindHiringAvailability = new List<MastersResponseModel>();

                MastersResponseModel BindHiringAvailability1 = new MastersResponseModel();
                BindHiringAvailability1.Id = 1;
                BindHiringAvailability1.Value = "Part Time";
                BindHiringAvailability.Add(BindHiringAvailability1);

                MastersResponseModel BindHiringAvailability2 = new MastersResponseModel();
                BindHiringAvailability2.Id = 2;
                BindHiringAvailability2.Value = "Full Time";
                BindHiringAvailability.Add(BindHiringAvailability2);


                Dictionary<string, string> BindLongTermShortTerm = new Dictionary<string, string>
            {
                { "Long Term", "Long Term" },
                { "Short Term", "Short Term" }
            };
                Dictionary<bool, string> BindIsHiringLimited = new Dictionary<bool, string>
            {
                { true, "Temporary" },
                { false, "Permanent" }
            };


                List<MastersResponseModel> BindInBoundDrps = new List<MastersResponseModel>();

                BindInBoundDrps.Add(new MastersResponseModel() { Id = 1, Value = "Inbound SEO" });
                BindInBoundDrps.Add(new MastersResponseModel() { Id = 2, Value = "Inbound DL" });
                BindInBoundDrps.Add(new MastersResponseModel() { Id = 3, Value = "Inbound CMS" });
                BindInBoundDrps.Add(new MastersResponseModel() { Id = 4, Value = "Inbound LI" });
                BindInBoundDrps.Add(new MastersResponseModel() { Id = 5, Value = "Inbound Direct" });
                BindInBoundDrps.Add(new MastersResponseModel() { Id = 6, Value = "Inbound ABM" });

                List<MastersResponseModel> RescheduleReasons = new List<MastersResponseModel>();

                RescheduleReasons.Add(new MastersResponseModel() { Id = 1, Value = "Client not available on given Slots" });
                RescheduleReasons.Add(new MastersResponseModel() { Id = 2, Value = "Client not available on selected Slot" });
                RescheduleReasons.Add(new MastersResponseModel() { Id = 3, Value = "Talent not available on given Slots" });
                RescheduleReasons.Add(new MastersResponseModel() { Id = 4, Value = "Talent not available on selected Slot" });
                RescheduleReasons.Add(new MastersResponseModel() { Id = 5, Value = "Other" });

                List<MastersResponseModel> BindAccessRoleType = new List<MastersResponseModel>();
                BindAccessRoleType.Add(new MastersResponseModel() { Id = 1, Value = "Admin" });
                BindAccessRoleType.Add(new MastersResponseModel() { Id = 2, Value = "All Jobs" });
                BindAccessRoleType.Add(new MastersResponseModel() { Id = 3, Value = "My Jobs" });

                var CompanyPerks = _talentConnectAdminDBContext.PrgCompanyPerks.Select(x => new MastersResponseModel { Value = x.PerksName, Id = x.Id }).ToList();

                dynamic fixedValues = new ExpandoObject();
                fixedValues.BindLeadType = bindLeadTypes;
                fixedValues.BindTeamManagement = BindTeamManagement;
                fixedValues.BindHiringSkillLevel = BindHiringSkillLevel;
                fixedValues.BindHiringAvailability = BindHiringAvailability;
                fixedValues.BindLongTermShortTerm = BindLongTermShortTerm;
                fixedValues.BindIsHiringLimited = BindIsHiringLimited;
                fixedValues.BindInBoundDrp = BindInBoundDrps;
                fixedValues.BindHiringCommunicationType = BindHiringCommunicationType;
                fixedValues.RescheduleReasons = RescheduleReasons;
                fixedValues.BindAccessRoleType = BindAccessRoleType;
                fixedValues.CompanyPerks = CompanyPerks;

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Fixed Values", Details = fixedValues });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetGeo")]
        public ObjectResult GetGEO()
        {
            try
            {
                var geoList = _talentConnectAdminDBContext.PrgGeos.Select(x => new MastersResponseModel { Value = x.Geo, Id = x.Id }).ToList();

                if (geoList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Geo List", Details = geoList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetCurrency")]
        public ObjectResult GetCurrency()
        {
            try
            {
                var currencyList = _talentConnectAdminDBContext.PrgCurrencyExchangeRates.Select(x => new MastersResponseModel { Value = x.CurrencyCode, text = x.CurrencyCode, Id = x.Id }).ToList();

                if (currencyList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Currency List", Details = currencyList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetTalentTimeZone")]
        public ObjectResult GetTalentTimeZone()
        {
            try
            {
                var talentTimeZoneList = _talentConnectAdminDBContext.PrgTalentTimeZones.Where(x => x.IsActive == true).Select(x => new MastersResponseModel { Value = x.TalentTimeZone, Id = x.Id }).ToList();

                if (talentTimeZoneList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Talent TimeZone List", Details = talentTimeZoneList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetTimeZonePreference")]
        public ObjectResult GetTimeZonePreference(long timezoneid)
        {
            try
            {
                var GetTimeZonePreference = _talentConnectAdminDBContext.PrgTimeZonePreferences.Where(x => x.HrTimeZoneId == timezoneid).Select(x => new MastersResponseModel { Value = x.WorkingTimePreference, Id = x.Id }).ToList();

                if (GetTimeZonePreference.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "GetTimeZonePreference", Details = GetTimeZonePreference });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetHowSoon")]
        public ObjectResult GetHowSoon()
        {
            try
            {
                var HowSoonTalents = _talentConnectAdminDBContext.PrgHowSoonClientWantTalents.Where(x => x.IsActive == true).Select(x => new MastersResponseModel { Value = x.HowSoonClientwantTalent, Id = x.Id }).ToList();

                if (HowSoonTalents.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "HowSoon Talent", Details = HowSoonTalents });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetSkills")]
        public ObjectResult GetSkills()
        {
            try
            {
                object[] objectsParam = new object[]
               {
                 0,0,0
               };
                var skillSet = _iMasters.GetSkills(CommonLogic.ConvertToParamString(objectsParam));

                if (skillSet.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Skill List", Details = skillSet });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }


        [Authorize]
        [HttpGet("GetHRSkills")]
        public ObjectResult GetHRSkills(long hrId)
        {
            try
            {
                object[] objectsParam = new object[]
               {
                 0,0,0
               };
                var skillSet = _iMasters.GetSkills(CommonLogic.ConvertToParamString(objectsParam));

                // Fetch the temp skills for the respective HR.

                var hrTempSkills = _talentConnectAdminDBContext.GenSalesHiringRequestSkillDetails.Where(x => x.HiringRequestId == hrId && x.TempSkillId != null).ToList();

                foreach (var skill in hrTempSkills)
                {
                    var tempSkills = _talentConnectAdminDBContext.PrgTempSkills.Where(s => s.TempSkillId == skill.TempSkillId).Select(x => new Sproc_Get_PrgSkill_NotusedinMappingtables_Result { Value = x.TempSkill, ID = x.TempSkillId }).FirstOrDefault();
                    if (tempSkills != null)
                    {
                        skillSet.Add(tempSkills);
                    }
                }

                if (skillSet.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Skill List", Details = skillSet });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }


        [Authorize]
        [HttpGet("GetPartialEngagementType")]
        public ObjectResult GetPartialEngagementType()
        {
            try
            {
                var PartialEngagementTypes = _talentConnectAdminDBContext.PrgPartialEngagementTypes.Where(x => x.IsActive == true).Select(x => new MastersResponseModel { Value = x.PartialEngagementType, Id = x.Id }).ToList();

                if (PartialEngagementTypes.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "PartialEngagementTypes", Details = PartialEngagementTypes });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetTalentsRoles")]
        public ObjectResult GetTalentsRoles()
        {
            try
            {
                var TalentsRoles = _talentConnectAdminDBContext.PrgTalentRoles.Where(x => x.IsActive == true).OrderBy(x => x.TalentRole).Select(x => new MastersResponseModel { Value = x.TalentRole, Id = x.Id }).ToList();

                if (TalentsRoles.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "TalentsRoles", Details = TalentsRoles });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetTypeOfPerson")]
        public ObjectResult GetTypeOfPerson()
        {
            try
            {
                var GetTypeOfPerson = _talentConnectAdminDBContext.PrgCompanyTypeofInterviewers.Where(x => x.IsActive == true).Select(x => new MastersResponseModel { Value = x.TypeofInterviewer, Id = x.Id }).ToList();

                if (GetTypeOfPerson.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Type of Persons", Details = GetTypeOfPerson });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetRegions")]
        public ObjectResult GetRegions()
        {
            try
            {
                var regions = _talentConnectAdminDBContext.PrgCountryRegions.Where(x => x.IsActive == true).Select(x => new MastersResponseModel { Value = x.Country, Id = x.Id }).ToList();

                if (regions.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "List of regions", Details = regions });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetTimeZone")]
        public ObjectResult GetTimeZone()
        {
            try
            {
                var timezones = _talentConnectAdminDBContext.PrgContactTimeZones.Where(x => x.IsActive == true).Select(x => new MastersResponseModel { Value = x.TimeZoneTitle, Id = x.Id }).ToList();

                if (timezones.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "List of Timezones", Details = timezones });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetCodeandFlag")]
        public ObjectResult GetCodeandFlag()
        {
            try
            {
                string parentFolderPath = string.Format("{0}/{1}", Path.Combine(Directory.GetCurrentDirectory(), "Media", "ContryFlag-Code"), "CodeFlag.json");
                string content = System.IO.File.ReadAllText(parentFolderPath);
                List<CountryFlagCode> results = JsonConvert.DeserializeObject<List<CountryFlagCode>>(content);

                if (results != null && results.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "ConutryCode and Flag", Details = results });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetSalesman")]
        public async Task<ObjectResult> GetSalesman()
        {
            try
            {
                List<Sproc_UTS_GetSalesPerson_Result> list = await _iMasters.GetSalesPerson();

                var SalesManSet = list.Select(x => new MastersResponseModel { Id = x.UserID, Value = x.UserName }).ToList();

                if (SalesManSet.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Sasles Man List", Details = SalesManSet });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }


        [Authorize]
        [HttpGet("GetHRDeleteReason")]
        public ObjectResult GetHRDeleteReason()
        {
            try
            {
                var GetHRDeleteReason = _talentConnectAdminDBContext.PrgHrdeleteReasons.OrderBy(t => t.SequenceId).Where(x => x.IsActive == true).Select(x => new MastersResponseModel { Value = x.Reason, Id = x.Id }).ToList();

                if (GetHRDeleteReason.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "List of reasons", Details = GetHRDeleteReason });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("FileUpload")]
        public ObjectResult FileUpload(FileUploadVModel fileUploadVModel)
        {
            try
            {
                var file = fileUploadVModel._file;
                var folderName = Path.Combine("Media", fileUploadVModel.FileUploadFor);
                var pathToSave = CommonLogic.GetFileUploadFolderFor(folderName);
                if (file != null)
                {
                    if (file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        string modifiedFilename = string.Format("{0}_{1}", DateTime.Now.ToFileTime(), fileName);
                        var fullPath = Path.Combine(pathToSave, modifiedFilename);
                        var dbPath = Path.Combine(folderName, modifiedFilename);
                        string host = _httpContextAccessor.HttpContext.Request.Host.Value;
                        string returnurl = string.Format("{0}\\{1}", string.Format("http://{0}", host), dbPath).Replace("\\Images\\", "//").Replace("\\", @"/").Replace("//", @"/");
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "File has been uploaded", Details = returnurl });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please select a file" });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please select a file" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetModeOfWork")]
        public ObjectResult GetModeOfWork()
        {
            try
            {
                var modeofworkList = _talentConnectAdminDBContext.PrgModeOfWorkings.Select(x => new MastersResponseModel { Value = x.ModeOfWorking, Id = x.Id }).ToList();

                if (modeofworkList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Mode of Work List", Details = modeofworkList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetCountry")]
        public ObjectResult GetCountry()
        {
            try
            {
                var GetCountry = _talentConnectAdminDBContext.PrgCountryRegions.Select(x => new MastersResponseModel { Value = x.Country + " (" + x.CountryRegion + ")", Id = x.Id }).ToList();

                if (GetCountry.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Country List", Details = GetCountry });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("IsNewUser")]
        public ObjectResult IsNewUser()
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>
                {
                    { "NBD", true },
                    { "AM", false },
                };
                List<MastersResponseModel> mastersResponses = MakeMasterList(dict);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Is New User", Details = mastersResponses });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("ODRPool")]
        public ObjectResult ODRPool()
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>
                {
                    { "ODR", true },
                    { "Pool", false },
                };
                List<MastersResponseModel> mastersResponses = MakeMasterList(dict);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "ODR Pool Values", Details = mastersResponses });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("ContractType")]
        public ObjectResult ContractType()
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>
                {
                    { "Full Time", "Full Time" },
                    { "Part Time", "Part Time" },
                };
                List<MastersResponseModel> mastersResponses = MakeMasterList(dict);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Contact Type Values", Details = mastersResponses });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("Buddy")]
        public ObjectResult Buddy()
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>
                {
                    { "Buddy", "Buddy" },
                    { "Select Later", "Select Later" },
                };
                List<MastersResponseModel> mastersResponses = MakeMasterList(dict);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Buddy Option", Details = mastersResponses });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("YesNoOption")]
        public ObjectResult YesNoOption()
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>
                {
                    { "yes", "yes" },
                    { "No", "No" },
                };
                List<MastersResponseModel> mastersResponses = MakeMasterList(dict);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Yes No Options", Details = mastersResponses });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("NetPaymentDays")]
        public ObjectResult NetPaymentDays()
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>
                {
                    { "7", 7 },
                    { "15", 15 },
                    { "30", 30 },
                    { "45", 45 },
                    { "60", 60 }
                };
                List<MastersResponseModel> mastersResponses = MakeMasterList(dict);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Net Payment Days", Details = mastersResponses });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("ContractRenewal")]
        public ObjectResult ContractRenewal()
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>
                {
                    { "0", 0 },
                    { "1", 1 },
                    { "2", 2 },
                    { "3", 3 },
                    { "4", 4 },
                    { "5", 5 }
                };

                List<MastersResponseModel> mastersResponses = MakeMasterList(dict);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Contract Renewal", Details = mastersResponses });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetUserType")]
        public ObjectResult GetUserType()
        {
            try
            {
                var GetUserTypeList = _talentConnectAdminDBContext.UsrUserTypes.Where(x => x.IsDisplay == true).Select(x => new MastersResponseModel { Value = x.UserType, Id = x.Id }).ToList();

                if (GetUserTypeList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "User Type List", Details = GetUserTypeList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetTeamManager")]
        public ObjectResult GetTeamManager()
        {
            try
            {
                var GetTeamManager = _talentConnectAdminDBContext.UsrUsers.Where(x => x.UserTypeId == 9 && x.IsActive == true).Select(x => new MastersResponseModel { Value = x.FullName, Id = x.Id }).ToList();

                if (GetTeamManager.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Team Manager List", Details = GetTeamManager });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetSalesManager_IsNewUser")]
        public ObjectResult GetSalesManager_IsNewUser()
        {
            try
            {
                var GetTeamManager = _talentConnectAdminDBContext.UsrUsers.Where(x => x.UserTypeId == 9 && x.IsNewUser == true).Select(x => new MastersResponseModel { Value = x.FullName, Id = x.Id }).ToList();

                if (GetTeamManager.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Team Manager List", Details = GetTeamManager });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetUserByType")]
        public async Task<ObjectResult> GetUserByType(int userType)
        {
            try
            {
                #region PreValidation
                if (userType == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please enter proper usertype" });
                #endregion

                List<Sproc_UTS_GetUserByType_Result> list = await _iMasters.GetUserByType(userType);

                var GetUserByType = list.Select(x => new MastersResponseModel
                {
                    Id = x.ID,
                    Value = x.UserRole
                });

                if (GetUserByType.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "UserRoles By type", Details = GetUserByType });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetFetchReporteeTeamManager")]
        public ObjectResult GetFetchReporteeTeamManager(int userType)
        {
            try
            {
                #region PreValidation
                if (userType == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please enter proper usertype" });
                #endregion

                if (userType > 0)
                {
                    var UserTypelist = _talentConnectAdminDBContext.UsrUserHierarchies.Where(x => x.ParentId == userType).ToList();
                    List<UsrUser> ReporteeUserList = new List<UsrUser>();
                    if (UserTypelist.Count > 0)
                    {
                        foreach (var item in UserTypelist)
                        {
                            var RepUserUser = _talentConnectAdminDBContext.UsrUsers.FirstOrDefault(x => x.Id == item.UserId);
                            if (RepUserUser != null)
                                ReporteeUserList.Add(RepUserUser);
                        }
                        var listUSers = ReporteeUserList.Select(x => new MastersResponseModel
                        {
                            Id = x.Id,
                            Value = x.FullName
                        });
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "UserRoles found", Details = listUSers });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "UserRoles Not found" });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "UserRoles Not found" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        [Authorize]
        [HttpGet("FetchBDRMarketingBasedOnUserType")]
        public ObjectResult FetchBDRMarketingBasedOnUserType(int RoleID)
        {
            try
            {
                #region PreValidation
                if (RoleID == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please enter proper RoleID" });
                #endregion

                object[] param = new object[] { RoleID };

                List<MastersResponseModel> userList = _iUsers.sproc_UTS_GetBDR_Marketingusers(CommonLogic.ConvertToParamString(param)).Select(x => new MastersResponseModel
                {
                    Id = x.ID,
                    Value = x.Fullname
                }).ToList();

                if (userList.Count > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Details = userList });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("FetchTeamManagerBasedOnUserType")]
        public ObjectResult FetchTeamManagerBasedOnUserType(long UserTypeID)
        {
            try
            {
                #region PreValidation
                if (UserTypeID == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please enter proper User Role Type ID" });
                #endregion
                var UserTypelist = _talentConnectAdminDBContext.UsrUsers.Where(x => x.UserTypeId == UserTypeID).ToList();
                if (UserTypelist.Count > 0)
                {
                    var teamManger = UserTypelist.Select(x => new MastersResponseModel
                    {
                        Id = x.Id,
                        Value = x.FullName
                    });
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Details = teamManger });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Teammangers not found" });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("AddOtherSkill")]
        public ObjectResult AddOtherSkill(string skillname, long LoggedInUserId = 0)
        {
            try
            {
                LoggedInUserId = SessionValues.LoginUserId;

                #region PreValidation
                if (string.IsNullOrEmpty(skillname))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please enter skill name" });
                #endregion

                var skillDetails = _talentConnectAdminDBContext.PrgTempSkills.FirstOrDefault(x => x.TempSkill.ToLower() == skillname.ToLower());
                if (skillDetails != null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Temp Skill Exists", Details = skillDetails });
                }
                else
                {
                    var MainskillDetails = _talentConnectAdminDBContext.PrgSkills.FirstOrDefault(x => x.Skill.ToLower() == skillname.ToLower() && x.IsActive == true);
                    if (MainskillDetails != null)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Skill Exists" });
                    }
                    object[] param = new object[] { skillname, LoggedInUserId, DateTime.Now.ToString("yyyy-MM-dd"), 0, DateTime.Now.ToString("yyyy-MM-dd"), true };
                    var result = _iMasters.AddSkills(CommonLogic.ConvertToParamString(param));
                    if (result != null)
                    {
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Skill has been added", Details = result });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Skill can not be added" });
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("CheckOtherRole")]
        public ObjectResult CheckOtherRole(string rolename, int RoleId)
        {
            try
            {
                #region PreValidation
                if (string.IsNullOrEmpty(rolename))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please enter Role name" });
                #endregion

                var RoleDetails = _talentConnectAdminDBContext.PrgTalentRoles.FirstOrDefault(x => x.TalentRole.ToLower() == rolename.ToLower() && x.Id != RoleId);
                if (RoleDetails != null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Role exists" });
                }
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Role Not Exists" });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetNRMargin")]
        public ObjectResult GetNRMargin()
        {
            try
            {
                var NRMargin = _talentConnectAdminDBContext.GenSystemConfigurations.Where(x => x.Key == "TalentCostCalculationPercentage" && x.IsActive == true).FirstOrDefault();
                if (NRMargin != null)
                {
                    MastersResponseModel mastersResponseModel = new MastersResponseModel() { text = NRMargin.Key, Value = Convert.ToDecimal(NRMargin.Value) };
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "NR Margin", Details = mastersResponseModel });
                }
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the department.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetDepartment")]
        public async Task<ObjectResult> GetDepartment()
        {
            try
            {
                var UserDepartment = await (_talentConnectAdminDBContext.PrgDepartments.Where(x => x.IsActive == true).Select(x => new MastersResponseModel { Value = x.DeptName, Id = x.Id })).ToListAsync();

                if (UserDepartment.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = UserDepartment });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Bad Request" });
            }
        }

        /// <summary>
        /// Gets the team.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetTeam")]
        public async Task<ObjectResult> GetTeam(long departmentID)
        {
            try
            {
                var UserTeam = await (_talentConnectAdminDBContext.GenTeams.Where(x => x.DeptId == departmentID).Select(x => new MastersResponseModel { Value = x.Team, Id = x.Id, text = x.DeptId.ToString() })).ToListAsync();

                if (UserTeam.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = UserTeam });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Bad Request" });
            }
        }

        /// <summary>
        /// Gets the level.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetLevel")]
        public async Task<ObjectResult> GetLevel()
        {
            try
            {
                var UserLevel = await (_talentConnectAdminDBContext.PrgOrgLevels.Select(x => new MastersResponseModel { Value = x.LevelName, Id = x.Id })).ToListAsync();

                if (UserLevel.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = UserLevel });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Bad Request" });
            }
        }

        /// <summary>
        /// Gets the reporting user.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetReportingUser")]
        public async Task<ObjectResult> GetReportingUser(long DeptID, long LevelID)
        {
            try
            {
                if (LevelID > 0 && DeptID > 0)
                {
                    var UsersList = await _talentConnectAdminDBContext.Set<sproc_GetReportingUsers_Result>().FromSqlRaw(string.Format("{0} {1}, {2}", Constants.ProcConstant.sproc_GetReportingUsers, DeptID, LevelID)).ToListAsync();

                    if (UsersList.Any())
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = UsersList });
                    else
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
                }
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Bad Request" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Bad Request" });
            }
        }

        [Authorize]
        [HttpGet("GetHiringNeed")]
        public ObjectResult GetHiringNeedDropdown()
        {
            try
            {
                var BindHiringLimited = new Dictionary<string, string>
                {
                    { "0", "--Select--" },
                    { "1", "Yes, it's for a limited Project" },
                    { "2", "No, They want to hire for long term" }
                };

                var HiringType = BindHiringLimited.Select(x => new SelectListItem { Text = x.Value, Value = x.Key.ToString() }).ToList();

                if (HiringType.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = HiringType });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Bad Request" });
            }
        }

        [Authorize]
        [HttpGet("GetModeOfWorking")]
        public async Task<ObjectResult> GetModeOfWorkingDropdown()
        {
            try
            {
                var ModeOfWorkingDrp = new List<SelectListItem>();

                ModeOfWorkingDrp = await (_talentConnectAdminDBContext.PrgModeOfWorkings.Where(x => x.IsActive == true).Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.ModeOfWorking
                })).ToListAsync();

                ModeOfWorkingDrp.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

                if (ModeOfWorkingDrp.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = ModeOfWorkingDrp });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Bad Request" });
            }
        }

        [Authorize]
        [HttpGet("GetTypeOfHR")]
        public async Task<ObjectResult> GetTypeOfHRDropdown()
        {
            try
            {
                var BindHRType = new Dictionary<string, string>
                {
                    { "-1", "Select" },
                    { "0", "Contractual" },
                    { "1", "Direct Placement" }
                };

                var HRType = BindHRType.ToList().Select(x => new SelectListItem
                {
                    Value = x.Key.ToString(),
                    Text = x.Value
                });

                if (HRType.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = HRType });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Bad Request" });
            }
        }

        [Authorize]
        [HttpGet("GetCompanCategory")]
        public async Task<ObjectResult> GetCompanCategoryDropdown()
        {
            try
            {
                var Company_Categories = new Dictionary<string, string>
                {
                    { "0", "Select" },
                    { "1", "A" },
                    { "2", "B" },
                    { "3", "C" }
                };

                var CompanyCategories = Company_Categories.ToList().Select(x => new SelectListItem
                {
                    Value = x.Key.ToString(),
                    Text = x.Value
                });

                if (CompanyCategories.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CompanyCategories });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Bad Request" });
            }
        }

        [Authorize]
        [HttpGet("GetReplacement")]
        public async Task<ObjectResult> GetReplacementDropdown()
        {
            try
            {
                var Replacement = new Dictionary<string, string>
                {
                    { "0", "No" },
                    { "1", "Yes" },
                };

                var ReplacementList = Replacement.ToList().Select(x => new SelectListItem
                {
                    Value = x.Key.ToString(),
                    Text = x.Value
                });

                if (ReplacementList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = ReplacementList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Bad Request" });
            }
        }

        [Authorize]
        [HttpGet("GetHead")]
        public async Task<ObjectResult> GetHeadDropdown()
        {
            try
            {
                var TeamManagerUserList = new List<SelectListItem>();

                TeamManagerUserList = await (_talentConnectAdminDBContext.UsrUsers.Where(x => x.DeptId == 1 && x.LevelId == 1 && x.IsActive == true).Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.FullName
                })).ToListAsync();

                if (TeamManagerUserList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = TeamManagerUserList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Bad Request" });
            }
        }

        /// <summary>
        /// Gets the managed dropdown.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetManaged")]
        public async Task<ObjectResult> GetManagedDropdown()
        {
            try
            {
                List<SelectListItem> managedList = new List<SelectListItem>()
                {
                    new SelectListItem() { Text = "Select ", Value = "0", Selected = true },
                    new SelectListItem() { Text = "Managed", Value = "1", Selected = false },
                    new SelectListItem() { Text = "Self Managed ", Value = "2", Selected = false }
                };
                if (managedList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = managedList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Bad Request" });
            }
        }

        [Authorize]
        [HttpGet("GetUserHierarchy")]
        public async Task<ObjectResult> GetUserHierarchy(int Parentid, bool IsOpsUser)
        {
            try
            {
                List<SROC_GET_HIERARCHY_Result> ListUsers = new List<SROC_GET_HIERARCHY_Result>();
                ListUsers = await _iMasters.GetHierarchy(Parentid, IsOpsUser).ConfigureAwait(false);

                if (ListUsers.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = ListUsers });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Bad Request" });
            }
        }
        #region Country
        [Authorize]
        [HttpPost("GetCountryList")]
        public async Task<ObjectResult> GetCountryList([FromBody] CommonFilterModel filter)
        {
            object[] param = new object[]
                {
                    filter.PageIndex > 0 ? filter.PageIndex : 1,
                    filter.PageSize > 0 ? filter.PageSize : 50,
                    string.IsNullOrEmpty(filter.SortExpression) ? "ID" : filter.SortExpression,
                    string.IsNullOrEmpty(filter.SortDirection) ? "asc" : filter.SortDirection
                };

            string paramasString = CommonLogic.ConvertToParamString(param);
            filter.PageIndex = filter.PageIndex > 0 ? filter.PageIndex : 1;
            filter.PageSize = filter.PageSize > 0 ? filter.PageSize : 50;

            List<Sproc_Get_CountryList_Result> varCountryList = await _talentConnectAdminDBContext.Set<Sproc_Get_CountryList_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_CountryList, paramasString)).ToListAsync();

            if (varCountryList.Any())
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.ListingResponses(varCountryList, Convert.ToInt64(varCountryList[0].TotalRecords), Convert.ToInt64(filter.PageSize), Convert.ToInt64(filter.PageIndex)) });
            else
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Country available" });

        }
        [Authorize]
        [HttpPost("AddCountry")]
        public async Task<ObjectResult> AddCountry([FromBody] CountryViewModel countryViewModel)
        {
            try
            {
                if (countryViewModel == null)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Data in valid" });

                if (!string.IsNullOrEmpty(countryViewModel.CountryName) && !string.IsNullOrEmpty(countryViewModel.CountryRegion))
                {
                    var varCountry = await _talentConnectAdminDBContext.PrgCountryRegions.Where(x => x.Country.ToLower() == countryViewModel.CountryName.ToLower() || x.CountryRegion.ToLower() == countryViewModel.CountryRegion.ToLower()).FirstOrDefaultAsync().ConfigureAwait(false);
                    if (varCountry != null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Country Exists" });
                    }
                    else
                    {
                        PrgCountryRegion _CountryRegion = new PrgCountryRegion();
                        _CountryRegion.Country = countryViewModel.CountryName;
                        _CountryRegion.CountryRegion = countryViewModel.CountryRegion;
                        _CountryRegion.IsActive = countryViewModel.IsActive;
                        _talentConnectAdminDBContext.PrgCountryRegions.Add(_CountryRegion);
                        _talentConnectAdminDBContext.SaveChanges();

                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = countryViewModel });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Country Name & Country Region Required" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpGet("CheckCountryName")]
        public async Task<ObjectResult> CheckCountryName(string CountryName)
        {
            try
            {
                if (!string.IsNullOrEmpty(CountryName))
                {
                    var countryname = await _talentConnectAdminDBContext.PrgCountryRegions.Where(x => x.Country.ToLower() == CountryName.ToLower()).FirstOrDefaultAsync().ConfigureAwait(false);
                    if (countryname != null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Country Name Exists" });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Country Name Not Exists" });
                    }

                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Country Name Required" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpGet("CheckCountryRegion")]
        public async Task<ObjectResult> CheckCountryRegion(string CountryRegion)
        {
            try
            {
                if (!string.IsNullOrEmpty(CountryRegion))
                {
                    var countryregion = await _talentConnectAdminDBContext.PrgCountryRegions.Where(x => x.CountryRegion.ToLower() == CountryRegion.ToLower()).FirstOrDefaultAsync().ConfigureAwait(false);
                    if (countryregion != null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Country Region Exists" });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Country Region Not Exists" });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Country Region Required" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
        #region Currancy
        [Authorize]
        [HttpPost("GetCurrencyExchangeRateList")]
        public async Task<ObjectResult> GetCurrencyExchangeRateList(CommonFilterModel filter)
        {
            try
            {
                object[] param = new object[]
                {
                    filter.PageIndex > 0 ? filter.PageIndex : 1,
                    filter.PageSize > 0 ? filter.PageSize : 50,
                    string.IsNullOrEmpty(filter.SortExpression) ? "ID" : filter.SortExpression,
                    string.IsNullOrEmpty(filter.SortDirection) ? "asc" : filter.SortDirection
                };
                string paramasString = CommonLogic.ConvertToParamString(param);

                filter.PageIndex = filter.PageIndex > 0 ? filter.PageIndex : 1;
                filter.PageSize = filter.PageSize > 0 ? filter.PageSize : 50;
                List<Sproc_CurrencyExchangeRate_Result> varCurrencyExchangeRateList = await _talentConnectAdminDBContext.Set<Sproc_CurrencyExchangeRate_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_CurrencyExchangeRate, paramasString)).ToListAsync();


                if (varCurrencyExchangeRateList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.ListingResponses(varCurrencyExchangeRateList, Convert.ToInt64(varCurrencyExchangeRateList[0].TotalRecords), Convert.ToInt64(filter.PageSize), Convert.ToInt64(filter.PageIndex)) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Currency Exchange Rate available" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("UpdateCurrencyExchangeRate")]
        public async Task<ObjectResult> UpdateCurrencyExchangeRate(int ID, decimal ExchangeRate)
        {
            try
            {
                var varIfExists = await _talentConnectAdminDBContext.PrgCurrencyExchangeRates.Where(x => x.Id == ID).FirstOrDefaultAsync().ConfigureAwait(false);

                if (varIfExists != null)
                {
                    object[] param = new object[]
                    {
                    varIfExists.Id,
                    ExchangeRate
                    };

                    var result = _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_UpdateCurrencyExchangeRate, param);

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = result });
                }
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Currency Exchange Rate available" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [Authorize]
        [HttpPost("GetDurationType")]
        public async Task<ObjectResult> GetDurationType()
        {
            try
            {
                var durationType = new Dictionary<string, string>
                {
                    { "0", "Select" },
                    { "Long Term", "Long Term" },
                    { "Short Term", "Short Term" },
                };

                var durationTypeList = durationType.ToList().Select(x => new SelectListItem
                {
                    Value = x.Key.ToString(),
                    Text = x.Value
                });

                if (durationTypeList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = durationTypeList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetContractDuration")]
        public ObjectResult GetContractDuration()
        {
            try
            {
                var contractDuration = new Dictionary<string, string>
                {
                    { "3", "3 months" },
                    { "6", "6 months" },
                    { "12", "12 months" },
                    { "Indefinite", "Indefinite" }
                };

                var contractDurationList = contractDuration.ToList().Select(x => new SelectListItem
                {
                    Value = x.Key.ToString(),
                    Text = x.Value
                });

                if (contractDurationList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = contractDurationList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetBudgetInformation")]
        public ObjectResult GetBudgetInformation()
        {
            try
            {
                var budgetInformation = new Dictionary<string, string>
                {
                    { "0", "Select" },
                    { "1", "I'm willing to spend not more than" },
                    { "2", "It can be anywhere between" },
                    //{ "3", "No bar for the right candidate" }, //UTS-6304 Confidential budget
                };

                var budgetInformationList = budgetInformation.ToList().Select(x => new SelectListItem
                {
                    Value = x.Key.ToString(),
                    Text = x.Value
                });

                if (budgetInformationList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = budgetInformationList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetStartEndTime")]
        public ObjectResult GetStartEndTime()
        {
            try
            {
                DateTime start = new DateTime(2019, 12, 17, 0, 0, 0);
                DateTime end = new DateTime(2019, 12, 17, 23, 59, 59);
                int i = 0;
                List<SelectListItem> list = new List<SelectListItem>();
                while (start <= end)
                {
                    list.Add(new SelectListItem() { Text = start.ToString("t"), Value = start.ToString("t") });
                    start = start.AddMinutes(30);
                    i += 1;
                }

                if (list.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = list });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetPrgClientStageWithHubSpotStage")]
        public async Task<ObjectResult> GetPrgClientStageWithHubSpotStage()
        {
            try
            {
                var GetUserTypeList = _talentConnectAdminDBContext.PrgClientStageWithHubSpotStages.Select(x => new MastersResponseModel { Value = x.ClientStageName, Id = x.Id }).ToList();

                if (GetUserTypeList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Client Stage With Hub Spot Stage List", Details = GetUserTypeList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetLeadByLeadType")]
        public ObjectResult GetLeadByLeadType(string leadType, long HrID)
        {
            try
            {
                dynamic dObject = new ExpandoObject();

                object[] param = new object[] { leadType, HrID };
                string paramstring = CommonLogic.ConvertToParamString(param);
                var LeadTypeList = _iUsers.Sproc_GetUserBy_LeadType(paramstring);

                dObject.LeadTypeList = LeadTypeList.Select(x => new SelectListItem
                {
                    Value = x.Value.ToString(),
                    Text = x.Text.ToString()
                }).ToList();

                dObject.LeadTypeList.Insert(0, new SelectListItem() { Value = "0", Text = "--Select--" });

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

        [Authorize]
        [HttpGet("GetprgHiringTypePricing")]
        public async Task<ObjectResult> GetprgHiringTypePricing()
        {
            try
            {
                List<PrgHiringTypePricing> GetHiringTypePricingList = _talentConnectAdminDBContext.PrgHiringTypePricings.Where(x => x.IsActive == true).ToList();

                if (GetHiringTypePricingList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Hiring Type Pricing List", Details = GetHiringTypePricingList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetprgPayrollType")]
        public async Task<ObjectResult> GetprgPayrollType()
        {
            try
            {
                var GetprgPayrollTypeList = _talentConnectAdminDBContext.PrgPayrollTypes.Where(x => x.IsActive == true).Select(x => new MastersResponseModel { Value = x.PayrollType, Id = x.Id }).ToList();

                if (GetprgPayrollTypeList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Get prg Payroll Type List", Details = GetprgPayrollTypeList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Role master

        [Authorize]
        [HttpPost("GetTalentRoleMaster")]
        public async Task<IActionResult> GetTalentRoleMaster([FromBody] CommonFilterModel filter, [FromQuery] bool IsExportToExcel = false)
        {
            object[] param = new object[]
                {
                    filter.PageIndex > 0 ? filter.PageIndex : 1,
                    filter.PageSize > 0 ? filter.PageSize : 50,
                    string.IsNullOrEmpty(filter.SortExpression) ? "ID" : filter.SortExpression,
                    string.IsNullOrEmpty(filter.SortDirection) ? "DESC" : filter.SortDirection,
                    filter.SearchText ?? ""
                };

            string paramasString = CommonLogic.ConvertToParamString(param);
            filter.PageIndex = filter.PageIndex > 0 ? filter.PageIndex : 1;
            filter.PageSize = filter.PageSize > 0 ? filter.PageSize : 50;

            List<Sproc_GetTalentRoles_Result> varRoleList = await _talentConnectAdminDBContext.Set<Sproc_GetTalentRoles_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_GetTalentRoles, paramasString)).ToListAsync();

            if (varRoleList.Any())
            {
                //UTS-4414: Add Export functionality for Role master.
                if (IsExportToExcel)
                {
                    return Export_RoleMasterList(varRoleList);
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.ListingResponses(varRoleList, Convert.ToInt64(varRoleList[0].TotalRecords), Convert.ToInt64(filter.PageSize), Convert.ToInt64(filter.PageIndex)) });
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Roles available" });
            }
        }

        [HttpGet("Role/CheckUserRightsForAddOperation")]
        public ObjectResult CheckUserRightsForAddRole()
        {
            try
            {
                #region check login User have special rights or not

                long LoggedInUserId = SessionValues.LoginUserId;
                var SpecialRights_UserIDs = _talentConnectAdminDBContext.GenSystemConfigurations.Where(x => x.Key == "User_AddRole" && x.IsActive == true).FirstOrDefault();
                bool IsSpecialRights = false;

                if (SpecialRights_UserIDs != null && !string.IsNullOrEmpty(SpecialRights_UserIDs.Value))
                {
                    string[] UserIDs = SpecialRights_UserIDs.Value.Split(",", StringSplitOptions.TrimEntries);
                    IsSpecialRights = UserIDs.Contains(LoggedInUserId.ToString());
                }

                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject()
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Details = IsSpecialRights
                });

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("Role/UploadIcon")]
        public async Task<ObjectResult> UploadFile([FromForm] IFormFile file)
        {
            try
            {
                #region Validation
                if (file == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "File Required", Details = null });
                }
                else if (file.Length == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "You are uploading corrupt file", Details = null });
                }

                var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
                string[] allowedFileExtension = { ".jpg", ".jpeg", ".png" };

                if (!allowedFileExtension.Contains(fileExtension))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Your file format is incorrect.", Details = null });
                }

                var fileSize = (file.Length / 1024) / 1024;
                if (fileSize >= 0.5)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "File size must be less than 500 KB", Details = null });
                }
                #endregion


                var pathToSave = CommonLogic.GetFileUploadFolderFor(Constants.MediaConstant.RoleFrontIcon);

                long loggedInUserID = SessionValues.LoginUserId;
                string DocfileName = file.FileName;
                string filePath = Path.Combine(pathToSave, DocfileName);
                string FileExtension = System.IO.Path.GetExtension(filePath);

                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                dynamic objResult = new ExpandoObject();

                objResult.FileName = DocfileName;

                return StatusCode(StatusCodes.Status200OK, new ResponseObject()
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "File Upload Successfully",
                    Details = objResult
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("Role/Add")]
        public async Task<ObjectResult> AddRole(string RoleName, string UploadIconFileName)
        {
            try
            {
                #region Validation
                if (string.IsNullOrEmpty(RoleName))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide Role", Details = null });
                }
                #endregion

                string TalentRole = RoleName.Trim();

                var IsRoleExists = await _talentConnectAdminDBContext.PrgTalentRoles.Where(x => x.TalentRole == TalentRole).FirstOrDefaultAsync().ConfigureAwait(false);
                if (IsRoleExists != null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject()
                    {
                        statusCode = StatusCodes.Status400BadRequest,
                        Message = "Role already exist"
                    });
                }
                else
                {
                    PrgTalentRole roleObj = new PrgTalentRole();
                    roleObj.TalentRole = TalentRole;
                    roleObj.FrontIconImage = string.IsNullOrEmpty(UploadIconFileName) ? "default-role-icon.png" : UploadIconFileName;
                    roleObj.IsActive = true;
                    roleObj.IsAdhoc = false;
                    roleObj.PitchMeRoleId = 0;
                    roleObj.ModifyById = 0;
                    roleObj.CreatedbyDatetime = DateTime.Now;
                    roleObj.ModifyByDatetime = DateTime.Now;
                    _talentConnectAdminDBContext.PrgTalentRoles.Add(roleObj);
                    _talentConnectAdminDBContext.SaveChanges();

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject()
                    {
                        statusCode = StatusCodes.Status200OK,
                        Message = "Role addded Successfully"
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        [Authorize]
        [HttpGet("Role/UpdateRole")]
        public async Task<ObjectResult> UpdateRole(int ID, int UpdatedRoleID)
        {
            try
            {
                long LoggedInUserId = SessionValues.LoginUserId;
                #region Validation
                if (ID <= 0 || UpdatedRoleID <= 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide Role ID", Details = null });
                }
                #endregion

                List<Sproc_UTS_Update_Role_Result> HrList = await _iMasters.UpdateRole(ID, UpdatedRoleID, LoggedInUserId);

                if (HrList.Any())
                {
                    foreach (var item in HrList)
                    {
                        #region ATSCall
                        if (_iConfiguration["HRDataSendSwitchForPHP"].ToLower() != "local")
                        {
                            if (_iConfiguration != null)
                            {

                                long HR_ID = (long)item.HRID;
                                try
                                {
                                    ATSCommonAPI commonAPI = new(_talentConnectAdminDBContext, _iConfiguration, _httpContextAccessor.HttpContext);
                                    commonAPI.SendHRDetailsToPMS(HR_ID);
                                }
                                catch (Exception)
                                {
                                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "HR Role Updated" });
                                }
                            }

                        }
                        #endregion

                    }
                }


                return StatusCode(StatusCodes.Status200OK, new ResponseObject()
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Role Updated Successfully"
                });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("Role/EditRole")]
        public async Task<ObjectResult> EditRole(string RoleName, int ID)
        {
            try
            {
                long LoggedInUserId = SessionValues.LoginUserId;
                #region Validation
                if (string.IsNullOrEmpty(RoleName))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide Role", Details = null });
                }
                #endregion

                string TalentRole = RoleName.Trim();

                var IsRoleExists = await _talentConnectAdminDBContext.PrgTalentRoles.Where(x => x.TalentRole.ToLower() == TalentRole.ToLower() && x.Id != ID).FirstOrDefaultAsync().ConfigureAwait(false);

                if (IsRoleExists != null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject()
                    {
                        statusCode = StatusCodes.Status400BadRequest,
                        Message = "Role already exist"
                    });
                }
                else
                {
                    var roleObj = await _talentConnectAdminDBContext.PrgTalentRoles.AsNoTracking().Where(x => x.Id == ID).FirstOrDefaultAsync().ConfigureAwait(false);
                    if (roleObj != null)
                    {
                        roleObj.TalentRole = TalentRole;
                        roleObj.ModifyById = (int)LoggedInUserId;
                        roleObj.ModifyByDatetime = DateTime.Now;
                        _talentConnectAdminDBContext.Entry(roleObj).State = EntityState.Modified;
                        _talentConnectAdminDBContext.SaveChanges();
                    }

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject()
                    {
                        statusCode = StatusCodes.Status200OK,
                        Message = "Role Updated Successfully"
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("UpdateTalentRoleStatus")]
        public async Task<ObjectResult> UpdateUpdateTalentRoleStatus(int id, bool status)
        {
            try
            {
                var varIfExists = await _talentConnectAdminDBContext.PrgTalentRoles.Where(x => x.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);

                if (varIfExists != null)
                {
                    varIfExists.IsActive = status;
                    _talentConnectAdminDBContext.PrgTalentRoles.Update(varIfExists);
                    _talentConnectAdminDBContext.SaveChanges();

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = varIfExists });
                }
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Talent Role available" });
            }
            catch (Exception)
            {
                throw;
            }
        }



        [Authorize]
        [HttpPost("GetTimeZoneMaster")]
        public async Task<IActionResult> GetTimeZoneMaster([FromBody] CommonFilterModel filter)
        {
            string SearchText = "";
            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                SearchText = filter.SearchText.Replace("'", "''");
            }

            object[] param = new object[]
                {
                    filter.PageIndex > 0 ? filter.PageIndex : 1,
                    filter.PageSize > 0 ? filter.PageSize : 50,
                    string.IsNullOrEmpty(filter.SortExpression) ? "ID" : filter.SortExpression,
                    string.IsNullOrEmpty(filter.SortDirection) ? "DESC" : filter.SortDirection,
                    filter.SearchText ?? ""
                };

            string paramasString = CommonLogic.ConvertToParamString(param);
            filter.PageIndex = filter.PageIndex > 0 ? filter.PageIndex : 1;
            filter.PageSize = filter.PageSize > 0 ? filter.PageSize : 50;

            List<Sproc_GetContactTimeZone_Result> Timezonelist = await _iMasters.GetTimeZone(paramasString).ConfigureAwait(false);

            if (Timezonelist.Any())
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.ListingResponses(Timezonelist, Convert.ToInt64(Timezonelist[0].TotalRecords), Convert.ToInt64(filter.PageSize), Convert.ToInt64(filter.PageIndex)) });
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No TimeZone available" });
            }
        }

        [Authorize]
        [HttpPost("EditTimeZone")]
        public async Task<ObjectResult> EditTimeZone(string TimeZone, int ID)
        {
            try
            {
                long LoggedInUserId = SessionValues.LoginUserId;
                #region Validation
                if (string.IsNullOrEmpty(TimeZone))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide TimeZone", Details = null });
                }
                #endregion

                string Time_zone = TimeZone.Trim();

                var IsTimeZoneExists = await _talentConnectAdminDBContext.PrgContactTimeZones.Where(x => x.TimeZoneTitle.ToLower() == Time_zone.ToLower() && x.Id != ID).FirstOrDefaultAsync().ConfigureAwait(false);

                if (IsTimeZoneExists != null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject()
                    {
                        statusCode = StatusCodes.Status400BadRequest,
                        Message = "TimeZone already exist"
                    });
                }
                else
                {
                    var TimeZoneObj = await _talentConnectAdminDBContext.PrgContactTimeZones.AsNoTracking().Where(x => x.Id == ID).FirstOrDefaultAsync().ConfigureAwait(false);
                    if (TimeZoneObj != null)
                    {
                        TimeZoneObj.TimeZoneTitle = Time_zone;
                        _talentConnectAdminDBContext.Entry(TimeZoneObj).State = EntityState.Modified;
                        _talentConnectAdminDBContext.SaveChanges();
                    }

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject()
                    {
                        statusCode = StatusCodes.Status200OK,
                        Message = "Time Zone Updated Successfully"
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion

        [HttpPost("FetchCountriesBasedonCity")]
        public ObjectResult FetchCountriesBasedonCity([FromBody] GetCitybyCountryViewModel cityName)
        {
            if (cityName == null || string.IsNullOrEmpty(cityName.City))
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request Object is empty" });
            }
            long LogedInUserIdval = SessionValues.LoginUserId;
            string pythonParsingURLForCountry = _iConfiguration["PythonParsingURLForCountry"] + "?Name=" + cityName?.City;

            string content = string.Empty;

            using (WebResponse wr = WebRequest.Create(pythonParsingURLForCountry).GetResponse())
            {
                using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                {
                    content = sr.ReadToEnd();
                    content = content.Trim();
                }
            }

            List<PrgCountryRegion> regions = new List<PrgCountryRegion>();
            string message = "List of countries";
            if (!string.IsNullOrEmpty(content))
            {
                string[] countries = content.Split(',');
                for (int i = 0; i < countries.Length; i++)
                {
                    PrgCountryRegion? region = _iMasters.GetCountryRegions().Where(x => x.Country?.ToLower() == countries[i].ToLower()).FirstOrDefault();
                    if (region != null)
                    {
                        regions.Add(region);
                    }
                }
            }

            if (!regions.Any())
            {
                message = "city not registered with us please select the country manually";
                regions = _iMasters.GetCountryRegions();
            }

            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = message, Details = regions });
        }

        [Authorize]
        [HttpGet("GetAMUser")]
        public async Task<ObjectResult> GetAMUser()
        {
            List<Sproc_Get_AM_User_Result> UserList = await _iMasters.GetAMUser();

            if (UserList.Any())
            {
                var AMUsers = UserList.Select(x => new MastersResponseModel { Value = x.FullName, Id = x.ID }).ToList();
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = AMUsers });
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Data not found" });
            }
        }


        [Authorize]
        [HttpGet("GetStateList")]
        public async Task<ObjectResult> GetStateList()
        {
            try
            {
                var GetStateList = _talentConnectAdminDBContext.PrgStates.Select(x => new MastersResponseModel { Value = x.State, Id = x.Id }).ToList();

                if (GetStateList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Get State List", Details = GetStateList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetJobTypes")]
        public ObjectResult GetJobTypes()
        {
            try
            {
                var jobTypes = _talentConnectAdminDBContext.PrgJobTypes.Where(x => x.IsActive == true).Select(x => new MastersResponseModel { Value = x.JobType, text = x.JobType, Id = x.Id }).ToList();

                if (jobTypes.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Job Type List", Details = jobTypes });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion

        #region Private method
        private static List<MastersResponseModel> MakeMasterList(Dictionary<string, object> dict)
        {
            List<MastersResponseModel> mastersResponses = new List<MastersResponseModel>();
            int cnt = 0;
            foreach (var obj in dict)
            {
                cnt = cnt + 1;
                MastersResponseModel mastersResponse = new MastersResponseModel();
                mastersResponse.Id = cnt;
                mastersResponse.text = Convert.ToString(obj.Key);
                mastersResponse.Value = Convert.ToString(obj.Value);
                mastersResponses.Add(mastersResponse);
            }
            return mastersResponses;
        }

        /// <summary>
        /// UTS-4414: Add Export functionality for Role master.
        /// </summary>
        /// <param name="roleList"></param>
        /// <returns></returns>
        private IActionResult Export_RoleMasterList(List<Sproc_GetTalentRoles_Result> roleList)
        {
            var ExportFileName = "RoleMasterExport_" + DateTime.Now.ToString("yyyyMMddHHmm") + @".xlsx";
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("RoleMasterList");

            var currentRow = 1;

            worksheet.Cell(currentRow, 1).Value = "Pitch Me Role Id";
            worksheet.Cell(currentRow, 2).Value = "Talent Role";
            worksheet.Cell(currentRow, 3).Value = "Active";

            foreach (var role in roleList)
            {
                currentRow++;
                var currentColumn = 1;

                worksheet.Cell(currentRow, currentColumn++).Value = role.PitchMeRoleID;
                worksheet.Cell(currentRow, currentColumn++).Value = role.TalentRole;
                worksheet.Cell(currentRow, currentColumn++).Value = role.IsActive ? "Yes" : "No";

            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ExportFileName);
        }
        #endregion
    }
}
