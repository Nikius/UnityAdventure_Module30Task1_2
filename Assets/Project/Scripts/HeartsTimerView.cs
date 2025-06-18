using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts
{
    public class HeartsTimerView : MonoBehaviour
    {
        private const float DisabledOpacity = 0.3f;
        
        [SerializeField] private Image _heartPrefab;
        [SerializeField] private GameObject _heartContainer;

        private CustomTimer _customTimer;
        
        private readonly List<Image> _hearts = new();
        private int _maxHeartsCount;

        private void OnDestroy()
        {
            _customTimer.ElapsedTime.Updated -= OnElapsedTimeUpdated;
            _customTimer.IsRunning.Updated -= OnStateUpdated;
            _customTimer.OnTimerEnded -= OnTimerEnded;
        }

        public void Initialize(CustomTimer customTimer)
        {
            _customTimer = customTimer;

            _customTimer.ElapsedTime.Updated += OnElapsedTimeUpdated;
            _customTimer.IsRunning.Updated += OnStateUpdated;
            _customTimer.OnTimerEnded += OnTimerEnded;
            
            _maxHeartsCount = Mathf.FloorToInt(_customTimer.Duration);
            Reset();
        }

        private void OnStateUpdated(bool newValue, bool oldValue)
        {
            if (newValue)
                Enable();
            else
                Disable();
        }

        private void OnElapsedTimeUpdated(float newValue, float oldValue)
        {
            if (newValue > 0)
            {
                Debug.Log("HeartsTimer Elapsed Time: " + newValue);
                UpdateView(newValue);
            }
            else
            {
                Debug.Log("HeartsTimer Reset");
                Reset();
            }
        }

        private void OnTimerEnded()
        {
            Debug.Log("Timer ended!");
        }

        private void Reset()
        {
            foreach (Image heart in _hearts)
                Destroy(heart.gameObject);
            
            _hearts.Clear();
            
            for (int i = 0; i < _maxHeartsCount; i++)
            {
                Image heart = Instantiate(_heartPrefab, _heartContainer.transform);
                _hearts.Add(heart);
            }
            
            Disable();
        }
        
        private void Enable()
        {
            for (int i = 0; i < _hearts.Count; i++)
            {
                Image heart = _hearts[i];
                
                Color color = heart.color;
                color.a = 1f;
                heart.color = color;
            }
        }

        private void Disable()
        {
            for (int i = 0; i < _hearts.Count; i++)
            {
                Image heart = _hearts[i];
                
                Color color = heart.color;
                color.a = DisabledOpacity;
                heart.color = color;
            }
        }

        private void UpdateView(float elapsedTime)
        {
            while (_hearts.Count > 0 && elapsedTime - (_maxHeartsCount - _hearts.Count) > 1)
            {
                Image heart = _hearts[^1]; 
                Destroy(heart.gameObject);
                _hearts.Remove(heart);
            }
        }
    }
}
