using System;
using System.Collections;
using Project.Scripts.Utility;
using UnityEngine;

namespace Project.Scripts
{
    public class CustomTimer
    {
        public event Action OnTimerEnded;
        
        private readonly MonoBehaviour _coroutineRunner;
        private Coroutine _currentCoroutine;
        
        private readonly ReactiveVariable<float> _elapsedTime;
        private readonly ReactiveVariable<bool> _isRunning;
        
        public float Duration { get; }
        public IReadOnlyVariable<float> ElapsedTime => _elapsedTime;
        public IReadOnlyVariable<bool> IsRunning => _isRunning;
        
        public bool IsTimerPaused() => IsRunning.Value == false && _currentCoroutine is not null;

        public CustomTimer(float duration, MonoBehaviour coroutineRunner)
        {
            Duration = duration;
            _coroutineRunner = coroutineRunner;
            
            _elapsedTime = new ReactiveVariable<float>();
            _isRunning = new ReactiveVariable<bool>();
        }

        public void Reset()
        {
            _elapsedTime.Value = 0;
            _isRunning.Value = false;
            
            if (_currentCoroutine != null)
                _coroutineRunner.StopCoroutine(_currentCoroutine);
        }

        public void Start()
        {
            _isRunning.Value = true;
            _currentCoroutine = _coroutineRunner.StartCoroutine(RunRoutine());
        }

        public void Pause() => _isRunning.Value = false;

        public void Continue() => _isRunning.Value = true;

        private IEnumerator RunRoutine()
        {
            while (ElapsedTime.Value < Duration)
            {
                if (IsRunning.Value)
                    _elapsedTime.Value += Time.deltaTime;
                
                yield return null;
            }
            
            _isRunning.Value = false;
            _currentCoroutine = null;
            OnTimerEnded?.Invoke();
        }
    }
}
