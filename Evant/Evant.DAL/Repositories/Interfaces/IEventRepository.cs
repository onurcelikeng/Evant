using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evant.DAL.Repositories.Interfaces
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<List<Event>> Timeline(Guid userId);
        Task<Event> EventDetail(Guid eventId);
        Task<List<Event>> UserEvents(Guid userId);
        Task<List<Event>> SimilarEvents(Event @event);
        Task<List<Event>> EventsByCategory(Guid categoryId);
        Task<List<Event>> Search(string query);
        Task<bool> SoftDelete(Guid eventId);
        Task<List<Event>> CityEvents(string city);
        Task<List<Event>> TownEvents(string town);
    }
}
