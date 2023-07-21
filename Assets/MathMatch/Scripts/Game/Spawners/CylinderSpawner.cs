using MathMatch.Game.Behaviours;
using MathMatch.Game.Pools;
using UnityEngine;

namespace MathMatch.Game.Spawners
{
    public class CylinderSpawner
    {
        private readonly CylinderPool _pool;

        public CylinderSpawner(CylinderPool pool)
        {
            _pool = pool;
        }
        
        public Cylinder Spawn(Vector3 position)
        {
            var cylinder = _pool.Spawn();
            cylinder.transform.position = position;
            return cylinder;
        }

        public void Despawn(Cylinder cylinder)
        {
            _pool.Despawn(cylinder);
        }
    }
}