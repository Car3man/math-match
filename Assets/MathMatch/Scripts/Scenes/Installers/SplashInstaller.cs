using MathMatch.Game.Behaviours;
using MathMatch.Game.Pools;
using MathMatch.Game.Spawners;
using Zenject;

namespace MathMatch.Scenes.Installers
{
    public class SplashInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallPools();
            InstallSpawners();
        }
        
        private void InstallPools()
        {
            Container.BindMemoryPool<Cube, CubePool>()
                .WithInitialSize(8)
                .FromComponentInNewPrefabResource($"Prefabs/{nameof(Cube)}");
        }

        private void InstallSpawners()
        {
            Container.Bind<CubeSpawner>().FromNew().AsSingle();
        }
    }
}
