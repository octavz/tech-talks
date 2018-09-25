using System;

namespace FP.Common {

    public delegate A Reader<E, A> (E e);

    public static class ReaderExtensions {

        public static Reader<E, A> Pure<E,A>(this A v) => new Reader<E,A>(e => v);

        // m a -> (a -> m b) -> m b
        public static Reader<E, C> SelectMany<E, A, B, C> (
            this Reader<E, A> readerA,
            Func<A, Reader<E, B>> f,
            Func<A, B, C> projection) {

            Func<E, C> fc = e => {
                A a = readerA (e);
                Reader<E, B> readerB = f (a);
                B b = readerB (e);
                return projection (a, b);
            };

            return new Reader<E, C> (fc);
        }

        public static Reader<E, B> Select<E, A, B> (this Reader<E, A> readerA, Func<A, B> f) {
            return new Reader<E, B> (e => f (readerA (e)));
        }

    }
}