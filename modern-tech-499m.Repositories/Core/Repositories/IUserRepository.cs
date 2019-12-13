﻿using System.Collections.Generic;
using modern_tech_499m.Repositories.Core.Domain;

namespace modern_tech_499m.Repositories.Core.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> GetUserFromGame(int id);
    }
}
