using UnityEngine;

namespace MathMatch.Utility
{
    public class Set60Fps : MonoBehaviour
    {
        private void Awake()
        {
#if UNITY_EDITOR
            Application.targetFrameRate = 0;
#else
            Application.targetFrameRate = 60;
#endif
        }
    }
}
