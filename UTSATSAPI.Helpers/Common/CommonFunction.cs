using System.Globalization;
using System.Text.RegularExpressions;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;

namespace UTSATSAPI.Helpers.Common
{
    public class CommonFunction
    {
        public static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            int num = (day - start.DayOfWeek + 7) % 7;
            return start.AddDays(num);
        }

        public static DateTime GetPreviousWeekday(DateTime start, DayOfWeek day)
        {
            int num = (day - start.DayOfWeek - 7) % 7;
            return start.AddDays(num);
        }

        public static string GetAmountTextInCommaSeprated(string input, string currency, bool PerMonth = true)
        {
            string[] arrSplit = { " - ", " to " };
            
            string[] cost = input.Split(arrSplit, StringSplitOptions.None);
            List<string> UpdatedCost = new();
            string FinalCost = string.Empty;
            bool isOpenForAnyBudget = false;

            foreach (string s in cost)
            {
                var update = s.Contains('.') ? s.Substring(0, s.LastIndexOf('.')) : s;
                string number = Regex.Replace(update, @"\D", "");
                string BudgetCostText = s;

                if (!string.IsNullOrEmpty(number))
                {
                    string fare = number.ToString(); //"1234567";
                    decimal parsed = decimal.Parse(fare, CultureInfo.InvariantCulture);

                    string CultureCode = GetCultureCode(currency);
                    BudgetCostText = FormatCurrency(parsed, CultureCode);
                }
                else
                {
                    isOpenForAnyBudget = true;
                }
                UpdatedCost.Add(BudgetCostText);
            }

            FinalCost = string.Join(" to ", UpdatedCost);
            if (PerMonth && !isOpenForAnyBudget)
            {
                FinalCost = FinalCost + " / Month";
            }
            return FinalCost;
        }

        static string GetCultureCode(string currencyCode)
        {
            switch (currencyCode)
            {
                case "USD": return "en-US"; // United States
                case "EUR": return "fn-FR"; // Euro
                case "AUD": return "en-AU"; // Australia
                case "GBP": return "en-GB"; // United Kingdom
                case "HKD": return "zh-HK"; // Hong Kong
                case "INR": return "en-IN"; // India
                case "CAD": return "en-CA"; // Canada
                case "AED": return "ar-AE"; // United Arab Emirates
                case "ZAR": return "en-ZA"; // South Africa
                default: return "en-IN";// India
            }
        }

        public static string FormatCurrency(decimal amount, string cultureCode)
        {
            CultureInfo cultureInfo = new CultureInfo(cultureCode);
            return string.Format(cultureInfo, "{0:C}", amount);
        }

    }
}