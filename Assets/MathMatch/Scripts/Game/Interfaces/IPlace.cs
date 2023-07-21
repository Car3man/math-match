using UnityEngine;

namespace MathMatch.Game.Interfaces
{
    public interface IPlace
    {
        Vector3 Point { get; }
        Vector3 TopPoint { get; }
        bool IsFall { get; }
        float FallValue { get; }
        void SetFallValue(float value);
    }
}