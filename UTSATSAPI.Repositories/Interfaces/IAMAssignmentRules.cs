using UTSATSAPI.Models.ComplexTypes;

namespace UTSATSAPI.Repositories.Interfaces
{
    public interface IAMAssignmentRules
    {
        Task<List<sproc_UTS_GetAMAssignmentRules_Result>> GetAMAssignmentRules(string paramasString);
    }
}
