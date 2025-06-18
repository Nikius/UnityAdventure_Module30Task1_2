using System;

namespace Project.Scripts.Utility
{
    public interface IReadOnlyVariable<T>
    {
        event Action<T, T> Updated;

        T Value { get; }
    }
}