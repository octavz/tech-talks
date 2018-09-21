namespace FP.Common
{
  public static class Helpers {
        public static Maybe<A> Just<A> (A a) => new Just<A> (a);
        public static Maybe<A> Nothing<A> () => new Nothing<A> (default (A));

        public static Maybe<A> ToJust<A> (this A a) => new Just<A> (a);
        public static Reader<E, E> Ask<E> () => new Reader<E, E> (e => e);
    }
}