using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Controllers
{
    [Route("Matchmaking/")]
    [ApiController]
    public class MatchMakingController : ControllerBase
    {
        #region Variables
        private readonly TalentConnectAdminDBContext _talentConnectAdminDBContext;
        private readonly IUniversalProcRunner _universalProcRunner;
        private readonly ICommonInterface _commonInterface;
        #endregion

        #region Constructor
        public MatchMakingController(TalentConnectAdminDBContext talentConnectAdminDBContext, IUniversalProcRunner universalProcRunner, ICommonInterface commonInterface)
        {
            _talentConnectAdminDBContext = talentConnectAdminDBContext;
            _universalProcRunner = universalProcRunner;
            _commonInterface = commonInterface;
        }

        #endregion

        #region Public API
        [Authorize]
        [HttpPost("GetTalentProfileLog")]
        public ObjectResult GetTalentProfileLog(GetTalentProfileLogViewModel getTalentProfileLogViewModel)
        {
            try
            {
                if (!string.IsNullOrEmpty(getTalentProfileLogViewModel.talentid.ToString()) && getTalentProfileLogViewModel.talentid > 0)
                {

                    if (!string.IsNullOrEmpty(getTalentProfileLogViewModel.fromDate) && !string.IsNullOrEmpty(getTalentProfileLogViewModel.toDate))
                    {
                        getTalentProfileLogViewModel.fromDate = CommonLogic.ConvertString2DateTime(getTalentProfileLogViewModel.fromDate).ToString("yyyy-MM-dd");
                        getTalentProfileLogViewModel.toDate = CommonLogic.ConvertString2DateTime(getTalentProfileLogViewModel.toDate).ToString("yyyy-MM-dd");
                    }

                    object[] param = new object[]
                    {
                        getTalentProfileLogViewModel.talentid, getTalentProfileLogViewModel.fromDate, getTalentProfileLogViewModel.toDate
                    };

                    var profileLog = _commonInterface.hiringRequest.sproc_UTS_GetTalentProfileLog_Result(CommonLogic.ConvertToParamString(param));
                    if (profileLog != null)
                    {
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Profile log found", Details = profileLog });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Profile log not found" });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide talent" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetTalentCostConversion")]
        public ObjectResult GetTalentCostConversion(decimal amount)
        {
            try
            {
                if (!string.IsNullOrEmpty(amount.ToString()) && amount > 0)
                {
                    var listOfCurrency = _talentConnectAdminDBContext.PrgCurrencyExchangeRates.ToList();

                    List<CurrencyData> currencyDatas = new List<CurrencyData>();
                    CurrencyData currencyData1 = new CurrencyData();

                    var inr = listOfCurrency.FirstOrDefault(x => x.CurrencyCode.Equals("INR"));
                    currencyData1.id = "talentCost1";
                    currencyData1.cost = String.Format("{0:N2}", (amount * inr.ExchangeRate));
                    currencyData1.currency = "INR";
                    currencyData1.currencyIcon = inr.CurrencySign;
                    currencyDatas.Add(currencyData1);

                    var aud = listOfCurrency.FirstOrDefault(x => x.CurrencyCode.Equals("AUD"));

                    CurrencyData currencyData2 = new CurrencyData();
                    currencyData2.id = "talentCost2";
                    currencyData2.cost = String.Format("{0:N2}", (amount * aud.ExchangeRate));
                    currencyData2.currency = "AUD";
                    currencyData2.currencyIcon = aud.CurrencySign;
                    currencyDatas.Add(currencyData2);

                    var eur = listOfCurrency.FirstOrDefault(x => x.CurrencyCode.Equals("EUR"));

                    CurrencyData currencyData3 = new CurrencyData();
                    currencyData3.id = "talentCost3";
                    currencyData3.cost = String.Format("{0:N2}", (amount * eur.ExchangeRate));
                    currencyData3.currency = "EUR";
                    currencyData3.currencyIcon = eur.CurrencySign;
                    currencyDatas.Add(currencyData3);

                    var gbp = listOfCurrency.FirstOrDefault(x => x.CurrencyCode.Equals("GBP"));

                    CurrencyData currencyData4 = new CurrencyData();
                    currencyData4.id = "talentCost4";
                    currencyData4.cost = String.Format("{0:N2}", (amount * gbp.ExchangeRate));
                    currencyData4.currency = "GBP";
                    currencyData4.currencyIcon = gbp.CurrencySign;
                    currencyDatas.Add(currencyData4);

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Conversion", Details = currencyDatas });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide proper amount" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetTalentTechScoreCard")]
        public ObjectResult GetTalentTechScoreCard(long talentid)
        {
            try
            {
                if (!string.IsNullOrEmpty(talentid.ToString()) && talentid > 0)
                {
                    GenTalent genTalent = _talentConnectAdminDBContext.GenTalents.FirstOrDefault(x => x.Id == talentid);
                    var listofScoredata = _commonInterface.hiringRequest.sproc_UTS_GetTalentScoreCard_Result(string.Format("{0}", talentid.ToString()));
                    if (listofScoredata != null && listofScoredata.Count > 0)
                    {
                        string percentage = String.Empty;
                        decimal totalgained = 0;
                        int totalskills = 0;
                        foreach (var score in listofScoredata)
                        {
                            totalskills += 1;
                            totalgained += score.Score;
                        }
                        if (totalskills > 0)
                            percentage = (totalgained / totalskills).ToString() + "%";
                        dynamic objResult = new ExpandoObject();
                        objResult.total = (totalskills * 100);
                        objResult.name = genTalent.Name;
                        objResult.totalGained = totalgained;
                        objResult.percentage = String.Format("{0:0.##}", percentage);
                        objResult.rows = listofScoredata;

                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "TalentTechScoreCard found", Details = objResult });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "TalentTechScoreCard not found" });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide proper request data" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetTalentProfileSharedDetail")]
        public ObjectResult GetTalentProfileSharedDetail(long talentid, int typeid, string fromDate, string toDate)
        {
            try
            {
                if (!string.IsNullOrEmpty(talentid.ToString()) && talentid > 0 && !string.IsNullOrEmpty(typeid.ToString()) && typeid > 0)
                {
                    if (string.IsNullOrEmpty(fromDate))
                        fromDate = String.Empty;
                    if (string.IsNullOrEmpty(toDate))
                        toDate = String.Empty;

                    List<sproc_UTS_GetProfileShareDetail_Result> profileShared;
                    if (string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
                    {
                        profileShared = _commonInterface.hiringRequest.sproc_UTS_GetProfileShareDetail_Result(string.Format("{0},{1}", talentid.ToString(), typeid.ToString()));
                    }
                    else
                    {
                        fromDate = CommonLogic.ConvertString2DateTime(fromDate).ToString("yyyy-MM-dd");
                        toDate = CommonLogic.ConvertString2DateTime(toDate).ToString("yyyy-MM-dd");
                        profileShared = _commonInterface.hiringRequest.sproc_UTS_GetProfileShareDetail_Result(string.Format("{0},{1},'{2}','{3}'", talentid.ToString(), typeid.ToString(), fromDate.ToString(), toDate.ToString()));
                    }

                    if (profileShared != null && profileShared.Count > 0)
                    {
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Profile Shared found", Details = profileShared });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Profile Shared not found" });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide proper request data" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
