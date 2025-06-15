using System;

namespace Project.Scripts.Utility
{
    public class ReactiveVariable<T> where T : IEquatable<T> 
    {
        public event Action<T, T> Updated;
        
        private T _value;

        public ReactiveVariable() => _value = default;
        
        public ReactiveVariable(T value) => _value = value;

        public T Value
        {
            get => _value;

            set
            {
                T oldValue = _value;
                _value = value;
                
                if (_value.Equals(oldValue) == false)
                    Updated?.Invoke(_value, oldValue);
            }
        }
    }
}