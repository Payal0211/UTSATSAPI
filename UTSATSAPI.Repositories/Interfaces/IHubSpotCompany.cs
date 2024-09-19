using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.ViewModels;

namespace UTSATSAPI.Repositories.Interfaces
{
    public interface IHubSpotCompany
    {
        List<Sproc_UTS_GetHubSpotCompanyList_Result> GetHubSpotCompanyList(string strparams);
    }
}
