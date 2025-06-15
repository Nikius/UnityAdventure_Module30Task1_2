using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts
{
    public class TimersExample : MonoBehaviour
    {
        [SerializeField] private SliderTimerView _sliderTimerView;
        [SerializeField] private HeartsTimerView _heartsTimerView;
        
        [SerializeField] private float _timerDuration;

        private CustomTimer _sliderTimer;
        private CustomTimer _heartTimer;
        
        private CustomTimer _currentTimer;

        private void Awake()
        {
            _sliderTimer = new CustomTimer(_timerDuration, this);
            _sliderTimerView.Initialize(_sliderTimer);
            
            _heartTimer = new CustomTimer(_timerDuration, this);
            _heartsTimerView.Initialize(_heartTimer);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _currentTimer = _sliderTimer;
                Debug.Log("Slider Timer Selected");
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _currentTimer = _heartTimer;
                Debug.Log("Hearts Timer Selected");
            }
            
            if (_currentTimer == null)
                return;
            
            if (Input.GetKeyDown(KeyCode.S) && _currentTimer.IsRunning.Value == false)
                _currentTimer.Start();
            
            if (Input.GetKeyDown(KeyCode.P) && _currentTimer.IsRunning.Value)
                _currentTimer.Pause();
            
            if (Input.GetKeyDown(KeyCode.C) && _currentTimer.IsTimerPaused())
                _currentTimer.Continue();
            
            if (Input.GetKeyDown(KeyCode.R))
                _currentTimer.Reset();
        }
    }
}
