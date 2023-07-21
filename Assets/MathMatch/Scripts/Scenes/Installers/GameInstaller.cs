using MathMatch.Game.Behaviours;
using MathMatch.Game.Managers;
using MathMatch.Game.Pools;
using MathMatch.Game.Spawners;
using NubikTowerBuilding.Ui;
using UnityEngine;
using Zenject;

namespace MathMatch.Scenes.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private SubSceneManager subSceneManager;
        [SerializeField] private UiInputTrigger playerUiTrigger;
        [SerializeField] private ScoreText scoreText;
        
        public override void InstallBindings()
        {
            InstallCamera();
            InstallSubScenes();
            InstallUiTrigger();
            InstallScoreText();
            InstallPools();
            InstallSpawners();
            InstallGameManagers();
        }

        private void InstallCamera()
        {
            Container.Bind<Camera>().FromInstance(Camera.main).AsSingle();
        }

        private void InstallSubScenes()
        {
            Container.Bind<SubSceneManager>().FromInstance(subSceneManager).AsSingle();
        }

        private void InstallUiTrigger()
        {
            Container.Bind<UiInputTrigger>().FromInstance(playerUiTrigger).AsSingle();
        }

        private void InstallScoreText()
        {
            Container.Bind<ScoreText>().FromInstance(scoreText).AsSingle();
        }

        private void InstallPools()
        {
            Container.BindMemoryPool<Cube, CubePool>()
                .WithInitialSize(8)
                .FromComponentInNewPrefabResource($"Prefabs/{nameof(Cube)}");
            Container.BindMemoryPool<Cylinder, CylinderPool>()
                .WithInitialSize(24)
                .FromComponentInNewPrefabResource($"Prefabs/{nameof(Cylinder)}");
            Container.BindMemoryPool<Player, PlayerPool>()
                .WithInitialSize(1)
                .FromComponentInNewPrefabResource($"Prefabs/{nameof(Player)}");
            Container.BindMemoryPool<SplashEffect, SplashEffectPool>()
                .WithInitialSize(6)
                .FromComponentInNewPrefabResource($"Prefabs/{nameof(SplashEffect)}");
            Container.BindMemoryPool<PlayerDesintegrationEffect, PlayerDesintegrationEffectPool>()
                .WithInitialSize(1)
                .FromComponentInNewPrefabResource($"Prefabs/{nameof(PlayerDesintegrationEffect)}");
        }

        private void InstallSpawners()
        {
            Container.Bind<CubeSpawner>().FromNew().AsSingle();
            Container.Bind<CylinderSpawner>().FromNew().AsSingle();
            Container.Bind<PlayerSpawner>().FromNew().AsSingle();
            Container.Bind<SplashEffectSpawner>().FromNew().AsSingle();
            Container.Bind<PlayerDesintegrationEffectSpawner>().FromNew().AsSingle();
        }

        private void InstallGameManagers()
        {
            Container.Bind<LevelGenerator>().FromNew().AsSingle();
            Container.Bind<PlayerMoveController>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerCameraController>().FromNew().AsSingle();
            Container.Bind<ScoreController>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<FallingPlaces>().FromNew().AsSingle();
            Container.Bind<GameOverObserver>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<SplashEffectSpawnController>().FromNew().AsSingle();
        }
    }
}
