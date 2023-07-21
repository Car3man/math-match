using MathMatch.Game.Behaviours;
using MathMatch.Game.Pools;
using UnityEngine;

namespace MathMatch.Game.Spawners
{
    public class PlayerSpawner
    {
        private readonly PlayerPool _pool;
        private readonly PlayerDesintegrationEffectSpawner _despawnEffectSpawner;
        
        public Player Player { get; private set; }
        
        public PlayerSpawner(PlayerPool pool, PlayerDesintegrationEffectSpawner despawnEffectSpawner)
        {
            _pool = pool;
            _despawnEffectSpawner = despawnEffectSpawner;
        }

        public Player Spawn(Vector3 position)
        {
            if (Player)
            {
                throw new System.Exception("Cannot spawn more player instances than one.");
            }
            Player = _pool.Spawn();
            Player.transform.position = position;
            return Player;
        }

        public void Despawn(bool withEffect)
        {
            if (withEffect)
            {
                _despawnEffectSpawner.Spawn(Player.transform.position);
            }
            _pool.Despawn(Player);
            Player = null;
        }
    }
}