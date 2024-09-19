using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenAmnbdAssignmentEmailsendDetailRepository : GenericRepository<GenAmnbdAssignmentEmailsendDetail>, IGenAmnbdAssignmentEmailsendDetailRepository
{
public GenAmnbdAssignmentEmailsendDetailRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
