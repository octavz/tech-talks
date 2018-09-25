using System;
using FP.Common;
using FP.Demo.Repositories;
using Xunit;

namespace FP.Demo.Service {
    using static Helpers;

    public class StubUserRepo : IUserRepo {
        private User user;
        public StubUserRepo(User u) => this.user = u; 
        public Reader<Env, User> Create (string email, string password) => e => user;
        public Reader<Env, User> Update (string id, string email, string password) => e => user;
        public Reader<Env, Maybe<User>> GetByEmail (string email) => e => Just (user);
        public Reader<Env, Maybe<User>> GetById (string id) => e => Just (user);
    }

    public class TestService {
        private User emptyUser = new User { Id = "id", Email = "email", Password = "pass" };

        [Fact]
        public void TestGetUser () {
            var defaultEnv = new Env { ConnectionString = "", UserRepo = new StubUserRepo (emptyUser) };
            var result = UserService.CreateOrUpdateUser ("email", "password") (defaultEnv);
            Assert.True (result.Id == "id");
            Assert.True (result.Email == "email");
            Assert.True (result.Password == "pass");
        }

        [Fact]
        public void TestLoginFail () {
            var defaultEnv = new Env { ConnectionString = "", UserRepo = new StubUserRepo (emptyUser) };
            var result = UserService.LogIn ("email", "fail") (defaultEnv);
            Assert.False (result);
        }

    }
}