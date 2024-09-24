using ElmahCore;
using System.Diagnostics;
using System.Text.Json;
using UTSATSAPI.Helpers;
using UTSATSAPI.Models.Models;

namespace UTSATSAPI.Config
{
    public class MyNotifier : IErrorNotifier
    {
        private readonly IConfiguration _configuration;
        public MyNotifier(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Notify(Error error)
        {
            Debug.WriteLine(error.Message);

            var errorResponse = new ResponseObject();
            //{
            //    statusCode = StatusCodes.Status500InternalServerError
            //};

            switch (error.StatusCode)
            {
                case 403:
                    errorResponse.Message = error.Message + "  Invalid Token";
                    errorResponse.Details = error.Detail;
                    break;
                case 400:
                    errorResponse.Message = error.Message;
                    errorResponse.Details = error.Detail;
                    break;
                case 500:
                    errorResponse.Message = error.Message;
                    errorResponse.Details = error.Detail;
                    break;
                default:
                    errorResponse.Message = error.Message;
                    errorResponse.Details = error.Detail;
                    break;
            }

            SendEmails sendEmails = new SendEmails();
            var result = JsonSerializer.Serialize(errorResponse);

            //EmailBinder binder = new EmailBinder(_configuration, null);
            //List<string> toemail = new List<string>() { "anit.s@uplers.in" };
            //List<string> toemailname = new List<string>() { "anits" };
            //binder.BindEmailForError(toemail, toemailname, "Exception Email", "<b> [ Error ] </b><br>" + result);

        }

        public string Name => "my";
    }
}
