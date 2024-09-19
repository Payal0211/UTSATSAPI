namespace UTSATSAPI.Helpers.Common
{
    using System.Text.RegularExpressions;

    public class Validator
    {
        #region Variables
        private List<Tuple<string, string, string>> _validators;
        #endregion

        #region Constructors
        public Validator(List<Tuple<string, string, string>> validators)
        {
            _validators = validators;
        }

        #endregion

        #region Public Methods
        public List<string> Validate()
        {
            List<string> errors = new List<string>();

            foreach (var validator in _validators)
            {
                switch (validator.Item1)
                {
                    case "notempty":
                        if (string.IsNullOrEmpty(validator.Item2))
                        {
                            errors.Add(string.Format("{0} should not be empty", validator.Item3));
                        }
                        break;
                    case "email":
                        if (!string.IsNullOrEmpty(validator.Item2))
                        {
                            if (!Regex.IsMatch(validator.Item2, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                            {
                                errors.Add(string.Format("{0} should be in proper format", validator.Item3));
                            }
                        }
                        break;
                    case "password":

                        break;

                    default:
                        break;
                }
            }
            return errors;
        }
        #endregion

    }
}
