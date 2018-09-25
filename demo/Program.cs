using System;
using System.Collections.Generic;
using FP.Common;
using FP.Demo.Service;
using FP.Demo.Repositories;

namespace FP.Demo {
    using static Helpers;
    using static UserService;

    class Program {
        private static Env env = new Env {
            ConnectionString = "Host=localhost;Username=postgres;Password=password;Database=fp",
            UserRepo = new Repositories.UserRepo ()
        };

        static void Main (string[] args) {
            var (users, loginOk, loginFail) = App(env);
            Console.WriteLine(loginOk);
            Console.WriteLine(loginFail);
            users.ForEach (u => Console.WriteLine (u.ToString()));
        }

        static Reader<Env, (List<User>, bool, bool)> App =>
            from lst in createUsers () 
            from ok in LogIn("email1@example.com", "pass")
            from notOk in LogIn("email2@example.com", "pass")
            select (lst, ok, notOk);

        static Reader<Env, List<User>> createUsers () =>
            from x1 in CreateOrUpdateUser ("email1@example.com", "pass")
            from u1 in GetUserById (x1.Id)
            from x2 in CreateOrUpdateUser ("email2@example.com", "pass")
            from u2 in GetUserById (x2.Id)
            from x3 in CreateOrUpdateUser ("email2@example.com", "another pass")
            from u3 in GetUserById (x3.Id)
            select new List<User> { u1.Value, u2.Value, u3.Value };
    }

}