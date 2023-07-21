using KlopoffGames.Core.Audio;
using UnityEngine;
using Zenject;

namespace MathMatch.Game.Behaviours
{
    public class CubeButton : MonoBehaviour
    {
        [SerializeField] private GameObject highlightObj;
        [SerializeField] private GameObject pressObj;
        [SerializeField] private string sfxName = "ClickGeneric1";

        private AudioManager _audioManager;
        
        private bool _isEnter;
        private bool _isDown;

        public delegate void ClickDelegate();
        public event ClickDelegate OnClick;

        [Inject]
        public void Construct(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }

        private void OnEnable()
        {
            UpdateStates();
        }

        private void OnDisable()
        {
            _isEnter = false;
            _isDown = false;
            UpdateStates();
        }

        private void OnMouseEnter()
        {
            _isEnter = true;
            UpdateStates();
        }

        private void OnMouseExit()
        {
            _isEnter = false;
            UpdateStates();
        }

        private void OnMouseDown()
        {
            _isDown = true;
            UpdateStates();
        }

        private void OnMouseUp()
        {
            _isDown = false;
            UpdateStates();
            _audioManager.PlaySound(sfxName, false,1f, 1f); 
            OnClick?.Invoke();
        }

        private void UpdateStates()
        {
            highlightObj.SetActive(_isEnter && !_isDown);
            pressObj.SetActive(_isDown);
        }
    }
}
