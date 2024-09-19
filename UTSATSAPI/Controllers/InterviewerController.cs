using Microsoft.AspNetCore.Mvc;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Controllers
{
    [ApiController]
    [Route("Interviewer/")]
    public class InterviewerController : ControllerBase
    {
        #region Variables
        private readonly TalentConnectAdminDBContext _db;
        private readonly IInterviewer _iInterviewer;
        #endregion

        #region Constructor
        public InterviewerController(TalentConnectAdminDBContext talentConnectAdminDBContext,IInterviewer iInterviewer)
        {
            _db = talentConnectAdminDBContext;
            _iInterviewer = iInterviewer;
        }
        #endregion

        #region Authorized APIs

        [Authorize]
        [HttpGet("GetDetails")]
        public ObjectResult GetInterviewerDetails(int InterviewMasterId)
        {
            try
            {
                GenTalentSelectedInterviewerDetail InterviewDetails = null;

                if (InterviewMasterId == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Interview Master ID should be greater than 0" });
                }
                else
                {
                    InterviewDetails = _db.GenTalentSelectedInterviewerDetails.Where(x => x.InterviewMasterId == InterviewMasterId).FirstOrDefault();
                    if (InterviewDetails == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Interview details Not Exists" });
                    }
                }

                object[] param = new object[]
                {
                    InterviewMasterId, InterviewDetails.InterviewerId ?? 0
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<sproc_UTS_FetchInterviewerDetails_Result> result = _iInterviewer.FetchInterviewerDetails(paramasString);

                if (result != null && result.Count > 0)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = result });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Interviewer Not exist" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPost("UpdateDetails")]
        public ObjectResult UpdateInterviewerDetails(InterviewerViewModel model)
        {
            try
            {
                #region PreValidation
                if (model == null || model.interviewer == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Data is in valid" });
                }
                if (string.IsNullOrEmpty(model.interviewer.interviewerFullName))
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please enter Interviewer Name" });
                }
                if (string.IsNullOrEmpty(model.interviewer.interviewerLinkedin))
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please enter Linkedin URL" });
                }
                if (model.interviewer.yearsOfexp == null || model.interviewer.yearsOfexp == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please enter Year of experience" });
                }
                if (model.interviewer.TypeofPerson == null || model.interviewer.TypeofPerson == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please select Type of Person" });
                }
                if (string.IsNullOrEmpty(model.interviewer.interviewerDesignation))
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please enter Designation" });
                }
                if (model.MoreInterviewer != null && model.MoreInterviewer.Count > 0)
                {
                    foreach (InterviewerModel more in model.MoreInterviewer)
                    {
                        if (string.IsNullOrEmpty(more.interviewerFullName))
                        {
                            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please enter Interviewer Name" });
                        }
                        if (string.IsNullOrEmpty(more.interviewerLinkedin))
                        {
                            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please enter Linkedin URL" });
                        }
                        if (more.yearsOfexp == null || more.yearsOfexp == 0)
                        {
                            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please enter Year of experience" });
                        }
                        if (more.TypeofPerson == null || more.TypeofPerson == 0)
                        {
                            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please select Type of Person" });
                        }
                        if (string.IsNullOrEmpty(more.interviewerDesignation))
                        {
                            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please enter Designation" });
                        }
                    }
                }
                #endregion

                #region crud for main interviewer
                int ActionMode = 0;

                if (model.interviewer.IsAdd) ActionMode = 1;
                if (model.interviewer.IsUpdate) ActionMode = 2;
                if (model.interviewer.IsDelete) ActionMode = 3;

                object[] param = new object[]
                {
                    model.interviewer.interviewerFullName ?? string.Empty,
                    model.interviewer.interviewerLinkedin ?? string.Empty,
                    model.interviewer.yearsOfexp ?? 0,
                    model.interviewer.interviewerDesignation?? string.Empty,
                    model.interviewer.interviewerEmail?? string.Empty,
                    model.interviewer.TypeofPerson ?? 0,
                    model.interviewer.InterviewMasterID, 0, model.interviewer.InterviewerId, ActionMode
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                _iInterviewer.AddUpdateDeleteInterviewerDetails(paramasString);
                #endregion

                #region crud for more interviewer
                if (model.MoreInterviewer != null && model.MoreInterviewer.Count > 0)
                {
                    foreach (InterviewerModel more in model.MoreInterviewer)
                    {
                        ActionMode = 0;

                        if (more.IsAdd) ActionMode = 1;
                        if (more.IsUpdate) ActionMode = 2;
                        if (more.IsDelete) ActionMode = 3;

                        param = new object[]
                        {
                            more.interviewerFullName ?? string.Empty,
                            more.interviewerLinkedin ?? string.Empty,
                            more.yearsOfexp ?? 0,
                            more.interviewerDesignation?? string.Empty,
                            more.interviewerEmail?? string.Empty,
                            more.TypeofPerson ?? 0,
                            more.InterviewMasterID,
                            more.SelectedInterviewerID > 0 && more.IsDelete ? more.SelectedInterviewerID : 0,
                            more.InterviewerId,
                            ActionMode
                        };

                        paramasString = CommonLogic.ConvertToParamString(param);

                        _iInterviewer.AddUpdateDeleteInterviewerDetails(paramasString);
                    }
                }
                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpGet("CheckLinkedinURL")]
        public ObjectResult CheckLinkedinURL(string linkedinurl, long Hr_DetailID)
        {
            try
            {
                if (string.IsNullOrEmpty(linkedinurl) || Hr_DetailID == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please enter Linkedin URL" });
                }

                var ExistedLinkedincontact = _db.GenContacts.Where(x => x.LinkedIn == linkedinurl).FirstOrDefault();

                if (ExistedLinkedincontact != null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Linkedin URL is already used" });
                }
                else
                {
                    var ExistedLinkedin = _db.GenSalesHiringRequestInterviewerDetails.Where(x => x.InterviewLinkedin == linkedinurl && x.HiringRequestDetailId == Hr_DetailID).FirstOrDefault();

                    if (ExistedLinkedin != null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Linkedin URL is already used" });
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Go ahead" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpGet("CheckInterviewerEmailId")]
        public ObjectResult CheckInterviewerEmailId(string EmailId, long Hr_DetailID)
        {
            try
            {
                if (string.IsNullOrEmpty(EmailId) || Hr_DetailID == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please enter Email ID" });
                }

                var ExistedEmailId = _db.GenSalesHiringRequestInterviewerDetails.Where(x => x.InterviewerEmailId == EmailId && x.HiringRequestDetailId == Hr_DetailID).Where(x => x.InterviewerEmailId != "").FirstOrDefault();

                if (ExistedEmailId != null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Interviewer Email Id is already used" });

                }
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Go ahead" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }

}
