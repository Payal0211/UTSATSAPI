using Microsoft.EntityFrameworkCore;
using UTSATSAPI.Helpers;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Repositories.Repositories
{
    public class ChatGPTRepository : IChatGPTResponse
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        #endregion

        #region Constructors
        public ChatGPTRepository(TalentConnectAdminDBContext _db)
        {
            db = _db;
        }
        #endregion

        public async Task<List<sproc_Get_ChatGPT_Response_For_UrlParsing_List_Result>> GetChatGPTResponse(string paramasString)
        {
            return db.Set<sproc_Get_ChatGPT_Response_For_UrlParsing_List_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_Get_ChatGPT_Response_For_UrlParsing_List, paramasString)).ToList();
        }
    }
}
