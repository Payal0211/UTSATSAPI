namespace UTSATSAPI.Repositories.Repositories
{
    using System.Collections.Generic;
    using UTSATSAPI.Models.Models;
    using UTSATSAPI.Models.ViewModel;
    using UTSATSAPI.Repositories.Interfaces;
    public class HRInterviewerRepository : IHRInterviewerDetail
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        #endregion

        #region Constructors
        public HRInterviewerRepository(TalentConnectAdminDBContext _db)
        {
            db = _db;
        }
        #endregion

        #region Public Methods
        public List<SalesHiringRequestInterviewerDetailViewModel> GetHRInterviewerDetails(long HiringRequestID)
        {
            List<SalesHiringRequestInterviewerDetailViewModel> salesHRInterviewerDetailList = new();
            PrgCompanyTypeofInterviewer prgCompanyTypeofInterviewer = new();
            SalesHiringRequestInterviewerDetailViewModel salesHiringRequestInterviewerDetailViewModel = new();

            var gen_SalesHiringRequest_InterviewerDetails = db.GenSalesHiringRequestInterviewerDetails.Where(x => x.HiringRequestId == HiringRequestID).ToList();
            if (gen_SalesHiringRequest_InterviewerDetails != null)
            {
                foreach (var interviwerDetail in gen_SalesHiringRequest_InterviewerDetails)
                {
                    prgCompanyTypeofInterviewer = db.PrgCompanyTypeofInterviewers.Where(x => x.Id == interviwerDetail.TypeofInterviewerId).FirstOrDefault();

                    salesHiringRequestInterviewerDetailViewModel = new SalesHiringRequestInterviewerDetailViewModel
                    {
                        Id = interviwerDetail.Id,
                        HiringRequestId = interviwerDetail.HiringRequestId,
                        HiringRequestDetailId = interviwerDetail.HiringRequestDetailId,
                        InterviewerName = interviwerDetail.InterviewerName,
                        InterviewLinkedin = interviwerDetail.InterviewLinkedin,
                        InterviewerYearofExperience = interviwerDetail.InterviewerYearofExperience,
                        TypeofInterviewerName = prgCompanyTypeofInterviewer == null ? "" : prgCompanyTypeofInterviewer.TypeofInterviewer,
                        InterviewerDesignation = interviwerDetail.InterviewerDesignation,
                        InterviewerEmailId = interviwerDetail.InterviewerEmailId
                    };

                    salesHRInterviewerDetailList.Add(salesHiringRequestInterviewerDetailViewModel);
                }
            }

            return salesHRInterviewerDetailList;
        }
        #endregion
    }
}
