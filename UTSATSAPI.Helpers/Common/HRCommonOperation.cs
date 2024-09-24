using Microsoft.EntityFrameworkCore;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;

namespace UTSATSAPI.Helpers.Common
{
    public class HRCommonOperation
    {
        TalentConnectAdminDBContext _DBContext;
        //public static string PerMonth = " / Month";
        public static string Transparent_HRType = "( Transparent )";
        public static string NonTransparent_HRType = "( Non Transparent )";

        #region Constructor
        public HRCommonOperation(TalentConnectAdminDBContext talentConnectAdminDBContext)
        {
            _DBContext = talentConnectAdminDBContext;
        }
        #endregion
        //public PayPerHireModel GetTransparentModelData(GenSalesHiringRequest SalesHiringRequest, GenSalesHiringRequestDetail SalesHiringRequestDetail, PrgHiringTypePricing getprgHiringTypePricing, PrgPayrollType GetprgPayrollType, string CompanyName)
        //{
        //    string Availability = SalesHiringRequest.Availability ?? "Full Time";

        //    PayPerHireModel transparentModel = new PayPerHireModel();

        //    transparentModel.JobType = PayPerHire_JobType(SalesHiringRequest.HiringTypePricingId ?? 0, SalesHiringRequestDetail.SpecificMonth ?? 0, SalesHiringRequest.PayrollTypeId ?? 0, Availability);
        //    transparentModel.IsTransparentPricing = SalesHiringRequest.IsTransparentPricing;
        //    transparentModel.HrTypeId = SalesHiringRequest.HrTypeId1;
        //    transparentModel.PayrollTypeId = SalesHiringRequest.PayrollTypeId;
        //    transparentModel.HrTypePricingId = SalesHiringRequest.HiringTypePricingId;
        //    transparentModel.HrTypePricing = string.Empty;
        //    transparentModel.PayrollType = string.Empty;

        //    bool IsBackendHR = string.IsNullOrEmpty(SalesHiringRequest.Guid) ? true : false;

        //    if (transparentModel.HrTypePricingId > 0)
        //    {
        //        if (getprgHiringTypePricing != null)
        //        {
        //            string Hr_Type;
        //            if ((bool)SalesHiringRequest.IsTransparentPricing)
        //                Hr_Type = Transparent_HRType;
        //            else
        //                Hr_Type = NonTransparent_HRType;

        //            transparentModel.HrTypePricing = getprgHiringTypePricing.Type;
        //            transparentModel.EngagementType = getprgHiringTypePricing.Type + " " + Hr_Type;

        //            if (SalesHiringRequest.IsHrtypeDp)
        //            {
        //                transparentModel.PricingPercent = SalesHiringRequest.Dppercentage;
        //            }
        //            else
        //            {
        //                transparentModel.PricingPercent = SalesHiringRequest.TalentCostCalcPercentage;
        //            }

        //            //transparentModel.PricingPercent = getprgHiringTypePricing.PricingPercent;
        //            if (SalesHiringRequestDetail != null)
        //                transparentModel.CalculatedUplersfees = SalesHiringRequestDetail.CalculatedUplersfees;
        //        }
        //    }

        //    if (SalesHiringRequest.PayrollTypeId > 0)
        //    {
        //        if (GetprgPayrollType != null)
        //        {
        //            transparentModel.PayrollType = GetprgPayrollType.PayrollType;

        //            if (SalesHiringRequest.PayrollTypeId == 3)
        //            {
        //                transparentModel.PayrollPartnerName = SalesHiringRequest.PayrollPartnerName;
        //            }
        //        }
        //    }

        //    //string currencySign = "";
        //    //if (_CurrencyExchangeRate != null)
        //    //{
        //    //    currencySign = _CurrencyExchangeRate.CurrencySign;
        //    //}

        //    transparentModel.BudgetText = SalesHiringRequestDetail.Cost;

        //    #region old logic
        //    //if (IsBackendHR)
        //    //{
        //    //    transparentModel.BudgetTitle = "Est. Client's Billing excl. Addons";

        //    //    if (SalesHiringRequest.IsHrtypeDp)
        //    //    {
        //    //        if (SalesHiringRequestDetail != null && !string.IsNullOrEmpty(SalesHiringRequestDetail.CalculatedUplersfees))
        //    //        {
        //    //            if (SalesHiringRequestDetail.AdhocBudgetCost > 0 && Convert.ToDecimal(SalesHiringRequestDetail.CalculatedUplersfees) > 0)
        //    //            {
        //    //                decimal? PerMonthCalculatedUplersFees = (Convert.ToDecimal(SalesHiringRequestDetail.CalculatedUplersfees) / 12);
        //    //                decimal? calculated = (SalesHiringRequestDetail.AdhocBudgetCost - PerMonthCalculatedUplersFees);

        //    //                transparentModel.ClientNeedsToPay = currencySign + SalesHiringRequestDetail.AdhocBudgetCost + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //                transparentModel.TalentsPay = currencySign + calculated + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //                transparentModel.CalculatedUplersfees = currencySign + SalesHiringRequestDetail.CalculatedUplersfees + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //            }
        //    //            else
        //    //            {
        //    //                if (!string.IsNullOrEmpty(SalesHiringRequestDetail.CalculatedUplersfees) && SalesHiringRequestDetail.BudgetFrom > 0 && SalesHiringRequestDetail.BudgetTo > 0)
        //    //                {
        //    //                    string[] calculatedUplersfees = SalesHiringRequestDetail.CalculatedUplersfees.Split('-');

