using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgClientHappinessSurveyFeedbackQuestionRepository : GenericRepository<PrgClientHappinessSurveyFeedbackQuestion>, IPrgClientHappinessSurveyFeedbackQuestionRepository
{
public PrgClientHappinessSurveyFeedbackQuestionRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
