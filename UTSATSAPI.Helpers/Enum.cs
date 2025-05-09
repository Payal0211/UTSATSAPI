using System.ComponentModel.DataAnnotations;

namespace UTSATSAPI.Helpers
{
    /// <summary>
    /// Enum
    /// </summary>
    public static class Enum
    {
        public enum ProcessType
        {
            URL_Parsing,
            JDFileUpload,
            AIGenerated,
            JDURLParsingGenerated,
            Skills,
            Text_Parsing,
            Manual
        }
        public enum AppActionDoneBy
        {
            OldAdmin = 1,
            ATS = 2,
            UTS = 3,
            FrontEnd = 4
        }
        public enum EntityStaus
        {
            Active = 1,
            Inactive = 2,
            Blocked = 3,
        }
        public enum ResponseCode
        {
            Success = 1,
            Failed = 0,
            NotFound = 3
        }
        public enum ManagerType
        {
            Administrator = 1,
            Developer = 2,
            Legal = 3,
            Sales = 4,
            TalentOps = 5,
            ResumeWriter = 6,
            PracticeHead = 7,
            FinanceExecutive = 8,
            SalesManager = 9,
            OpsTeamManager = 10,
            BDR = 11,
            Marketing = 12,
            ATSUser = 13
        }
        public enum HRStatusCodes
        {
            Draft = 101,

            [Display(Name = "HR Accepted")]
            HRAccepted = 102,

            [Display(Name = "Acceptance Pending")]
            AcceptancePending = 103,

            [Display(Name = "Info Pending")]
            InfoPending = 104,

            Completed = 105,

            [Display(Name = "In Process")]
            InProcess = 106,

            Cancelled = 107,
            Other = 201
        }
        public enum HRPriorityStatusCodes
        {
            NoPriority = 0,
            NextWeekPriority = 101,
            CurrentWeekPriority = 102
        }
        public enum Action_Of_TalentHistory
        {
            Create_Talent,
            Update_Talent,
            Talent_Status_Selected,
            Talent_Status_Shortlisted,
            Talent_Status_In_Interview,
            Talent_Status_Hired,
            Talent_Status_Cancelled,
            Talent_Status_On_Hold,
            Talent_Status_Rejected,
            Talent_Status_Replacement,
            ATS_OnBoardingActive,
            ATS_ReverseMatchMaking,
            ATS_TalentID_Update,
            Talent_ActiveInactive,
            Talent_LegalDone,
            Talent_Approved,
            Talent_Remove_OnHold
        }
        public enum Action_Of_History
        {
            Create_HR = 1,
            Update_HR,
            Cancel_HR,
            Create_HRDBrifing,
            Update_HRDBrifing,
            Talent_Matchmacking,
            Slot_Given,
            Cancelled,
            Interview_Scheduled,
            Interview_in_Process,
            Interview_Completed,
            Feedback_Submitted_With_Hire,
            HR_SaveAsDraft,
            Interview_Status_Change,
            Talent_Status_Change,
            Add_Interviwer_Details,
            HR_Acceptance,
            HRPost_Acceptance,
            Update_Slots,
            Talent_Status_Cancelled,
            Talent_Status_On_Hold,
            Talent_Status_Rejected,
            Talent_Status_Replacement,
            OnboardingTalent_Pending,
            OnboardingTalent_Done,
            OnboardingTalent_Raise_Concern,
            OnboardingTalent_Concern_Resolved_and_Done,
            OnboardingClient_Pending,
            OnboardingClient_Done,
            OnboardingClient_Raise_Concern,
            OnboardingClient_Concern_Resolved_and_Done,
            OnboardingClient_Uncompleted_Data,
            OnboardingClient_In_Process,
            Legal_Client_Pending,
            Legal_Client_Done,
            Legal_Client_Sent,
            Legal_Talent_Pending,
            Legal_Talent_Done,
            Legal_Talent_Sent,
            Kick_Off_Pending,
            Kick_Off_Schedule,
            Kick_Off_Done,
            HR_PasstoAdhoc,
            WaitFor_More_Info,
            Feedback_Submitted_With_NoHire,
            Feedback_Submitted_With_OnHold,
            Feedback_Submitted_With_AnotherRound,
            HR_PasstoPool,
            Delete_Talent,
            HR_PassToPoolAndODR,
            Profile_Waiting_For_Feedback,
            Talent_Fees_Update,
            HR_Cost_Update,
            Managed_HR_And_TalentInfo,
            Update_On_Hold,
            Update_Client_Feedback,
            Another_Round_Slot,
            TR_Accepted,
            ManagedTalent_Replacement,
            Proposal_Confirmed_For_ManagedHR,
            Talent_Auto_Cancel_On_Hire,
            Delete_Engagement,
            Delete_HR_AfterOnBoard,
            HR_OnHold,
            AM_Assignment,
            Convert_HR_To_DP,
            Convert_HR_To_Contractual,
            Create_User,
            Update_User,
            End_Engagement,
            Onboard_ClientFeedback,
            HR_AutoCancelledORLost,
            RemovePartnershipHR_FromAMAssignment,
            RemovePartnershipHR,
            AM_Assignment_Update,
            Contract_Renewal,
            Manual_HR_Reopen,
            CloneHR,
            TR_Increase,
            TR_Decrease,
            Talent_Auto_Cancel,
            Close_HR,
            Reopen_HR,
            HR_Focused,
            EditHR_COETeam,
            TSC_AutoAssignment,
            Edit_TSC,
            Job_Posting,
            SLA_Edit,
            Edit_Role_By_COE,
            Add_TSC,
            Talent_Status_ByClient_CRHR,
            Talent_Status_ByUser_CRHR
        }
        public enum DeleteHRType
        {
            OnHold = 1,
            Loss = 2
        }
        public enum Talent_Status
        {
            Selected = 1,
            Shortlisted = 2,
            In_Interview = 3,
            Hired = 4,
            Cancelled = 5,
            On_Hold = 6,
            Rejected = 7,
            Replacement = 8
        }
        public enum Upchat_ChannelLibrary
        {
            images = 1,
            videos = 2,
            document = 3
        }

