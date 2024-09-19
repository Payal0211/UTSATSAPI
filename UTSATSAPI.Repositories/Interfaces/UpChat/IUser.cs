using UTSATSAPI.Models.ComplexTypes.UpChat;
using UTSATSAPI.Models.ViewModels.UpChat;

namespace UTSATSAPI.Repositories.Interfaces.UpChat
{
    public interface IUser
    {
        Task<List<Sproc_UpChat_Get_UserList_Result>> Sproc_Get_UserList();
        Task<List<Sproc_UpChat_Get_UserList_Result>> GetUserListBasedOnActionPerformBy(long hrID);
        bool AddUserHistory(object[] args);
        Task<long> AddAPIResponse(UpchatStoreApiurl upchatStoreApiurl);
        Task<bool> UpdateAPIResponse(UpchatStoreApiurl upchatStoreApiurl);
    }
}
