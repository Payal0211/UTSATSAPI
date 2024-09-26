using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentEducationDetailRepository : GenericRepository<GenTalentEducationDetail>, IGenTalentEducationDetailRepository
{
public GenTalentEducationDetailRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
