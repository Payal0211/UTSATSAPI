
using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;

namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
    public class UsrUserRepository : GenericRepository<UsrUser>, IUsrUserRepository
    {
        public UsrUserRepository(UTSATSAPIDBConnection dbContext) : base(dbContext)
        {

        }
        //public async Task<Category> GetCoolestCategory()
        //{
        //    return await GetAll()
        //        .OrderByDescending(c => c.Name)
        //        .FirstOrDefaultAsync();
        //}
        public async Task<UsrUser> LoginUser(string username, string password)
        {
            try
            {
                var usrUser = await GetSingleByCondition(x => (x.Username.ToLower().Equals(username.ToLower()) || x.EmployeeId.ToLower().Equals(username)) && x.Password.Equals(password));
                return usrUser;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
