using MathMatch.Game.Behaviours;
using MathMatch.Game.Interfaces;
using MathMatch.Game.Managers;
using UnityEngine;
using Zenject;

namespace MathMatch.Game.SubScenes
{
    public class TutorialSubScene : MonoBehaviour, ISubScene
    {
        [SerializeField] private Transform cameraPos;
        [SerializeField] private CubeButton buttonOk;

        private SubSceneManager _subSceneManager;
        private Camera _sceneCamera;
        
        public bool FromMenu { get; set; }
        public bool IsActive { get; private set; }

        [Inject]
        public void Construct(SubSceneManager subSceneManager, Camera sceneCamera)
        {
            _subSceneManager = subSceneManager;
            _sceneCamera = sceneCamera;
        }
        
        private void OnEnable()
        {
            buttonOk.OnClick += OnButtonOkayClick;
        }

        private void OnDisable()
        {
            buttonOk.OnClick -= OnButtonOkayClick;
        }

        public void SetActive(bool value)
        {
            IsActive = value;
            
            if (value)
            {
                _sceneCamera.transform.position = cameraPos.position;
            }
            
            gameObject.SetActive(value);
        }

        private void OnButtonOkayClick()
        {
            if (FromMenu)
            {
                _subSceneManager.SwitchTo<MenuSubScene>();
            }
            else
            {
                _subSceneManager.SwitchTo<PlaySubScene>();
            }
        }
    }
}
