using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwtex
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
}
