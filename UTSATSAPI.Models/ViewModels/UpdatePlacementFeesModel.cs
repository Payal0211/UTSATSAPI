namespace UTSATSAPI.Models.ViewModels
{
    public class UpdatePlacementFeesModel
    {
        public int ID { get; set; }
        public decimal? SalesConsultant { get; set; }
        public decimal? PODManagers { get; set; }
        public decimal? BDR { get; set; }
        public decimal? BDR_Lead { get; set; }
        public decimal? BDRManager_Head { get; set; }
        public decimal? MarketingTeam { get; set; }
        public decimal? MarketingLead { get; set; }
        public decimal? MarketingHead { get; set; }
        public decimal? AM { get; set; }
        public decimal? AMHead { get; set; }
    }

    public class Update_ContractModel
    {
        public int ID { get; set; }
        public decimal? SalesConsultant { get; set; }
        public decimal? PODManagers { get; set; }
        public decimal? BDR_USD { get; set; }
        public decimal? BDRLead_USD { get; set; }
        public decimal? BDRManagerHead_USD { get; set; }
        public decimal? MarketingTeam { get; set; } 
        public decimal? MarketingLead { get; set; }
        public decimal? MarketingHead { get; set; }
        public decimal? AM { get; set; }
        public decimal? AMHead { get; set; }
    }

    public class SalGoal
    {
        public List<SalGoal_INR> SalGoals { get; set; }
        public List<SalGoal_USD_FirstClosure> FirstClosure { get; set; }
    }


    public class SalGoal_INR
    {
        public int? ID { get; set; }
        public string? Slab { get; set; }
        public decimal? BDR_INR { get; set; }
        public decimal? BDRLead_INR { get; set; }
    }

    public class SalGoal_USD_FirstClosure
    {
        public int? ID { get; set; }
        public string? Slab { get; set; }
        public decimal? SalesConsultant_USD { get; set; }
        public decimal? PODManagers_USD { get; set; }
        public decimal? BDR_USD { get; set; }
        public decimal? BDRLead_USD { get; set; }
        public decimal? BDRManagerHead_USD { get; set; } 
        public decimal? MarketingTeam_USD { get; set; }
        public decimal? MarketingLead_USD { get; set; }
        public decimal? MarketingHead_USD { get; set; }
        public decimal? AM_USD { get; set; }
        public decimal? AMHead_USD { get; set; }
    }
}
