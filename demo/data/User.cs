using System;
using FP.Common;

namespace FP.Demo
{
  public class User {

    public String Id { get; set; }
    public String Email { get; set; }
    public String Password { get; set; }

    public override string ToString(){
        return $"{Id} - {Email} - {Password}";
    }
  }

}