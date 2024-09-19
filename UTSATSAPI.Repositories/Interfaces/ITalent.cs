using UTSATSAPI.Models.ComplexTypes;

namespace UTSATSAPI.Repositories.Interfaces
{
    public interface ITalent
    {
        List<sproc_UTS_GetTalentList_Result> GetTalentList(string paramasString);
        List<Sproc_UTS_ManagedTalent_Result> GetManagedTalentList(string paramasString);
        List<Sproc_UTS_GetTalentLegalInfo_Result> GetTalentLegalInfo(string paramasString);
    }
}
