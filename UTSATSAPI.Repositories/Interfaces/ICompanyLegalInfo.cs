using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.ViewModels;

namespace UTSATSAPI.Repositories.Interfaces
{
    public interface ICompanyLegalInfo
    {
        List<sproc_GetCompanyLegalInfo_Result> GetLegalInfoList(string strparams);
    }
}
