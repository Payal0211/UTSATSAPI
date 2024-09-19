namespace UTSATSAPI.Repositories.Repositories
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using UTSATSAPI.Models.Models;
    using UTSATSAPI.Repositories.Interfaces;
    public class CommonRepository : ICommonInterface
    {
        private TalentConnectAdminDBContext db;
        private IConfiguration iConfiguration;
        private IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private IUpChatCall _iUpChatCall;
        /// <summary>
        /// Initializes a new instance of the <see cref="CommonRepository"/> class.
        /// </summary>
        /// <param name="_db">The database.</param>
        /// <param name="_iConfiguration">The i configuration.</param>
        /// <param name="_webHostEnvironment">The web host environment.</param>
        public CommonRepository(TalentConnectAdminDBContext _db, IUniversalProcRunner universalProcRunner, IConfiguration _iConfiguration, IWebHostEnvironment _webHostEnvironment, IUpChatCall iUpChatCall, IHttpContextAccessor httpContextAccessor)
        {
            db = _db;
            iConfiguration = _iConfiguration;
            webHostEnvironment = _webHostEnvironment;
            ViewAllHR = new ViewAllHRRepository(db, universalProcRunner, _iConfiguration, iUpChatCall, httpContextAccessor);
            hiringRequest = new HiringRequestRepository(db, httpContextAccessor);
            SendEmailNotes = new EmailRepository(db, iConfiguration, webHostEnvironment);
            interview = new InterviewRepository(db);
            TalentStatus = new TalentStatusRepository(db);
            _iUpChatCall = iUpChatCall;
        }
        public IViewAllHR ViewAllHR { get; set; }
        public ISendEmailNotes SendEmailNotes { get; set; }
        public IInterview interview { get; set; }
        public IHiringRequest hiringRequest { get; set; }
        public ITalentStatus TalentStatus { get; }
    }
}
