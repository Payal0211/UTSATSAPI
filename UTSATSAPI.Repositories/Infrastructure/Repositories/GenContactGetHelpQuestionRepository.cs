using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenContactGetHelpQuestionRepository : GenericRepository<GenContactGetHelpQuestion>, IGenContactGetHelpQuestionRepository
{
public GenContactGetHelpQuestionRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
