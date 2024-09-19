using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentProfessionalExperienceRepository : GenericRepository<GenTalentProfessionalExperience>, IGenTalentProfessionalExperienceRepository
{
public GenTalentProfessionalExperienceRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
