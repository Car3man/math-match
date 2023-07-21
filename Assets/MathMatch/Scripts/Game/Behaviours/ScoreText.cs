using TMPro;
using UnityEngine;

namespace MathMatch.Game.Behaviours
{
    public class ScoreText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        
        public void SetScore(int score)
        {
            text.text = score.ToString();
        }
    }
}
