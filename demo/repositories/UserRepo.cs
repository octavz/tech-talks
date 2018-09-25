using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using FP.Common;
using Npgsql;

namespace FP.Demo.Repositories {
  using static Helpers;

  static class RepoHelpers {

    public static User ToUser (this DbDataReader reader) {
      var dic = Enumerable.Range (0, reader.FieldCount).ToDictionary (reader.GetName, x => reader.GetValue (x) as String);
      return new User {
        Id = dic["user_id"],
          Email = dic["user_email"],
          Password = dic["user_password"]
      };
    }

    public static T RunDb<T> (this Env e, Func<NpgsqlCommand, T> f) {
      using (var conn = new NpgsqlConnection (e.ConnectionString))
      using (var cmd = new NpgsqlCommand ()) {
        conn.Open ();
        cmd.Connection = conn;
        var ret = f (cmd);
        // Console.WriteLine($"Trace: {f.Method.Name} {cmd.CommandText}");
        // cmd.Parameters.ToList().ForEach(p => Console.WriteLine(p.ParameterName + " = " + p.NpgsqlValue));
        return ret;
      }
    }
  }

  class UserRepo : IUserRepo {

    public Reader<Env, Maybe<User>> GetById (string id) => e => e.RunDb (cmd => {
      cmd.CommandText = "select * from users where user_id = @id";
      cmd.Parameters.AddWithValue ("id", id);
      using (var reader = cmd.ExecuteReader ()) 
        return reader.Read() ? reader.ToUser ().ToJust () : Nothing<User>();
    });

    public Reader<Env, Maybe<User>> GetByEmail (string email) => e => e.RunDb (cmd => {
      cmd.CommandText = "select * from users where user_email = @email";
      cmd.Parameters.AddWithValue ("email", email);
      using (var reader = cmd.ExecuteReader ()) 
        return reader.Read() ? reader.ToUser ().ToJust () : Nothing<User>();
    });

    public Reader<Env, User> Update (string id, string email, string password) => e => e.RunDb (cmd => {
      cmd.CommandText = "update users set user_email=@email, user_password=@pass where user_id=@id";
      cmd.Parameters.AddWithValue ("id", id);
      cmd.Parameters.AddWithValue ("email", email);
      cmd.Parameters.AddWithValue ("pass", password);
      cmd.ExecuteNonQuery ();
      return new User { Id = id, Email = email, Password = password };
    });

    public Reader<Env, User> Create (string email, string password) => e => e.RunDb (cmd => {
      var userId = Guid.NewGuid ().ToString ();
      cmd.CommandText = "insert into users(user_id, user_email, user_password) values(@id, @email, @pass)";
      cmd.Parameters.AddWithValue ("id", userId);
      cmd.Parameters.AddWithValue ("email", email);
      cmd.Parameters.AddWithValue ("pass", password);
      cmd.ExecuteNonQuery ();

      return new User { Id = userId, Email = email, Password = password };
    });

  }
}