using System;

namespace FP {
    using static MaybeBuilders;

    public abstract class Maybe<A> {
        virtual public A Value =>
        throw new NotImplementedException ();

        public A GetOrElse(A d) => (this is Nothing<A>)  ? d : Value;
    }

    public class Just<A> : Maybe<A> {

        public Just (A value) => this.Value = value;

        override public A Value { get; }
    }

    public class Nothing<A> : Maybe<A> {
        public Nothing (A value) { }
    }

    public static class MaybeBuilders {
        public static Maybe<A> Just<A> (A a) => new Just<A> (a);
        public static Maybe<A> Nothing<A> () => new Nothing<A> (default (A));

        public static Maybe<A> ToJust<A> (this A a) => new Just<A> (a);
    }

    public static class MaybeExtensions {

        public static Maybe<B> Select<A, B> (this Maybe<A> maybeA, Func<A, B> fab) =>
            (maybeA is Nothing<A>) ? Nothing<B> () : fab (maybeA.Value).ToJust ();

        public static Maybe<C> SelectMany<A, B, C> (this Maybe<A> ma, Func<A, Maybe<B>> f, Func<A, B, C> select) {
            switch (ma) {
                case Just<A> justA:
                    switch (f (justA.Value)) {
                        case Just<B> justB:
                            return select (justA.Value, justB.Value).ToJust ();
                        default:
                            return Nothing<C> ();
                    }
                default:
                    return Nothing<C> ();
            }
        }

    }
}