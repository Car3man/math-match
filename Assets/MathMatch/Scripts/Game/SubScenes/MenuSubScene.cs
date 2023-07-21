using MathMatch.Game.Behaviours;
using MathMatch.Game.Interfaces;
using MathMatch.Game.Managers;
using MathMatch.Game.Ui;
using UnityEngine;
using Zenject;

namespace MathMatch.Game.SubScenes
{
    public class MenuSubScene : MonoBehaviour, ISubScene
    {
        [SerializeField] private Transform cameraPos;
        [SerializeField] private CubeButton buttonPlay;
        [SerializeField] private MenuCanvas canvas;

        private SubSceneManager _subSceneManager;
        private Camera _sceneCamera;
        
        public bool IsActive { get; private set; }

        [Inject]
        public void Construct(SubSceneManager subSceneManager, Camera sceneCamera)
        {
            _subSceneManager = subSceneManager;
            _sceneCamera = sceneCamera;
        }
        
        private void OnEnable()
        {
            buttonPlay.OnClick += OnButtonPlayClick;
        }

        private void OnDisable()
        {
            buttonPlay.OnClick -= OnButtonPlayClick;
        }

        public void SetActive(bool value)
        {
            IsActive = value;
            
            if (value)
            {
                _sceneCamera.transform.position = cameraPos.position;
            }
            
            gameObject.SetActive(value);
            canvas.gameObject.SetActive(value);
        }

        private void OnButtonPlayClick()
        {
            _subSceneManager.SwitchTo<PlaySubScene>();
        }
    }
}
