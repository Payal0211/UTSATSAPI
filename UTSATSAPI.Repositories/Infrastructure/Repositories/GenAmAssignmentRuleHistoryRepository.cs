using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenAmAssignmentRuleHistoryRepository : GenericRepository<GenAmAssignmentRuleHistory>, IGenAmAssignmentRuleHistoryRepository
{
public GenAmAssignmentRuleHistoryRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
