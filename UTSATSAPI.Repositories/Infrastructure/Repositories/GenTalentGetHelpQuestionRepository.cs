using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentGetHelpQuestionRepository : GenericRepository<GenTalentGetHelpQuestion>, IGenTalentGetHelpQuestionRepository
{
public GenTalentGetHelpQuestionRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
