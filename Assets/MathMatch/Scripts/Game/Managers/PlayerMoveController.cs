using MathMatch.Game.Behaviours;
using NubikTowerBuilding.Ui;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MathMatch.Game.Managers
{
    public class PlayerMoveController
    {
        private readonly LevelGenerator _levelGenerator;
        private readonly UiInputTrigger _playerUiTrigger;

        private Player _player;
        private bool _trackInput;

        public delegate void PlayerJumpStartDelegate(Player player);
        public event PlayerJumpStartDelegate OnPlayerJumpStart;
        
        public delegate void PlayerJumpEndDelegate(Player player);
        public event PlayerJumpEndDelegate OnPlayerJumpEnd;

        public PlayerMoveController(
            LevelGenerator levelGenerator,
            UiInputTrigger playerUiTrigger
        )
        {
            _levelGenerator = levelGenerator;
            _playerUiTrigger = playerUiTrigger;
        }

        public void SetPlayer(Player player)
        {
            var firstNonEmptySegment = _levelGenerator.Segments[LevelGenerator.CountStartEmptySegments];
            
            _player = player;
            _player.PlaceTo(firstNonEmptySegment.Start);
        }

        public void StartInputTrack()
        {
            if (_trackInput)
            {
                Debug.LogWarning("Already track the input.");
                return;
            }

            _trackInput = true;
            _playerUiTrigger.OnClick += OnPlayerUiTriggerClick;
        }

        public void StopInputTrack()
        {
            if (!_trackInput)
            {
                Debug.LogWarning("Didn't tracked the input before.");
                return;
            }

            _trackInput = false;
            _playerUiTrigger.OnClick -= OnPlayerUiTriggerClick;
        }

        private void OnPlayerUiTriggerClick(PointerEventData eventData)
        {
            if (!_trackInput)
            {
                return;
            }

            if (_player == null)
            {
                return;
            }
            
            bool isJumpingBefore = _player.IsJumping;
            
            if (_player.Jump())
            {
                if (!isJumpingBefore)
                {
                    _player.OnJumpEnd += OnJumpEnd;
                }
            
                OnPlayerJumpStart?.Invoke(_player);
            }
        }

        private void OnJumpEnd()
        {
            _player.OnJumpEnd -= OnJumpEnd;
            OnPlayerJumpEnd?.Invoke(_player);
        }
    }
}