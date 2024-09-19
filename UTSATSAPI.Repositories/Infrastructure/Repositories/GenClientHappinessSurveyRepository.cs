using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenClientHappinessSurveyRepository : GenericRepository<GenClientHappinessSurvey>, IGenClientHappinessSurveyRepository
{
public GenClientHappinessSurveyRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
