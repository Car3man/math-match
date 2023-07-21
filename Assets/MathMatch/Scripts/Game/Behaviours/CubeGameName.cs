using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MathMatch.Game.Behaviours
{
    public class CubeGameName : MonoBehaviour
    {
        [SerializeField] private Cube[] cubes;
        [SerializeField] private bool animate;

        private void OnEnable()
        {
            for (int i = 0; i < cubes.Length; i++)
            {
                var cubeTrans = cubes[i].transform;
                var cubeLocPos = cubeTrans.transform.localPosition;
                cubeLocPos.y = 0f;
                cubeTrans.localPosition = cubeLocPos;
                cubes[i].SetRandomColor();
                
                if (animate)
                {
                    PlayCubeAnimation(i);
                }
            }
        }

        private void PlayCubeAnimation(int index)
        {
            var nextPos = cubes[index].transform.localPosition.y < 0f ? 0f : -Random.Range(0.4f, 0.5f);
            cubes[index].transform
                .DOLocalMoveY(nextPos, 1f)
                .SetLink(cubes[index].gameObject)
                .SetEase(Ease.Linear)
                .SetDelay(Random.Range(0f, 2f))
                .OnComplete(() =>
                {
                    PlayCubeAnimation(index);
                });
        }
        
        private void OnDisable()
        {
            for (int i = 0; i < cubes.Length; i++)
            {
                DOTween.Kill(cubes[i].transform);
            }
        }
    }
}
