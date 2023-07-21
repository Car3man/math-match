using DG.Tweening;
using UnityEngine;

namespace MathMatch.Game.Ui
{
    public class FadeInOutTweenButton : MonoBehaviour
    {
        [SerializeField] private Vector2 from;
        [SerializeField] private Vector2 to;
        [SerializeField] private float duration;

        private RectTransform _rectTransform;

        private RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = GetComponent<RectTransform>();
                }
                return _rectTransform;
            }
        }
        
        public void FadeIn(float delay, System.Action onEnd)
        {
            RectTransform.anchoredPosition = from;
            RectTransform
                .DOAnchorPos(to, duration)
                .From(from)
                .SetDelay(delay)
                .SetLink(gameObject)
                .OnComplete(() => onEnd?.Invoke());
        }

        public void FadeOut(float delay, System.Action onEnd)
        {
            RectTransform.anchoredPosition = to;
            RectTransform
                .DOAnchorPos(from, duration)
                .From(to)
                .SetDelay(delay)
                .SetLink(gameObject)
                .OnComplete(() => onEnd?.Invoke());
        }
    }
}
