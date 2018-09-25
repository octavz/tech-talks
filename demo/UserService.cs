using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using FP.Common;
using Npgsql;

namespace FP.Demo.Service {
  using static Helpers;

  public static class UserService {

    public static Reader<Env, Boolean> LogIn (string email, string password) =>
      from e in Ask<Env> ()
      from u in e.UserRepo.GetByEmail (email)
      select (u is Just<User> && u.Value.Password == password);

    public static Reader<Env, User> CreateOrUpdateUser (string email, string password) =>
      from e in Ask<Env> ()
      from u in e.UserRepo.GetByEmail (email)
      from ret in (u is Nothing<User>) ? e.UserRepo.Create (email, password) : e.UserRepo.Update(u.Value.Id, email, password) 
      select ret;

    public static Reader<Env, Maybe<User>> GetUserById (string id) =>
      from e in Ask<Env> ()
      from u in e.UserRepo.GetById (id)
      select u;

  }

}