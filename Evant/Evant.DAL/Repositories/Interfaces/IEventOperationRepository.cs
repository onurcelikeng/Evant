using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evant.DAL.Repositories.Interfaces
{
    public interface IEventOperationRepository : IRepository<EventOperation>
    {
        Task<List<EventOperation>> Participants(Guid eventId);
        Task<bool> Attend(EventOperation entity);
        Task<bool> Leave(EventOperation entity);
    }
}
