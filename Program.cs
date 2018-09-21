using System;

namespace FP {
    using static FP.Maybe;

    class Program {

        static void Main (string[] args) {
            TestMaybe ();
            TestReader ();
        }

        static void TestMaybe () {
            var r1 =
                from a in Just (0)
            from b in Just (1)
            from c in Just (2)
            from d in Just (3)
            from e in Just (36)
            select a + b + c + d + e;
            Console.WriteLine (r1.Value);

            var r2 = from a in Just ("a")
            from b in Just ("b")
            from c in Nothing<string> ()
            from d in Just ("c")

            select a + b + c + d;
            switch (r2) {
                case Nothing<string> v:
                    Console.WriteLine ("No value");
                    break;
                case Just<string> v:
                    Console.WriteLine (v.Value);
                    break;
            }
        }

        class Env {

            public Env (String connString, String urlToCall, Func<double, double, double> mathOperation) {
                ConnString = connString;
                UrlToCall = urlToCall;
                MathOperation = mathOperation;
            }

            public String ConnString { get; }
            public String UrlToCall { get; }

            public Func<double, double, double> MathOperation { get; }

        }

        static void TestReader () {
            Reader<Env, (int, string, double)> program =
                from count in CountStuffInDatabase ()
                from request in MakeHTTPRequest ()
                from result in DoMath (23, 19)
                select (count, request, result);

            var envTest = new Env ("connectionString-test", "http://test.example.com", (x, y) => x + y);
            Console.WriteLine (program (envTest));

            var envProd = new Env ("connectionString-prod", "http://www.example.com", (x, y) => x * y);
            Console.WriteLine (program (envProd));
        }


        static Reader<Env, int> CountStuffInDatabase () => e => {
            Console.WriteLine ($"Making a connection to db using {e.ConnString}");
            return 42;
        };

        static Reader<Env, string> MakeHTTPRequest () => e => {
            Console.WriteLine ($"Making a http request at {e.UrlToCall}");
            return "<html>body</html>";
        };

        static Reader<Env, double> DoMath (double x, double y) => e => e.MathOperation (x, y);

    }

}