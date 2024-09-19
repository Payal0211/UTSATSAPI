using UTSATSAPI.Models.Models;

namespace UTSATSAPI.Repositories.Interfaces
{
    public interface IAdminUserLogin
    {
        Task<UsrUser> LoginUser(string username, string password);
        Task<UsrUser> LoginUser(long UserID);
        Task<UsrUser> IsValidEmail(string email);
        Task<bool> ChangePassword(UsrUser loginUser);
        Task<bool> IsAddTokenInMemory(string token, string userId);
        Task<bool> IsValidJwtToken(string token);
        Task<IList<JwtTokenDataModel>> GetActiveTokenList();
        Task<bool> IsLogoutUser(string token);
    }
}