        //    //                    decimal calculatedMinUplersfee = Convert.ToDecimal(calculatedUplersfees[0].Trim());
        //    //                    decimal calculatedMaxUplersfee = Convert.ToDecimal(calculatedUplersfees[1].Trim());

        //    //                    decimal calculatedMinUplersfeePerMonth = calculatedMinUplersfee / 12;
        //    //                    decimal calculatedMaxUplersfeePerMonth = calculatedMaxUplersfee / 12;

        //    //                    decimal? TalentPay_Min = (SalesHiringRequestDetail.BudgetFrom - calculatedMinUplersfeePerMonth);
        //    //                    decimal? TalentPay_Max = (SalesHiringRequestDetail.BudgetTo - calculatedMaxUplersfeePerMonth);

        //    //                    transparentModel.ClientNeedsToPay = currencySign + SalesHiringRequestDetail.BudgetFrom + " " + SalesHiringRequestDetail.Currency + " - " + currencySign + SalesHiringRequestDetail.BudgetTo + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //                    transparentModel.TalentsPay = currencySign + TalentPay_Min + " " + SalesHiringRequestDetail.Currency + " - " + currencySign + TalentPay_Max + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //                    transparentModel.CalculatedUplersfees = currencySign + calculatedMinUplersfee + " " + SalesHiringRequestDetail.Currency + " - " + currencySign + calculatedMaxUplersfee + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //                }
        //    //            }
        //    //            transparentModel.BudgetText = transparentModel.ClientNeedsToPay;
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        if (SalesHiringRequestDetail != null && !string.IsNullOrEmpty(SalesHiringRequestDetail.CalculatedUplersfees))
        //    //        {
        //    //            if (SalesHiringRequestDetail.AdhocBudgetCost > 0 && Convert.ToDecimal(SalesHiringRequestDetail.CalculatedUplersfees) > 0)
        //    //            {
        //    //                decimal? calculated = (SalesHiringRequestDetail.AdhocBudgetCost - Convert.ToDecimal(SalesHiringRequestDetail.CalculatedUplersfees));

        //    //                transparentModel.ClientNeedsToPay = currencySign + SalesHiringRequestDetail.AdhocBudgetCost + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //                transparentModel.TalentsPay = currencySign + calculated + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //                transparentModel.CalculatedUplersfees = currencySign + SalesHiringRequestDetail.CalculatedUplersfees + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //            }
        //    //            else
        //    //            {
        //    //                if (!string.IsNullOrEmpty(SalesHiringRequestDetail.CalculatedUplersfees) && SalesHiringRequestDetail.BudgetFrom > 0 && SalesHiringRequestDetail.BudgetTo > 0)
        //    //                {
        //    //                    string[] calculatedUplersfees = SalesHiringRequestDetail.CalculatedUplersfees.Split('-');

        //    //                    decimal calculatedMinUplersfee = Convert.ToDecimal(calculatedUplersfees[0].Trim());
        //    //                    decimal calculatedMaxUplersfee = Convert.ToDecimal(calculatedUplersfees[1].Trim());

        //    //                    decimal? TalentPay_Min = (SalesHiringRequestDetail.BudgetFrom - calculatedMinUplersfee);
        //    //                    decimal? TalentPay_Max = (SalesHiringRequestDetail.BudgetTo - calculatedMaxUplersfee);

