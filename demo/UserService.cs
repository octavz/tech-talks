using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Npgsql;
using FP.Common;

namespace FP.Demo {
  using static Helpers;

  public class UserService {

    private static bool checkPass (Maybe<User> u, string password) {
      switch (u) {
        case Just<User> _:
          return u.Value.Password == password;
        default:
          return false;
      }
    }

    public static Reader<Env, Boolean> LogIn (string email, string password) => from e in Ask<Env> ()
    from u in e.UserRepo.GetByEmail (email)
    select checkPass (u, password);

    public static Reader<Env, User> CreateUser (string email, string password) => from e in Ask<Env> ()
    from u in e.UserRepo.Create (Nothing<String> (), email, password)
    select u;

    public static Reader<Env, List<User>> CreateAndGetUsers () => from e in Ask<Env> ()
    from x1 in e.UserRepo.Create (Nothing<String> (), "email1@example.com", "password")
    from u1 in e.UserRepo.GetById (x1.Id)
    from x2 in e.UserRepo.Create (Nothing<String> (), "email2@example.com", "password")
    from u2 in e.UserRepo.GetById (x2.Id)
    select new List<User> { u1.Value, u2.Value };

  }

}