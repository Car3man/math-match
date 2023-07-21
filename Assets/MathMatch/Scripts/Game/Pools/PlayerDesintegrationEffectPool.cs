using MathMatch.Game.Behaviours;
using Zenject;

namespace MathMatch.Game.Pools
{
    public class PlayerDesintegrationEffectPool : MonoMemoryPool<PlayerDesintegrationEffect>
    {
        protected override void Reinitialize(PlayerDesintegrationEffect item)
        {
            item.ResetItem();
        }
    }
}