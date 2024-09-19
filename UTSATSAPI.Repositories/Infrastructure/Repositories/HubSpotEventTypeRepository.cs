using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class HubSpotEventTypeRepository : GenericRepository<HubSpotEventType>, IHubSpotEventTypeRepository
{
public HubSpotEventTypeRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
