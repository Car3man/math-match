using System.Collections.Generic;
using DG.Tweening;
using KlopoffGames.Core.Audio;
using MathMatch.Game.Interfaces;
using MathMatch.Game.Managers;
using MathMatch.Utility;
using UnityEngine;
using Zenject;

namespace MathMatch.Game.Behaviours
{
    public class Player : MonoBehaviour, IPoolItem
    {
        [SerializeField] private TrailRenderer trailRenderer;
        [SerializeField] private Transform numbersBody;
        [SerializeField] private GameObject[] numbers;
        
        private AudioManager _audioManager;
        private LevelGenerator _levelGenerator; // TODO: maybe get rid of it
        
        public float Height => numbersBody.localScale.y;
        public int Digit { get; private set; }
        public IPlace Place { get; private set; }
        public bool IsJumping { get; private set; }

        private Vector3 _jumpAnimationPlaceFromPos;
        private IPlace _jumpAnimationPlaceTo;
        private float _jumpAnimationVal;
        
        public delegate void JumpEndDelegate();
        public event JumpEndDelegate OnJumpEnd;

        private const float JumpDuration = 0.5f;

        [Inject]
        public void Construct(AudioManager audioManager, LevelGenerator levelGenerator)
        {
            _audioManager = audioManager;
            _levelGenerator = levelGenerator;
        }
        
        public void ResetItem()
        {
            trailRenderer.Clear();
            Place = null;
            IsJumping = false;
            _jumpAnimationPlaceTo = null;
        }

        private void Start()
        {
            numbersBody
                .DOScaleY(1.2f, 1f)
                .SetLink(numbersBody.gameObject)
                .SetLoops(-1, LoopType.Yoyo);
        }

        public void SetDigit(int digit)
        {
            if (digit < 0)
            {
                throw new System.Exception("Sub-zero digit number in player.");
            }

            if (digit > 9)
            {
                throw new System.Exception("Two-digits number in player.");
            }
            
            Digit = digit;

            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i].SetActive(digit == i); 
            }
        }

        public void PlaceTo(IPlace place)
        {
            Place = place;
            transform.position = place.TopPoint + Vector3.up * (Height / 2f);
            trailRenderer.Clear();
        }
        
        public bool Jump()
        {
            var currPlace = Place;
            if (_jumpAnimationPlaceTo != null)
            {
                currPlace = _jumpAnimationPlaceTo;
            }
            var nextPlace = _levelGenerator.GetNextPlace(currPlace);
            if (currPlace != Place && currPlace is Cube)
            {
                return false;
            }
            
            _audioManager.PlaySound("Jump1", false, 0.5f, 1f);

            IsJumping = true;
            
            DOTween.Kill("player_jump");
            
            _jumpAnimationPlaceFromPos = transform.position;
            _jumpAnimationPlaceTo = nextPlace;
            DOTween.To(
                    () => _jumpAnimationVal,
                    x =>
                    {
                        _jumpAnimationVal = x;
                        JumpAnimationFunc();
                    },
                    1f,
                    JumpDuration
                )
                .From(0f)
                .SetId("player_jump")
                .SetEase(Ease.Linear)
                .SetLink(gameObject)
                .OnComplete(() =>
                {
                    _jumpAnimationVal = 0f;
                    _jumpAnimationPlaceFromPos = transform.position;
                    _jumpAnimationPlaceTo = null;

                    PlaceTo(nextPlace);

                    IsJumping = false;
                    OnJumpEnd?.Invoke();
                });

            return true;
        }

        private void JumpAnimationFunc()
        {
            var jumpTo = _jumpAnimationPlaceTo.TopPoint + Vector3.up * (Height / 2f);
            var jumpMiddlePoint = (jumpTo + _jumpAnimationPlaceFromPos) * 0.5f + Vector3.up * 1.5f;
            var currTrans = transform;
            var newPos = BezierCurve.Point3(_jumpAnimationVal, new List<Vector3>
            {
                _jumpAnimationPlaceFromPos, jumpMiddlePoint, jumpTo
            });
            currTrans.position = newPos;
        }
    }
}
