﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace reserby.buildingBlocks.maybe
{
    public struct Maybe<T>
    {
        private readonly IEnumerable<T> _values;

        public static Maybe<T> Some(T value)
        {
            if (value == null)
                throw new InvalidOperationException();

            return new Maybe<T>(new[] {value});
        }

        public static Maybe<T> None => new Maybe<T>(new T[0]);

        private Maybe(IEnumerable<T> values)
        {
            _values = values;
        }

        public bool HasValue => _values != null && _values.Any();

        public T Value
        {
            get
            {
                if (!HasValue)
                    throw new InvalidOperationException("Maybe does not have a value");

                return _values.Single();
            }
        }

        public T ValueOrDefault(T @default)
        {
            if (!HasValue)
                return @default;
            return _values.Single();
        }

        public T ValueOrThrow(Exception e)
        {
            if (HasValue)
                return Value;
            throw e;
        }

        public U Case<U>(Func<T, U> some, Func<U> none)
        {
            return HasValue
                ? some(Value)
                : none();
        }

        public void Case(Action<T> some, Action none)
        {
            if (HasValue)
                some(Value);
            else
                none();
        }

        public void IfSome(Action<T> some)
        {
            if (HasValue)
                some(Value);
        }

        public Maybe<U> Map<U>(Func<T, Maybe<U>> map)
        {
            return HasValue
                ? map(Value)
                : Maybe<U>.None;
        }

        public Maybe<U> Map<U>(Func<T, U> map)
        {
            return HasValue
                ? Maybe.Some(map(Value))
                : Maybe<U>.None;
        }
    }

    public static class Maybe
    {
        public static Maybe<T> Some<T>(T value)
        {
            return Maybe<T>.Some(value);
        }
    }
}