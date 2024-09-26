using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentAssesmentScoreRepository : GenericRepository<GenTalentAssesmentScore>, IGenTalentAssesmentScoreRepository
{
public GenTalentAssesmentScoreRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
