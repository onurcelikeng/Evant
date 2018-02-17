﻿using Evant.DAL.EF;
using Evant.DAL.EF.Tables;
using Evant.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Evant.DAL.Repositories
{
    public class FriendOperationRepository : Repository<FriendOperation>, IFriendOperationRepository
    {
        public FriendOperationRepository(DataContext dbContext) : base(dbContext)
        {

        }

        public async Task<List<FriendOperation>> List()
        {
            return await Table.Include(t => t.FollowerUser)
                              .Include(t => t.FollowingUser).ToListAsync();
        }


    }
}
