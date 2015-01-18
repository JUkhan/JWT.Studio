using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.Core
{
    public class Fluently
    {
        public static Fluently<T> With<T>(T obj)
        {
            return Fluently<T>.With(obj);
        }

        public static Fluently With<T>(Func<T> f)
        {
            return Fluently<T>.With(f());
        }
    }

    public class Fluently<T> : Fluently
    {
        private readonly T _obj;
        private Fluently(T obj)
        {
            _obj = obj;
        }

        public static new Fluently<T2> With<T2>(T2 obj)
        {
            return new Fluently<T2>(obj);
        }

        public Fluently<T2> Cast<T2>() where T2 : class
        {
            return new Fluently<T2>(_obj as T2);
        }

        public Fluently<T> Do(Action<T> action)
        {
            action(_obj);
            return this;
        }

        public T Done()
        {
            return _obj;
        }
    }
    public static class ExtMethods
    {
        public static TResult With<TSource, TResult>(this TSource source, Func<TSource, TResult> action) where TSource : class
        {
            if (source != default(TSource)) return action(source);
            return default(TResult);
        }
        public static TSource If<TSource>(this TSource source, Func<TSource, bool> condition) where TSource : class
        {
            if (source != default(TSource) && condition(source)) return source;
            return default(TSource);
        }
        public static TSource Do<TSource>(this TSource source, Action<TSource> action) where TSource : class
        {
            if (source != default(TSource)) { action(source); return source; }
            return default(TSource);
        }
        public static TSource Log<TSource>(this TSource source, String msg) where TSource : class
        {
            if (source != default(TSource))
                Console.WriteLine(msg);
            return source;
        }
        public static TSource Log2<TSource>(this TSource source, Func<TSource, string> msgAction) where TSource : class
        {
            if (source != default(TSource)) Console.WriteLine(msgAction(source));
            return source;
        }
    }

}
