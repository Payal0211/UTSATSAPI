
namespace UTSATSAPI.Models.ViewModels.Validators
{
    using FluentValidation;
    using UTSATSAPI.Models.ViewModels.Request_ResponseModels;

    public class HiringRequestDebriefModelValidator : AbstractValidator<HiringRequestDebriefModel>
    {
        public HiringRequestDebriefModelValidator()
        {
            //RuleFor(x => x.requirements).NotEmpty();
            //RuleFor(x => x.roleAndResponsibilites).NotEmpty();
            RuleFor(x => x.JobDescription).NotEmpty();
            RuleFor(x => x.en_Id).NotEmpty();
            RuleFor(x => x.skills).NotEmpty();
            //RuleFor(x => x.aboutCompanyDesc).NotEmpty();
            //RuleFor(x => x.role).NotEmpty();
            RuleFor(x => x.hrTitle).NotEmpty();
        }
    }

    public class DirectHR_or_CreditHR_Debrief_ModelValidator : AbstractValidator<HiringRequestDebriefModel>
    {
        public DirectHR_or_CreditHR_Debrief_ModelValidator()
        {
            //RuleFor(x => x.requirements).NotEmpty();
            //RuleFor(x => x.roleAndResponsibilites).NotEmpty();
            RuleFor(x => x.JobDescription).NotEmpty();
            RuleFor(x => x.en_Id).NotEmpty();
            RuleFor(x => x.skills).NotEmpty();
            //RuleFor(x => x.aboutCompanyDesc).NotEmpty();
            RuleFor(x => x.hrTitle).NotEmpty();
        }
    }
}
