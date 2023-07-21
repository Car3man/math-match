using System.Collections.Generic;
using MathMatch.Game.Interfaces;
using MathMatch.Game.Spawners;
using UnityEngine;
using Zenject;

namespace MathMatch.Game.Managers
{
    public class SplashEffectSpawnController : ITickable
    {
        private readonly SplashEffectSpawner _splashEffectSpawner;
        private readonly LevelGenerator _levelGenerator;
        
        private bool _started;
        private float _lastEffectTime;

        private const float EffectsRate = 1f;

        public SplashEffectSpawnController(SplashEffectSpawner splashEffectSpawner, LevelGenerator levelGenerator)
        {
            _splashEffectSpawner = splashEffectSpawner;
            _levelGenerator = levelGenerator;
        }

        public void StartSpawn()
        {
            if (_started)
            {
                Debug.LogWarning("Splash effect spawn controller already started.");
                return;
            }
            
            _started = true;
        }

        public void StopSpawn()
        {
            if (!_started)
            {
                Debug.LogWarning("Splash effect spawn controller didn't started.");
                return;
            }
            
            _started = false;
        }

        public void Tick()
        {
            if (!_started)
            {
                return;
            }
            
            if (Time.time - _lastEffectTime >= 1f / EffectsRate)
            {
                SpawnSplashEffect();
                _lastEffectTime = Time.time;
            }
        }

        private void SpawnSplashEffect()
        {
            var places = new List<IPlace>();
            for (var i = 0; i < _levelGenerator.Segments.Count; i++)
            {
                places.Add(_levelGenerator.Segments[i].Start);
                places.AddRange(_levelGenerator.Segments[i].Cylinders);
                places.Add(_levelGenerator.Segments[i].End);
            }
            places.RemoveAll(x => x.IsFall);
            var rnd = Random.Range(0, places.Count);
            var place = places[rnd];
            _splashEffectSpawner.Spawn(place.Point + Vector3.up * 0.1f);
        }
    }
}