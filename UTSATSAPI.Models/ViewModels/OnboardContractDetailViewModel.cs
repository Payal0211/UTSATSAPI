using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;

namespace UTSATSAPI.Models.ViewModels
{
    public class OnboardContractDetailViewModel
    {
        public Sproc_UTS_Get_OnBoardContract_Details_Result onboardContractDetails { get; set; }
        public List<Sproc_UTS_Get_OnBoardClientTeamMemberDeatils_Result> onBoardClientTeamMembers { get; set; }
    }
}
