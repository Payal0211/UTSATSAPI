using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.ComplexTypes.UpChat;

namespace UTSATSAPI.Repositories.Interfaces.UpChat
{
    public interface IChannel
    {
        Task<List<Sproc_GetChannels_Result>> Sproc_GetChannelsList(long hrID);
        Task<List<Sproc_GetChannels_Result>> Sproc_GetChannelsList();
        Task<List<sp_get_HRHistory_Result>> GetHRHistory(long hrID);
        Task<List<Sproc_GetChannelUsers_Result>> GetChannelUsers(long hrID);
        Task<List<sp_UTS_get_HRHistoryUpChat_Result>> GetRecentHRActivity(string param);
        Task<List<Sproc_UpChat_GetHRNotes_Result>> GetHrNote(string param);
        bool AddRestoreActivityWithStatus(object[] args);
        Task<List<sp_Upchat_GetInActiveHRList_Result>> GetInActiveHRList();
    }
}
