using System.Text;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using KlopoffGames.Core.Analytics;
using KlopoffGames.Core.Audio;
using KlopoffGames.Core.Localization;
using KlopoffGames.Core.Saving;
using MathMatch.Scenes.Refs;
using MathMatch.Services;
using UnityEngine;
using Zenject;
#if YANDEX_GAMES && !UNITY_EDITOR
using KlopoffGames.WebPlatforms.Yandex;
#endif
#if VK_GAMES && !UNITY_EDITOR
using KlopoffGames.WebPlatforms.VK;
#endif

namespace MathMatch.Splash
{
    public class SplashEntryPoint : MonoBehaviour
    {
        [SerializeField] private CubeText cubeText;

        private IAnalytics _analytics;
        private LocalizationManager _localizationManager;
        private ISavingManager _savingManager;
        private AudioManager _audioManager;
        private SavingService _savingService;
#if YANDEX_GAMES && !UNITY_EDITOR
        private YandexManager _yandex;
#endif
#if VK_GAMES && !UNITY_EDITOR
        private VKManager _vk;
#endif

        private float _loaderTextDots;

        [Inject]
        public void Construct(
            IAnalytics analytics,
            LocalizationManager localizationManager,
            ISavingManager savingManager,
            AudioManager audioManager,
            SavingService savingService
#if YANDEX_GAMES && !UNITY_EDITOR
            ,YandexManager yandex
#endif
#if VK_GAMES && !UNITY_EDITOR
            ,VKManager vk
#endif
        )
        {
            _analytics = analytics;
            _localizationManager = localizationManager;
            _savingManager = savingManager;
            _audioManager = audioManager;
            _savingService = savingService;
#if YANDEX_GAMES && !UNITY_EDITOR
            _yandex = yandex;
#endif
#if VK_GAMES && !UNITY_EDITOR
            _vk = vk;
#endif
        }
        
        private async void Start()
        {
            cubeText.gameObject.SetActive(false);
            
#if YANDEX_GAMES && !UNITY_EDITOR
            await UniTask.WaitUntil(() => _yandex.IsSdkInit);
            _yandex.OnGameReady();
            
            await UniTask.WaitUntil(() => _yandex.IsSdkReady);
            _localizationManager.SetLanguage(_yandex.Lang);
#endif
            
#if VK_GAMES && !UNITY_EDITOR
            await UniTask.WaitUntil(() => _vk.IsSdkInit);
            _localizationManager.SetLanguage(_vk.Lang);
#endif
            
            AnimateLoadingText();
            cubeText.gameObject.SetActive(true);

            await _analytics.Initialize();
            await LoadSaves();
            
            ApplySavedSettings();
            
#if UNITY_EDITOR
            await UniTask.Delay(System.TimeSpan.FromSeconds(4f));
#endif
            
            GameSceneRef.Load();
        }
        
        private async UniTask LoadSaves()
        {
            var taskCompletionSource = new TaskCompletionSource<object>();
            _savingManager.Load(() =>
            {
                taskCompletionSource.SetResult(true);
            });
            await taskCompletionSource.Task;
        }
        
        
        private void ApplySavedSettings()
        {
            var musicVolume = _savingService.GetMusicVolume();
            var soundVolume = _savingService.GetSoundVolume();
            
            _audioManager.MusicMute = musicVolume < 1f;
            _audioManager.SoundMute = soundVolume < 1f;
        }
        
        private void AnimateLoadingText()
        {
            string loadingLocalized = _localizationManager.GetString("lbl_loading");

            DOTween
                .To(
                    () => _loaderTextDots,
                    dots =>
                    {
                        _loaderTextDots = dots;

                        var sb = new StringBuilder();
                        sb.Append(loadingLocalized);
                        for (int i = 0; i < 3; i++)
                        {
                            sb.Append(i >= Mathf.FloorToInt(_loaderTextDots) ? " " : ".");
                        }
                        cubeText.SetText(sb.ToString());
                    },
                    4f,
                    1.5f)
                .From(0f)
                .SetLink(gameObject)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);
        }
    }
}
