using System;
using System.Collections;
using Project.Scripts.Utility;
using UnityEngine;

namespace Project.Scripts
{
    public class CustomTimer: IDisposable
    {
        public event Action<float, float> ElapsedTimeUpdated;
        public event Action<bool, bool> StateUpdated;
        public event Action OnTimerEnded;
        
        private readonly MonoBehaviour _coroutineRunner;
        private Coroutine _currentCoroutine;
        
        public float Duration { get; }
        public ReactiveVariable<float> ElapsedTime { get; }
        public ReactiveVariable<bool> IsRunning { get; }
        
        public bool IsTimerPaused() => IsRunning.Value == false && _currentCoroutine is not null;

        public CustomTimer(float duration, MonoBehaviour coroutineRunner)
        {
            Duration = duration;
            _coroutineRunner = coroutineRunner;
            
            ElapsedTime = new ReactiveVariable<float>();
            ElapsedTime.Updated += OnElapsedTimeUpdated;
            
            IsRunning = new ReactiveVariable<bool>();
            IsRunning.Updated += OnStateUpdated;
        }

        public void Dispose()
        {
            ElapsedTime.Updated -= OnElapsedTimeUpdated;
            IsRunning.Updated -= OnStateUpdated;
        }

        public void Reset()
        {
            ElapsedTime.Value = 0;
            IsRunning.Value = false;
            
            if (_currentCoroutine != null)
                _coroutineRunner.StopCoroutine(_currentCoroutine);
        }

        public void Start()
        {
            IsRunning.Value = true;
            _currentCoroutine = _coroutineRunner.StartCoroutine(RunRoutine());
        }

        public void Pause() => IsRunning.Value = false;

        public void Continue() => IsRunning.Value = true;

        private void OnStateUpdated(bool newValue, bool oldValue) => StateUpdated?.Invoke(newValue, oldValue);

        private void OnElapsedTimeUpdated(float newValue, float oldValue) => ElapsedTimeUpdated?.Invoke(newValue, oldValue);

        private IEnumerator RunRoutine()
        {
            while (ElapsedTime.Value < Duration)
            {
                if (IsRunning.Value)
                    ElapsedTime.Value += Time.deltaTime;
                
                yield return null;
            }
            
            IsRunning.Value = false;
            _currentCoroutine = null;
            OnTimerEnded?.Invoke();
        }
    }
}
