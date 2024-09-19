using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UTSATSAPI.Helpers;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.ComplexTypes.UpChat;
using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels.UpChat;
using UTSATSAPI.Repositories.Interfaces.UpChat;

namespace UTSATSAPI.Repositories.Repositories.UpChat
{
    public class UserRepository : IUser
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        private IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructors
        public UserRepository(TalentConnectAdminDBContext _db, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            db = _db;
        }
        #endregion

        #region public Methods
        public async Task<List<Sproc_UpChat_Get_UserList_Result>> Sproc_Get_UserList()
        {
            return await db.Set<Sproc_UpChat_Get_UserList_Result>().FromSqlRaw(string.Format("{0}", Constants.UpChatSproc.Sproc_UpChat_Get_UserList)).ToListAsync();
        }

        public async Task<List<Sproc_UpChat_Get_UserList_Result>> GetUserListBasedOnActionPerformBy(long hrID)
        {
            return await db.Set<Sproc_UpChat_Get_UserList_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.UpChatSproc.Sp_Upchat_GetUserListBasedOnActionPerformBy, hrID)).ToListAsync();
        }

        public bool AddUserHistory(object[] args)
        {
            string paramasString = CommonLogic.ConvertToParamString(args);
            db.Database.ExecuteSqlRaw(string.Format("{0} {1}", Constants.UpChatSproc.Sproc_UpChat_AddChannelHistory, paramasString));
            return true;
        }

        public async Task<long> AddAPIResponse(UpchatStoreApiurl upchatStoreApiurl)
        {
            long InsertedID = 0;

            var obj = new StoreApiurl();
            if (upchatStoreApiurl != null)
            {
                var UpchatPortURL = _configuration.GetValue("UpChatAPI_URL", "");

                obj.Hrid = upchatStoreApiurl.Hrid ?? 0;
                obj.Apiurl = string.Format("{0}{1}", UpchatPortURL, upchatStoreApiurl.Apiurl);
                obj.Payload = upchatStoreApiurl.Payload;
                obj.ResponseStatus = "";
                obj.CreatedDt = DateTime.Now;
                obj.FromAts = upchatStoreApiurl.FromAts;

                await _unitOfWork.storeAPIUrl.Add(obj);
                _unitOfWork.Save();

                InsertedID = obj.Id;
            }

            return InsertedID;
        }

        public async Task<bool> UpdateAPIResponse(UpchatStoreApiurl upchatStoreApiurl)
        {
            if (upchatStoreApiurl != null && upchatStoreApiurl.Id > 0)
            {
                var obj = _unitOfWork.storeAPIUrl.GetSingleByCondition(x => x.Id == upchatStoreApiurl.Id);
                if (obj != null && obj.Result != null)
                {
                    obj.Result.CreatedDt = DateTime.Now;
                    obj.Result.ResponseStatus = upchatStoreApiurl.ResponseStatus;

                    _unitOfWork.storeAPIUrl.Update(obj.Result);
                    _unitOfWork.Save();
                }
            }
            return true;
        }
        #endregion
    }
}
