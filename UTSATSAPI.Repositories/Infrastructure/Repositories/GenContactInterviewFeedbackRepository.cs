using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenContactInterviewFeedbackRepository : GenericRepository<GenContactInterviewFeedback>, IGenContactInterviewFeedbackRepository
{
public GenContactInterviewFeedbackRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
