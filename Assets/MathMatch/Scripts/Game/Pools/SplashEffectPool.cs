using MathMatch.Game.Behaviours;
using Zenject;

namespace MathMatch.Game.Pools
{
    public class SplashEffectPool : MonoMemoryPool<SplashEffect>
    {
        protected override void Reinitialize(SplashEffect item)
        {
            item.ResetItem();
        }
    }
}