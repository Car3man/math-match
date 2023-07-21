using MathMatch.Game.Behaviours;
using Zenject;

namespace MathMatch.Game.Pools
{
    public class PlayerPool : MonoMemoryPool<Player>
    {
        protected override void Reinitialize(Player item)
        {
            item.ResetItem();
        }
    }
}