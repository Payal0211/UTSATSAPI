using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentSecondarySkillDetailRepository : GenericRepository<GenTalentSecondarySkillDetail>, IGenTalentSecondarySkillDetailRepository
{
public GenTalentSecondarySkillDetailRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