        //    //                    transparentModel.ClientNeedsToPay = currencySign + SalesHiringRequestDetail.BudgetFrom + " " + SalesHiringRequestDetail.Currency + " - " + currencySign + SalesHiringRequestDetail.BudgetTo + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //                    transparentModel.TalentsPay = currencySign + TalentPay_Min + " " + SalesHiringRequestDetail.Currency + " - " + currencySign + TalentPay_Max + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //                    transparentModel.CalculatedUplersfees = currencySign + calculatedMinUplersfee + " " + SalesHiringRequestDetail.Currency + " - " + currencySign + calculatedMaxUplersfee + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //                }
        //    //            }
        //    //            transparentModel.BudgetText = transparentModel.ClientNeedsToPay;
        //    //        }
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    if (transparentModel.HrTypePricingId == 1 ||
        //    //        transparentModel.HrTypePricingId == 2 ||
        //    //        transparentModel.HrTypePricingId == 7) // Transparent (25%, 35%) contratual
        //    //    {
        //    //        transparentModel.BudgetTitle = "Est. Talent's Pay";

        //    //        if (SalesHiringRequestDetail != null && !string.IsNullOrEmpty(SalesHiringRequestDetail.CalculatedUplersfees))
        //    //        {
        //    //            if (SalesHiringRequestDetail.AdhocBudgetCost > 0 && Convert.ToDecimal(SalesHiringRequestDetail.CalculatedUplersfees) > 0)
        //    //            {
        //    //                decimal? calculated = SalesHiringRequestDetail.AdhocBudgetCost + Convert.ToDecimal(SalesHiringRequestDetail.CalculatedUplersfees);
        //    //                transparentModel.TalentsPay = currencySign + SalesHiringRequestDetail.AdhocBudgetCost + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //                transparentModel.ClientNeedsToPay = currencySign + calculated + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //                transparentModel.CalculatedUplersfees = currencySign + SalesHiringRequestDetail.CalculatedUplersfees + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //            }
        //    //            else
        //    //            {
        //    //                if (!string.IsNullOrEmpty(SalesHiringRequestDetail.CalculatedUplersfees) && SalesHiringRequestDetail.BudgetFrom > 0 && SalesHiringRequestDetail.BudgetTo > 0)
        //    //                {
        //    //                    string[] calculatedUplersfees = SalesHiringRequestDetail.CalculatedUplersfees.Split('-');

        //    //                    decimal calculatedMinUplersfee = Convert.ToDecimal(calculatedUplersfees[0].Trim());
        //    //                    decimal calculatedMaxUplersfee = Convert.ToDecimal(calculatedUplersfees[1].Trim());

        //    //                    decimal? ClientNeedsToPay_Min = (SalesHiringRequestDetail.BudgetFrom + calculatedMinUplersfee);
        //    //                    decimal? ClientNeedsToPay_Max = (SalesHiringRequestDetail.BudgetTo + calculatedMaxUplersfee);

        //    //                    transparentModel.TalentsPay = currencySign + SalesHiringRequestDetail.BudgetFrom + " " + SalesHiringRequestDetail.Currency + " - " + currencySign + SalesHiringRequestDetail.BudgetTo + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //                    transparentModel.ClientNeedsToPay = currencySign + ClientNeedsToPay_Min + " " + SalesHiringRequestDetail.Currency + " - " + currencySign + ClientNeedsToPay_Max + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //                    transparentModel.CalculatedUplersfees = currencySign + calculatedMinUplersfee + " " + SalesHiringRequestDetail.Currency + " - " + currencySign + calculatedMaxUplersfee + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //                }
        //    //            }
        //    //            transparentModel.BudgetText = transparentModel.TalentsPay;
        //    //        }
        //    //    }
        //    //    else if (transparentModel.HrTypePricingId == 3)// dp 10% Transparent
        //    //    {
        //    //        transparentModel.BudgetTitle = "Est. Talent's Pay";
        //    //        if (SalesHiringRequestDetail != null && !string.IsNullOrEmpty(SalesHiringRequestDetail.CalculatedUplersfees))
        //    //        {
        //    //            if (SalesHiringRequestDetail.AdhocBudgetCost > 0 && Convert.ToDecimal(SalesHiringRequestDetail.CalculatedUplersfees) > 0)
        //    //            {
        //    //                transparentModel.TalentsPay = currencySign + SalesHiringRequestDetail.AdhocBudgetCost + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //                transparentModel.CalculatedUplersfees = currencySign + SalesHiringRequestDetail.CalculatedUplersfees + " " + SalesHiringRequestDetail.Currency; // Annully amount
        //    //            }
        //    //            else
        //    //            {
        //    //                if (!string.IsNullOrEmpty(SalesHiringRequestDetail.CalculatedUplersfees) && SalesHiringRequestDetail.BudgetFrom > 0 && SalesHiringRequestDetail.BudgetTo > 0)
        //    //                {
        //    //                    string[] calculatedUplersfees = SalesHiringRequestDetail.CalculatedUplersfees.Split('-');

        //    //                    decimal calculatedMinUplersfee = Convert.ToDecimal(calculatedUplersfees[0].Trim());
        //    //                    decimal calculatedMaxUplersfee = Convert.ToDecimal(calculatedUplersfees[1].Trim());

        //    //                    decimal? TalentPay_Min = Convert.ToDecimal(SalesHiringRequestDetail.BudgetFrom);
        //    //                    decimal? TalentPay_Max = Convert.ToDecimal(SalesHiringRequestDetail.BudgetTo);

        //    //                    transparentModel.TalentsPay = currencySign + TalentPay_Min + " " + SalesHiringRequestDetail.Currency + " - " + currencySign + TalentPay_Max + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //                    transparentModel.CalculatedUplersfees = currencySign + calculatedMinUplersfee + " " + SalesHiringRequestDetail.Currency + " - " + currencySign + calculatedMaxUplersfee + " " + SalesHiringRequestDetail.Currency;// Annully amount
        //    //                }
        //    //            }
        //    //            transparentModel.BudgetText = transparentModel.TalentsPay;
        //    //        }
        //    //        transparentModel.ClientNeedsToPay = "";
        //    //    }
        //    //    else
        //    //    {
        //    //        transparentModel.BudgetTitle = "Est. Client's Billing excl. Addons";

        //    //        if (SalesHiringRequestDetail != null && !string.IsNullOrEmpty(SalesHiringRequestDetail.CalculatedUplersfees))
        //    //        {
        //    //            if (SalesHiringRequestDetail.AdhocBudgetCost > 0 && Convert.ToDecimal(SalesHiringRequestDetail.CalculatedUplersfees) > 0)
        //    //            {
        //    //                decimal? calculated = SalesHiringRequestDetail.AdhocBudgetCost - Convert.ToDecimal(SalesHiringRequestDetail.CalculatedUplersfees);

        //    //                transparentModel.ClientNeedsToPay = currencySign + SalesHiringRequestDetail.AdhocBudgetCost + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //                transparentModel.TalentsPay = currencySign + calculated + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //                transparentModel.CalculatedUplersfees = currencySign + SalesHiringRequestDetail.CalculatedUplersfees + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //            }
        //    //            else
        //    //            {
        //    //                if (!string.IsNullOrEmpty(SalesHiringRequestDetail.CalculatedUplersfees) && SalesHiringRequestDetail.BudgetFrom > 0 && SalesHiringRequestDetail.BudgetTo > 0)
        //    //                {
        //    //                    string[] calculatedUplersfees = SalesHiringRequestDetail.CalculatedUplersfees.Split('-');

        //    //                    decimal calculatedMinUplersfee = Convert.ToDecimal(calculatedUplersfees[0].Trim());
        //    //                    decimal calculatedMaxUplersfee = Convert.ToDecimal(calculatedUplersfees[1].Trim());

        //    //                    decimal? TalentPay_Min = (SalesHiringRequestDetail.BudgetFrom - calculatedMinUplersfee);
        //    //                    decimal? TalentPay_Max = (SalesHiringRequestDetail.BudgetTo - calculatedMaxUplersfee);

        //    //                    transparentModel.ClientNeedsToPay = currencySign + SalesHiringRequestDetail.BudgetFrom + " " + SalesHiringRequestDetail.Currency + " - " + currencySign + SalesHiringRequestDetail.BudgetTo + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //                    transparentModel.TalentsPay = currencySign + TalentPay_Min + " " + SalesHiringRequestDetail.Currency + " - " + currencySign + TalentPay_Max + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //                    transparentModel.CalculatedUplersfees = currencySign + calculatedMinUplersfee + " " + SalesHiringRequestDetail.Currency + " - " + currencySign + calculatedMaxUplersfee + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //                }
        //    //            }
        //    //            transparentModel.BudgetText = transparentModel.ClientNeedsToPay;
        //    //        }
        //    //    }
        //    //}
        //    #endregion

        //    #region NEw logic -- but add comment beacuse need to pass data from SP side
        //    //if (SalesHiringRequest.IsHrtypeDp && transparentModel.HrTypePricingId == 3)
        //    //{
        //    //    transparentModel.BudgetTitle = "Est. Talent's Pay";
        //    //    if (SalesHiringRequestDetail != null && !string.IsNullOrEmpty(SalesHiringRequestDetail.CalculatedUplersfees))
        //    //    {
        //    //        if (SalesHiringRequestDetail.AdhocBudgetCost > 0 && Convert.ToDecimal(SalesHiringRequestDetail.CalculatedUplersfees) > 0)
        //    //        {
        //    //            transparentModel.TalentsPay = currencySign + SalesHiringRequestDetail.AdhocBudgetCost + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //            transparentModel.CalculatedUplersfees = currencySign + SalesHiringRequestDetail.CalculatedUplersfees + " " + SalesHiringRequestDetail.Currency; // Annully amount
        //    //        }
        //    //        else
        //    //        {
        //    //            if (!string.IsNullOrEmpty(SalesHiringRequestDetail.CalculatedUplersfees) && SalesHiringRequestDetail.BudgetFrom > 0 && SalesHiringRequestDetail.BudgetTo > 0)
        //    //            {
        //    //                string[] calculatedUplersfees = SalesHiringRequestDetail.CalculatedUplersfees.Split('-');

        //    //                decimal calculatedMinUplersfee = Convert.ToDecimal(calculatedUplersfees[0].Trim());
        //    //                decimal calculatedMaxUplersfee = Convert.ToDecimal(calculatedUplersfees[1].Trim());

        //    //                decimal? TalentPay_Min = Convert.ToDecimal(SalesHiringRequestDetail.BudgetFrom);
        //    //                decimal? TalentPay_Max = Convert.ToDecimal(SalesHiringRequestDetail.BudgetTo);

        //    //                transparentModel.TalentsPay = currencySign + TalentPay_Min + " " + SalesHiringRequestDetail.Currency + " - " + currencySign + TalentPay_Max + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //                transparentModel.CalculatedUplersfees = currencySign + calculatedMinUplersfee + " " + SalesHiringRequestDetail.Currency + " - " + currencySign + calculatedMaxUplersfee + " " + SalesHiringRequestDetail.Currency;// Annully amount
        //    //            }
        //    //        }
        //    //        transparentModel.BudgetText = transparentModel.TalentsPay;
        //    //    }
        //    //    transparentModel.ClientNeedsToPay = "";
        //    //}
        //    //else
        //    //{
        //    //    if (SalesHiringRequestDetail != null && !string.IsNullOrEmpty(SalesHiringRequestDetail.CalculatedUplersfees))
        //    //    {
        //    //        if (SalesHiringRequestDetail.AdhocBudgetCost > 0 && Convert.ToDecimal(SalesHiringRequestDetail.CalculatedUplersfees) > 0)
        //    //        {
        //    //            decimal? calculated = (SalesHiringRequestDetail.AdhocBudgetCost - Convert.ToDecimal(SalesHiringRequestDetail.CalculatedUplersfees));

        //    //            transparentModel.ClientNeedsToPay = currencySign + SalesHiringRequestDetail.AdhocBudgetCost + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //            transparentModel.TalentsPay = currencySign + calculated + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //            transparentModel.CalculatedUplersfees = currencySign + SalesHiringRequestDetail.CalculatedUplersfees + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //        }
        //    //        else
        //    //        {
        //    //            if (!string.IsNullOrEmpty(SalesHiringRequestDetail.CalculatedUplersfees) && SalesHiringRequestDetail.BudgetFrom > 0 && SalesHiringRequestDetail.BudgetTo > 0)
        //    //            {
        //    //                string[] calculatedUplersfees = SalesHiringRequestDetail.CalculatedUplersfees.Split('-');

        //    //                decimal calculatedMinUplersfee = Convert.ToDecimal(calculatedUplersfees[0].Trim());
        //    //                decimal calculatedMaxUplersfee = Convert.ToDecimal(calculatedUplersfees[1].Trim());

        //    //                decimal? TalentPay_Min = (SalesHiringRequestDetail.BudgetFrom - calculatedMinUplersfee);
        //    //                decimal? TalentPay_Max = (SalesHiringRequestDetail.BudgetTo - calculatedMaxUplersfee);

        //    //                transparentModel.ClientNeedsToPay = currencySign + SalesHiringRequestDetail.BudgetFrom + " " + SalesHiringRequestDetail.Currency + " - " + currencySign + SalesHiringRequestDetail.BudgetTo + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //                transparentModel.TalentsPay = currencySign + TalentPay_Min + " " + SalesHiringRequestDetail.Currency + " - " + currencySign + TalentPay_Max + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //                transparentModel.CalculatedUplersfees = currencySign + calculatedMinUplersfee + " " + SalesHiringRequestDetail.Currency + " - " + currencySign + calculatedMaxUplersfee + " " + SalesHiringRequestDetail.Currency + PerMonth;
        //    //            }
        //    //        }
        //    //        transparentModel.BudgetText = transparentModel.ClientNeedsToPay;
        //    //    }
        //    //}

        //    //Format changes for currency // UTS-5686
        //    //transparentModel.BudgetText = !string.IsNullOrEmpty(transparentModel.BudgetText) ? CommonFunction.GetAmountTextInCommaSeprated(transparentModel.BudgetText, SalesHiringRequestDetail.Currency, true) : string.Empty;
        //    //transparentModel.TalentsPay = !string.IsNullOrEmpty(transparentModel.TalentsPay) ? CommonFunction.GetAmountTextInCommaSeprated(transparentModel.TalentsPay, SalesHiringRequestDetail.Currency, true) : string.Empty;
        //    //transparentModel.ClientNeedsToPay = !string.IsNullOrEmpty(transparentModel.ClientNeedsToPay) ? CommonFunction.GetAmountTextInCommaSeprated(transparentModel.ClientNeedsToPay, SalesHiringRequestDetail.Currency, true) : string.Empty;
        //    //if (transparentModel.HrTypePricingId == 3)
        //    //    transparentModel.CalculatedUplersfees = !string.IsNullOrEmpty(transparentModel.CalculatedUplersfees) ? CommonFunction.GetAmountTextInCommaSeprated(transparentModel.CalculatedUplersfees, SalesHiringRequestDetail.Currency, false) : string.Empty;
        //    //else
        //    //    transparentModel.CalculatedUplersfees = !string.IsNullOrEmpty(transparentModel.CalculatedUplersfees) ? CommonFunction.GetAmountTextInCommaSeprated(transparentModel.CalculatedUplersfees, SalesHiringRequestDetail.Currency, true) : string.Empty;
        //    #endregion

        //    object[] Param = new object[]
        //    {
        //          SalesHiringRequest.Id,
        //          string.Empty
        //    };
        //    string paramString = CommonLogic.ConvertToParamString(Param);

        //    SP_PayPerHire_CalculationDetails_Result obj = GetHRWiseCalculation(paramString);
        //    if (obj != null)
        //    {
        //        transparentModel.TalentsPay = obj.TalentPay;
        //        transparentModel.ClientNeedsToPay = obj.ClientPay;
        //        transparentModel.CalculatedUplersfees = obj.UplersFeeInAmount;

        //        if (SalesHiringRequest.IsHrtypeDp && transparentModel.HrTypePricingId == 3)
        //        {
        //            transparentModel.BudgetTitle = "Est. Talent's Pay";
        //            transparentModel.BudgetText = transparentModel.TalentsPay;
        //        }
        //        else
        //        {
        //            transparentModel.BudgetTitle = "Est. Client's Billing excl. Addons";
        //            transparentModel.BudgetText = transparentModel.ClientNeedsToPay;
        //        }
        //    }

        //    if (!(bool)transparentModel.IsTransparentPricing)
        //    {
        //        transparentModel.EngagementText = "";
        //    }
        //    else
        //    {
        //        transparentModel.EngagementText = EngagementText(SalesHiringRequest.HiringTypePricingId, SalesHiringRequest.PayrollTypeId, SalesHiringRequest.PayrollPartnerName, CompanyName);
        //    }

        //    transparentModel.PayPerHire_I_Info = null;

        //    if (!string.IsNullOrEmpty(SalesHiringRequestDetail.CalculatedUplersfees))
        //    {
        //        transparentModel.PayPerHire_I_Info = PayPerHire_I_Information(transparentModel, IsBackendHR);
        //    }

        //    return transparentModel;
        //}

        //public List<string> PayPerHire_I_Information(PayPerHireModel transparentModel, bool isBackendHR)
        //{
        //    List<string> payPerHire_Info = new();
        //    payPerHire_Info.Add(string.Format("Uplers Fees % : {0}", transparentModel.PricingPercent));
        //    if (transparentModel.HrTypePricingId == 3)//Transparent dp
        //    {
        //        payPerHire_Info.Add(string.Format("Est. Uplers Fees Anuallly (In Amount) : {0}", transparentModel.CalculatedUplersfees));
        //    }
        //    else
        //    {
        //        payPerHire_Info.Add(string.Format("Est. Uplers Fees (In Amount) : {0}", transparentModel.CalculatedUplersfees));
        //        payPerHire_Info.Add(string.Format("Est. Talent's Pay : {0}", transparentModel.TalentsPay));
        //    }

        //    #region old logic

        //    //if (isBackendHR)
        //    //{
        //    //if (transparentModel.HrTypePricingId == 3) //10% Transparent dp
        //    //    payPerHire_Info.Add(string.Format("Est. Uplers Fees Anuallly (In Amount) : {0}", transparentModel.CalculatedUplersfees));
        //    //else
        //    //    payPerHire_Info.Add(string.Format("Est. Uplers Fees (In Amount) : {0}", transparentModel.CalculatedUplersfees));

        //    //payPerHire_Info.Add(string.Format("Est. Talent's Pay : {0}", transparentModel.TalentsPay));
        //    //}
        //    //else
        //    //{
        //    //    if (transparentModel.HrTypePricingId == 3) //10% Transparent dp
        //    //    {
        //    //        payPerHire_Info.Add(string.Format("Uplers Fees % : {0}", transparentModel.PricingPercent));
        //    //        payPerHire_Info.Add(string.Format("Est. Uplers Fees Anuallly (In Amount) : {0}", transparentModel.CalculatedUplersfees));
        //    //    }
        //    //    else
        //    //    {
        //    //        payPerHire_Info.Add(string.Format("Uplers Fees % : {0}", transparentModel.PricingPercent));
        //    //        payPerHire_Info.Add(string.Format("Est. Uplers Fees (In Amount) : {0}", transparentModel.CalculatedUplersfees));
        //    //    }

        //    //    if (transparentModel.HrTypePricingId == 1 ||
        //    //    transparentModel.HrTypePricingId == 2 ||
        //    //    transparentModel.HrTypePricingId == 7) // Transparent (25%, 35%) contructual
        //    //    {
        //    //        payPerHire_Info.Add(string.Format("Est. Client's Billing Amount Excl. Addons : {0}", transparentModel.ClientNeedsToPay));
        //    //    }
        //    //    else if (transparentModel.HrTypePricingId == 4 ||
        //    //             transparentModel.HrTypePricingId == 5 ||
        //    //             transparentModel.HrTypePricingId == 8) //Non transparent (25%, 35%) contructual
        //    //    {
        //    //        payPerHire_Info.Add(string.Format("Est. Talent's Pay : {0}", transparentModel.TalentsPay));
        //    //    }
        //    //}
        //    #endregion

        //    return payPerHire_Info;
        //}

        public string EngagementText(int? HiringTypePricingId, int? PayrollTypeID, string PartnerName, string CompanyName)
        {
            string EngagementText = "";
            if (HiringTypePricingId == 1 || HiringTypePricingId == 7) // 25%
            {
                EngagementText = "";
            }
            if (HiringTypePricingId == 2) // 35%
            {
                EngagementText = "Payroll and Compliacne to be managed by: Uplers Solutions Pvt. Ltd. ";
            }
            if (HiringTypePricingId == 3) //10%
            {
                if (PayrollTypeID == 2)
                {
                    EngagementText = "Payroll and Compliacne to be managed by: " + CompanyName;
                }
                else if (PayrollTypeID == 3)
                {
                    EngagementText = "Payroll and Compliacne to be managed by: " + PartnerName;
                }
                else if (PayrollTypeID == 4)
                {
                    EngagementText = string.Empty;
                }
            }
            return EngagementText;
        }

        //public List<string> PayPerHire_JobType(int HiringTypePricingId, int SpecificMonth, int PayrollTypeID, string Availability)
        //{
        //    List<string> jobType = new List<string>();
        //    if (HiringTypePricingId == 1 || HiringTypePricingId == 4 || HiringTypePricingId == 7 || HiringTypePricingId == 8)
        //    {
        //        if (SpecificMonth == -1)
        //        {
        //            jobType.Add(string.Format("Client / Uplers Agreement - {0} Indefinite Contract", Availability));
        //            jobType.Add(string.Format("Uplers / Talent Agreement - {0} Indefinite Contract", Availability));
        //        }
        //        else
        //        {
        //            jobType.Add(string.Format("Client / Uplers Agreement - {0} Contract for {1} months", Availability, SpecificMonth));
        //            jobType.Add(string.Format("Uplers / Talent Agreement - {0} Contract for {1} months", Availability, SpecificMonth));
        //        }
        //        jobType.Add("Client / Talent Agreement - NA");
        //    }
        //    if (HiringTypePricingId == 2 || HiringTypePricingId == 5)
        //    {
        //        if (SpecificMonth == -1)
        //        {
        //            jobType.Add("Client / Uplers Agreement - Full-time Permanent position ");
        //            jobType.Add("Uplers / Talent Agreement - Full-time Permanent position ");
        //        }
        //        else
        //        {
        //            jobType.Add(string.Format("Client / Uplers Agreement - Full-time Contract for {0} Months", SpecificMonth));
        //            jobType.Add(string.Format("Uplers / Talent Agreement - Full-time {0} months Project Based Employment", SpecificMonth));
        //        }
        //        jobType.Add("Client / Talent Agreement - NA");
        //    }
        //    if (HiringTypePricingId == 3 || HiringTypePricingId == 6)
        //    {
        //        jobType.Add("Client / Uplers Agreement - Direct Hire (Full-time Permanent position)");
        //        if (PayrollTypeID == 2 || PayrollTypeID == 3)
        //        {
        //            jobType.Add("Client / Talent Agreement - Full-time Permanent position");
        //        }
        //        else if (PayrollTypeID == 4)
        //        {
        //            if (SpecificMonth == -1)
        //                jobType.Add("Client / Talent Agreement - Full-time Indefinite Contract");
        //            else
        //                jobType.Add(string.Format("Client / Talent Agreement - Full-time Contract for {0} months", SpecificMonth));

        //        }
        //        jobType.Add("Uplers / Talent Agreement - NA (Just placement)");
        //    }
        //    return jobType;
        //}

        //public List<TalentDynamicInfo> TalentInfo(sp_UTS_get_HRTalentDetails_UsingPagination_Result talentDetails, bool IsHrTypeDP = false, bool IsPayPerCredit = false)
        //{
        //    List<TalentDynamicInfo> TalentInfo = new();

        //    if (IsPayPerCredit)
        //    {
        //        if (IsHrTypeDP) // Direct placement
        //        {
        //            //Talent's Current Pay
        //            TalentDynamicInfo CurrentPayInfo = new TalentDynamicInfo();
        //            CurrentPayInfo.Title = "Talent's Current Pay:";
        //            CurrentPayInfo.Value = string.IsNullOrEmpty(talentDetails.PayRateDP) ? "NA" : talentDetails.PayRateDP;
        //            CurrentPayInfo.IsEditable = false;
        //            TalentInfo.Add(CurrentPayInfo);

        //            //Talent's Expected Pay
        //            TalentDynamicInfo ExpectedPayInfo = new TalentDynamicInfo();
        //            ExpectedPayInfo.Title = "Talent's Expected Pay:";
        //            ExpectedPayInfo.Value = string.IsNullOrEmpty(talentDetails.PayRate) ? "NA" : talentDetails.PayRate;
        //            ExpectedPayInfo.IsEditable = false;
        //            TalentInfo.Add(ExpectedPayInfo);
        //        }
        //        else //Contratual
        //        {
        //            //Talent's Current Pay
        //            TalentDynamicInfo CurrentPayInfo = new TalentDynamicInfo();
        //            CurrentPayInfo.Title = "Talent's Current Pay:";
        //            CurrentPayInfo.Value = string.IsNullOrEmpty(talentDetails.PayRateDP) ? "NA" : talentDetails.PayRateDP;
        //            CurrentPayInfo.IsEditable = false;
        //            TalentInfo.Add(CurrentPayInfo);

        //            //Talent's Expected Pay
        //            TalentDynamicInfo ExpectedPayInfo = new TalentDynamicInfo();
        //            ExpectedPayInfo.Title = "Talent's Expected Pay:";
        //            ExpectedPayInfo.Value = string.IsNullOrEmpty(talentDetails.PayRate) ? "NA" : talentDetails.PayRate;
        //            ExpectedPayInfo.IsEditable = false;
        //            TalentInfo.Add(ExpectedPayInfo);
        //        }
        //    }
        //    else// Pay per hire
        //    {
        //        if (IsHrTypeDP) // Direct placement
        //        {
        //            //Talent's Current Pay
        //            TalentDynamicInfo CurrentPayInfo = new TalentDynamicInfo();
        //            CurrentPayInfo.Title = "Talent's Current Pay:";
        //            CurrentPayInfo.Value = string.IsNullOrEmpty(talentDetails.PayRateDP) ? "NA" : talentDetails.PayRateDP;
        //            CurrentPayInfo.IsEditable = false;
        //            TalentInfo.Add(CurrentPayInfo);

        //            //Talent's Expected Pay
        //            TalentDynamicInfo ExpectedPayInfo = new TalentDynamicInfo();
        //            ExpectedPayInfo.Title = "Talent's Expected Pay:";
        //            ExpectedPayInfo.Value = string.IsNullOrEmpty(talentDetails.PayRate) ? "NA" : talentDetails.PayRate;
        //            ExpectedPayInfo.IsEditable = false;
        //            TalentInfo.Add(ExpectedPayInfo);

        //            //Uplers Fees %
        //            TalentDynamicInfo UplersfeesInPer = new TalentDynamicInfo();
        //            UplersfeesInPer.Title = "Uplers Fees %:";
        //            if (!string.IsNullOrEmpty(talentDetails.DPPercentage))
        //                UplersfeesInPer.Value = talentDetails.DPPercentage + " on Talent's Annual Salary";
        //            else
        //                UplersfeesInPer.Value = "NA";
        //            UplersfeesInPer.IsEditable = true;
        //            TalentInfo.Add(UplersfeesInPer);

        //            //Uplers Fees (in Amount)
        //            TalentDynamicInfo UplersfeesInAmount = new TalentDynamicInfo();
        //            UplersfeesInAmount.Title = "Uplers Fees (in Amount):";
        //            if (!string.IsNullOrEmpty(talentDetails.UplersfeesAmount))
        //                UplersfeesInAmount.Value = talentDetails.CurrencySign + talentDetails.UplersfeesAmount + " " + talentDetails.TalentCurrenyCode + " Talent's Annual Salary";
        //            else
        //                UplersfeesInAmount.Value = "NA";
        //            UplersfeesInAmount.IsEditable = false;
        //            TalentInfo.Add(UplersfeesInAmount);

        //        }
        //        else //Contratual
        //        {
        //            //Talent's Current Pay
        //            TalentDynamicInfo CurrentPayInfo = new TalentDynamicInfo();
        //            CurrentPayInfo.Title = "Talent's Current Pay:";
        //            CurrentPayInfo.Value = string.IsNullOrEmpty(talentDetails.PayRateDP) ? "NA" : talentDetails.PayRateDP;
        //            CurrentPayInfo.IsEditable = false;
        //            TalentInfo.Add(CurrentPayInfo);

        //            //Talent's Expected Pay
        //            TalentDynamicInfo ExpectedPayInfo = new TalentDynamicInfo();
        //            ExpectedPayInfo.Title = "Talent's Expected Pay:";
        //            ExpectedPayInfo.Value = string.IsNullOrEmpty(talentDetails.PayRate) ? "NA" : talentDetails.PayRate;
        //            ExpectedPayInfo.IsEditable = false;
        //            TalentInfo.Add(ExpectedPayInfo);

        //            //Uplers Fees %
        //            TalentDynamicInfo UplersfeesInPer = new TalentDynamicInfo();
        //            UplersfeesInPer.Title = "Uplers Fees %:";
        //            UplersfeesInPer.Value = string.IsNullOrEmpty(talentDetails.NR) ? "NA" : talentDetails.NR;
        //            UplersfeesInPer.IsEditable = true;
        //            TalentInfo.Add(UplersfeesInPer);

        //            //Uplers Fees (in Amount)
        //            TalentDynamicInfo UplersfeesInAmount = new TalentDynamicInfo();
        //            UplersfeesInAmount.Title = "Uplers Fees (in Amount):";
        //            if (!string.IsNullOrEmpty(talentDetails.UplersfeesAmount))
        //                UplersfeesInAmount.Value = talentDetails.CurrencySign + talentDetails.UplersfeesAmount + " " + talentDetails.TalentCurrenyCode;
        //            else
        //                UplersfeesInAmount.Value = "NA";
        //            UplersfeesInAmount.IsEditable = false;
        //            TalentInfo.Add(UplersfeesInAmount);

        //            //Client's Bill Amount
        //            TalentDynamicInfo ClientBillAmount = new TalentDynamicInfo();
        //            ClientBillAmount.Title = "Client's Bill Amount:";
        //            ClientBillAmount.Value = string.IsNullOrEmpty(talentDetails.BillRate) ? "NA" : talentDetails.BillRate + " (with i excl. Addons)";
        //            ClientBillAmount.IsEditable = false;
        //            TalentInfo.Add(ClientBillAmount);
        //        }
        //    }
        //    return TalentInfo;
        //}

        public List<string> PayPerCredit_JobType(bool isHiringLimited, int SpecificMonth, string Availability)
        {
            List<string> jobType = new List<string>();

            jobType.Add("Client / Uplers Agreement: Post a job Credit Based Agreement ");

            if (isHiringLimited)
            {
                if (SpecificMonth > 0)
                    jobType.Add(string.Format("Client / Talent Agreement: {0} Contract for {1} months", Availability, SpecificMonth));
                if (SpecificMonth == -1)
                    jobType.Add(string.Format("Client / Talent Agreement - {0} Indefinite Contract", Availability));
                jobType.Add("Uplers / Talent Agreement - No Agreement");
            }
            else
            {
                jobType.Add("Client / Talent Agreement: Full time Permanent position");
                jobType.Add("Uplers / Talent Agreement - NA");
            }

            return jobType;
        }

        //public SP_PayPerHire_CalculationDetails_Result GetHRWiseCalculation(string paramString)
        //{
        //    return _DBContext.Set<SP_PayPerHire_CalculationDetails_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.SP_PayPerHire_CalculationDetails, paramString)).AsEnumerable().FirstOrDefault();
        //}
    }
}
