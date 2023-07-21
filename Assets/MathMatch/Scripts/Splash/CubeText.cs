using System.Collections.Generic;
using MathMatch.Game.Behaviours;
using MathMatch.Game.Spawners;
using UnityEngine;
using Zenject;

namespace MathMatch.Splash
{
    public class CubeText : MonoBehaviour
    {
        private CubeSpawner _cubeSpawner;
        private readonly List<Cube> _cubes = new();

        [Inject]
        public void Construct(CubeSpawner cubeSpawner)
        {
            _cubeSpawner = cubeSpawner;
        }
        
        public void SetText(string text)
        {
            var textLength = text.Length;
            for (int i = 0; i < textLength; i++)
            {
                var cubePosition = GetCubePosition(i, textLength);
                if (i + 1 >= _cubes.Count)
                {
                    var cubeInstance = _cubeSpawner.Spawn(Vector3.zero);
                    cubeInstance.SetColor(3);
                    cubeInstance.transform.SetParent(transform);
                    _cubes.Add(cubeInstance);
                }
                var cube = _cubes[i];
                cube.transform.localPosition = cubePosition;
                cube.SetTextRotation(-90f);
                cube.SetText(text[i].ToString(), false);
            }
            while (_cubes.Count > textLength)
            {
                _cubeSpawner.Despawn(_cubes[^1]);
                _cubes.RemoveAt(_cubes.Count - 1);
            }
        }
        
        private Vector3 GetCubePosition(int charIndex, int textLength)
        {
            return new Vector3(0f, 0f, (0.55f + charIndex * 1.1f) + (textLength * 1.1f / -2f));
        }
    }
}
