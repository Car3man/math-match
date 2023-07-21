using MathMatch.Game.Behaviours;
using Zenject;

namespace MathMatch.Game.Pools
{
    public class CylinderPool : MonoMemoryPool<Cylinder>
    {
        protected override void Reinitialize(Cylinder item)
        {
            item.ResetItem();
        }
    }
}
