using KlopoffGames.Core.Audio;
using MathMatch.Game.Behaviours;
using MathMatch.Game.Interfaces;
using MathMatch.Game.Models;
using MathMatch.Game.Spawners;
using UnityEngine;

namespace MathMatch.Game.Managers
{
    public class GameOverObserver
    {
        private readonly AudioManager _audioManager;
        private readonly PlayerSpawner _playerSpawner;
        private readonly PlayerMoveController _playerMoveController;
        private readonly FallingPlaces _fallingPlaces;
        private readonly LevelGenerator _levelGenerator;

        private bool _observeStarted;
        private Player _player;
        
        public bool IsGameOver { get; private set; }

        public delegate void GameOverDelegate(GameOverKind gameOverKind);
        public event GameOverDelegate OnGameOver;

        public GameOverObserver(
            AudioManager audioManager,
            PlayerSpawner playerSpawner,
            PlayerMoveController playerMoveController,
            FallingPlaces fallingPlaces,
            LevelGenerator levelGenerator
        )
        {
            _audioManager = audioManager;
            _playerSpawner = playerSpawner;
            _playerMoveController = playerMoveController;
            _fallingPlaces = fallingPlaces;
            _levelGenerator = levelGenerator;
        }

        public void StartObserve()
        {
            if (_observeStarted)
            {
                Debug.LogWarning("GameOver observe already started.");
                return;
            }

            IsGameOver = false;
            _observeStarted = true;
            _player = _playerSpawner.Player;

            _fallingPlaces.OnPlaceFallUpdate += OnPlaceFallUpdate;
            _playerMoveController.OnPlayerJumpEnd += OnPlayerJumpEnd;
        }

        public void StopObserve()
        {
            if (!_observeStarted)
            {
                Debug.LogWarning("GameOver observe didn't started.");
                return;
            }
            
            _observeStarted = false;

            _fallingPlaces.OnPlaceFallUpdate -= OnPlaceFallUpdate;
            _playerMoveController.OnPlayerJumpEnd -= OnPlayerJumpEnd;
        }
        
        private void OnPlaceFallUpdate(IPlace place, float fallValue)
        {
            if (IsGameOver)
            {
                return;
            }
            
            if (!_player.IsJumping && _player.Place == place && fallValue > 0.1f)
            {
                HandleGameOver(GameOverKind.NoTime);
            }
        }
        
        private void OnPlayerJumpEnd(Player player)
        {
            if (player.Place.IsFall)
            {
                HandleGameOver(GameOverKind.NoTime);
                return;
            }
            
            if (player.Place is Cube cube)
            {
                if (player.Digit != cube.Digit)
                {
                    HandleGameOver(GameOverKind.Mismatch);
                    return;
                }
                
                player.SetDigit(cube.Digit);
                cube.PlayTextFadeOutAnimation();
                
                _levelGenerator.SpawnNextSegment(false);
                _audioManager.PlaySound("SuccessJump", false, 1f, 1f);
            } 
            else if (player.Place is Cylinder cylinder)
            {
                var newDigit = player.Digit + cylinder.Operation;

                if (newDigit < 0)
                {
                    HandleGameOver(GameOverKind.SubZero);
                    return;
                }

                if (newDigit > 9)
                {
                    HandleGameOver(GameOverKind.DoubleDigit);
                    return;
                }
                
                player.SetDigit(newDigit);
                cylinder.PlayTextFadeOutAnimation();
                
                _audioManager.PlaySound("SuccessJump", false, 1f, 1f);
            }
        }

        private void HandleGameOver(GameOverKind gameOverKind)
        {
            _audioManager.PlaySound("FailJump2", false, 0.75f, 1f);
            IsGameOver = true;
            OnGameOver?.Invoke(gameOverKind);
        }
    }
}