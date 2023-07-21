using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace MathMatch.Game.Behaviours
{
    public class GameOverText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textRenderer;
        [SerializeField] private float scaleFrom = 0f;
        [SerializeField] private float scaleTo = 3f;
        [SerializeField] private float scaleDuration = 2f;
        [SerializeField] private AnimationCurve scaleCurve;

        public void SetText(string text)
        {
            textRenderer.text = text;
        }
        
        public void PlayAnimation(Action onEnd)
        {
            transform
                .DOScale(scaleTo, scaleDuration)
                .From(scaleFrom)
                .SetLink(gameObject)
                .SetEase(scaleCurve)
                .OnComplete(() =>
                {
                    onEnd?.Invoke();
                });
            textRenderer
                .DOFade(1f, scaleDuration / 2f)
                .From(0f)
                .SetLink(gameObject);
        }
    }
}
