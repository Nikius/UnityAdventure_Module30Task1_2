using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts
{
    public class SliderTimerView : MonoBehaviour
    {
        [SerializeField] private Slider _slider;

        private CustomTimer _customTimer;

        private void OnDestroy()
        {
            _customTimer.ElapsedTimeUpdated -= OnElapsedTimeUpdated;
            _customTimer.StateUpdated -= OnStateUpdated;
            _customTimer.OnTimerEnded -= OnTimerEnded;
        }

        public void Initialize(CustomTimer customTimer)
        {
            _customTimer = customTimer;
            
            _customTimer.ElapsedTimeUpdated += OnElapsedTimeUpdated;
            _customTimer.StateUpdated += OnStateUpdated;
            _customTimer.OnTimerEnded += OnTimerEnded;
            
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
            _slider.value = 1;
            _slider.enabled = false;
        }

        private void Enable()
        {
            _slider.enabled = true;
        }

        private void Disable()
        {
            _slider.enabled = false;
        }

        private void UpdateView(float elapsedTime)
        {
            Debug.Log("SliderTimer Elapsed Time: " + elapsedTime);
            _slider.value = 1 - elapsedTime / _customTimer.Duration;
        }
    }
}
