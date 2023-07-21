using MathMatch.Game.Behaviours;
using Zenject;

namespace MathMatch.Game.Pools
{
    public class CubePool : MonoMemoryPool<Cube>
    {
        protected override void Reinitialize(Cube item)
        {
            item.ResetItem();
        }
    }
}
