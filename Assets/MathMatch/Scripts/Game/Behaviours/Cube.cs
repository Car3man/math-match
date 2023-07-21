using System.Collections.Generic;
using DG.Tweening;
using MathMatch.Game.Interfaces;
using MathMatch.Game.Models;
using TMPro;
using UnityEngine;

namespace MathMatch.Game.Behaviours
{
    public class Cube : MonoBehaviour, IPlace, IPoolItem
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private List<TriplanarColor> availableColors;
        [SerializeField] private TextMeshPro textRenderer;
        [SerializeField] private Vector3 topPointOffset;
        
        private static readonly int TopColor = Shader.PropertyToID("_TopColor");
        private static readonly int FrontColor = Shader.PropertyToID("_FrontColor");
        private static readonly int SideColor = Shader.PropertyToID("_SideColor");

        public int Digit { get; private set; }
        public Vector3 Point => transform.position;
        public Vector3 TopPoint => transform.position + topPointOffset;
        public float FallValue { get; private set; }
        public bool IsFall => FallValue >= 1f;

        public void ResetItem()
        {
            gameObject.SetActive(true);
            FallValue = 0f;
            textRenderer.gameObject.SetActive(true);
            textRenderer.color = Color.white;
            textRenderer.transform.localScale = Vector3.one;
        }

        public void PlayTextFadeOutAnimation()
        {
            textRenderer
                .DOFade(0f, 1f)
                .From(1f)
                .SetLink(gameObject);
            textRenderer.transform
                .DOScale(5f, 1f)
                .From(1f)
                .SetLink(gameObject)
                .OnComplete(() =>
                {
                    textRenderer.gameObject.SetActive(false);
                });
        }

        public void SetFallValue(float value)
        {
            FallValue = value;
            
            var currTrans = transform;
            var currPos = currTrans.position;
            currPos.y = value * -1.6f;
            currTrans.position = currPos;
            
            var textColor = textRenderer.color;
            textColor.a = 1f - value;
            textRenderer.color = textColor;
        }

        public void SetColor(int index)
        {
            var randomColor = availableColors[index];
            meshRenderer.material.SetColor(TopColor, randomColor.top);
            meshRenderer.material.SetColor(FrontColor, randomColor.front);
            meshRenderer.material.SetColor(SideColor, randomColor.side);
        }

        public void SetRandomColor()
        {
            var randomColor = availableColors[Random.Range(0, availableColors.Count)];
            meshRenderer.material.SetColor(TopColor, randomColor.top);
            meshRenderer.material.SetColor(FrontColor, randomColor.front);
            meshRenderer.material.SetColor(SideColor, randomColor.side);
        }

        public void SetTextRotation(float angle)
        {
            textRenderer.transform.localRotation = Quaternion.Euler(90f, angle, 0f);
        }

        public void SetText(string text, bool empty)
        {
            textRenderer.text = text;
            textRenderer.enabled = !empty;
        }

        public void SetDigit(int digit, bool empty)
        {
            if (digit < 0)
            {
                throw new System.Exception("Sub-zero digit number in cube.");
            }

            if (digit > 9)
            {
                throw new System.Exception("Two-digits number in cube.");
            }

            Digit = digit;
            
            textRenderer.text = digit.ToString();
            textRenderer.enabled = !empty;
        }
    }
}
