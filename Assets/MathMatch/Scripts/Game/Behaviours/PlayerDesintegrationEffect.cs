using System;
using Cysharp.Threading.Tasks;
using MathMatch.Game.Interfaces;
using UnityEngine;

namespace MathMatch.Game.Behaviours
{
    public class PlayerDesintegrationEffect : MonoBehaviour, IPoolItem
    {
        [SerializeField] private ParticleSystem effectParticles;

        public async void Play(Action<PlayerDesintegrationEffect> onEnd)
        {
            effectParticles.Play(true);
            await UniTask.WaitUntil(
                () => !effectParticles.isPlaying, 
                PlayerLoopTiming.Update,
                effectParticles.GetCancellationTokenOnDestroy()
            );
            onEnd?.Invoke(this);
        }
        
        public void ResetItem()
        {
            effectParticles.Stop(true);
        }
    }
}
