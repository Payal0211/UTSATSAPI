using System.Net.Http.Headers;
using TalentConnectAdminAPI.Models.Models;

namespace TalentConnectAdminAPI.UpChatCalls
{
    public class UpChatCall
    {
        #region Variables
        private readonly IConfiguration _configuration;
        private readonly TalentConnectAdminDBContext _talentConnectAdminDBContext;
        private readonly IHttpClientFactory _httpClientFactory;
        #endregion

        #region Constructors
        public UpChatCall(IConfiguration configuration, TalentConnectAdminDBContext talentConnectAdminDBContext, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _talentConnectAdminDBContext = talentConnectAdminDBContext;
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
        private async Task GetUpChatCall(string uri)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(uri);
            httpClient.DefaultRequestHeaders.Add("X-API-KEY", _configuration["UpChatApiKey"]);
            var response = await httpClient.GetAsync("");
            string str = await response.Content.ReadAsStringAsync();
        }
        #endregion
    }
}
