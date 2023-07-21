using KlopoffGames.Core.Localization;
using MathMatch.Game.Behaviours;
using MathMatch.Game.Managers;
using MathMatch.Game.Models;
using TMPro;
using UnityEngine;
using Zenject;

namespace MathMatch.Game.Ui
{
    public class PlayCanvas : MonoBehaviour
    {
        private LocalizationManager _localizationManager;
        private ScoreController _scoreController;

        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private GameOverText gameOverText;

        [Inject]
        public void Construct(
            LocalizationManager localizationManager,
            ScoreController scoreController
        )
        {
            _localizationManager = localizationManager;
            _scoreController = scoreController;
            
            _scoreController.OnScoreChange += OnScoreChange;
        }

        private void Start()
        {
            gameOverText.gameObject.SetActive(false);
        }

        public void ShowGameOverText(GameOverKind gameOverKind, System.Action onEnd)
        {
            var localizationString = gameOverKind switch
            {
                GameOverKind.Mismatch => _localizationManager.GetString("lbl_go_mismatch"),
                GameOverKind.DoubleDigit => _localizationManager.GetString("lbl_go_doubledigit"),
                GameOverKind.SubZero => _localizationManager.GetString("lbl_go_subzero"),
                GameOverKind.NoTime => _localizationManager.GetString("lbl_go_notime"),
                _ => throw new System.ArgumentOutOfRangeException(nameof(gameOverKind), "Unknown gameOverKind, provided value: " + gameOverKind)
            };
            
            gameOverText.gameObject.SetActive(true);
            gameOverText.SetText(localizationString);
            gameOverText.PlayAnimation(() =>
            {
                gameOverText.gameObject.SetActive(false);
                onEnd?.Invoke();
            });
        }

        public void SetScore(int score)
        {
            scoreText.text = score.ToString();
        }
        
        private void OnScoreChange(int score)
        {
            SetScore(score);
        }
    }
}
