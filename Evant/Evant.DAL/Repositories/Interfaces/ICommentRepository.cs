using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evant.DAL.Repositories.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<List<Comment>> Comments(Guid eventId);
        Task<bool> AddComment(Comment entity);
        Task<bool> DeleteComment(Comment entity);
    }
}
