using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenClientCalendarEventRepository : GenericRepository<GenClientCalendarEvent>, IGenClientCalendarEventRepository
{
public GenClientCalendarEventRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
