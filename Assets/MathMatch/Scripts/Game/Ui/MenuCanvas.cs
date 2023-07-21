using KlopoffGames.Core.Audio;
using KlopoffGames.Core.Localization;
using MathMatch.Game.Managers;
using MathMatch.Game.SubScenes;
using MathMatch.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
#if VK_GAMES && !UNITY_EDITOR
using KlopoffGames.WebPlatforms.VK;
#endif

namespace MathMatch.Game.Ui
{
    public class MenuCanvas : MonoBehaviour
    {
        [SerializeField] private Button buttonMusic;
        [SerializeField] private Color buttonMusicColorOn;
        [SerializeField] private Color buttonMusicColorOff;
        [SerializeField] private Button buttonSounds;
        [SerializeField] private Color buttonSoundsColorOn;
        [SerializeField] private Color buttonSoundsColorOff;
        [SerializeField] private Button buttonLeaderboard;
        [SerializeField] private Button buttonShare;
        [SerializeField] private Button buttonHelp;
        [SerializeField] private FadeInOutTweenButton[] tweenButtons;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI bestScoreText;

        private AudioManager _audioManager;
        private LocalizationManager _localization;
        private ScoreController _scoreController;
        private GameOverObserver _gameOverObserver;
        private SavingService _savingService;
        private SubSceneManager _subSceneManager;
#if VK_GAMES && !UNITY_EDITOR
        private VKManager _vk;
#endif
        
        [Inject]
        public void Construct(
            AudioManager audioManager,
            LocalizationManager localization,
            ScoreController scoreController,
            GameOverObserver gameOverObserver,
            SavingService savingService,
            SubSceneManager subSceneManager
#if VK_GAMES && !UNITY_EDITOR
            ,VKManager vk
#endif
        )
        {
            _audioManager = audioManager;
            _localization = localization;
            _scoreController = scoreController;
            _gameOverObserver = gameOverObserver;
            _savingService = savingService;
            _subSceneManager = subSceneManager;
#if VK_GAMES && !UNITY_EDITOR
            _vk = vk;
#endif
        }
        
        private void OnEnable()
        {
#if VK_GAMES && !UNITY_EDITOR
            if (_vk.Platform != "web")
            {
                buttonLeaderboard.gameObject.SetActive(true);
            }
            else
            {
                buttonLeaderboard.gameObject.SetActive(false);
            }
            
            buttonShare.gameObject.SetActive(true);
#else
            buttonLeaderboard.gameObject.SetActive(false);
            buttonShare.gameObject.SetActive(false);
#endif
            
            buttonMusic.onClick.AddListener(OnButtonMusicClick);
            buttonSounds.onClick.AddListener(OnButtonSoundsClick);
            buttonLeaderboard.onClick.AddListener(OnButtonLeaderboardClick);
            buttonShare.onClick.AddListener(OnButtonShareClick);
            buttonHelp.onClick.AddListener(OnButtonHelpClick);
            
            SetScore(_scoreController.Score, _gameOverObserver.IsGameOver);
            SetBestScore(_savingService.GetBestScore(), true);

            UpdateMusicAndSoundsButtons();

            for (int i = 0; i < tweenButtons.Length; i++)
            {
                tweenButtons[i].FadeIn(i * 0.033f, null);
            }
            
            _audioManager.OnMusicMuteChange += OnMusicMuteChange;
            _audioManager.OnSoundMuteChange += OnSoundsMuteChange;
        }

        private void OnDisable()
        {
            buttonMusic.onClick.RemoveListener(OnButtonMusicClick);
            buttonSounds.onClick.RemoveListener(OnButtonSoundsClick);
            buttonLeaderboard.onClick.RemoveListener(OnButtonLeaderboardClick);
            buttonShare.onClick.RemoveListener(OnButtonShareClick);
            buttonHelp.onClick.RemoveListener(OnButtonHelpClick);
            
            _audioManager.OnMusicMuteChange -= OnMusicMuteChange;
            _audioManager.OnSoundMuteChange -= OnSoundsMuteChange;
        }

        public void SetScore(int score, bool active)
        {
            scoreText.gameObject.SetActive(active);
            scoreText.text = score.ToString();
        }

        public void SetBestScore(int score, bool active)
        {
            bestScoreText.gameObject.SetActive(active);
            bestScoreText.text = $"{_localization.GetString("lbl_best_score")}: {score}";
        }
        
        private void OnButtonMusicClick()
        {
            _audioManager.MusicMute = !_audioManager.MusicMute;
        }

        private void OnButtonSoundsClick()
        {
            _audioManager.SoundMute = !_audioManager.SoundMute;
        }

        private void OnButtonLeaderboardClick()
        {
#if VK_GAMES && !UNITY_EDITOR
            _vk.UpdateAndShowLeaderboardBox(_savingService.GetBestScore());
#endif
        }

        private void OnButtonShareClick()
        {
#if VK_GAMES && !UNITY_EDITOR
            _vk.ShowInviteBox();
#endif
        }
        
        private void OnButtonHelpClick()
        {
            _subSceneManager.Get<TutorialSubScene>().FromMenu = true;
            _subSceneManager.SwitchTo<TutorialSubScene>();
        }

        private void OnMusicMuteChange(bool val)
        {
            UpdateMusicAndSoundsButtons();
        }

        private void OnSoundsMuteChange(bool val)
        {
            UpdateMusicAndSoundsButtons();
        }
        
        private void UpdateMusicAndSoundsButtons()
        {
            var buttonMusicGraphics = (Image)buttonMusic.targetGraphic;
            var buttonSoundsGraphics = (Image)buttonSounds.targetGraphic;

            buttonMusicGraphics.color = _audioManager.MusicMute ? buttonMusicColorOff : buttonMusicColorOn;
            buttonSoundsGraphics.color = _audioManager.SoundMute ? buttonSoundsColorOff : buttonSoundsColorOn;
        }
    }
}
