using MathMatch.Game.Behaviours;
using MathMatch.Game.Pools;
using UnityEngine;

namespace MathMatch.Game.Spawners
{
    public class CubeSpawner
    {
        private readonly CubePool _pool;

        public CubeSpawner(CubePool pool)
        {
            _pool = pool;
        }
        
        public Cube Spawn(Vector3 position)
        {
            var cube = _pool.Spawn();
            cube.transform.position = position;
            return cube;
        }

        public void Despawn(Cube cube)
        {
            _pool.Despawn(cube);
        }
    }
}