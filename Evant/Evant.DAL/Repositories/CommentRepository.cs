﻿using Evant.DAL.EF;
using Evant.DAL.EF.Tables;
using Evant.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evant.DAL.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(DataContext dbContext) : base(dbContext)
        {

        }


        public async Task<List<Comment>> Comments(Guid eventId)
        {
            return await Table
                .Include(t => t.User)
                .Where(t => t.EventId == eventId)
                .ToListAsync();
        }

    }
}