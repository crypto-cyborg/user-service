﻿using UserService.Core.Models;
using UserService.Persistence.Contexts;

namespace UserService.Persistence.Repositories;

public class UserRepository(UserDbContext context) : RepositoryBase<User>(context) { }