using FluentValidation;
using UTSATSAPI.Models.ViewModel;

namespace UTSATSAPI.Models.ViewModels.Validators
{
    public class UserValidator : AbstractValidator<UserViewModel>
    {
        public UserValidator()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
            RuleFor(x => x.FullName).NotEmpty();
            RuleFor(x => x.DeptID).GreaterThan(0);
            RuleFor(x => x.EmailId).NotEmpty();
        }
    }
}
