using KlopoffGames.Core;
using KlopoffGames.Core.Ads;
using KlopoffGames.Core.Analytics;
using KlopoffGames.Core.Audio;
using KlopoffGames.Core.Interfaces;
using KlopoffGames.Core.Localization;
using KlopoffGames.Core.Saving;
using MathMatch.Adapters;
using MathMatch.Services;
using Zenject;
#if YANDEX_GAMES && !UNITY_EDITOR
using KlopoffGames.WebPlatforms.Yandex;
#endif
#if VK_GAMES && !UNITY_EDITOR
using KlopoffGames.WebPlatforms.VK;
#endif
#if LOCAL_WEBGL && !UNITY_EDITOR
using KlopoffGames.WebPlatforms.LocalWebGL;
#endif

namespace MathMatch
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallProjectEnvironment();
            InstallWebPlatforms();
            InstallAdvertisement();
            InstallAnalytics();
            InstallSaves();
            InstallLocalization();
            InstallAudio();
            InstallServices();
        }

        private void InstallProjectEnvironment()
        {
            Container.Bind<ProjectEnvironment>().FromInstance(new ProjectEnvironment
            {
                Name = "production"
            });
        }

        private void InstallWebPlatforms()
        {
#if YANDEX_GAMES && !UNITY_EDITOR
            Container.Bind<UnityYandexBridge>()
                .FromNewComponentOnNewGameObject()
                .WithGameObjectName("YandexUnityBridge")
                .AsSingle()
                .NonLazy();
            Container.Bind<YandexManager>()
                .FromNew()
                .AsSingle()
                .NonLazy();
            Container.BindInterfacesAndSelfTo<YandexAppFocusObserver>()
                .FromNew()
                .AsSingle()
                .NonLazy();
#elif VK_GAMES && !UNITY_EDITOR
            Container.Bind<UnityVKBridge>()
                .FromNewComponentOnNewGameObject()
                .WithGameObjectName("VKUnityBridge")
                .AsSingle()
                .NonLazy();
            Container.Bind<VKManager>()
                .FromNew()
                .AsSingle()
                .NonLazy();
            Container.BindInterfacesAndSelfTo<VKAppFocusObserver>()
                .FromNew()
                .AsSingle()
                .NonLazy();
#elif LOCAL_WEBGL && !UNITY_EDITOR
            Container.Bind<UnityLocalWebGLBridge>()
                .FromNewComponentOnNewGameObject()
                .WithGameObjectName("LocalWebGLUnityBridge")
                .AsSingle()
                .NonLazy();
            Container.Bind<LocalWebGLManager>().AsSingle().NonLazy();
#endif
        }

        private void InstallAdvertisement()
        {
#if YANDEX_GAMES && !UNITY_EDITOR
            Container.Bind<IAdvertisement>().To<YandexAdvertisement>().AsSingle();
#elif VK_GAMES && !UNITY_EDITOR
            Container.Bind<IAdvertisement>().To<VKAdvertisement>().AsSingle();
#elif LOCAL_WEBGL && !UNITY_EDITOR
            Container.Bind<IAdvertisement>().To<DummyAdvertisement>().AsSingle();
#else
            Container.Bind<IAdvertisement>().To<DummyAdvertisement>().AsSingle();
#endif
        }

        private void InstallAnalytics()
        {
            Container.Bind<IAnalytics>().To<UnityAnalytics>().AsSingle().NonLazy();
        }

        private void InstallSaves()
        {
#if YANDEX_GAMES && !UNITY_EDITOR
            Container.Bind<ISavingManager>().To<YandexSavingManager>().AsSingle();
#elif VK_GAMES && !UNITY_EDITOR
            Container.Bind<ISavingManager>().To<VKSavingManager>().AsSingle();
#elif LOCAL_WEBGL && !UNITY_EDITOR
            Container.Bind<ISavingManager>().To<SimpleSavingManager>().AsSingle();
#else
            Container.Bind<ISavingManager>().To<SimpleSavingManager>().AsSingle();
#endif
        }

        private void InstallLocalization()
        {
            Container.Bind<ICsvParser>().To<CsvParserAdapter>().AsSingle();
            Container.Bind<LocalizationManager>().AsSingle();
        }

        private void InstallAudio()
        {
            Container.Bind<AudioResourceProvider>().AsSingle();
            Container.BindMemoryPool<AudioSource, AudioSourcePool>()
                .FromComponentInNewPrefabResource("Audio/AudioSource");
            Container.BindInterfacesAndSelfTo<AudioManager>().AsSingle();
        }
        
        private void InstallServices()
        {
            Container.Bind<SavingService>().FromNew().AsSingle();
        }
    }
}
