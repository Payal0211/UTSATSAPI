using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgAmAssignmentRuleRepository : GenericRepository<PrgAmAssignmentRule>, IPrgAmAssignmentRuleRepository
{
public PrgAmAssignmentRuleRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
