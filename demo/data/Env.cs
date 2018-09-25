using Npgsql;
using FP.Common;
using FP.Demo.Repositories;
using System;

namespace FP.Demo
{
  public class Env {

    public IUserRepo UserRepo { get; set;}


    public String ConnectionString { get; set;}

  }

}