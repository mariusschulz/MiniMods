using System;
using System.Collections.Generic;

namespace MiniMod.Optional
{
    public class Optional
    {
        public static readonly Optional Empty = new Optional();

        private Optional()
        {

        }

        public static Optional<T> Create<T>(T value)
        {
            return new Optional<T>(value);
        }

        public static Optional<T> FirstWithValue<T>(params Optional<T>[] optionals)
        {
            foreach (var optional in optionals)
            {
                if (optional.HasValue)
                    return optional;
            }

            return Empty;
        }
    }

    public struct Optional<T> : IEquatable<Optional<T>>
    {
        private static readonly Optional<T> Empty = new Optional<T>();

        private readonly T _value;
        private readonly bool _hasValue;

        public T Value
        {
            get
            {
                if (HasValue)
                    return _value;

                throw new InvalidOperationException("No value was set for this Optional");
            }
        }

        public bool HasValue
        {
            get { return _hasValue; }
        }

        public Optional(T value)
            : this()
        {
            _value = value;
            _hasValue = true;
        }

        public Optional<T> Or(Optional<T> other)
        {
            return HasValue ? this : other;
        }

        public T OrDefault(T defaultValue)
        {
            return HasValue ? _value : defaultValue;
        }

        public bool Equals(Optional<T> other)
        {
            if (HasValue != other.HasValue)
                return false;

            if (HasValue)
                return true;

            return EqualityComparer<T>.Default.Equals(_value, other._value);
        }

        public override bool Equals(object other)
        {
            return other is Optional<T>
                && Equals((Optional<T>)other);
        }

        public override int GetHashCode()
        {
            if (!HasValue)
                return 0;

            return EqualityComparer<T>.Default.GetHashCode(_value);
        }

        public static implicit operator Optional<T>(T value)
        {
            return new Optional<T>(value);
        }

        public static implicit operator Optional<T>(Optional optional)
        {
            return Empty;
        }

        public static bool operator ==(Optional<T> left, Optional<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Optional<T> left, Optional<T> right)
        {
            return !left.Equals(right);
        }
    }
}