        public enum prg_JobStatus_ClientPortal
        {
            Active = 1,
            Closed = 2,
            Draft = 3,
            Open = 4,
            ReOpen = 5
        }
        public enum prg_HiringRequestStatus
        {
            Open = 1,           //Open
            InProcess = 2,      //In Process
            Won = 3,            //Completed ---> Won
            Cancelled = 4,      //Cancelled
            OnHold = 5,         //On Hold
            Lost = 6,           //Lost
            Paused = 7,         //Paused
            Reposted = 8,       //Reposted
            Expired = 9         //Expired
        }
        public enum prg_HiringRequest_RoleStatus
        {
            Open = 1,               //Open
            StartedSearching = 2,   //Started Searching
            Selected = 3,           //Selected
            Shortlisted = 4,        //Shortlisted
            InInterview = 5,        //In Interview
            Fulfilled = 6,          //Fulfilled
            Cancelled = 7,          //Cancelled
            OnHold = 8,             //On Hold
            Lost = 9,               //Lost
            Paused = 10,            //Paused
            Reposted = 11,          //Reposted
            Expired = 12            //Expired
        }
        public enum prg_TalentStatus_AfterClientSelection
        {
            Selected = 1,           //Selected
            ProfileShared = 2,      //Profile Shared --- Shortlisted --> ProfileShared //during talent status change
            InInterview = 3,        //InInterview
            Offered = 4,            //Offered  --  Hire --> Offered  //during talent status change
            Cancelled = 5,          //Cancelled
            OnHold = 6,             //On Hold
            Rejected = 7,           //Rejected
            InReplacement = 8,      //In Replacement -- Replacement --> In Replacement  //during talent status change
            Deleted = 9,            //Deleted
            Hired = 10,             //Hired
            InAssessment = 11,      //In Assessment
            Joined = 12,            //Joined   
        }
        public enum prg_InterviewStatus
        {
            Slot_Given = 1,                  //Slot Given
            Cancelled = 3,                   //Cancelled
            Interview_Scheduled = 4,         //Interview Scheduled
            Interview_in_Process = 5,        //Interview in Proces
            Interview_Completed = 6,         //Interview Completed
            Feedback_Submitted = 7,          //Feedback Submitted
        }

        public enum prg_HiringRequest_Type
        {
            SalesHR = 1,                                       //Direct HR(Pay per Hire)
            //DirectHR = 2,                                        //Sales HR(Pay per Hire)
            PostaJobViewBasedCredit = 3,                        //post a job + view based credit(Pay per credit)
            PostaJobCreditBased = 4,                            //post a job credit based(Pay per credit)
            PostaJobGlobal = 5,                                 //post a job global(Pay per Hire)
            PostajobWithViewCreditsButnoJobPostCredits = 6,     //Post a job with view credits but no job post credits(Pay per credit)
        }

        public enum PayPerHire
        {
            SalesHR = 1,                                        //Pay per Hire(Pay per Hire)
            //DirectHR = 2,                                       //Direct HR(Pay per Hire)
            //PostaJobGlobal = 5,                                 //post a job global(Pay per Hire)
        }

        public enum PayPerCredit
        {
            PostaJobViewBasedCredit = 3,                        //post a job + view based credit(Pay per credit)
            PostaJobCreditBased = 4,                            //post a job credit based(Pay per credit)
            PostajobWithViewCreditsButnoJobPostCredits = 6,     //Post a job with view credits but no job post credits(Pay per credit)
        }

        public enum StatusChangeAction
        {
            Sales,
            Client
        }

        public enum Action_Of_Company_History
        {
            Create_Company,
            Update_Company,
            Update_Company_Details_From_Preview,
            Send_Resend_Invite,
            Added_New_Client,
            Added_New_Client_Update,
            Update_Client
        }

    }
}
