using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentInterviewFeedbackRepository : GenericRepository<GenTalentInterviewFeedback>, IGenTalentInterviewFeedbackRepository
{
public GenTalentInterviewFeedbackRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
