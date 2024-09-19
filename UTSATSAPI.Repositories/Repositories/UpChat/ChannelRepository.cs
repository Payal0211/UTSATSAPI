using Microsoft.EntityFrameworkCore;
using UTSATSAPI.Helpers;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.ComplexTypes.UpChat;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Repositories.Interfaces.UpChat;

namespace UTSATSAPI.Repositories.Repositories.UpChat
{
    public class ChannelRepository : IChannel
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        #endregion

        #region Constructors
        public ChannelRepository(TalentConnectAdminDBContext _db)
        {
            db = _db;
        }
        #endregion

        #region public Methods
        public async Task<List<Sproc_GetChannels_Result>> Sproc_GetChannelsList(long hrID)
        {
            return await db.Set<Sproc_GetChannels_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.UpChatSproc.sproc_GetChannels, hrID)).ToListAsync();
        }

        public async Task<List<Sproc_GetChannels_Result>> Sproc_GetChannelsList()
        {
            return await db.Set<Sproc_GetChannels_Result>().FromSqlRaw(string.Format("{0}", Constants.UpChatSproc.sproc_GetChannels)).ToListAsync();
        }

        public async Task<List<sp_get_HRHistory_Result>> GetHRHistory(long hrID)
        {
            return await db.Set<sp_get_HRHistory_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sp_UTS_get_HRHistory, hrID)).ToListAsync();
        }
        public async Task<List<Sproc_GetChannelUsers_Result>> GetChannelUsers(long hrID)
        {
            return await db.Set<Sproc_GetChannelUsers_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.UpChatSproc.Sproc_GetChannelUsers, hrID)).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<sp_UTS_get_HRHistoryUpChat_Result>> GetRecentHRActivity(string param)
        {
            return await db.Set<sp_UTS_get_HRHistoryUpChat_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.UpChatSproc.sp_UTS_get_HRHistoryUpChat, param)).ToListAsync();
        }

        public async Task<List<Sproc_UpChat_GetHRNotes_Result>> GetHrNote(string param)
        {
            return await db.Set<Sproc_UpChat_GetHRNotes_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.UpChatSproc.Sproc_UpChat_GetHRNotes, param)).ToListAsync();
        }

        public bool AddRestoreActivityWithStatus(object[] args)
        {
            string paramasString = CommonLogic.ConvertToParamString(args);
            db.Database.ExecuteSqlRaw(string.Format("{0} {1}", Constants.UpChatSproc.sp_Upchat_Insert_Into_StoreAPIURL, paramasString));
            return true;
        }

        public async Task<List<sp_Upchat_GetInActiveHRList_Result>> GetInActiveHRList()
        {
            return await db.Set<sp_Upchat_GetInActiveHRList_Result>().FromSqlRaw(string.Format("{0}", Constants.UpChatSproc.sp_Upchat_GetInActiveHRList)).ToListAsync();
        }
        #endregion
    }
}
