using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenClientHappinessSurveyOptionRepository : GenericRepository<GenClientHappinessSurveyOption>, IGenClientHappinessSurveyOptionRepository
{
public GenClientHappinessSurveyOptionRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
