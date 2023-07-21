using DG.Tweening;
using MathMatch.Game.Interfaces;
using TMPro;
using UnityEngine;

namespace MathMatch.Game.Behaviours
{
    public class Cylinder : MonoBehaviour, IPlace, IPoolItem
    {
        [SerializeField] private Color positiveColor = new Color32(19, 196, 153, 255);
        [SerializeField] private Color negativeColor  = new Color32(209, 70, 72, 255);
        [SerializeField] private TextMeshPro operationText;
        [SerializeField] private Vector3 topPointOffset;
        
        public int Operation { get; private set; }
        public Vector3 Point => transform.position;
        public Vector3 TopPoint => transform.position + topPointOffset;
        public float FallValue { get; private set; }
        public bool IsFall => FallValue >= 1f;

        public void ResetItem()
        {
            gameObject.SetActive(true);
            FallValue = 0f;
            operationText.gameObject.SetActive(true);
            operationText.color = Color.white;
            operationText.transform.localScale = Vector3.one;
        }

        public void PlayTextFadeOutAnimation()
        {
            operationText
                .DOFade(0f, 1f)
                .From(1f)
                .SetLink(gameObject);
            operationText.transform
                .DOScale(2f, 1f)
                .From(1f)
                .SetLink(gameObject)
                .OnComplete(() =>
                {
                    operationText.gameObject.SetActive(false);
                });
        }

        public void SetFallValue(float value)
        {
            FallValue = value;
            
            var currTrans = transform;
            var currPos = currTrans.position;
            currPos.y = value * -1.3f;
            currTrans.position = currPos;

            var textColor = operationText.color;
            textColor.a = 1f - value;
            operationText.color = textColor;
        }

        public void SetOperation(int operation, bool empty)
        {
            if (operation > 9)
            {
                throw new System.Exception("Two-digits operation in cube.");
            }

            Operation = operation;

            operationText.color = operation >= 0 ? positiveColor : negativeColor;
            operationText.text = operation > 0 ? $"+{operation}" : $"{operation}";
            operationText.enabled = !empty;
        }
    }
}
