using MathMatch.Game.Behaviours;
using MathMatch.Game.Pools;
using UnityEngine;

namespace MathMatch.Game.Spawners
{
    public class PlayerDesintegrationEffectSpawner
    {
        private readonly PlayerDesintegrationEffectPool _pool;

        public PlayerDesintegrationEffectSpawner(PlayerDesintegrationEffectPool pool)
        {
            _pool = pool;
        }

        public PlayerDesintegrationEffect Spawn(Vector3 position)
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