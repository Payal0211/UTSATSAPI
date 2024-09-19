using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenUtsAssignmentHistoryRepository : GenericRepository<GenUtsAssignmentHistory>, IGenUtsAssignmentHistoryRepository
{
public GenUtsAssignmentHistoryRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
