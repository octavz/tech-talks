using System;

namespace FP {
    using static Maybe;

    public abstract class Maybe<T> {
        // public static implicit operator Maybe<T> (Nothing nothing) => new Nothing<T> (default (T));

        virtual public T Value => throw new NotImplementedException ();
    }

    public class Just<A> : Maybe<A> {

        public Just (A value)  => this.Value = value;

        public override A Value { get; }
    }

    internal class Nothing<A> : Maybe<A> {
        public Nothing (A value) { }
    }

    public static class Maybe {
        public static Maybe<A> Just<A> (A a) => new Just<A> (a);
        public static Maybe<A> Nothing<A> () => new Nothing<A> (default (A));
    }

    public static class MaybeExtensions {

        public static Maybe<B> Select<A, B> (this Maybe<A> maybeA, Func<A, B> fab) =>
            (maybeA is Nothing<A>) ? Maybe.Nothing<B> () : Maybe.Just (fab (maybeA.Value));

        public static Maybe<C> SelectMany1<A, B, C> (this Maybe<A> maybeA, Func<A, Maybe<B>> f, Func<A, B, C> projection) {

            if (maybeA is Nothing<A>) return Maybe.Nothing<C> ();
            else {
                Maybe<B> maybeB = f (maybeA.Value);
                if (maybeB is Nothing<B>) return Maybe.Nothing<C> ();
                else return Maybe.Just (projection (maybeA.Value, maybeB.Value));
            }
        }

        public static Maybe<C> SelectMany<A, B, C> (this Maybe<A> maybeA, Func<A, Maybe<B>> f, Func<A, B, C> projection) {
            switch(maybeA) {
                case Nothing<A> _: return Maybe.Nothing<C>();
                case Just<A> _:
                    switch(f (maybeA.Value)){
                        case Nothing<B> _ : return Maybe.Nothing<C>();
                        case Just<B> maybeB : return Maybe.Just (projection (maybeA.Value, maybeB.Value));
                        default: throw new NotImplementedException();
                    }
                default: throw new NotImplementedException();
            }
        }

    }
}