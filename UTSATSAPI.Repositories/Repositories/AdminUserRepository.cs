using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Repositories.Repositories
{
    public class AdminUserRepository : IAdminUserLogin
    {
        #region Variables
        private IUnitOfWork _unitOfWork;
        private readonly InMemoryDbContext _dbcontext;
        #endregion

        #region Constructor
        public AdminUserRepository(IUnitOfWork unitOfWork, InMemoryDbContext dbcontext)
        {
            _unitOfWork = unitOfWork;
            _dbcontext = dbcontext;
        }


        #endregion

        #region Public Methods
        public async Task<UsrUser> LoginUser(string username, string password)
        {
            try
            {
                var usrUser = await _unitOfWork.usrUsers.GetSingleByCondition(x => (x.Username.ToLower().Equals(username.ToLower()) || x.EmployeeId.ToLower().Equals(username)) && x.Password.Equals(password) && x.IsActive == true);
                if (usrUser != null && password == usrUser.Password)
                {
                    return usrUser;
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<UsrUser> LoginUser(long UserID)
        {
            try
            {
                var usrUser = await _unitOfWork.usrUsers.GetSingleByCondition(x => x.Id == UserID && x.IsActive == true);
                return usrUser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<UsrUser> IsValidEmail(string email)
        {
            try
            {
                var usrUser = await _unitOfWork.usrUsers.GetSingleByCondition(_ => _.EmailId.Equals(email));
                UsrUser usrUsers = usrUser;

                return usrUsers;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public async Task<bool> ChangePassword(UsrUser loginUser)
        {
            try
            {
                if (loginUser != null)
                {
                    _unitOfWork.Save();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> IsAddTokenInMemory(string token, string userId)
        {
            try
            {
                if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(userId))
                {
                    JwtTokenDataModel objJwtTokenDataModel = new();
                    objJwtTokenDataModel.JwtToken = token;
                    objJwtTokenDataModel.UserId = userId;
                    _dbcontext.JwtTokenDataModels.Add(objJwtTokenDataModel);
                    await _dbcontext.SaveChangesAsync();
                    return true;
                }
                else
                { return false; }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> IsValidJwtToken(string token)
        {
            try
            {
                var result = await _dbcontext.JwtTokenDataModels.Where(x => x.JwtToken == token).FirstOrDefaultAsync();
                if (result != null)
                {
                    return true;
                }
                { return false; }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IList<JwtTokenDataModel>> GetActiveTokenList()
        {
            try
            {
                return await _dbcontext.JwtTokenDataModels.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> IsLogoutUser(string token)
        {
            try
            {
                var result = _dbcontext.JwtTokenDataModels.Where(x => x.JwtToken == token).FirstOrDefault();

                if (result != null)
                {
                    _dbcontext.JwtTokenDataModels.Remove(result);
                    await _dbcontext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion
    }
}
