using KlopoffGames.Core.Audio;
using MathMatch.Game.Managers;
using MathMatch.Game.SubScenes;
using MathMatch.Services;
using UnityEngine;
using Zenject;
using KlopoffGames.Core.Ads;

namespace MathMatch.Game
{
    public class GameEntryPoint : MonoBehaviour
    {
        private AudioManager _audioManager;
        private SavingService _savingService;
        private SubSceneManager _subSceneManager;
        private IAdvertisement _ads;

        [Inject]
        public void Construct(
            AudioManager audioManager,
            SavingService savingService,
            SubSceneManager subSceneManager
            ,IAdvertisement ads
        )
        {
            _audioManager = audioManager;
            _savingService = savingService;
            _subSceneManager = subSceneManager;
            _ads = ads;
        }
        
        private void Start()
        {
            Random.InitState(System.DateTime.UtcNow.Millisecond);
            
            _audioManager.PlayMusic("Background", 0.05f);
            
#if YANDEX_GAMES && !UNITY_EDITOR
            _ads.ShowInterstitialAd();
#endif
            
            if (_savingService.GetIsFTUE())
            {
                _subSceneManager.Get<TutorialSubScene>().FromMenu = false;
                _subSceneManager.SwitchTo<TutorialSubScene>();
                _savingService.SetIsFTUE(false);
            }
            else
            {
                _subSceneManager.SwitchTo<MenuSubScene>();
            }
        }
    }
}