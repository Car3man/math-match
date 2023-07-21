using System.Collections.Generic;
using System.Linq;
using MathMatch.Game.Behaviours;
using MathMatch.Game.Interfaces;
using MathMatch.Game.Models;
using MathMatch.Game.Spawners;
using MathMatch.Game.Utility;
using UnityEngine;

namespace MathMatch.Game.Managers
{
    public class LevelGenerator
    {
        private readonly CubeSpawner _cubeSpawner;
        private readonly CylinderSpawner _cylinderSpawner;
        
        public readonly List<Segment> Segments = new();
            
        private readonly Vector3 _firstSegmentPosition = new(3f, 0f, -7f);
        private int _startDigit;
        private int _additionalCylinders;

        public const int CountStartEmptySegments = 1;
        
        public LevelGenerator(
            CubeSpawner cubeSpawner,
            CylinderSpawner cylinderSpawner
        )
        {
            _cubeSpawner = cubeSpawner;
            _cylinderSpawner = cylinderSpawner;
        }
        
        public void InitialSpawn(int startDigit)
        {
            _startDigit = startDigit;
            
            DespawnSegments();
            
            for (int i = 0; i < CountStartEmptySegments; i++)
            {
                SpawnNextSegment(true);
            }
            
            for (int i = 0; i < 6; i++)
            {
                SpawnNextSegment(false);
            }
        }

        public void SetAdditionalCylinders(int additionalCylinders)
        {
            _additionalCylinders = additionalCylinders;
        }

        public void SpawnNextSegment(bool empty)
        {
            var segmentId = Segments.Count;
            var isLeftDirection = IsLeftDirectionSegment(segmentId);
            var segmentDifficult = GetNextCylindersCount();

            var startDigit = segmentId > 0 ? Segments[^1].End.Digit : 0;
            if (segmentId == CountStartEmptySegments)
            {
                startDigit = _startDigit;
            }
            
            var endDigit = 0;
            do
            {
                endDigit = Random.Range(0, 10);
            } while (endDigit == startDigit);
            
            var isPositive = endDigit - startDigit >= 0;
            var absDelta = Mathf.Abs(endDigit - startDigit);
            var possibleSolutions = MathHelper.CoinChangeProblem(MathHelper.Digits, absDelta);
            if (!isPositive)
            {
                for (int i = 0; i < possibleSolutions.Count; i++)
                {
                    for (int j = 0; j < possibleSolutions[i].Count; j++)
                    {
                        possibleSolutions[i][j] *= -1;
                    }
                }
            }
            possibleSolutions = possibleSolutions.Where(x => x.Count <= segmentDifficult).ToList();
            
            var oneSolution = possibleSolutions[Random.Range(0, possibleSolutions.Count)];
            var operations = new List<int>(oneSolution);
            while (operations.Count < segmentDifficult)
            {
                operations.Add(Random.Range(-9, 10));
            }
            operations.Shuffle();

            var startCube = segmentId > 0 ? Segments[^1].End : _cubeSpawner.Spawn(_firstSegmentPosition);
            if (segmentId == 0)
            {
                startCube.SetDigit(startDigit, empty);
            }

            var cylinders = new List<Cylinder>();
            for (int i = 0; i < segmentDifficult; i++)
            {
                var cylinderPosition = i > 0
                    ? GetNextCylinderPosition(cylinders[i - 1], isLeftDirection)
                    : GetNextCylinderPosition(startCube, isLeftDirection);
                var cylinder = _cylinderSpawner.Spawn(cylinderPosition);
                cylinder.SetOperation(operations[i], empty);
                cylinders.Add(cylinder);
            }
            
            var endCube = _cubeSpawner.Spawn(GetNextCubePosition(cylinders[^1], isLeftDirection));
            endCube.SetDigit(endDigit, empty);
            
            var segment = new Segment
            {
                Start = startCube,
                Cylinders = cylinders,
                End = endCube
            };
            Segments.Add(segment);
        }

        public void HidePlace(IPlace place)
        {
            if (place is Cube cube)
            {
                cube.gameObject.SetActive(false);
            } 
            else if (place is Cylinder cylinder)
            {
                cylinder.gameObject.SetActive(false);
            }
        }

        public Segment GetSegmentByPlace(IPlace place)
        {
            for (var i = 0; i < Segments.Count; i++)
            {
                var segment = Segments[i];
                if (ReferenceEquals(segment.Start, place))
                {
                    return segment;
                }
                for (int j = 0; j < segment.Cylinders.Count; j++)
                {
                    if (ReferenceEquals(segment.Cylinders[j], place))
                    {
                        if (j >= segment.Cylinders.Count - 1)
                        {
                            return segment;
                        }
                        return segment;
                    }
                }
                if (i == Segments.Count - 1 && ReferenceEquals(segment.End, place))
                {
                    return segment;
                }
            }
            throw new System.Exception("Segment by place not found, unexpected behaviour.");
        }

        public IPlace GetNextPlace(IPlace place)
        {
            for (var i = 0; i < Segments.Count; i++)
            {
                var segment = Segments[i];
                if (ReferenceEquals(segment.Start, place))
                {
                    return segment.Cylinders[0];
                }
                for (int j = 0; j < segment.Cylinders.Count; j++)
                {
                    if (ReferenceEquals(segment.Cylinders[j], place))
                    {
                        if (j >= segment.Cylinders.Count - 1)
                        {
                            return segment.End;
                        }
                        return segment.Cylinders[j + 1];
                    }
                }
            }
            Debug.LogWarning("Next place not found, returned provided place.");
            return place;
        }

        private Vector3 GetNextCylinderPosition(Cube cube, bool isLeftDirection)
        {
            return cube.transform.position + 1.5f * (isLeftDirection ? Vector3.left : Vector3.forward);
        }
        
        private Vector3 GetNextCylinderPosition(Cylinder cylinder, bool isLeftDirection)
        {
            return cylinder.transform.position + 1.25f * (isLeftDirection ? Vector3.left : Vector3.forward);
        }

        private Vector3 GetNextCubePosition(Cylinder cylinder, bool isLeftDirection)
        {
            return cylinder.transform.position + 1.5f * (isLeftDirection ? Vector3.left : Vector3.forward);
        }

        private bool IsLeftDirectionSegment(int segmentId)
        {
            return segmentId % 2 != 0;
        }

        private int GetNextCylindersCount()
        {
            if (Segments.Count == 0)
            {
                return 3;
            }
            return Random.Range(2, 4) + _additionalCylinders;
        }

        private void DespawnSegments()
        {
            for (var i = 0; i < Segments.Count; i++)
            {
                var segment = Segments[i];
                
                if (i == 0)
                {
                    _cubeSpawner.Despawn(segment.Start);
                }
                
                for (var j = 0; j < segment.Cylinders.Count; j++)
                {
                    _cylinderSpawner.Despawn(segment.Cylinders[j]);
                }
                
                _cubeSpawner.Despawn(segment.End);
            }
            
            Segments.Clear();
        }
    }
}
