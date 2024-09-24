using AutoMapper;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;

namespace UTSATSAPI.Config
{
    public class UTSProfile : Profile
    {
        //public UTSProfile()
        //{
        //    CreateMap<GenCompany, Company>()
        //        .ForMember(d=>d.en_Id,opt=>opt.MapFrom(s=> CommonLogic.Encrypt(s.Id.ToString())))
        //        .IgnoreAllPropertiesWithAnInaccessibleSetter();

        //    CreateMap<sproc_UTS_GetCompanyDetails_Result, CompanyDetails>();
        //    CreateMap<sproc_UTS_GetContactDetails_Result, ContactDetails>();
        //    CreateMap<GenCompanyContractDetail, CompanyContract>();
        //}
    }
}
