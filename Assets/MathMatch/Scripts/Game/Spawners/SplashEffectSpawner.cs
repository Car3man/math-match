using MathMatch.Game.Behaviours;
using MathMatch.Game.Pools;
using UnityEngine;

namespace MathMatch.Game.Spawners
{
    public class SplashEffectSpawner
    {
        private readonly SplashEffectPool _pool;

        public SplashEffectSpawner(SplashEffectPool pool)
        {
            _pool = pool;
        }

        public SplashEffect Spawn(Vector3 position)
        {
            var instance = _pool.Spawn();
            var instanceTrans = instance.transform;
            instanceTrans.position = position;
            instance.Play(effect =>
            {
                _pool.Despawn(effect);
            });
            return instance;
        }
    }
}