﻿using SocialNetwork.Core.Application.Interfaces.Repositories;
using SocialNetwork.Core.Domain.Entities;
using SocialNetwork.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace SocialNetwork.Infrastructure.Persistence.Repositories
{
    public class PostRepository : GenericRepository<Posts>, IPostsRepository
    {
        private readonly ApplicationContext _dbContext;

        public PostRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
