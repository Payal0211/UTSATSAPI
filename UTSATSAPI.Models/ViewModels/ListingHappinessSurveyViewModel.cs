namespace UTSATSAPI.Models.ViewModels
{
    public class ListingHappinessSurveyViewModel 
    {
        public int pagenumber { get; set; }

        public int totalrecord { get; set; }
        public FilterFields_HappinessSurvey filterFields_HappinessSurvey { get; set; }
    }

    public class FilterFields_HappinessSurvey
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string selectedFormat { get; set; }
        public string FeedbackStatus { get; set; }
        public int RatingFrom { get; set; }
        public int RatingTo { get; set; }
        public string Company { get; set; }
        public string Client { get; set; }
        public string Email { get; set; }
        public int Rating { get; set; }
        public string Question { get; set; }
        public string Options { get; set; }
        public string search { get; set; }
    }
}
