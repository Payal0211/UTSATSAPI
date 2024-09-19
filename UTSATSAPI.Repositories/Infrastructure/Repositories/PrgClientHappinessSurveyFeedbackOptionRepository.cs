using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgClientHappinessSurveyFeedbackOptionRepository : GenericRepository<PrgClientHappinessSurveyFeedbackOption>, IPrgClientHappinessSurveyFeedbackOptionRepository
{
public PrgClientHappinessSurveyFeedbackOptionRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
