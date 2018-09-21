using System;
using FP.Common;

namespace FP.Demo.Repositories
{
  public interface IUserRepo {
    Reader<Env, Maybe<User>> GetById (string id);
    Reader<Env, Maybe<User>> GetByEmail (string id);
    Reader<Env, User> Create (Maybe<String> id, string email, string password);
  }

}