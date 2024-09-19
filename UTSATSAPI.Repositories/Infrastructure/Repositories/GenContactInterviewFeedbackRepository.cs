using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenContactInterviewFeedbackRepository : GenericRepository<GenContactInterviewFeedback>, IGenContactInterviewFeedbackRepository
{
public GenContactInterviewFeedbackRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
