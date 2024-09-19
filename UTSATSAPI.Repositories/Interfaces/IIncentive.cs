using UTSATSAPI.Models.ComplexTypes;

namespace UTSATSAPI.Repositories.Interfaces
{
    public interface IIncentive
    {
        List<Sproc_UTS_Get_Inc_PlacementFees_Slab_Result> GetPlacementFees(string param);
        List<Sproc_UTS_Get_Acheived_Target_Details_Result> Get_Acheived_Target_Details(string param);
        List<Sproc_UTS_Get_Inc_AM_NRSlab_Result> Get_AM_NRSlab(string param);
        List<Sproc_Get_Inc_Contracts_Result> GetContracts(string param);
        List<Sproc_Get_Inc_TalentDeploySlab_Result> GetAMTalentDeployment(string param);
        List<Sproc_Get_Inc_SALGoal_Result> GetSALGoal(string param);
    }
}
