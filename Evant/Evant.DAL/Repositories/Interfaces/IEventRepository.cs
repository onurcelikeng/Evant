﻿using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evant.DAL.Repositories.Interfaces
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<List<Event>> UserEvents(Guid userId);
        Task<List<Event>> EventsByCategory(Guid categoryId);
    }
}