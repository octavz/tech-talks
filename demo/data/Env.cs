using Npgsql;
using FP.Common;
using FP.Demo.Repositories;

namespace FP.Demo
{
  public class Env {

    public IUserRepo UserRepo { get; }

    public NpgsqlConnection Connection { get; }
  }

}