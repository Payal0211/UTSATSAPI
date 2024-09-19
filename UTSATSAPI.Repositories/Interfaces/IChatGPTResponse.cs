using System.Linq.Expressions;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Models.ViewModels.Reports;

namespace UTSATSAPI.Repositories.Interfaces
{
    public interface IChatGPTResponse
    {
        Task<List<sproc_Get_ChatGPT_Response_For_UrlParsing_List_Result>> GetChatGPTResponse(string paramasString);
    }
}
