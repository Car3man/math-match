using MathMatch.Game.Behaviours;
using UnityEngine;

namespace MathMatch.Game.Managers
{
    public class ScoreController
    {
        private readonly PlayerMoveController _playerMoveController;
        
        private bool _countScore;
        private int _score;

        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                OnScoreChange?.Invoke(_score);
            }
        }

        public delegate void ScoreChangeDelegate(int score);
        public event ScoreChangeDelegate OnScoreChange;

        public ScoreController(PlayerMoveController playerMoveController)
        {
            _playerMoveController = playerMoveController;
        }

        public void StartCountScore()
        {
            if (_countScore)
            {
                Debug.LogWarning("Already count the score.");
                return;
            }

            _countScore = true;
            
            _playerMoveController.OnPlayerJumpEnd += OnPlayerJumpEnd;
            Score = 0;
        }

        public void StopCountScore()
        {
            if (!_countScore)
            {
                Debug.LogWarning("Didn't count the score before.");
                return;
            }

            _countScore = false;

            _playerMoveController.OnPlayerJumpEnd -= OnPlayerJumpEnd;
        }
        
        private void OnPlayerJumpEnd(Player player)
        {
            if (player.Place is Cube cube)
            {
                if (player.Digit == cube.Digit)
                {
                    Score++;
                }
            }
        }
    }
}
