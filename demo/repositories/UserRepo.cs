using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Npgsql;
using FP.Common;

namespace FP.Demo.Repositories {
  using static Helpers;

  class UserRepo : IUserRepo {

    private Dictionary<String, String> reader2dic (DbDataReader reader) =>
      Enumerable.Range (0, reader.FieldCount).ToDictionary (reader.GetName, x => (string) reader.GetValue (x));

    public User reader2user (DbDataReader reader) {
      var dic = reader2dic (reader);
      return new User {
        Id = dic["user_id"],
          Email = dic["user_email"],
          Password = dic["user_password"]
      };
    }

    public Reader<Env, Maybe<User>> GetById (string id) => e => {
      var sql = "select from users where user_id = @id";
      using (var cmd = new NpgsqlCommand (sql, e.Connection)) {
        cmd.Parameters.AddWithValue ("id", id);
        using (var reader = cmd.ExecuteReader ()) {
          if (!reader.Read ()) return Nothing<User> ();
          return reader2user (reader).ToJust ();
        }
      }
    };

    public Reader<Env, Maybe<User>> GetByEmail (string email) => e => {
      var sql = "select from users where user_email = @email";
      using (var cmd = new NpgsqlCommand (sql, e.Connection)) {
        cmd.Parameters.AddWithValue ("email", email);
        using (var reader = cmd.ExecuteReader ()) {
          if (!reader.Read ()) return Nothing<User> ();
          return reader2user (reader).ToJust ();
        }
      }
    };

    public Reader<Env, User> Create (Maybe<String> id, string email, string password) => e => {
      var sql = "insert into users(user_id, user_email, user_password) values((@id, @email, @password))";
      using (var cmd = new NpgsqlCommand (sql, e.Connection)) {
        var userId = id.GetOrElse (Guid.NewGuid ().ToString ());
        cmd.Parameters.AddWithValue ("id", userId);
        cmd.Parameters.AddWithValue ("email", email);
        cmd.Parameters.AddWithValue ("password", password);
        cmd.ExecuteNonQuery ();
        return new User { Id = userId, Email = email, Password = password };
      }
    };

  }

}