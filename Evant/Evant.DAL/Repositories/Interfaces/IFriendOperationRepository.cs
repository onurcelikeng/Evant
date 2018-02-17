using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evant.DAL.Repositories.Interfaces
{
    public interface IFriendOperationRepository : IRepository<FriendOperation>
    {
        Task<List<FriendOperation>> Followers(Guid userId);
        Task<List<FriendOperation>> Followings(Guid userId);
    }
}
