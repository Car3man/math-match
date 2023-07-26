using KlopoffGames.Core.Ads;
#if VK_GAMES && !UNITY_EDITOR
using KlopoffGames.WebPlatforms.VK;
#endif
#if YANDEX_GAMES && !UNITY_EDITOR
using KlopoffGames.WebPlatforms.Yandex;
#endif
using MathMatch.Game.Behaviours;
using MathMatch.Game.Interfaces;
using MathMatch.Game.Managers;
using MathMatch.Game.Models;
using MathMatch.Game.Spawners;
using MathMatch.Game.Ui;
using MathMatch.Services;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace MathMatch.Game.SubScenes
{
    public class PlaySubScene : MonoBehaviour, ISubScene
    {
        [SerializeField] private Transform cameraPos;
        [SerializeField] private PlayCanvas canvas;

        private SavingService _savingService;
        private IAdvertisement _ads;
#if YANDEX_GAMES && !UNITY_EDITOR
        private YandexManager _yandex;
#endif
#if VK_GAMES && !UNITY_EDITOR
        private VKManager _vk;
#endif
        private SubSceneManager _subSceneManager;
        private Camera _sceneCamera;
        private PlayerSpawner _playerSpawner;
        private LevelGenerator _levelGenerator;
        private PlayerCameraController _playerCameraController;
        private PlayerMoveController _playerMoveController;
        private ScoreController _scoreController;
        private FallingPlaces _fallingPlaces;
        private GameOverObserver _gameOverObserver;
        private SplashEffectSpawnController _splashEffectSpawnController;

        public bool IsActive { get; private set; }

        [Inject]
        public void Construct(
            SavingService savingService,
            IAdvertisement ads,
#if YANDEX_GAMES && !UNITY_EDITOR
            YandexManager yandex,
#endif
#if VK_GAMES && !UNITY_EDITOR
            VKManager vk,
#endif
            SubSceneManager subSceneManager,
            Camera sceneCamera,
            PlayerSpawner playerSpawner,
            LevelGenerator levelGenerator,
            PlayerCameraController playerCameraController,
            PlayerMoveController playerMoveController,
            ScoreController scoreController,
            FallingPlaces fallingPlaces,
            GameOverObserver gameOverObserver,
            SplashEffectSpawnController splashEffectSpawnController
        )
        {
            _savingService = savingService;
            _ads = ads;
#if YANDEX_GAMES && !UNITY_EDITOR
            _yandex = yandex;
#endif
#if VK_GAMES && !UNITY_EDITOR
            _vk = vk;
#endif
            _subSceneManager = subSceneManager;
            _sceneCamera = sceneCamera;
            _playerSpawner = playerSpawner;
            _levelGenerator = levelGenerator;
            _playerCameraController = playerCameraController;
            _playerMoveController = playerMoveController;
            _scoreController = scoreController;
            _fallingPlaces = fallingPlaces;
            _gameOverObserver = gameOverObserver;
            _splashEffectSpawnController = splashEffectSpawnController;
        }
        
        public void SetActive(bool value)
        {
            IsActive = value;
            
            if (value)
            {
                _sceneCamera.transform.position = cameraPos.position;
                
                var player = _playerSpawner.Spawn(Vector3.zero);
                player.SetDigit(Random.Range(0, 10));
                
                _levelGenerator.InitialSpawn(player.Digit);
                
                _playerMoveController.SetPlayer(player);
                _playerMoveController.StartInputTrack();
                _playerMoveController.OnPlayerJumpStart += OnFirstPlayerJump;

                _playerCameraController.StartFollowPlayer(player);
                _playerCameraController.MoveToPlayerForced();

                _scoreController.StartCountScore();
                
                _splashEffectSpawnController.StartSpawn();
            } 
            else 
            {
                _splashEffectSpawnController.StopSpawn();
            }
            
            gameObject.SetActive(value);
            canvas.gameObject.SetActive(value);
        }

        private void OnFirstPlayerJump(Player player)
        {
            _playerMoveController.OnPlayerJumpStart -= OnFirstPlayerJump;
            _fallingPlaces.StartFall();
            _gameOverObserver.StartObserve();
            _gameOverObserver.OnGameOver += OnGameOver;
        }

        private void OnGameOver(GameOverKind gameOverKind)
        {
            _playerCameraController.StopFollowPlayer();
            _scoreController.StopCountScore();
            _playerMoveController.StopInputTrack();
            _fallingPlaces.StopFall();
            
            _gameOverObserver.OnGameOver -= OnGameOver;
            _gameOverObserver.StopObserve();
            
            if (_scoreController.Score > _savingService.GetBestScore())
            {
                _savingService.SetBestScore(_scoreController.Score);
                
#if YANDEX_GAMES && !UNITY_EDITOR
                _yandex.SetLeaderboardScore("default", _scoreController.Score);
#endif
#if VK_GAMES && !UNITY_EDITOR
                _vk.UpdateAndShowLeaderboardBox(_scoreController.Score);
#endif
            }
            
            _playerSpawner.Despawn(true);
            canvas.ShowGameOverText(gameOverKind, BackToMenuAfterGameOver);
        }

        private void BackToMenuAfterGameOver()
        {
            if (Time.time - _ads.LastAdRequestTime >= 60f * 1f)
            {
                _ads.OnInterstitialAdClose += InterstitialAdClose;
                _ads.ShowInterstitialAd();
            
                void InterstitialAdClose()
                {
                    _ads.OnInterstitialAdClose -= InterstitialAdClose;
                
                    _subSceneManager.SwitchTo<MenuSubScene>();
                }
            }
            else
            {
                _subSceneManager.SwitchTo<MenuSubScene>();
            }
        }
    }
}
