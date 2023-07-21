using MathMatch.Game.Interfaces;
using MathMatch.Game.SubScenes;
using UnityEngine;
using Zenject;

namespace MathMatch.Game.Managers
{
    public class SubSceneManager : MonoBehaviour
    {
        [SerializeField] private TutorialSubScene tutorialSubScene;
        [SerializeField] private MenuSubScene menuSubScene;
        [SerializeField] private PlaySubScene playSubScene;
        
        private ISubScene[] _subScenes;

        [Inject]
        public void Construct()
        {
            _subScenes = new ISubScene[]
            {
                tutorialSubScene,
                menuSubScene,
                playSubScene
            };
        }

        public T Get<T>() where T : ISubScene
        {
            for (var i = 0; i < _subScenes.Length; i++)
            {
                if (_subScenes[i] is T)
                {
                    return (T)_subScenes[i];
                }
            }

            throw new System.ArgumentOutOfRangeException(typeof(T).Name, $"Unknown sub scene type, provided value: {typeof(T)}");
        }

        public void SwitchTo<T>() where T : ISubScene
        {
            for (int i = 0; i < _subScenes.Length; i++)
            {
                _subScenes[i].SetActive(false);
            }
            
            for (var i = 0; i < _subScenes.Length; i++)
            {
                if (_subScenes[i] is T)
                {
                    _subScenes[i].SetActive(true);
                    return;
                }
            }

            throw new System.ArgumentOutOfRangeException(typeof(T).Name, $"Unknown sub scene type, provided value: {typeof(T)}");
        }
    }
}