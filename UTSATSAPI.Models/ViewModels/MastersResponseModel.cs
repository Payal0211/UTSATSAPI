namespace UTSATSAPI.Models.ViewModels
{
    public class MastersResponseModel
    {
        public long Id { get; set; }
        public object Value { get; set; }
        public string text { get; set; }
        public bool disabled { get; set; }
        public object group { get; set; }
        public bool seletected { get; set; }
        public string Color { get; set; }
        public string StringIdValue { get; set; }
    }

    public class CountryFlagCode
    {
        public string flag { get; set; }
        public string ccode { get; set; }
    }

    public class ErrorModel
    {
        public string ErrorMessage { get; set; }
        public string ModuleName { get; set; }
        public string FieldName { get; set; }
    }
}
