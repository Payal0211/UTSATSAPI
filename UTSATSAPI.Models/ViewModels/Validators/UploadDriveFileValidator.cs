namespace UTSATSAPI.Models.ViewModels.Validators
{
    using FluentValidation;
    using UTSATSAPI.Models.ViewModels.Request_ResponseModels;
    public class UploadDriveFileValidator : AbstractValidator<UploadDriveFileModel>
    {
        public UploadDriveFileValidator()
        {
            RuleFor(x => x.FileId).NotEmpty();
            RuleFor(x => x.FileName).NotEmpty();
        }
    }
}
