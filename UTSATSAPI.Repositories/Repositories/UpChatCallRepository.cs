using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net;
using UTSATSAPI.Models.ViewModels.UpChat;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Repositories.Repositories
{
    public class UpChatCallRepository : IUpChatCall
    {
        #region Variables
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        #endregion

        #region Constructors
        public UpChatCallRepository(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        #endregion

        #region Public Methods
        public async Task CreateChannel(long HrID)
        {
            string AddChannelAPIUrl = string.Format("{0}Channel/AddChannel?HrID={1}", _configuration["UpChatAPI_URL"], HrID);
            await GetUpChatCall(AddChannelAPIUrl).ConfigureAwait(false);
        }
        public async Task UpdateChannel(long HrID)
        {
            string UpdateChannelAPIUrl = string.Format("{0}Channel/UpdateHrStatus?HrID={1}", _configuration["UpChatAPI_URL"], HrID);
            await GetUpChatCall(UpdateChannelAPIUrl).ConfigureAwait(false);
        }
        public async Task AddHRActivityInChats(long HrId, long HistoryId)
        {
            string AddHRActivityInChatsUrl = string.Format("{0}Channel/AddHRActivityInChats?HrID={1}&HistoryID={2}", _configuration["UpChatAPI_URL"], HrId, HistoryId);
            await GetUpChatCall(AddHRActivityInChatsUrl).ConfigureAwait(false);
        }
        public async Task AddNotesInChats(long HrId, long NoteID)
        {
            string AddNotesAPIUrl = string.Format("{0}Channel/AddNotesInChats?HrID={1}&NoteID={2}", _configuration["UpChatAPI_URL"], HrId, NoteID);
            await GetUpChatCall(AddNotesAPIUrl).ConfigureAwait(false);
        }
        public async Task<string> GetChannelLibrary(string Type, string ChannelID)
        {
            string AddNotesAPIUrl = string.Format("{0}Storage/HrWiseChannelLibrary?Type={1}&ChannelID={2}", _configuration["UpChatAPI_URL"], Type, ChannelID);
            string Json = await GetUpChatCall(AddNotesAPIUrl).ConfigureAwait(false);
            return Json;    
        }

        private async Task<string> GetUpChatCall(string uri)
        {
            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri(uri);
                httpClient.DefaultRequestHeaders.Add("X-API-KEY", _configuration["UpChatApiKey"]);
                var response = await httpClient.GetAsync("");
                string Json =  await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return Json;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
