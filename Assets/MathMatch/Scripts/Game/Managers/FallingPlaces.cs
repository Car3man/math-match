using MathMatch.Game.Interfaces;
using UnityEngine;
using Zenject;

namespace MathMatch.Game.Managers
{
    public class FallingPlaces : ITickable
    {
        private readonly LevelGenerator _levelGenerator;

        private bool _fallingStarted;
        private IPlace _currPlaceToFall;
        private float _currFallTime;

        public delegate void PlaceFallStartDelegate(IPlace place);
        public event PlaceFallStartDelegate OnPlaceFallStart;
        
        public delegate void OnPlaceFallUpdateDelegate(IPlace place, float fallValue);
        public event OnPlaceFallUpdateDelegate OnPlaceFallUpdate;
        
        public delegate void PlaceFallEndDelegate(IPlace place);
        public event PlaceFallEndDelegate OnPlaceFallEnd;
        
        private const float PlaceFallDuration = 0.6f;

        public FallingPlaces(LevelGenerator levelGenerator)
        {
            _levelGenerator = levelGenerator;
        }

        public void StartFall()
        {
            if (_fallingStarted)
            {
                Debug.LogWarning("FallingPlaces already started.");
                return;
            }
            
            _fallingStarted = true;
            _currPlaceToFall = _levelGenerator.Segments[0].Start;
            _currFallTime = PlaceFallDuration;
            OnPlaceFallStart?.Invoke(_currPlaceToFall);
        }
        
        public void StopFall()
        {
            if (!_fallingStarted)
            {
                Debug.LogWarning("FallingPlaces didn't started.");
                return;
            }
            
            _fallingStarted = false;
        }

        public void Tick()
        {
            if (!_fallingStarted)
            {
                return;
            }
            
            _currFallTime -= Time.deltaTime;
            _currPlaceToFall.SetFallValue(1f - _currFallTime / PlaceFallDuration);
            OnPlaceFallUpdate?.Invoke(_currPlaceToFall, _currPlaceToFall.FallValue);
                
            if (_currFallTime <= 0f)
            {
                OnPlaceFallEnd?.Invoke(_currPlaceToFall);
                _levelGenerator.HidePlace(_currPlaceToFall);
                
                _currPlaceToFall = _levelGenerator.GetNextPlace(_currPlaceToFall);
                _currFallTime = PlaceFallDuration;
                OnPlaceFallStart?.Invoke(_currPlaceToFall);
            }
        }
    }
}
